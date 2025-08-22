# TODO: Consider using https://github.com/ebekker/pwsh-github-action-tools or https://github.com/Amadevus/pwsh-script
function Invoke-GitHubAction {
  [CmdletBinding()] [OutputType([string])]
  param(
    [Parameter(Mandatory)]
    [scriptblock] $InputObject
  )
  try {
    $Output = . $InputObject
    $Output | ConvertTo-GitHubOutput >> $env:GITHUB_OUTPUT
  }
  catch {
    $err = $_ | Out-String
    @{
      error = $err
    } | ConvertTo-GitHubOutput >> $env:GITHUB_OUTPUT
    Set-GitHubActionFailed $err
  }
}

function Get-AutoOption($Value, $Auto) {
  return @{ auto = $Auto; yes = $true; no = $false }[$Value]
}

function ConvertTo-GitHubOutput {
  [CmdletBinding()] [OutputType([string])]
  param(
    [Parameter(Mandatory, ValueFromPipeline)]
    [psobject] $InputObject
  )
  process {
    $props = $InputObject -is [hashtable] ? $InputObject.GetEnumerator() : $InputObject.psobject.Properties
    foreach ($prop in $props) {
      $key = $prop.Name
      $value = `
        $prop.Value -is [bool] ? ($prop.Value ? 'true' : 'false') :`
        $prop.Value -is [string] ? $prop.Value :`
        $null -ne $prop.Value ? "$($prop.Value)" :`
        ""
      if ($value.Contains("`n") -or $value.Contains("`r")) {
        $delimiter = "GH_EOF_$(New-Guid)"
        "$key<<$delimiter`n$value`n$delimiter"
      }
      else {
        "$key=$value"
      }
    }
  }
}

function Set-SarifCategory
{
  param(
    [Parameter(Mandatory, ValueFromPipeline)]
    [IO.FileInfo[]] $InputObject,
    [Parameter(Mandatory, ParameterSetName = 'Category')]
    [string] $Category,
    [Parameter(Mandatory, ParameterSetName = 'CategorySetter')]
    [scriptblock] $CategorySetter
  )
  process {
    foreach ($file in $InputObject.FullName) {
      $sarif = Get-Content -LiteralPath $file -Raw | ConvertFrom-Json -Depth 16 -AsHashtable
      foreach ($run in $sarif.runs) {
        $run.automationDetails = @{}
        if ($Category) {
          $run.automationDetails.id = $Category
        } elseif ($CategorySetter) {
          & $CategorySetter $run.automationDetails -File $file
        }
      }
      $sarif | ConvertTo-Json -Depth 16 | Set-Content -LiteralPath $file
    }
  }
}

function Update-CscSarifCategory
{
  param(
    [Parameter(Mandatory)]
    [string] $Path
  )
  Get-ChildItem $Path | Set-SarifCategory -CategorySetter {
    param($_, [IO.FileInfo] $File)
    $cats = $File.BaseName -split '-' | Where-Object { $_ }
    $project = $cats[0]
    $platform = ($cats | Select-Object -Skip 1) -join ':'
    $_.id = "MSBuild/$($cats -join '/')/language:csharp"
    $_.description = @{ text = "C# compiler log of project $project built for $platform" }
    $_.properties = @{
      project = $project
      platform = $platform
    }
  }
}

if ($env:TERM_PROGRAM -eq 'vscode') {
  Update-CscSarifCategory 'Artifacts/logs/sarif/*.sarif'
}
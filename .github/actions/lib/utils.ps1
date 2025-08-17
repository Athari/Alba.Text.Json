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
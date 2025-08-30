# [System.Diagnostics.CodeAnalysis.SuppressMessage('PSUseDeclaredVarsMoreThanAssignments', '')]
# param()

function ConvertTo-Sarif
{
  [CmdletBinding()]
  [OutputType([ordered])]
  param(
    [Parameter(Mandatory, ValueFromPipeline)]
    [psobject[]] $InputObject,
    [string] $Category = 'PSScriptAnalyzer/language:powershell',
    [string] $IgnoreFiles = ''
  )
  begin {
    $version = '0.1.0'
    $helpUrlBase = "https://learn.microsoft.com/powershell/utility-modules/psscriptanalyzer"
    $records = [Collections.ArrayList]::new()
  }
  process {
    [void] $records.Add($InputObject)
  }
  end {
    $records = $records |
      Where-Object { [string]::IsNullOrEmpty($IgnoreFiles) ? $true : $_.ScriptPath -notmatch $IgnoreFiles } |
      Sort-Object -Property ScriptPath, Line, Severity, RuleName, Message

    function Format-RuleName([string] $RuleName)
    {
      $RuleName `
        -replace '^PS', '' `
        -creplace '(?<!^)([A-Z])(?=[a-z])', ' $1' `
        -creplace '([a-z])([A-Z])', '$1 $2' `
        -replace '\A\s+|\s+\z', ''
    }

    function Format-RuleHelpUrl([string] $RuleName)
    {
      $helpId = $RuleName.Substring(2).ToLowerInvariant()
      return "$helpUrlBase/rules/$helpId"
    }

    function Format-PathUri([string] $Path) {
      [Uri]::new((Resolve-Path $Path)).AbsoluteUri
    }

    $module = Get-Module PSScriptAnalyzer
    $moduleDesc = @{
      text = $module.Description
    }

    $sarif = [ordered] @{
      #'$schema' = "https://schemastore.azurewebsites.net/schemas/json/sarif-2.1.0-rtm.6.json"
      '$schema' = "https://docs.oasis-open.org/sarif/sarif/v2.1.0/errata01/os/schemas/sarif-schema-2.1.0.json"
      version = "2.1.0"
      runs = @(
        [ordered] @{
          defaultSourceLanguage = 'powershell'
          columnKind = 'utf16CodeUnits'
          tool = [ordered] @{
            driver = [ordered] @{
              name = $module.Name
              version = "$($module.Version)"
              fullName = "$($module.Name) $($module.Version)"
              informationUri = $module.ProjectUri
              guid = $module.Guid
              organization = $module.CompanyName
              shortDescription = $moduleDesc
              rules = @()
            }
            properties = @{
              tags = @($module.Tags | Select-Object -Unique)
            }
          }
          automationDetails = @{
            id = $Category
            description = @{
              text = "$($module.Name) $($module.Version) run with category '$Category'"
            }
          }
          runAggregates = @(
            @{
              id = "$($module.Name)/language:powershell"
              description = $moduleDesc
            }
          )
          conversion = @{
            tool = @{
              driver = @{
                name = $MyInvocation.MyCommand.Name
                version = $version
                fullName = "$($MyInvocation.MyCommand.Name) $version"
              }
            }
          }
          artifacts = @()
          results = @()
        }
      )
    }

    $rules = [Collections.ArrayList]::new()
    $ruleIds = @{}
    $artifacts = [Collections.ArrayList]::new()
    $artifactPaths = @{}
    $results = [Collections.ArrayList]::new()

    function Get-RuleId($RuleName) {
      if ($null -eq $ruleIds.$RuleName) {
        $ruleIds[$RuleName] = $true
        [void] $rules.Add(
          [ordered] @{
            id = $RuleName
            name = Format-RuleName $RuleName
            helpUri = Format-RuleHelpUrl $RuleName
          }
        )
      }
      return $RuleName
    }

    function Get-ArtifactLocation($ScriptPath) {
      $index = $artifactPaths.$ScriptPath
      if ($null -eq $index) {
        $index = $artifacts.Count
        [void] $artifacts.Add(
          @{
            mimeType = 'text/x-powershell'
            roles = @('analysisTarget')
            location = @{
              index = $index
              uri = Format-PathUri $ScriptPath
            }
          }
        )
        $artifactPaths.$ScriptPath = $index
      }
      return @{
        index = $index
        uri = Format-PathUri $ScriptPath
      }
    }

    $severities = @('note', 'warning', 'error', 'error')

    foreach ($r in $records) {
      $result = [ordered] @{
        ruleId = Get-RuleId $r.RuleName
        message = @{
          text = $r.Message
        }
        level = $severities[[uint] $r.Severity]
        locations = @(
          @{
            physicalLocation = @{
              artifactLocation = Get-ArtifactLocation $r.ScriptPath
              region = @{
                startLine = $r.Extent.StartLineNumber
                startColumn = $r.Extent.StartColumnNumber
                endLine = $r.Extent.EndLineNumber
                endColumn = $r.Extent.EndColumnNumber
              }
            }
          }
        )
      }
      if ($r.Suppression) {
        $result.suppressions = @(
          $r.Suppression | foreach {
            $supp = @{
              kind = 'inSource'
              location = @{
                physicalLocation = @{
                  artifactLocation = Get-ArtifactLocation $r.ScriptPath
                  region = @{
                    startLine = $_.StartAttributeLine
                  }
                }
              }
            }
            if ($_.Justification) {
              $supp.justification = $_.Justification
            }
            if ($_.Scope -or $_.Target) {
              $supp.properties = @{}
              if ($_.Scope) {
                $supp.properties.scope = $_.Scope
              }
              if ($_.Target) {
                $supp.properties.target = $_.Target
              }
            }
            return $supp
          }
        )
      }
      [void] $results.Add($result)
    }

    $problemSeverities = @('recommendation', 'warning', 'error', 'error')
    $securitySeverities = @{
      PSAvoidUsingUsernameAndPasswordParams = 7.0
      PSAvoidUsingAllowUnencryptedAuthentication = 5.5
      PSAvoidUsingBrokenHashAlgorithms = 5.0
      PSAvoidUsingComputerNameHardcoded = 7.0
      PSAvoidUsingConvertToSecureStringWithPlainText = 7.0
      PSAvoidUsingInvokeExpression = 6.0
      PSAvoidUsingPlainTextForPassword = 7.0
      PSUsePSCredentialType = 6.0
    }

    foreach ($rule in $rules) {
      $info = Get-ScriptAnalyzerRule -Name $rule.id -ErrorAction Ignore
      if ($null -ne $info) {
        $helpUrl = Format-RuleHelpUrl $rule.id
        $helpText = @(
          $info.Description
          ""
          "See:"
          "* [$($rule.id) rule help]($helpUrl)"
          "* [$($module.Name) module help]($helpUrlBase/overview)"
          "* [$($module.Name) module]($($module.ProjectUri))"
        ) -join "`n"
        $rule.name = $info.CommonName
        $rule.help = @{
          markdown = $helpText
          text = $helpText
        }
        # https://docs.github.com/en/code-security/code-scanning/integrating-with-code-scanning/sarif-support-for-code-scanning#reportingdescriptor-object
        $rule.properties = @{
          tags = @($info.SourceName, "$($info.SourceType)")
          'problem.severity' = $problemSeverities[[uint] $info.Severity]
        }
        if ($null -ne $securitySeverities[$rule.id]) {
          $rule.properties.'security-severity' = '{0:0.0}' -f $securitySeverities[$rule.id]
        }
      }
    }

    $sarif.runs[0].tool.driver.rules = $rules
    $sarif.runs[0].artifacts = $artifacts
    $sarif.runs[0].results = $results

    return $sarif
  }
}

function Invoke-ScriptAnalyzerAction {
  param(
    [Parameter(Mandatory)]
    [string] $OutputPath
  )

  function Get-Config($Str) {
    $tempFile = "./$(New-Guid).psd1"
    Set-Content -LiteralPath $tempFile -Value $Str
    $value = Import-PowerShellDataFile -LiteralPath $tempFile
    Remove-Item -LiteralPath $tempFile
    return $value ?? @{}
  }

  if ($null -eq (Get-Module -ListAvailable -Name 'PSScriptAnalyzer')) {
    Install-Module -Name 'PSScriptAnalyzer' -Force
  }

  $analysis = Get-Config $env:analysis
  $conversion = Get-Config $env:conversion
  $serialization = Get-Config $env:serialization

  @{
    analysis = $analysis
    conversion = $conversion
    serialization = $serialization
  } | ConvertTo-Json -Depth 4 | Write-Host

  Invoke-ScriptAnalyzer @analysis |
    ConvertTo-Sarif @conversion |
    ConvertTo-Json @serialization -Depth 16 |
    Set-Content $OutputPath
}

if ($env:TERM_PROGRAM -eq 'vscode') {
  $env:analysis = "@{
    Path = './'
    Recurse = `$true
    IncludeSuppressed = `$true
    Settings = './.vscode/PSScriptAnalyzerSettings.psd1'
  }"
  $env:conversion = "@{
    Category = 'PSScriptAnalyzerEx/language:powershell'
  }"
  $env:serialization = "@{
    Compress = `$false
  }"
  Invoke-ScriptAnalyzerAction -OutputPath 'ps3.sarif'
  return
  $password = ConvertTo-SecureString -String 'MyPassword' | Invoke-Expression
}
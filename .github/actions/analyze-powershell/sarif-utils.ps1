function ConvertTo-Sarif
{
  [CmdletBinding()]
  [OutputType([pscustomobject])]
  param(
    [Parameter(Mandatory, ValueFromPipeline)]
    [psobject[]] $InputObject
  )
  begin {
    $records = [Collections.ArrayList]::new()
  }
  process {
    [void] $records.Add($InputObject)
  }
  end {
    $records = $records | Sort-Object -Property ScriptPath, Line, Severity, RuleName, Message

    function Format-RuleName($RuleName)
    {
      $RuleName `
        -replace '^PS', '' `
        -creplace '(?<!^)([A-Z])(?=[a-z])', ' $1' `
        -creplace '([a-z])([A-Z])', '$1 $2' `
        -replace '\A\s+|\s+\z', ''
    }

    $module = Get-Module PSScriptAnalyzer

    $sarif = [pscustomobject] @{
      #'$schema' = "https://schemastore.azurewebsites.net/schemas/json/sarif-2.1.0-rtm.6.json"
      '$schema' = "https://docs.oasis-open.org/sarif/sarif/v2.1.0/errata01/os/schemas/sarif-schema-2.1.0.json"
      version = "2.1.0"
      runs = @(
        [pscustomobject] @{
          defaultSourceLanguage = 'powershell'
          tool = [pscustomobject] @{
            driver = [pscustomobject] @{
              name = $module.Name
              version = "$($module.Version)"
              fullName = "$($module.Name) $($module.Version)"
              informationUri = $module.ProjectUri
              guid = $module.Guid
              organization = $module.CompanyName
              shortDescription = [pscustomobject] @{
                text = $module.Description
              }
              fullDescription = [pscustomobject] @{
                text = $module.Description
              }
              rules = @()
            }
            properties = [pscustomobject] @{
              tags = $module.Tags | Select-Object -Unique
            }
          }
          conversion = [pscustomobject] @{
            tool = [pscustomobject] @{
              driver = [pscustomobject] @{
                name = 'ConvertTo-Sarif'
                version = "0.1.0"
                fullName = "ConvertTo-Sarif 0.1.0"
              }
            }
          }
          artifacts = @()
          results = @()
        }
      )
    }

    $severities = @('note', 'warning', 'error', 'error')

    $rules = [Collections.ArrayList]::new()
    $ruleIds = @{}
    $artifacts = [Collections.ArrayList]::new()
    $artifactPaths = @{}
    $results = [Collections.ArrayList]::new()

    function Get-RuleId($RuleName) {
      if ($null -eq $ruleIds.$RuleName) {
        $ruleIds[$RuleName] = $true
        $helpId = $RuleName.Substring(2).ToLowerInvariant()
        [void] $rules.Add(
          [pscustomobject] @{
            id = $RuleName
            name = Format-RuleName $RuleName
            helpUri = "https://learn.microsoft.com/powershell/utility-modules/psscriptanalyzer/rules/$helpId"
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
          [pscustomobject] @{
            location = [pscustomobject] @{
              index = $index
              uri = [Uri]::new((Resolve-Path $ScriptPath)).AbsoluteUri
              mimeType = 'text/x-powershell'
              roles = @('analysisTarget')
            }
          }
        )
        $artifactPaths.$ScriptPath = $index
      }
      return [pscustomobject] @{
        index = $index
        # uri = "file:///C:/Docs/Projects/Json/.github/actions/setup/calc-dotnet-version.ps1"
      }
    }

    foreach ($r in $records) {
      $result = [pscustomobject] @{
        ruleId = Get-RuleId $r.RuleName
        message = [pscustomobject] @{
          text = $r.Message
        }
        level = $severities[[uint] $r.Severity]
        locations = @(
          [pscustomobject] @{
            physicalLocation = [pscustomobject] @{
              artifactLocation = Get-ArtifactLocation $r.ScriptPath
              region = [pscustomobject] @{
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
            [pscustomobject] @{
              kind = 'inSource'
              justification = $_.Justification
              location = [pscustomobject] @{
                physicalLocation = [pscustomobject] @{
                  artifactLocation = Get-ArtifactLocation $r.ScriptPath
                  region = [pscustomobject] @{
                    startLine = $_.StartAttributeLine
                    startColumn = $_.StartOffset
                    endColumn = $_.EndOffset
                  }
                }
              }
            }
          }
        )
      }
      [void] $results.Add($result)
    }

    $sarif.runs[0].tool.driver.rules = $rules
    $sarif.runs[0].artifacts = $artifacts
    $sarif.runs[0].results = $results

    return $sarif
  }
}

# Invoke-ScriptAnalyzer . -Recurse | ConvertTo-Sarif | ConvertTo-Json -Depth 10 | Set-Content ps2.sarif
# Invoke-ScriptAnalyzer -Path './' -Recurse -IncludeSuppressed |
#   ConvertTo-Sarif | ConvertTo-Json -Depth 10 | Set-Content ps3.sarif
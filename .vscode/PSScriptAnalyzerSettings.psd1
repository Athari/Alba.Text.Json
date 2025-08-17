@{
  Severity = @(
    'Error'
    'Warning'
    'Information'
  )
  ExcludeRules = @(
    'PSAlignAssignmentStatement'
    'PSAvoidAssignmentToAutomaticVariable'
    'AvoidSemicolonsAsLineTerminators'
    'PSAvoidUsingWriteHost'
  )
  Rules = @{
    PSAvoidLongLines = @{
      Enable = $true
      MaximumLineLength = 120
    }
    PSAvoidUsingCmdletAliases = @{
      Enable = $true
      AllowList = 'foreach', 'group', 'select', 'sort', 'tee', 'where'
    }
    PSAvoidUsingPositionalParameters = @{
      Enable = $true
      CommandAllowList = 'Join-Path'
    }
    PSUseCompatibleSyntax = @{
      Enable = $true
      TargetVersions = '7.4'
    }
    PSUseConsistentIndentation = @{
      Enable = $true
      IndentationSize = 2
      PipelineIndentation = 'IncreaseIndentationForFirstPipeline'
      Kind = 'space'
    }
    PSUseConsistentWhitespace = @{
      Enable = $true
    }
    PSUseCorrectCasing = @{
      Enable = $true
    }
    PSUseSingularNouns = @{
      Enable = $true
      NounAllowList = 'Data', 'Windows'
    }
  }
}
param(
  [string] $dotnetVersion,
  [string] $runtime = 'sdk'
)

$versions = $dotnetVersion -split '\s+'

$runtimeName = @{ dotnet = 'NETCore'; aspnetcore = 'AspNetCore'; windowsdesktop = 'WindowsDesktop' }[$runtime]
$runtimeName = "Microsoft.$runtimeName.App"
$runtimeListArg = $runtime -eq 'sdk' ? '--list-sdks' : '--list-runtimes'

$installedVersions = (& dotnet $runtimeListArg) -split "[`r`n]+" |
  where { $runtime -eq 'sdk' -or $_.Contains($runtimeName) } |
  Select-String '(\d+(\.\d){2,4})' | foreach { $_.Matches.Groups[1].Value }
$installedMajorVersions = $installedVersions |
  Select-String '\d+' | foreach { $_.Matches.Value }
$missingVersions = $versions |
  where { $installedMajorVersions -notcontains $_ } |
  foreach { "$_.0.x" }

Write-Host "$($runtime -eq 'sdk' ? '.NET SDK' : $runtimeName) versions:
  requested: $($versions -join ', ')
  installed: $($installedVersions -join ', ')
  missing: $($missingVersions -join ', ')"

@{
  'dotnet-version' = $missingVersions -join "`n"
} | ConvertTo-GitHubOutput >> $env:GITHUB_OUTPUT
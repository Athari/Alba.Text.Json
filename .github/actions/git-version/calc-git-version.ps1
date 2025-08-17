[CmdletBinding()]
param(
  [Parameter(Mandatory)] [psobject] $gh
)

function Format-Ref($str) {
  return ($str.ToLower() -replace '/', '-') -replace '[^a-z0-9-]', ''
}

$releaseUrl = ""
$releaseMeta = ""
$prerelease = "false"

if ($gh.ref -like "refs/tags/v*") {
  $versionSuffix = ""
  $releaseUrl = "$($gh.server_url)/$($gh.repository)/releases/tag/$($gh.ref_name)"
  if ($gh.ref_name -like "*-*") {
    $versionSuffix = $gh.ref_name -ireplace '^[^-]+-(.*)$', '$1'
    $prerelease = "true"
  }
} elseif ($gh.event_name -eq 'pull_request') {
  $versionSuffix = "pr-$($gh.event.number)-$(Format-Ref $gh.base_ref)-$(Format-Ref $gh.head_ref)"
  $releaseMeta = "$($gh.event_name) by @$($gh.actor)"
} elseif ($gh.event_name -eq 'push' -and $gh.ref_name -ne 'main') {
  $versionSuffix = "f-$(Format-Ref $gh.ref_name)"
} else {
  $versionSuffix = "preview"
}

if ($versionSuffix -ne "") {
  $versionSuffix = "$($versionSuffix).$($gh.run_number)"
}

$releaseNotes = "See $releaseUrl for more information.`n$releaseMeta".Trim()

@{
  'version-suffix' = $versionSuffix
  'release-url' = $releaseUrl
  'release-meta' = $releaseMeta
  'release-notes' = $releaseNotes
  'prerelease' = $prerelease
} | ConvertTo-GitHubOutput >> $env:GITHUB_OUTPUT
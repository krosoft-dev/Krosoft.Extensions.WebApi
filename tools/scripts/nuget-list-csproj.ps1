$template = Get-Content -Path ./tools/scripts/nuget-template-pipeline.yml

(Get-ChildItem ./src -Filter *.csproj -Recurse) | ForEach-Object {
  Write-Host =========== $_.BaseName
  $fileName = "./tools/azure-pipelines/nuget-$($_.BaseName)-pipeline.yml"
  New-Item $fileName -Force  
 
  $array = @($_.BaseName)

  Get-Content $_.FullName `
  | Find "<ProjectReference Include=" `
  | ForEach-Object { $_ -replace '<ProjectReference Include=', '' -replace '/>', '' }  `
  | Sort-Object -Unique  `
  | Split-Path -Leaf   `
  | ForEach-Object { $array += $_.replace('.csproj"', '') } 
  

  $current = $template
  $current = $current.replace('KRO_PACKAGE_NAME', $_.BaseName)
 
  $sb = [System.Text.StringBuilder]::new()
  $array | ForEach-Object { [void]$sb.Append( '     - ' )
    [void]$sb.AppendLine( '"' + $PSItem.replace(' ', '') + '"' ) }

 
  $current = $current.replace('KRO_PACKAGES', $sb.ToString() )
  
  Set-Content $fileName $current
  
  Write-Host ===========
  Write-Host  
}   
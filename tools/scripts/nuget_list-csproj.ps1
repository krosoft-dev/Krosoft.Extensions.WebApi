$rootSrc = "./src"
$template = Get-Content -Path ./tools/scripts/nuget-template-pipeline.yml

function ExtractReferences($rootSrc, $context) { 
  Get-Content $context.FullName `
| Find "<ProjectReference Include=" `
| ForEach-Object { $_ -replace '<ProjectReference Include=', '' -replace '/>', '' }  `
| Sort-Object -Unique  `
| ForEach-Object {
    $csproj = $PSItem.trim().replace("..", '').replace('"', '') 
    $outputPath = Join-Path $rootSrc $csproj 
    $file = Get-Item $outputPath 
    $global:array += $file.BaseName
    ExtractReferences $rootSrc $file   
  }
}

(Get-ChildItem $rootSrc -Filter *.csproj -Recurse) | ForEach-Object {  

  $fileName = "./tools/devops/nuget-$($_.BaseName)-pipeline.yml"
  New-Item $fileName -Force  
       
  $global:array = @($_.BaseName) 

  ExtractReferences $rootSrc $_   

  $current = $template
  $current = $current.replace('KRO_PACKAGE_NAME', $_.BaseName)
   
  $sb = [System.Text.StringBuilder]::new()
  $array | Sort-Object | Get-Unique | ForEach-Object { [void]$sb.Append( '     - ' )
    [void]$sb.AppendLine( '"' + $PSItem.replace(' ', '') + '"' ) }
  
   
  $current = $current.replace('KRO_PACKAGES', $sb.ToString() )
    
  Set-Content $fileName $current 
}  
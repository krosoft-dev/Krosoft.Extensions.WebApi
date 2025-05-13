 
function CleanDotNetFolders {
    param (
        [string]$rootPath
    )
    
    Write-Host "Nettoyage des dossiers bin et obj pour le répertoire : $rootPath"    
 
    Get-ChildItem -Path $rootPath -Directory -Recurse |
        Where-Object { $_.FullName -match "\\(bin|obj)$" } |
        ForEach-Object {
            Write-Host "   Suppression de: $($_.FullName)"
            Remove-Item $_.FullName -Force -Recurse -ErrorAction SilentlyContinue
        }
}

$path = Get-Location
CleanDotNetFolders -rootPath $path 
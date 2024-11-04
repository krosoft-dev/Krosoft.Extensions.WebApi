function AzDevOpsLogin() {   
    Write-Host "=========================================="
    Write-Host "AzDevOpsLogin"  
    Write-Host "==========================================" 
    az login --allow-no-subscriptions 
    Write-Host  
}

function AzDevOpsInitCli() {
    Write-Host "=========================================="
    Write-Host "AzDevOpsInitCli :  $global:url > $global:projet"  
    Write-Host "==========================================" 
    az extension add --name azure-devops 
    az devops configure -d organization=$global:url project=$global:projet
    az devops configure -l  
    Write-Host 
}

function AzDevOpsPipelinesList() { 
    Write-Host "=========================================="
    Write-Host "AzDevOpsPipelinesList"
    Write-Host "=========================================="     
    az pipelines build definition list -o table  
    Write-Host  
}
 

$global:url = "https://krosoft.visualstudio.com" 
$global:projet = "Krosoft.Extensions"
$global:serviceConnection = "krosoft-dev"
$global:repository = "https://github.com/krosoft-dev/Krosoft.Extensions"
$global:queue = "Azure Pipelines"

AzDevOpsLogin
AzDevOpsInitCli $global:url $global:projet
AzDevOpsPipelinesList 


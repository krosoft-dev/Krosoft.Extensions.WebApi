$sourcesDirectory = "C:\Dev\Krosoft.Extensions\"
$path = "C:\Dev\Krosoft.Extensions\tests\Krosoft.Extensions.Validations.Tests\Krosoft.Extensions.Validations.Tests.csproj"
$resultPath = "temp"
$configuration = "Release"


# dotnet new console -n testt -f net8.0
# cd ./testt
# dotnet add package ReportGenerator --version 5.2.0
 


# dotnet build $path --configuration $configuration

# dotnet test $path  `
#     --logger trx `
#     --results-directory $resultPath  `
#     --no-build  `
#     --configuration $configuration  `
#     -p:CollectCoverage=true  `
#     -p:Platform=AnyCPU  `
#     -p:CoverletOutputFormat="cobertura"

# dotnet $(UserProfile)\.nuget\packages\reportgenerator\5.2.0\tools\net6.0\ReportGenerator.dll `
#     -reports:$sourcesDirectory/**/coverage.cobertura.xml  `
#     -targetdir:$sourcesDirectory/CodeCoverage `
#     "-reporttypes:HtmlInline_AzurePipelines;Cobertura" `
#     -filefilters: "-$sourcesDirectory/**/Migrations/*.cs;-$sourcesDirectory/samples/*.*" `
#     -title:"Hello" 
 

 
 
 
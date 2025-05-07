rmdir /s /q analytics
mkdir analytics\ClientApi
mkdir analytics\ClientBackground

dotnet publish ..\src\PcAnalytics.ClientApi -c Release -o analytics\ClientApi
dotnet publish ..\src\PcAnalytics.ClientBackground -c Release -o analytics\ClientBackground

echo @echo off > published\run.bat
echo start "" /min ClientApi\PcAnalytics.ClientApi.exe >> analytics\run.bat
echo start "" /min ClientBackground\PcAnalytics.ClientBackground.exe >> analytics\run.bat

if exist configs.json (
    copy /Y configs.json analytics\configs.json
) else (
    > published\configs.json (
        echo {
        echo   "storagePath": "",
        echo   "onlineEndpoint": ""
        echo }
    )
)

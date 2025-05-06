rmdir /s /q published
mkdir published\ClientApi
mkdir published\ClientBackground

dotnet publish ..\src\PcAnalytics.ClientApi -c Release -o published\ClientApi
dotnet publish ..\src\PcAnalytics.ClientBackground -c Release -o published\ClientBackground

echo @echo off > published\run.bat
echo start "" /min ClientApi\PcAnalytics.ClientApi.exe >> published\run.bat
echo start "" /min ClientBackground\PcAnalytics.ClientBackground.exe >> published\run.bat

if exist configs.json (
    copy /Y configs.json published\configs.json
) else (
    > published\configs.json (
        echo {
        echo   "storagePath": "",
        echo   "onlineEndpoint": ""
        echo }
    )
)

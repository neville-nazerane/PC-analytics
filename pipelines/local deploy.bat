rmdir /s /q analytics
mkdir analytics\ClientApi
mkdir analytics\ClientBackground

dotnet publish ..\src\PcAnalytics.ClientApi -c Release -o analytics\ClientApi
dotnet publish ..\src\PcAnalytics.ClientBackground -c Release -o analytics\ClientBackground

echo taskkill /f /im PcAnalytics.ClientApi.exe >nul 2>nul >> analytics\run.bat
echo taskkill /f /im PcAnalytics.ClientBackground.exe >nul 2>nul >> analytics\run.bat
echo start "" /min ClientApi\PcAnalytics.ClientApi.exe --urls http://localhost:6060 >> analytics\run.bat
echo start "" /min ClientBackground\PcAnalytics.ClientBackground.exe >> analytics\run.bat


if exist configs.json (
    copy /Y configs.json analytics\configs.json
) else (
    > published\configs.json (
        echo {
        echo   "storagePath": "",
        echo   "onlineEndpoint": "",
        echo   "localEndpoint": ""
        echo }
    )
)


powershell -Command "$s = [System.IO.Path]::GetFullPath('analytics\\run.bat'); $startup = [Environment]::GetFolderPath('Startup'); $w = New-Object -ComObject WScript.Shell; $sc = $w.CreateShortcut(\"$startup\\run.lnk\"); $sc.TargetPath = $s; $sc.Save()"

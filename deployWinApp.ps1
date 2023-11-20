$FolderToCreate = "C:\\Program Files\\HalcyonManager"
if (!(Test-Path $FolderToCreate -PathType Container)) {
    New-Item -ItemType Directory -Force -Path $FolderToCreate
}

$FolderPath = "C:\\Program Files\\HalcyonManager"
Get-ChildItem -Path $FolderPath -Recurse -Force | Remove-Item -Force -Recurse -ErrorAction SilentlyContinue


cd "D:\\HalcyonSoft.visualstudio.com\\HalcyonManager\\HalcyonManager"

dotnet publish -c release  -f net7.0-windows10.0.19041 -p RuntimeIdentifierOverride=win10-x64 -o "C:\\Program Files\\HalcyonManager"



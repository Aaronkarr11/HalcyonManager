$FolderToCreate = "C:\\Program Files\\HalcyonManager"
if (!(Test-Path $FolderToCreate -PathType Container)) {
    New-Item -ItemType Directory -Force -Path $FolderToCreate
}

$FolderPath = "C:\\Program Files\\HalcyonManager"
Get-ChildItem -Path $FolderPath -Recurse -Force | Remove-Item -Force -Recurse -ErrorAction SilentlyContinue


cd "D:\\HalcyonSoft\\HalcyonManager\\HalcyonManager"

dotnet publish -f net8.0-windows10.0.19041.0 -c Release -p:RuntimeIdentifierOverride=win10-x64 -p:WindowsPackageType=None -o "C:\\Program Files\\HalcyonManager"

pause



@echo Converting...
rem Following command is used for converter before 1.0.8.0
rem DesktopAppConverter.exe -Installer "GreaterFileShare.Installer\GreaterFileShare.Installer\Express\SingleImage\DiskImages\DISK1\setup.exe" -Destination UWPDesktopBridge  -Publisher "CN=4D2E4444-E4BB-4498-B89B-C3BC2D2D50F2" -Version 1.0.3.0 -InstallerArguments "/S /v/qn"  -AppExecutable ".\bin\GreaterFileShare.Hosts.WPF.exe" -PackageDisplayName "Greater File Share" -PackagePublisherDisplayName "Waynebaby Presents"  -PackageName "57081WaynebabyPresents.GreaterFileShare" -AppId "App" -MakeAppx


rem Following command is used for converter after 1.0.9.0 
DesktopAppConverter.exe -Installer "D:\GitHub\GreaterFileShare\ReleaseFolder" -Destination %cd%\UWPDesktopBridge  -Publisher "CN=4D2E4444-E4BB-4498-B89B-C3BC2D2D50F2" -Version 1.0.5.0   -AppExecutable "GreaterFileShare.Hosts.WPF.exe" -PackageDisplayName "Greater File Share" -PackagePublisherDisplayName "Waynebaby Presents"  -PackageName "57081WaynebabyPresents.GreaterFileShare" -AppId "App" -MakeAppx -PackageArch x86

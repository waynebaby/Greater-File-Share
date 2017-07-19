
call 0.Configurations.bat

@echo Converting...
@rem Following command is used for converter after 1.0.9.0 
DesktopAppConverter.exe -Installer "%Installer%" -Destination %cd%\UWPDesktopBridge  -Publisher "%Publisher%" -Version "%Version%"   -AppExecutable "%AppExecutable%" -PackageDisplayName "%PackageDisplayName%" -PackagePublisherDisplayName "%PackagePublisherDisplayName%"  -PackageName "%PackageName%" -AppId "%AppId%"  -PackageArch %PackageArch% -MakeAppx
@echo Converted...

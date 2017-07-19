@rem Converterparameters
set Installer=D:\GitHub\GreaterFileShare\ReleaseFolder
set PackageName=57081WaynebabyPresents.GreaterFileShare
set Publisher=CN=4D2E4444-E4BB-4498-B89B-C3BC2D2D50F2
set Version=1.0.11.0
set AppExecutable=GreaterFileShare.Hosts.WPF.exe
set PackageDisplayName=GreaterFileShare
set PackagePublisherDisplayName=WaynebabyPresents
set AppId=GreaterFileShareApp
set PackageArch=x86


@rem Extract and Change Parameters
set RootWorkingPath=UWPDesktopBridge\%PackageName%
set PackageAppxPath=%RootWorkingPath%\%PackageName%.appx
set PackageRoot=%RootWorkingPath%\ExtractedPackageFiles
@rem set ForTestingSignOrEmpty=ForTestingSign
set ForTestingSignOrEmpty=
set StringResourceSource=GreaterFileShare.UWP\Strings

@rem Tools Path
set MakePriPath=%ProgramFiles(x86)%\Windows Kits\10\bin\10.0.15063.0\x64\makepri.exe
set MakeAppxPath=%ProgramFiles(x86)%\Windows Kits\10\bin\10.0.15063.0\x64\makeappx.exe
set SignToolPath=%ProgramFiles(x86)%\Windows Kits\10\bin\10.0.15063.0\x64\signtool.exe


set SigningPFXPath=GreaterFileShare.UWP\GreaterFileShare.UWP_StoreKey.pfx
set SigningForTestingPFXPath=AppxTestRootAgency.pfx

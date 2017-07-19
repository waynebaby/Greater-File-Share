call 0.Configurations.bat


@echo Cleaning...
@rem Incase There are gabage from last running,  delelte this dir.
RD "%packageRoot%" /S /Q 
@echo Cleaning finished.

@echo Extracting...
@rem Extract the converted packages
@rem powershell.exe -nologo -noprofile -command "& { Add-Type -A 'System.IO.Compression.FileSystem'; [IO.Compression.ZipFile]::ExtractToDirectory('%PackageAppxPath%', '%packageRoot%'); }"
"%makeappxPath%" unpack /l /p  "%PackageAppxPath%" /d  "%packageRoot%"
@echo Extracted...

@rem Copy Resource files here. This app supports en-US_zh-Hans_zh-Hant
xcopy %stringResourceSource% %RootWorkingPath%\ExtractedPackageFiles\Strings /s   /y  /i
"%makepriPath%" createconfig  /cf "%RootWorkingPath%\resources.pri.xml"  /dq en-US_zh-Hans_zh-Hant /pv 10.0  /o
"%makepriPath%" new /pr "%packageRoot%" /cf "%RootWorkingPath%\resources.pri.xml" /of "%packageRoot%\resources.pri" /mf AppX /o


@rem make more modifies. the logic is written in BuildMe\bin\BuildMe.exe. 
BuildMe\bin\BuildMe.exe "%packageRoot%" %ForTestingSignOrEmpty%

@rem if there are gabage files blocked MakeAppx steps, we can deleted them here.

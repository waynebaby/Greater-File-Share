

set  configPath=UWPDesktopBridge\57081WaynebabyPresents.GreaterFileShare
set  rp=UWPDesktopBridge\57081WaynebabyPresents.GreaterFileShare\ExtractedPackageFiles

rem Incase There are gabage from last running,  delelte this dir.
RD UWPDesktopBridge\57081WaynebabyPresents.GreaterFileShare\ExtractedPackageFiles /S /Q 



rem Extract the converted packages
powershell.exe -nologo -noprofile -command "& { Add-Type -A 'System.IO.Compression.FileSystem'; [IO.Compression.ZipFile]::ExtractToDirectory('%configPath%\57081WaynebabyPresents.GreaterFileShare.appx', '%rp%'); }"


rem Copy Resource files here
xcopy GreaterFileShare.UWP\Strings %configPath%\ExtractedPackageFiles\Strings /s   /y  /i




rem dump all  file  into xml files
rem for /f "delims=" %%a in ('dir/b /s %rp%\*.pri') do "%ProgramFiles(x86)%\Windows Kits\10\bin\x64\makepri" dump  /if "%%a"   /of "%%a.dump" 

"%ProgramFiles(x86)%\Windows Kits\10\bin\10.0.15063.0\x64\makepri" createconfig  /cf "%configPath%\resources.pri.xml"  /dq en-US_zh-Hans_zh-Hant /pv 10.0  /o

"%ProgramFiles(x86)%\Windows Kits\10\bin\10.0.15063.0\x64\makepri" new /pr "%rp%" /cf "%configPath%\resources.pri.xml" /of "%rp%\resources.pri" /mf AppX /o

rem dump all  file  into xml files
rem for /f "delims=" %%a in ('dir/b /s %rp%\*.pri') do "%ProgramFiles(x86)%\Windows Kits\10\bin\x64\makepri" dump  /if "%%a"    /of "%%a2.dump" 



rem make more modifies. the logic is written in BuildMe\bin\BuildMe.exe. 
BuildMe\bin\BuildMe.exe UWPDesktopBridge\57081WaynebabyPresents.GreaterFileShare\ExtractedPackageFiles\



rem if there are gabage files blocked MakeAppx steps, we can deleted them here.
rem RD UWPDesktopBridge\57081WaynebabyPresents.GreaterFileShare\ExtractedPackageFiles\VFS\SystemX64\config\systemprofile\AppData\LocalLow\Microsoft\CryptnetUrlCache /S /Q 
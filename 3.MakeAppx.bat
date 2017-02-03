

rem makeAppx

cd UWPDesktopBridge\57081WaynebabyPresents.GreaterFileShare\ExtractedPackageFiles

"%ProgramFiles(x86)%\Windows Kits\10\bin\x64\makeAppx" pack  /d "." /p "..\57081WaynebabyPresents.GreaterFileShare.appx"  /l

cd ..\..\..


rem makeAppx

cd UWPDesktopBridge\57081WaynebabyPresents.GreaterFileShare\ExtractedPackageFiles

del "..\57081WaynebabyPresents.GreaterFileShare.appx"
"%ProgramFiles(x86)%\Windows Kits\10\bin\10.0.15063.0\x64\makeappx" pack  /d "." /p "..\57081WaynebabyPresents.GreaterFileShare.appx"  /l

cd ..\..\..
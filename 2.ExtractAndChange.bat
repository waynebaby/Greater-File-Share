
RD UWPDesktopBridge\57081WaynebabyPresents.GreaterFileShare\ExtractedPackageFiles /S /Q 
powershell.exe -nologo -noprofile -command "& { Add-Type -A 'System.IO.Compression.FileSystem'; [IO.Compression.ZipFile]::ExtractToDirectory('UWPDesktopBridge\57081WaynebabyPresents.GreaterFileShare\57081WaynebabyPresents.GreaterFileShare.appx', 'UWPDesktopBridge\57081WaynebabyPresents.GreaterFileShare\ExtractedPackageFiles'); }"



BuildMe\bin\BuildMe.exe UWPDesktopBridge\57081WaynebabyPresents.GreaterFileShare\ExtractedPackageFiles\AppxManifest.xml

RD UWPDesktopBridge\57081WaynebabyPresents.GreaterFileShare\ExtractedPackageFiles\VFS\SystemX64\config\systemprofile\AppData\LocalLow\Microsoft\CryptnetUrlCache /S /Q 
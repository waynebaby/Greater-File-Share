call 0.Configurations.bat

del %PackageAppxPath%
cd %RootWorkingPath%\ExtractedPackageFiles\


"%MakeAppxPath%" pack  /d "." /p "..\%PackageName%.appx"  /l

cd ..\..\..
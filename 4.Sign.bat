call 0.Configurations.bat

if "%ForTestingSignOrEmpty%"=="ForTestingSign"  "%SignToolPath%" sign -f "%SigningForTestingPFXPath%" -fd SHA256 -v "%PackageAppxPath%" 

if NOT "%ForTestingSignOrEmpty%"=="ForTestingSign"   "%SignToolPath%" sign -f "%SigningPFXPath%" -fd SHA256 -v "%PackageAppxPath%"

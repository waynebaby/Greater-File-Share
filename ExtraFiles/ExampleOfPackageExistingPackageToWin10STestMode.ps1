
param($packagePath)

Add-Type -A 'System.IO';
Add-Type -A 'System.IO.Compression.FileSystem'; 
Add-Type -A 'System.Xml.Linq'; 

$pfxPath='..\AppxTestRootAgency.pfx'
$folderPath=[IO.Path]::GetDirectoryName($packagePath);
$packageName=[IO.Path]::GetFileNameWithoutExtension($packagePath);
$packageExtension=[IO.Path]::GetExtension($packagePath);
$extractedPath=[IO.Path]::Combine($folderPath,'extractedPath');

if([IO.Directory]::Exists($extractedPath)){
    Write-Host "Deleting old trash"
    rd $extractedPath -Recurse 
}


Write-Host "Extraction files from package $packageName"
[IO.Compression.ZipFile]::ExtractToDirectory($packagePath, $extractedPath); 
$xmlPath=[IO.Path]::Combine($extractedPath,'AppxManifest.xml');

$doc=[Xml.Linq.XDocument]::Load($xmlPath);

foreach($element in $doc.Descendants())
{
   
    if($element.Name.LocalName -eq 'Identity')
    {
        Write-Host "Found element: $element"  
        foreach($attribute in $element.Attributes())
        {
            if($attribute.Name.LocalName -eq "Publisher")
            {
                Write-Host "Found attribute: $attribute"  
                $attribute.value='CN=Appx Test Root Agency Ex';
            }
        }
    }
}
$doc.Save($xmlPath);





$MakePriPath="${Env:ProgramFiles(x86)}\Windows Kits\10\bin\10.0.15063.0\x64\makepri.exe"
$MakeAppxPath="${Env:ProgramFiles(x86)}\Windows Kits\10\bin\10.0.15063.0\x64\makeappx.exe"
$SignToolPath="${Env:ProgramFiles(x86)}\Windows Kits\10\bin\10.0.15063.0\x64\signtool.exe"
$targetBakFile ="$packagePath.bak"
while([IO.File]::Exists($targetBakFile))
{
    $targetBakFile="$targetBakFile.bak"
}


Write-Host "Renaming   $packagePath to $targetBakFile "
ren  $packagePath $targetBakFile

Write-Host "Packaging"
& $MakeAppxPath pack  /d $extractedPath  /p  $packagePath  /l
Write-Host "Signing"
& $SignToolPath sign -f  $pfxPath -fd SHA256 -v $packagePath

Write-Host "All Done"
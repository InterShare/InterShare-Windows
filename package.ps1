param (
    [string]$Version
)

if (-not $Version) {
    Write-Error "Error: Version argument is required. Usage: .\package.ps1 '0.1.1-alpha.6'"
    exit 1
}

dotnet publish -c Release -p:Platform=x64 -p:Version=$Version --self-contained true -p:WindowsAppSDKSelfContained=true -o .\publish
vpk pack -u InterShare -v $Version -p .\publish -e InterShare.exe

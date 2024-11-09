dotnet publish -c Release -p:Platform=x64 -p:Version=0.1.0-alpha.1 --self-contained true -p:WindowsAppSDKSelfContained=true -o .\publish
vpk pack -u InterShare -v 0.1.0-alpha.1 -p .\publish -e InterShare.exe

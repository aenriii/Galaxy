wget https://download.visualstudio.microsoft.com/download/pr/6788a5a5-1879-4095-948d-72c7fbdf350f/c996151548ec9f24d553817db64c3577/dotnet-sdk-5.0.402-linux-x64.tar.gz -O .dotnet.tar.gz
mkdir dotnet # this should be done by tar but :(
tar xvf .dotnet.tar.gz -C dotnet/
cp -r dotnet ~
rm -rf dotnet .dotnet.tar.gz
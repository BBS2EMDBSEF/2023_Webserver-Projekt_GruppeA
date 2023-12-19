#!/bin/bash

apt update
sudo apt install raspberrypi-ui-mods
update-locale LANG=de_DE.UTF-8 LC_MESSAGES=POSIX
timedatectl set-timezone Europe/Berlin
sudo apt install wget
sudo mkdir /usr/share/dotnet
if getconf LONG_BIT == 32; then
    wget sudo tar zxf dotnet-sdk-6.0.417-linux-arm64.tar.gz -C /usr/share/dotnet/;
    sudo tar zxf dotnet-sdk-6.0.417-linux-arm.tar.gz -C /usr/share/dotnet/
else
    wget https://download.visualstudio.microsoft.com/download/pr/03972b46-ddcd-4529-b8e0-df5c1264cd98/285a1f545020e3ddc47d15cf95ca7a33/dotnet-sdk-6.0.417-linux-arm64.tar.gz;
    sudo tar zxf dotnet-sdk-6.0.417-linux-arm64.tar.gz -C /usr/share/dotnet/
fi

echo 'export PATH=$PATH:/usr/share/dotnet' | sudo tee -a ~/.profile
echo 'export DOTNET_ROOT=/usr/share/dotnet' | sudo tee -a ~/.profile


@reboot continueInstall.sh
echo "@reboot continueInstall.sh" | sudo crontab -
sudo chmod +x continueInstall.sh
sudo reboot -h now




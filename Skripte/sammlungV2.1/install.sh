#!/bin/bash

apt update
sudo apt install raspberrypi-ui-mods
update-locale LANG=de_DE.UTF-8 LC_MESSAGES=POSIX
timedatectl set-timezone Europe/Berlin
sudo apt install wget
sudo mkdir /usr/share/dotnet
if getconf LONG_BIT == 32; then
    wget https://download.visualstudio.microsoft.com/download/pr/a72dea03-21fd-48c6-bf0c-78e621b60514/e0b8f186730fce858eb1bffc83c9e41c/dotnet-sdk-6.0.417-linux-arm.tar.gz
    sudo tar zxf dotnet-sdk-6.0.417-linux-arm.tar.gz -C /usr/share/dotnet/
else
    wget https://download.visualstudio.microsoft.com/download/pr/03972b46-ddcd-4529-b8e0-df5c1264cd98/285a1f545020e3ddc47d15cf95ca7a33/dotnet-sdk-6.0.417-linux-arm64.tar.gz;
    sudo tar zxf dotnet-sdk-6.0.417-linux-arm64.tar.gz -C /usr/share/dotnet/
fi

echo 'export PATH=$PATH:/usr/share/dotnet' >> ~/.profile 
echo 'export DOTNET_ROOT=/usr/share/dotnet' >> ~/.profile

echo 'sudo sh /home/github/sammlungV2.1/continueInstall.sh' >> ~/.bashrc
#sudo reboot -h now




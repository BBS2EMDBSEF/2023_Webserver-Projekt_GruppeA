#!/bin/bash

# Überprüfe, ob das Skript als Root (Administrator) ausgeführt wird
if [ "$EUID" -ne 0 ]; then
  echo "Dieses Skript muss als Root ausgeführt werden. Bitte mit 'sudo' starten."
  exit 1
fi

# Setze die gewünschte Sprache und Tastaturbelegung (hier als Beispiel: Englisch/US)
export DEBIAN_FRONTEND=noninteractive
export LANG=en_US.UTF-8
export LANGUAGE=en_US.UTF-8
export LC_ALL=en_US.UTF-8

# Automatisiere die Partitionierung (Achtung: Dies kann Datenverlust verursachen)
cat <<EOL | sudo fdisk /dev/sda
n
p
1


w
EOL

# Formatieren und mounten der Partition
mkfs.ext4 /dev/sda1
mount /dev/sda1 /mnt

# Installation von Ubuntu Server (hier als Beispiel für Ubuntu 20.04)
debootstrap focal /mnt http://archive.ubuntu.com/ubuntu/

# Chroot in das neue System
mount --bind /dev /mnt/dev
mount --bind /proc /mnt/proc
mount --bind /sys /mnt/sys
chroot /mnt /bin/bash

# Konfiguriere das Netzwerk (ersetze eth0 und die IP-Adresse nach Bedarf)
echo "auto eth0" >> /etc/network/interfaces
echo "iface eth0 inet dhcp" >> /etc/network/interfaces

# Setze den Hostnamen
echo "server" > /etc/hostname

# Füge DNS-Server hinzu (ersetze sie durch deine eigenen)
echo "nameserver 8.8.8.8" > /etc/resolv.conf

# Aktualisiere die Paketliste und installiere grundlegende Pakete
apt update
apt install -y openssh-server

# Setze das Root-Passwort (ersetze 'DEIN_PASSWORT' durch dein gewünschtes Passwort)
echo "root:DEIN_PASSWORT" | chpasswd

# Generiere ein SSH-Schlüsselpaar (optional)
ssh-keygen -t rsa -b 4096

# Beende das Chroot und unmounte die Dateisysteme
exit
umount /mnt/dev
umount /mnt/proc
umount /mnt/sys
umount /mnt

# Starte den Server neu
reboot
#!/bin/bash

Überprüfe, ob das Skript als Root (Administrator) ausgeführt wird
if [ "$EUID" -ne 0 ]; then
  echo "Dieses Skript muss als Root ausgeführt werden. Bitte mit 'sudo' starten."
  exit 1
fi

Aktualisiere das Paketverzeichnis
apt update

Installiere Apache2
apt install apache2 -y

Starte Apache2 und aktiviere ihn beim Systemstart
systemctl start apache2
systemctl enable apache2

Installiere MySQL-Server
apt install mysql-server -y

Führe die MySQL-Sicherheitskonfiguration aus
mysql_secure_installation

Installiere den FTP-Server (vsftpd)
apt install vsftpd -y

Starte den FTP-Server und aktiviere ihn beim Systemstart
systemctl start vsftpd
systemctl enable vsftpd

Öffne Port 20 und 21 in der Firewall für FTP (optional)
ufw allow 20/tcp
ufw allow 21/tcp
ufw enable
echo "Apache2, MySQL und vsftpd wurden erfolgreich installiert."
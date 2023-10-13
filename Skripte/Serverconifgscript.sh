#!/bin/bash

# Aktualisiere die Paketliste
apt update

# Installiere die deutschen Sprachpakete
apt install -y language-pack-de
update-locale LANG=de_DE.UTF-8 LC_MESSAGES=POSIX

# Konfiguriere die Zeitzone
timedatectl set-timezone Europe/Berlin

# Installiere SSH-Server
apt install -y openssh-server

# Installiere Apache2
apt install -y apache2

# Installiere MySQL-Server
apt install -y mysql-server

# MySQL-Root-Passwort setzen
echo "mysql-server mysql-server/root_password password schule" | debconf-set-selections
echo "mysql-server mysql-server/root_password_again password schule" | debconf-set-selections

# Installiere phpMyAdmin
apt install -y phpmyadmin

# Konfiguriere phpMyAdmin
echo "phpmyadmin phpmyadmin/dbconfig-install boolean true" | debconf-set-selections
echo "phpmyadmin phpmyadmin/app-password-confirm password schule" | debconf-set-selections
echo "schule"
echo "phpmyadmin phpmyadmin/mysql/admin-pass password schule" | debconf-set-selections
echo "schule"
echo "phpmyadmin phpmyadmin/mysql/app-pass password schule" | debconf-set-selections
echo "schule"
echo "phpmyadmin phpmyadmin/reconfigure-webserver multiselect apache2" | debconf-set-selections

# Erstelle einen MySQL-Benutzer
mysql -u root -p$dein_root_passwort -e "CREATE USER 'Service'@'localhost' IDENTIFIED BY 'Emden123';"
mysql -u root -p$dein_root_passwort -e "GRANT ALL PRIVILEGES ON *.* TO 'Service'@'localhost' WITH GRANT OPTION;"
mysql -u root -p$dein_root_passwort -e "FLUSH PRIVILEGES;"

# Starte die Dienste
systemctl start apache2
systemctl start mysql

# Aktiviere die Dienste, um sie beim Start automatisch zu starten
systemctl enable apache2
systemctl enable mysql

# Öffne die benötigten Ports in der Firewall
ufw allow OpenSSH
ufw allow "Apache Full"
ufw allow mysql

# Aktiviere die Firewall
ufw enable

# Zeige eine Erfolgsmeldung
echo "Konfiguration abgeschlossen. Du kannst jetzt auf deinen Server zugreifen."



# Configuratuion PhP my admin fehlt
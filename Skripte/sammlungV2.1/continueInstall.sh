#!/bin/bash

apt install -y openssh-server

apt install -y nginx
sudo apt update
sudo apt install -y apt-transport-https lsb-release ca-certificates
sudo apt install -y lsb-release apt-transport-https ca-certificates
sudo wget -O /etc/apt/trusted.gpg.d/php.gpg https://packages.sury.org/php/apt.gpg
sudo sh -c 'echo "deb https://packages.sury.org/php/ $(lsb_release -sc) main" > /etc/apt/sources.list.d/php.list'
sudo apt install php8.1 php8.1-fpm php8.1-mysql php8.1-xml php8.1-curl php8.1-gd -y
sudo mv /etc/nginx/sites-available/default /etc/nginx/sites-available/default.backup


sudo apt install -y mariadb-server
echo "mysql-server mysql-server/root_password password schule" | sudo debconf-set-selections
echo "mysql-server mysql-server/root_password_again password schule" | sudo debconf-set-selections
mysql -u root -p$github-e "CREATE USER 'Service'@'localhost' IDENTIFIED BY 'Emden123';"
mysql -u root -p$github -e "GRANT ALL PRIVILEGES ON *.* TO 'Service'@'localhost' WITH GRANT OPTION;"
mysql -u root -p$github -e "FLUSH PRIVILEGES;"



sudo tee /etc/nginx/sites-available/default <<EOF
server {
        listen 80;
        server_name github_web;
        location / {
                proxy_pass http://0.0.0.0:5000;
                proxy_http_version 1.1;
                proxy_set_header Upgrade \$http_upgrade;
                proxy_set_header Connection keep-alive;
                proxy_set_header Host \$host;
                proxy_cache_bypass \$http_upgrade;
        }
        include snippets/phpmyadmin.conf;
}
EOF

$uri = \$'$uri'
$document_root = \$'$document_root'
$fastcgi_script_name = \$'$fastcgi_script_name'
sudo tee /etc/nginx/snippets/phpmyadmin.conf <<EOF
location /phpmyadmin {
    root /usr/share/;
    index index.php index.html index.htm;
    location ~ ^/phpmyadmin/(.+\.php)$ {
        try_files $uri =404;
        root /usr/share/;
        fastcgi_pass unix:/run/php/php8.1-fpm.sock;
        fastcgi_index index.php;
        fastcgi_param SCRIPT_FILENAME $document_root$fastcgi_script_name;
        include /etc/nginx/fastcgi_params;
    }

    location ~* ^/phpmyadmin/(.+\.(jpg|jpeg|gif|css|png|js|ico|html|xml|txt))$ {
        root /usr/share/;
    }
}
EOF

sudo tee /etc/systemd/system/backend.service <<EOF
#backend.service -File
#Dir: /etc/systemd/system/

[Unit]
Description=Projekt Backend

[Service]
WorkingDirectory=/home/github/backend/
ExecStart=/usr/bin/dotnet /home/github/backend/ServerAppSchule.dll --urls http://0.0.0.0:5000
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
EOF

echo "phpmyadmin phpmyadmin/dbconfig-install boolean true" | sudo debconf-set-selections
echo "phpmyadmin phpmyadmin/app-password-confirm password github" | sudo debconf-set-selections
echo "phpmyadmin phpmyadmin/mysql/admin-pass password github" | sudo debconf-set-selections
echo "phpmyadmin phpmyadmin/mysql/app-pass password github" | sudo debconf-set-selections
echo "phpmyadmin phpmyadmin/reconfigure-webserver multiselect nginx" | sudo debconf-set-selections

systemctl enable backend 
systemctl start backend
systemctl start nginx
systemctl start mysql
systemctl enable nginx
systemctl enable mysql
systemctl status nginx
systemctl status mysql
ufw allow OpenSSH
ufw allow "nginx Full"
ufw allow mysql
ufw enable

(sudo crontab -l | grep -v '@reboot /path/to/continueInstall.sh') | sudo crontab -
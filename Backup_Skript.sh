#!/bin/bash

# Backup && Log Pfad
BACKUP_DIR="/var/www/html"
LOG_FILE="/var/log/backup.log"

# E-Mail Adresse für Benachrichtigungen
EMAIL_TO="deine_email@example.com"
EMAIL_SUBJECT="Backup-Status"

# Protokollierung von Nachrichten
log() {
  local log_message="$1"
  echo "$(date "+%Y-%m-%d %H:%M:%S"): $log_message" >> "$LOG_FILE"
}

# Funktion zur Sendung von E-Mail-Benachrichtigungen
send_email() {
  local email_message="$1"
  echo "$email_message" | mail -s "$EMAIL_SUBJECT" "$EMAIL_TO"
}

# Führe das Backup durch
backup() {
  log "Backup wird gestartet..."
  
 tar -czf "$BACKUP_DIR/backup_$(date +%Y%m%d).tar.gz" /var/www/html
  
  if [ $? -eq 0 ]; then
    log "Backup erfolgreich abgeschlossen."
    send_email "Das Backup wurde erfolgreich abgeschlossen."
  else
    log "Fehler beim Backup!"
    send_email "Fehler beim Backup. Bitte überprüfen Sie das Backup-Skript."
  fi
}

# Hauptprogramm
backup

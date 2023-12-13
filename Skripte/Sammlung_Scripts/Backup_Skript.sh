#!/bin/bash

# Backup && Log Pfad
BACKUP_DIR="/var/www/html"
LOG_FILE="/var/log/backup.log"

# Protokollierung von Nachrichten
log() {
  local log_message="$1"
  echo "$(date "+%Y-%m-%d %H:%M:%S"): $log_message" >> "$LOG_FILE"
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
  fi
}

# Hauptprogramm
backup

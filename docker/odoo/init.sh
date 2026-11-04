#!/bin/bash
# One-shot initialization of the test database: create DB with demo data + modules, then run seed.py.
set -e

ODOO_MODULES="${ODOO_MODULES//[[:space:]]/}"
DB_ARGS=(--db_host="$HOST" --db_user="$USER" --db_password="$PASSWORD")

if psql -h "$HOST" -U "$USER" -d postgres -tAc "SELECT 1 FROM pg_database WHERE datname='$ODOO_DB'" | grep -q 1; then
  echo "Database $ODOO_DB already exists - skipping init (run 'docker compose down -v' to reset)."
  exit 0
fi

echo "==> Creating $ODOO_DB with modules: $ODOO_MODULES"
odoo "${DB_ARGS[@]}" -d "$ODOO_DB" -i "$ODOO_MODULES" \
     --load-language="$ODOO_LANGUAGES" --stop-after-init

echo "==> Seeding test data"
odoo shell "${DB_ARGS[@]}" -d "$ODOO_DB" --no-http < /seed/seed.py

echo "==> Done"

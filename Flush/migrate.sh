#!/usr/bin/env bash

database=Application
migration=InitialCreate

dotnet ef migrations add "$migration" --context "$database"Context --output-dir Databases/"$database"/Migrations


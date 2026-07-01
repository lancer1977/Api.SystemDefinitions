#!/usr/bin/env bash
set -euo pipefail

repo_root="$(cd "$(dirname "${BASH_SOURCE[0]}")/.." && pwd)"
cd "$repo_root"

dotnet restore Api.SystemDefinitions.sln
dotnet build Api.SystemDefinitions.sln --configuration Release --no-restore
dotnet test Api.SystemDefinitions.sln --configuration Release --no-restore --no-build --verbosity normal

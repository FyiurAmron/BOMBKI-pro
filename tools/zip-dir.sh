#!/usr/bin/env bash
# tools/zip-dir.sh <dir> <out.zip>
# Zip a directory into out.zip. Uses the C# zip-dir helper because runners
# and WSL images do not always ship a zip binary. Pure ASCII.
set -euo pipefail

DIR="$1"
OUT="$2"
ROOT="$(readlink -f "$(dirname "$0")/..")"

dotnet run --project "$ROOT/tools/zip-dir" -- "$DIR" "$OUT"

echo "wrote $OUT"

#!/usr/bin/env bash
# tools/run.sh - build the game if stale, then run it.
# -d enables debug mode (BOMBKI_DEBUG=2137). Remaining arguments
# pass through to the game. Pure ASCII.
set -euo pipefail
ROOT="$(readlink -f "$(dirname "$0")/..")"
DEBUG=""
ARGS=()
for arg in "$@"; do
    if [ "$arg" = "-d" ]; then
        DEBUG=1
    else
        ARGS+=("$arg")
    fi
done
dotnet build "$ROOT/BombkiPro.slnx" --nologo -v q
BIN="$ROOT/build/out/Core/Debug/net11.0/Bombki"
case "$(uname -s)" in
    MINGW*|MSYS*|CYGWIN*) BIN="$BIN.exe" ;;
esac
if [ -n "$DEBUG" ]; then
    export BOMBKI_DEBUG=2137
fi
exec "$BIN" "${ARGS[@]}"

# New features

- Intro cleanup
  - new ASCII banner from `data/banner.aa`
  - no ticks, no keypress wait
  - name before race, validated and stored
  - input auto-uppercased
  - name doubles as save file, load or new game
  - Ctrl+C exits the game
  - name shown on the JA sheet
  - rainbow version line
  - trimmed start sequence
- Removed MODE/UNMODE ceremony
  - all commands run from the normal context
  - ground items listed on room entry
- Debugging improvements
  - all debugging commands are now available in all rooms
  - debug commands and env vars need BOMBKI_DEBUG=2137 in env
- Save functionality improvements

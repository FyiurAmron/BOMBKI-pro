# Possible bugs

Triaged audit of the pro version. Items completed or dismissed during
triage are not listed here. **pinned** = current tests assert the
behaviour, so a fix must re-pin them.

## Mechanical inconsistencies

1. Abbreviation expansion is room-dependent (on: corridor, ivy, cave,
    most concert rooms; off: start, salon, school, shops, streets, bar,
    cages 12-17, stages, roads, quest master).

## Smells / architecture

2. `Game.cs` is a ~3.3k-line monolith (combat, economy, shops, saves,
    input, RNG); `Rooms.cs` ~1.36k; `Texts.cs` ~1k.
3. Verb/rest convention (trailing space in verb consts, first-space
    split) is undocumented and fails silently; SplitCache is a mutable
    static dictionary.
4. The `Command` setter throws on null (readers currently never return
    null).
5. `ExpandAbbrev` allocates two lists per command; `DebugEnabled` reads
    the environment on every command.
6. `TRoom.turn` is `Action<bool>` with undocumented "entering"
    semantics; some lambdas peek at `Session.Room`.

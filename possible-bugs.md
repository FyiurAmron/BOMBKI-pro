# Possible bugs

Triaged audit of the pro version. Items completed or dismissed during
triage are not listed here. **pinned** = current tests assert the
behaviour, so a fix must re-pin them.

## Text and data

1. Sentinel conventions are mixed: some text and command lists start with
    "", data tables use `null!`; consumers mix Skip(1) with 1-based indexing.

## Mechanical inconsistencies

2. Abbreviation expansion is room-dependent (on: corridor, ivy, cave,
    most concert rooms; off: start, salon, school, shops, streets, bar,
    cages 12-17, stages, roads, quest master).

## Smells / architecture

3. `Game.cs` is a ~3.3k-line monolith (combat, economy, shops, saves,
    input, RNG); `Rooms.cs` ~1.36k; `Texts.cs` ~1k.
4. Verb/rest convention (trailing space in verb consts, first-space
    split) is undocumented and fails silently; SplitCache is a mutable
    static dictionary.
5. The `Command` setter throws on null (readers currently never return
    null).
6. `ExpandAbbrev` allocates two lists per command; `DebugEnabled` reads
    the environment on every command.
7. `TRoom.turn` is `Action<bool>` with undocumented "entering"
    semantics; some lambdas peek at `Session.Room`.

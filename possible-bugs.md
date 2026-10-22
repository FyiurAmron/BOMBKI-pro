# Possible bugs

Triaged audit of the pro version. Items completed or dismissed during
triage are not listed here. **pinned** = current tests assert the
behaviour, so a fix must re-pin them.

## Text and data

1. `TxtGeneralStoreShopLines1` is unused (and omits
    TxtGeneralStoreShop_02, which is used elsewhere).
2. Sentinel styles mixed: line lists start with "", data tables with
    `null!`; consumption mixes Skip(1) and direct indexes
    (`TxtRoom1Lines1[1..3]`).

## Vestigial / dead code

3. Flavor prose lives in lists with dummy first elements (e.g.
    TxtRoom5_01..03).

## Mechanical inconsistencies

4. Abbreviation expansion is room-dependent (on: corridor, ivy, cave,
    most concert rooms; off: start, salon, school, shops, streets, bar,
    cages 12-17, stages, roads, quest master).

## Smells / architecture

5. `Game.cs` is a ~3.3k-line monolith (combat, economy, shops, saves,
    input, RNG); `Rooms.cs` ~1.36k; `Texts.cs` ~1k.
6. Verb/rest convention (trailing space in verb consts, first-space
    split) is undocumented and fails silently; SplitCache is a mutable
    static dictionary.
7. The `Command` setter throws on null (readers currently never return
    null).
8. `ExpandAbbrev` allocates two lists per command; `DebugEnabled` reads
    the environment on every command.
9. `TRoom.turn` is `Action<bool>` with undocumented "entering"
    semantics; some lambdas peek at `Session.Room`.

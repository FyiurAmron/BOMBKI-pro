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

3. `Crt.Sound`/`Crt.NoSound` are empty unused stubs.
4. Flavor prose lives in lists with dummy first elements (e.g.
    TxtRoom5_01..03).

## Mechanical inconsistencies

5. Abbreviation expansion is room-dependent (on: corridor, ivy, cave,
    most concert rooms; off: start, salon, school, shops, streets, bar,
    cages 12-17, stages, roads, quest master).
6. `ShowCharacter` has side effects: recomputes Bag.Limit and
    Gear.ArmorRating; ArmorRating is display-only (combat uses Armor).
7. `Bag.Count`/`Limit` have no sanity checks.
8. Permit bookkeeping is inconsistent: the permit is a carried item
    (buying a forged one adds a bag slot, so quest 1's reward adding one
    is right), but quests 2/3 and the quest-master kill grant it without
    a slot, and ZNISZCZ PRZEPUSTKA adds a slot instead of freeing one
    (Game.cs:1598). Permits also stack with no use for more than one.
    The pipe's +1 wisdom is capped on pickup but its hand-in penalty is
    not, so a pipe taken at max wisdom loses a point. **pinned**

## Smells / architecture

9. `Game.cs` is a ~3.3k-line monolith (combat, economy, shops, saves,
    input, RNG); `Rooms.cs` ~1.36k; `Texts.cs` ~1k.
10. Verb/rest convention (trailing space in verb consts, first-space
    split) is undocumented and fails silently; SplitCache is a mutable
    static dictionary.
11. The `Command` setter throws on null (readers currently never return
    null).
12. `ExpandAbbrev` allocates two lists per command; `DebugEnabled` reads
    the environment on every command.
13. `TRoom.turn` is `Action<bool>` with undocumented "entering"
    semantics; some lambdas peek at `Session.Room`.

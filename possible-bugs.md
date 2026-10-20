# Possible bugs

Triaged audit of the pro version. Items completed or dismissed during
triage are not listed here. **pinned** = current tests assert the
behaviour, so a fix must re-pin them.

## Bugs

1. `TxtRoom0_02` and `TxtRoom3_04` name the destination "hala główna
    Szkoły Życia" but both moves go to the salon (room 1; the hall is
    room 4).

## Text and data

2. `TxtGeneralStoreShopLines1` is unused (and omits
    TxtGeneralStoreShop_02, which is used elsewhere).
3. Sentinel styles mixed: line lists start with "", data tables with
    `null!`; consumption mixes Skip(1) and direct indexes
    (`TxtRoom1Lines1[1..3]`).

## Vestigial / dead code

4. `RoomAny` is used only as the teleport sentinel now, no longer for
    dispatch.
5. `Crt.Sound`/`Crt.NoSound` are empty unused stubs.
6. Flavor prose lives in lists with dummy first elements (e.g.
    TxtRoom5_01..03).

## Mechanical inconsistencies

7. Abbreviation expansion is room-dependent (on: corridor, ivy, cave,
    most concert rooms; off: start, salon, school, shops, streets, bar,
    cages 12-17, stages, roads, quest master).
8. `ShowCharacter` has side effects: recomputes Bag.Limit and
    Gear.ArmorRating; ArmorRating is display-only (combat uses Armor).
9. `Bag.Count`/`Limit` have no sanity checks.
10. Permit bookkeeping is inconsistent: the permit is a carried item
    (buying a forged one adds a bag slot, so quest 1's reward adding one
    is right), but quests 2/3 and the quest-master kill grant it without
    a slot, and ZNISZCZ PRZEPUSTKA adds a slot instead of freeing one
    (Game.cs:1598). Permits also stack with no use for more than one.
    The pipe's +1 wisdom is capped on pickup but its hand-in penalty is
    not, so a pipe taken at max wisdom loses a point. **pinned**

## Smells / architecture

11. `Game.cs` is a ~3.3k-line monolith (combat, economy, shops, saves,
    input, RNG); `Rooms.cs` ~1.36k; `Texts.cs` ~1k.
12. Verb/rest convention (trailing space in verb consts, first-space
    split) is undocumented and fails silently; SplitCache is a mutable
    static dictionary.
13. The `Command` setter throws on null (readers currently never return
    null).
14. `ExpandAbbrev` allocates two lists per command; `DebugEnabled` reads
    the environment on every command.
15. `TRoom.turn` is `Action<bool>` with undocumented "entering"
    semantics; some lambdas peek at `Session.Room`.
16. Data/graph divergence has no validation (e.g. item 1).

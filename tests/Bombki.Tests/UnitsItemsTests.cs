using static Game;

namespace Bombki.Tests;

public class UnitsItemsTests {
    [Fact]
    public void BraniePicksUpSword() {
        Session.OuterRoom = 7;
        Bag.Items[TBagItem.OldSword] = 0;
        Bag.ItemRooms[TBagItem.OldSword] = 7;
        Bag.Count = 0;
        Command = "BIERZ STARY";
        TakeDrop();
        Assert.Equal(1, Bag.Items[TBagItem.OldSword]);
        Assert.Equal(Rooms.RoomAny, Bag.ItemRooms[TBagItem.OldSword]);
        Assert.Equal(1, Bag.Count);
    }

    [Fact]
    public void BranieDropsSword() {
        Session.OuterRoom = 7;
        Bag.Items[TBagItem.OldSword] = 1;
        Bag.Count = 1;
        Command = "ODRZUC STARY";
        TakeDrop();
        Assert.Equal(0, Bag.Items[TBagItem.OldSword]);
        Assert.Equal(7, Bag.ItemRooms[TBagItem.OldSword]);
        Assert.Equal(0, Bag.Count);
    }

    [Fact]
    public void OdrzucanieNieDzialaNaWladanymMieczu() {
        Session.OuterRoom = 7;
        Bag.Items[TBagItem.OldSword] = 2;
        Bag.ItemRooms[TBagItem.OldSword] = Rooms.RoomAny;
        Bag.Count = 2;
        Gear.Values[TGearItem.WeaponKind] = 0;
        Player.Stats[TStat.Strength] = 0;
        Gear.Values[TGearItem.LuckRoll] = 0;
        Assert.True(Equip(TBagItem.OldSword));
        Command = "ODRZUC STARY";
        TakeDrop();
        Assert.Equal(1, Bag.Items[TBagItem.OldSword]);
        Assert.Equal(Rooms.RoomAny, Bag.ItemRooms[TBagItem.OldSword]);
        Assert.Equal(1, Bag.Count);
        Assert.True(Unequip(TBagItem.OldSword));
        Assert.Equal(2, Bag.Items[TBagItem.OldSword]);
        Assert.Equal(2, Bag.Count);
    }

    [Fact]
    public void OdrzucanieNieDzialaNaWladanejTarczy() {
        Session.OuterRoom = 7;
        Bag.Items[TBagItem.SmallShield] = 2;
        Bag.ItemRooms[TBagItem.SmallShield] = Rooms.RoomAny;
        Bag.Count = 2;
        Gear.Values[TGearItem.ShieldKind] = 0;
        Gear.Values[TGearItem.Armor] = 0;
        Gear.Values[TGearItem.WornCount] = 0;
        Assert.True(Equip(TBagItem.SmallShield));
        Command = "ODRZUC MALA";
        TakeDrop();
        Assert.Equal(1, Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(Rooms.RoomAny, Bag.ItemRooms[TBagItem.SmallShield]);
        Assert.Equal(1, Bag.Count);
        Assert.True(Unequip(TBagItem.SmallShield));
        Assert.Equal(2, Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(2, Bag.Count);
    }

    [Fact]
    public void OdrzucanieBezSztukiNieDziala() {
        Session.OuterRoom = 7;
        Bag.Items[TBagItem.OldSword] = 0;
        Bag.ItemRooms[TBagItem.OldSword] = Rooms.RoomAny;
        Gear.Values[TGearItem.WeaponKind] = 0;
        Bag.Count = 0;
        Command = "ODRZUC STARY";
        TakeDrop();
        Assert.Equal(0, Bag.Items[TBagItem.OldSword]);
        Assert.Equal(Rooms.RoomAny, Bag.ItemRooms[TBagItem.OldSword]);
        Assert.Equal(0, Bag.Count);
    }

    [Fact]
    public void OdrzucanieStosuZostawiaReszte() {
        Session.OuterRoom = 7;
        Bag.Items[TBagItem.OldSword] = 2;
        Bag.ItemRooms[TBagItem.OldSword] = Rooms.RoomAny;
        Gear.Values[TGearItem.WeaponKind] = 0;
        Bag.Count = 2;
        Command = "ODRZUC STARY";
        TakeDrop();
        Assert.Equal(1, Bag.Items[TBagItem.OldSword]);
        Assert.Equal(7, Bag.ItemRooms[TBagItem.OldSword]);
        Assert.Equal(1, Bag.Count);
    }

    [Fact]
    public void WladaMieczZeStosu() {
        Bag.Items[TBagItem.OldSword] = 2;
        Bag.Count = 2;
        Gear.Values[TGearItem.WeaponKind] = 0;
        Player.Stats[TStat.Strength] = 10;
        Gear.Values[TGearItem.LuckRoll] = 0;
        CmdWieldOldSword();
        Assert.Equal(1, Gear.Values[TGearItem.WeaponKind]);
        Assert.Equal(1, Bag.Items[TBagItem.OldSword]);
        Assert.Equal(1, Bag.Count);
        CmdUnwieldOldSword();
        Assert.Equal(0, Gear.Values[TGearItem.WeaponKind]);
        Assert.Equal(2, Bag.Items[TBagItem.OldSword]);
        Assert.Equal(2, Bag.Count);
        Player.Stats[TStat.Strength] = 0;
        Gear.Values[TGearItem.LuckRoll] = 0;
    }

    [Fact]
    public void WladaTarczePrzenosiJaDoZalozenia() {
        Bag.Items[TBagItem.SmallShield] = 1;
        Bag.Count = 1;
        Gear.Values[TGearItem.ShieldKind] = 0;
        Gear.Values[TGearItem.Armor] = 0;
        Gear.Values[TGearItem.WornCount] = 0;
        CmdWieldSmallShield();
        Assert.Equal(1, Gear.Values[TGearItem.ShieldKind]);
        Assert.Equal(0, Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(0, Bag.Count);
        Assert.Equal(15, Gear.Values[TGearItem.Armor]);
        CmdUnwieldSmallShield();
        Assert.Equal(0, Gear.Values[TGearItem.ShieldKind]);
        Assert.Equal(1, Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(1, Bag.Count);
        Assert.Equal(0, Gear.Values[TGearItem.Armor]);
        Assert.Equal(0, Gear.Values[TGearItem.WornCount]);
    }

    [Fact]
    public void LootDropsWhenTheItemIsAlreadyCarried() {
        Bag.Items[TBagItem.OldSword] = 1;
        Bag.Count = 1;
        Bag.ItemRooms[TBagItem.OldSword] = Rooms.RoomAny;
        Session.Room = 7;
        TryLoot(TBagItem.OldSword, 100, "");
        Assert.Equal(7, Bag.ItemRooms[TBagItem.OldSword]);
        Bag.Items[TBagItem.OldSword] = 0;
        Bag.Count = 0;
        Bag.ItemRooms[TBagItem.OldSword] = Rooms.RoomAny;
    }

    [Fact]
    public void BranieIgnoresMissingSword() {
        Session.OuterRoom = 7;
        Bag.Items[TBagItem.OldSword] = 0;
        Bag.ItemRooms[TBagItem.OldSword] = Rooms.RoomAny;
        Bag.Count = 0;
        Command = "BIERZ STARY";
        TakeDrop();
        Assert.Equal(0, Bag.Items[TBagItem.OldSword]);
        Assert.Equal(0, Bag.Count);
    }

    [Fact]
    public void BranieIgnoresStartRoomWhenNothingLiesThere() {
        Session.OuterRoom = Rooms.RoomStart;
        Bag.Items[TBagItem.Diploma] = 0;
        Bag.ItemRooms[TBagItem.Diploma] = Rooms.RoomAny;
        Bag.Count = 0;
        Command = "BIERZ DYPLOM";
        TakeDrop();
        Assert.Equal(0, Bag.Items[TBagItem.Diploma]);
        Assert.Equal(Rooms.RoomAny, Bag.ItemRooms[TBagItem.Diploma]);
        Assert.Equal(0, Bag.Count);
    }

    [Fact]
    public void BraniePicksUpBluszczItems() {
        Session.OuterRoom = 7;
        Bag.Items[TBagItem.SmallShield] = 0;
        Bag.Items[TBagItem.Heart] = 0;
        Bag.Items[TBagItem.Diploma] = 0;
        Bag.Items[TBagItem.Pipe] = 0;
        Bag.ItemRooms[TBagItem.SmallShield] = 7;
        Bag.ItemRooms[TBagItem.Heart] = 7;
        Bag.ItemRooms[TBagItem.Diploma] = 7;
        Bag.ItemRooms[TBagItem.Pipe] = 7;
        Bag.Count = 0;
        Player.Stats[TStat.Wisdom] = 0;
        Player.Stats[TStat.MaxWisdom] = 5;
        Player.MaxEnergy = 50;
        Command = "BIERZ MALA";
        TakeDrop();
        Command = "BIERZ SERCE";
        TakeDrop();
        Command = "BIERZ DYPLOM";
        TakeDrop();
        Command = "BIERZ FAJKA";
        TakeDrop();
        Assert.Equal(1, Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(1, Bag.Items[TBagItem.Heart]);
        Assert.Equal(1, Bag.Items[TBagItem.Diploma]);
        Assert.Equal(1, Bag.Items[TBagItem.Pipe]);
        Assert.Equal(4, Bag.Count);
        Assert.Equal(55, Player.MaxEnergy);
        Assert.Equal(1, Player.Stats[TStat.Wisdom]);
    }

    [Fact]
    public void BranieDropsBluszczItems() {
        Session.OuterRoom = 7;
        Bag.Items[TBagItem.SmallShield] = 1;
        Bag.Items[TBagItem.Heart] = 1;
        Bag.Items[TBagItem.Diploma] = 1;
        Bag.Items[TBagItem.Pipe] = 1;
        Bag.Count = 4;
        Player.Stats[TStat.Wisdom] = 1;
        Player.MaxEnergy = 55;
        Command = "ODRZUC FAJKA";
        TakeDrop();
        Command = "ODRZUC MALA";
        TakeDrop();
        Command = "ODRZUC SERCE";
        TakeDrop();
        Command = "ODRZUC DYPLOM";
        TakeDrop();
        Assert.Equal(0, Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(0, Bag.Items[TBagItem.Heart]);
        Assert.Equal(0, Bag.Items[TBagItem.Diploma]);
        Assert.Equal(0, Bag.Items[TBagItem.Pipe]);
        Assert.Equal(7, Bag.ItemRooms[TBagItem.SmallShield]);
        Assert.Equal(7, Bag.ItemRooms[TBagItem.Heart]);
        Assert.Equal(7, Bag.ItemRooms[TBagItem.Diploma]);
        Assert.Equal(7, Bag.ItemRooms[TBagItem.Pipe]);
        Assert.Equal(0, Bag.Count);
        Assert.Equal(50, Player.MaxEnergy);
        Assert.Equal(0, Player.Stats[TStat.Wisdom]);
    }

    [Fact]
    public void UzywaSerce() {
        Bag.Items[TBagItem.Heart] = 1;
        Player.Energy = 10;
        Player.MaxEnergy = 50;
        Bag.Count = 1;
        Command = "UZYJ SERCE";
        UseItems();
        Assert.Equal(0, Bag.Items[TBagItem.Heart]);
        Assert.Equal(15, Player.Energy);
        Assert.Equal(0, Bag.Count);
    }

    [Fact]
    public void UzywaSerceClampsAtMaxE() {
        Bag.Items[TBagItem.Heart] = 1;
        Player.Energy = 48;
        Player.MaxEnergy = 50;
        Bag.Count = 1;
        Command = "UZYJ SERCE";
        UseItems();
        Assert.Equal(50, Player.Energy);
    }

    [Fact]
    public void UzywaSerceZuzywaJedno() {
        Bag.Items[TBagItem.Heart] = 2;
        Player.Energy = 10;
        Player.MaxEnergy = 50;
        Bag.Count = 2;
        Command = "UZYJ SERCE";
        UseItems();
        Assert.Equal(1, Bag.Items[TBagItem.Heart]);
        Assert.Equal(15, Player.Energy);
        Assert.Equal(1, Bag.Count);
    }

    [Fact]
    public void UzywaKomplet() {
        Bag.Items[TBagItem.Tracksuit] = 1;
        Bag.Count = 1;
        Gear.WornGarment = Texts.TxtEmpty;
        Gear.Values[TGearItem.Armor] = 0;
        Gear.Values[TGearItem.Wearing] = 0;
        Command = "UZYJ KOMPLET";
        UseItems();
        Assert.Equal(7, Gear.Values[TGearItem.Armor]);
        Assert.Equal(1, Gear.Values[TGearItem.Wearing]);
        Assert.Equal(Texts.TxtWornSyf, Gear.WornGarment);
        Assert.Equal(0, Bag.Items[TBagItem.Tracksuit]);
        Assert.Equal(0, Bag.Count);
        Command = "ODLOZ KOMPLET";
        UseItems();
        Assert.Equal(0, Gear.Values[TGearItem.Armor]);
        Assert.Equal(0, Gear.Values[TGearItem.Wearing]);
        Assert.Equal(Texts.TxtEmpty, Gear.WornGarment);
        Assert.Equal(1, Bag.Items[TBagItem.Tracksuit]);
        Assert.Equal(1, Bag.Count);
    }

    [Fact]
    public void UzywaKompletWorn() {
        Bag.Items[TBagItem.Tracksuit] = 1;
        Bag.Count = 1;
        Gear.WornGarment = Texts.TxtWornGarnitur;
        Gear.Values[TGearItem.Armor] = 0;
        Gear.Values[TGearItem.Wearing] = 1;
        Command = "UZYJ KOMPLET";
        UseItems();
        Assert.Equal(0, Gear.Values[TGearItem.Armor]);
        Assert.Equal(1, Gear.Values[TGearItem.Wearing]);
        Assert.Equal(Texts.TxtWornGarnitur, Gear.WornGarment);
        Assert.Equal(1, Bag.Items[TBagItem.Tracksuit]);
        Assert.Equal(1, Bag.Count);
        Gear.WornGarment = Texts.TxtEmpty;
        Gear.Values[TGearItem.Wearing] = 0;
    }

    [Fact]
    public void UzywaGarnitur() {
        Bag.Items[TBagItem.SpikedSuit] = 1;
        Bag.Count = 1;
        Gear.WornGarment = Texts.TxtEmpty;
        Gear.Values[TGearItem.WornCount] = 0;
        Gear.Values[TGearItem.Armor] = 0;
        Gear.Values[TGearItem.LuckRoll] = 0;
        Command = "UZYJ GARNITUR";
        UseItems();
        Assert.Equal(10, Gear.Values[TGearItem.Armor]);
        Assert.Equal(1, Gear.Values[TGearItem.WornCount]);
        Assert.Equal(15, Gear.Values[TGearItem.LuckRoll]);
        Assert.Equal(Texts.TxtWornGarnitur, Gear.WornGarment);
        Assert.Equal(0, Bag.Items[TBagItem.SpikedSuit]);
        Assert.Equal(0, Bag.Count);
        Command = "ODLOZ GARNITUR";
        UseItems();
        Assert.Equal(0, Gear.Values[TGearItem.Armor]);
        Assert.Equal(0, Gear.Values[TGearItem.WornCount]);
        Assert.Equal(0, Gear.Values[TGearItem.LuckRoll]);
        Assert.Equal(Texts.TxtEmpty, Gear.WornGarment);
        Assert.Equal(1, Bag.Items[TBagItem.SpikedSuit]);
        Assert.Equal(1, Bag.Count);
    }

    [Fact]
    public void UzywaButelkaMany() {
        Bag.Items[TBagItem.ManaBottle] = 1;
        Player.Mana = 10;
        Player.MaxMana = 50;
        Bag.Count = 1;
        Command = "UZYJ MALA BUTELKA MANY";
        UseItems();
        Assert.Equal(40, Player.Mana);
        Assert.Equal(0, Bag.Count);
    }

    [Fact]
    public void UzywaButelkaManyClampsAtMaxMana() {
        Bag.Items[TBagItem.ManaBottle] = 1;
        Player.Mana = 45;
        Player.MaxMana = 50;
        Bag.Count = 1;
        Command = "UZYJ MALA BUTELKA MANY";
        UseItems();
        Assert.Equal(50, Player.Mana);
    }

    [Fact]
    public void PorownanieScrollNieDzialaBezMany() {
        Bag.Items[TBagItem.CompareScroll] = 1;
        Bag.Count = 1;
        Player.MaxMana = 100;
        Player.Mana = 0;
        CmdUseCompareScroll();
        Assert.Equal(0, Player.Mana);
        Assert.Equal(1, Bag.Items[TBagItem.CompareScroll]);
        Assert.Equal(1, Bag.Count);
    }

    [Fact]
    public void ScrollPowrotNieDzialaBezMany() {
        Bag.Items[TBagItem.ReturnScroll] = 1;
        Bag.Count = 1;
        Player.MaxMana = 100;
        Player.Mana = 0;
        Session.Room = Rooms.RoomCityCentre;
        Command = "UZYJ SCROLL POWROT";
        CmdOpenPortal();
        Assert.Equal(Rooms.RoomCityCentre, Session.Room);
        Assert.Equal(0, Player.Mana);
        Assert.Equal(1, Bag.Items[TBagItem.ReturnScroll]);
        Assert.Equal(1, Bag.Count);
    }

    [Fact]
    public void NiszczyPrzepustke() {
        Bag.Permit = 1;
        Bag.Count = 0;
        Command = "ZNISZCZ PRZEPUSTKA";
        UseItems();
        Assert.Equal(0, Bag.Permit);
        Assert.Equal(1, Bag.Count);
    }

    [Fact]
    public void PatrzPrzepustke() {
        Bag.Permit = 1;
        Bag.Count = 0;
        Command = "PATRZ PRZEPUSTKA";
        UseItems();
        Assert.Equal(1, Bag.Permit);
        Assert.Equal(0, Bag.Count);
    }

    [Fact]
    public void UzywaDyplom() {
        Bag.Items[TBagItem.Diploma] = 1;
        Command = "UZYJ DYPLOM";
        UseItems();
        Assert.Equal(1, Bag.Items[TBagItem.Diploma]);
    }

    static void EatSetup(TBagItem item) {
        Bag.Items[item] = 1;
        Player.Energy = 10;
        Player.MaxEnergy = 50;
        Bag.Count = 1;
    }

    [Fact]
    public void UzywaPaczek() {
        EatSetup(TBagItem.Doughnut);
        Command = "UZYJ PACZEK";
        UseItems();
        Assert.Equal(0, Bag.Items[TBagItem.Doughnut]);
        Assert.Equal(18, Player.Energy);
        Assert.Equal(0, Bag.Count);
    }

    [Fact]
    public void UzywaCiastko() {
        EatSetup(TBagItem.Cookie);
        Command = "UZYJ CIASTKO";
        UseItems();
        Assert.Equal(0, Bag.Items[TBagItem.Cookie]);
        Assert.Equal(22, Player.Energy);
    }

    [Fact]
    public void UzywaSuchaRacja() {
        EatSetup(TBagItem.DryRation);
        Command = "UZYJ SUCHA RACJA";
        UseItems();
        Assert.Equal(0, Bag.Items[TBagItem.DryRation]);
        Assert.Equal(26, Player.Energy);
    }

    [Fact]
    public void UzywaBulka() {
        EatSetup(TBagItem.BreadRoll);
        Command = "UZYJ BULKA";
        UseItems();
        Assert.Equal(0, Bag.Items[TBagItem.BreadRoll]);
        Assert.Equal(30, Player.Energy);
    }

    [Fact]
    public void UzywaChleb() {
        EatSetup(TBagItem.Bread);
        Command = "UZYJ CHLEB";
        UseItems();
        Assert.Equal(0, Bag.Items[TBagItem.Bread]);
        Assert.Equal(36, Player.Energy);
    }

    [Fact]
    public void UzyjaWeka() {
        EatSetup(TBagItem.Sausage);
        Command = "UZYJ WEKA";
        UseItems();
        Assert.Equal(0, Bag.Items[TBagItem.Sausage]);
        Assert.Equal(44, Player.Energy);
    }

    [Fact]
    public void UzywaBigos() {
        EatSetup(TBagItem.Bigos);
        Command = "UZYJ BIGOS";
        UseItems();
        Assert.Equal(0, Bag.Items[TBagItem.Bigos]);
        Assert.Equal(30, Player.Energy);
    }

    [Fact]
    public void UzywaPigulkaLow() {
        Bag.Items[TBagItem.TeleportPill] = 1;
        Player.Stats[TStat.Wisdom] = 5;
        Player.MaxEnergy = 50;
        Player.Energy = 40;
        Player.Experience = 100;
        Command = "UZYJ PIGULKA";
        UseItems();
        Assert.Equal(0, Bag.Items[TBagItem.TeleportPill]);
        Assert.Equal(1, Player.Energy);
        Assert.Equal(49, Player.MaxEnergy);
        Assert.Equal(50, Player.Experience);
    }

    [Fact]
    public void UzywaPigulkaMid() {
        Bag.Items[TBagItem.TeleportPill] = 1;
        Player.Stats[TStat.Wisdom] = 12;
        Player.MaxEnergy = 100;
        Player.Energy = 100;
        Player.Experience = 100;
        Command = "UZYJ PIGULKA";
        UseItems();
        Assert.Equal(60, Player.Energy);
        Assert.Equal(70, Player.Experience);
    }

    [Fact]
    public void UzywaPigulkaHigh() {
        Bag.Items[TBagItem.TeleportPill] = 1;
        Player.Stats[TStat.Wisdom] = 20;
        Player.MaxEnergy = 100;
        Player.Energy = 100;
        Player.Experience = 100;
        Command = "UZYJ PIGULKA";
        UseItems();
        Assert.Equal(0, Bag.Items[TBagItem.TeleportPill]);
        Assert.Equal(100, Player.Energy);
        Assert.Equal(100, Player.Experience);
    }

    [Fact]
    public void GarniturZyskInvariant() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.SpikedSuit] = 0;
        Bag.Count = 0;
        SuitGain();
        Assert.True(Bag.Items[TBagItem.SpikedSuit] == 0
            || Bag.Items[TBagItem.SpikedSuit] == 1);
        Assert.True(Bag.Count == 0 || Bag.Count == 1);
    }

    [Fact]
    public void PigulkaZyskInvariant() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.TeleportPill] = 0;
        Bag.Count = 0;
        PillGain();
        Assert.True(Bag.Items[TBagItem.TeleportPill] == 0
            || Bag.Items[TBagItem.TeleportPill] == 1);
        Assert.True(Bag.Count == 0 || Bag.Count == 1);
    }

    [Fact]
    public void KasetaZyskInvariant() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.Cassette] = 0;
        Player.MaxEnergy = 0;
        Gear.Values[TGearItem.Armor] = 0;
        Bag.Count = 0;
        Player.Stats[TStat.Dexterity] = 0;
        Session.Room = 20;
        CassetteGain();
        Assert.True(Bag.Items[TBagItem.Cassette] == 0
            || Bag.Items[TBagItem.Cassette] == 1);
        Assert.True(Player.MaxEnergy == 0 || Player.MaxEnergy == 5);
        Assert.True(Gear.Values[TGearItem.Armor] == 0
            || Gear.Values[TGearItem.Armor] == -8);
        Assert.True(Bag.Count == 0 || Bag.Count == 1);
        Assert.True(Player.Stats[TStat.Dexterity] == 0
            || Player.Stats[TStat.Dexterity] == 1);
    }

    [Fact]
    public void ListekZyskInvariant() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.LuckyLeaf] = 0;
        Bag.Count = 0;
        Player.MaxMana = 0;
        Session.Room = 20;
        LeafGain();
        Assert.True(Bag.Items[TBagItem.LuckyLeaf] == 0
            || Bag.Items[TBagItem.LuckyLeaf] == 1);
        Assert.True(Bag.Count == 0 || Bag.Count == 1);
        Assert.True(Player.MaxMana == 0 || Player.MaxMana == 40);
    }

    [Fact]
    public void ScrollPorZyskInvariant() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.CompareScroll] = 0;
        Bag.Count = 0;
        Session.Room = 20;
        ScrollGain();
        Assert.True(Bag.Items[TBagItem.CompareScroll] == 0
            || Bag.Items[TBagItem.CompareScroll] == 1);
        Assert.True(Bag.Count == 0 || Bag.Count == 1);
    }

    [Fact]
    public void GarniturZyskGain() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.SpikedSuit] = 0;
        Bag.Count = 0;
        int i = 0;
        while (Bag.Count == 0 && i < 2000) {
            SuitGain();
            i++;
        }
        Assert.True(Bag.Count > 0);
        Assert.Equal(1, Bag.Items[TBagItem.SpikedSuit]);
    }

    [Fact]
    public void PigulkaZyskGain() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.TeleportPill] = 0;
        Bag.Count = 0;
        int i = 0;
        while (Bag.Count == 0 && i < 2000) {
            PillGain();
            i++;
        }
        Assert.True(Bag.Count > 0);
        Assert.Equal(1, Bag.Items[TBagItem.TeleportPill]);
    }

    [Fact]
    public void KasetaZyskGain() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.Cassette] = 0;
        Player.MaxEnergy = 0;
        Gear.Values[TGearItem.Armor] = 0;
        Bag.Count = 0;
        Player.Stats[TStat.Dexterity] = 0;
        Session.Room = 20;
        int i = 0;
        while (Bag.Count == 0 && i < 2000) {
            CassetteGain();
            i++;
        }
        Assert.True(Bag.Count > 0);
        Assert.Equal(1, Bag.Items[TBagItem.Cassette]);
        Assert.Equal(5, Player.MaxEnergy);
        Assert.Equal(-8, Gear.Values[TGearItem.Armor]);
        Assert.Equal(1, Player.Stats[TStat.Dexterity]);
    }

    [Fact]
    public void ListekZyskGain() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.LuckyLeaf] = 0;
        Bag.Count = 0;
        Player.MaxMana = 0;
        Session.Room = 20;
        int i = 0;
        while (Bag.Count == 0 && i < 2000) {
            LeafGain();
            i++;
        }
        Assert.True(Bag.Count > 0);
        Assert.Equal(1, Bag.Items[TBagItem.LuckyLeaf]);
        Assert.Equal(40, Player.MaxMana);
    }

    [Fact]
    public void ScrollPorZyskGain() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.CompareScroll] = 0;
        Bag.Count = 0;
        Session.Room = 20;
        int i = 0;
        while (Bag.Count == 0 && i < 2000) {
            ScrollGain();
            i++;
        }
        Assert.True(Bag.Count > 0);
        Assert.Equal(1, Bag.Items[TBagItem.CompareScroll]);
    }

    [Fact]
    public void SprzedazMieczaZeStosu() {
        Bag.Items[TBagItem.OldSword] = 2;
        Bag.Count = 2;
        Gear.Values[TGearItem.WeaponKind] = 0;
        Player.Money = 0;
        Assert.True(Sell(TBagItem.OldSword));
        Assert.Equal(1, Bag.Items[TBagItem.OldSword]);
        Assert.Equal(1, Bag.Count);
        Assert.Equal(30, Player.Money);
    }

    [Fact]
    public void SprzedazTarczyZeStosu() {
        Bag.Items[TBagItem.SmallShield] = 2;
        Bag.Count = 2;
        Gear.Values[TGearItem.ShieldKind] = 0;
        Player.Money = 0;
        Assert.True(Sell(TBagItem.SmallShield));
        Assert.Equal(1, Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(1, Bag.Count);
        Assert.Equal(8, Player.Money);
    }

    [Fact]
    public void SprzedazTarczyBezSztukiNieDziala() {
        Bag.Items[TBagItem.SmallShield] = 0;
        Bag.Count = 0;
        Player.Money = 0;
        Gear.Values[TGearItem.ShieldKind] = 0;
        Assert.False(Sell(TBagItem.SmallShield));
        Assert.Equal(0, Bag.Count);
        Assert.Equal(0, Player.Money);
    }

    [Fact]
    public void SprzedazPigulkiBezSztukiNieDziala() {
        Bag.Items[TBagItem.TeleportPill] = 0;
        Bag.Count = 0;
        Player.Money = 0;
        Assert.False(Sell(TBagItem.TeleportPill));
        Assert.Equal(0, Bag.Count);
        Assert.Equal(0, Player.Money);
    }

    [Fact]
    public void SprzedazFajkiZeStosu() {
        Bag.Items[TBagItem.Pipe] = 2;
        Bag.Count = 2;
        Player.Money = 0;
        Player.Stats[TStat.Wisdom] = 10;
        Assert.True(Sell(TBagItem.Pipe));
        Assert.Equal(1, Bag.Items[TBagItem.Pipe]);
        Assert.Equal(1, Bag.Count);
        Assert.Equal(120, Player.Money);
        Assert.Equal(9, Player.Stats[TStat.Wisdom]);
    }

    [Fact]
    public void SprzedazListkaWymagaListka() {
        Bag.Items[TBagItem.LuckyLeaf] = 0;
        Bag.Count = 0;
        Player.Money = 1000;
        Player.MaxMana = 40;
        Assert.False(Sell(TBagItem.LuckyLeaf));
        Bag.Items[TBagItem.LuckyLeaf] = 1;
        Bag.Count = 1;
        Assert.True(Sell(TBagItem.LuckyLeaf));
        Assert.Equal(1400, Player.Money);
        Assert.Equal(0, Player.MaxMana);
    }

    [Fact]
    public void SprzedazGarnituruWymagaGarnituru() {
        Bag.Items[TBagItem.SpikedSuit] = 0;
        Bag.Count = 0;
        Gear.WornGarment = Texts.TxtEmpty;
        Assert.False(Sell(TBagItem.SpikedSuit));
        Bag.Items[TBagItem.SpikedSuit] = 1;
        Bag.Count = 1;
        Assert.True(Sell(TBagItem.SpikedSuit));
        Assert.Equal(0, Bag.Items[TBagItem.SpikedSuit]);
        Assert.Equal(0, Bag.Count);
    }

    [Fact]
    public void SprzedazWladanegoMieczaNieDziala() {
        Bag.Items[TBagItem.OldSword] = 2;
        Bag.Count = 2;
        Player.Money = 0;
        Gear.Values[TGearItem.WeaponKind] = 0;
        Player.Stats[TStat.Strength] = 0;
        Gear.Values[TGearItem.LuckRoll] = 0;
        Assert.True(Equip(TBagItem.OldSword));
        Assert.False(Sell(TBagItem.OldSword));
        Assert.Equal(1, Bag.Items[TBagItem.OldSword]);
        Assert.Equal(1, Bag.Count);
        Assert.Equal(0, Player.Money);
        Assert.True(Unequip(TBagItem.OldSword));
        Assert.Equal(2, Bag.Items[TBagItem.OldSword]);
        Assert.Equal(2, Bag.Count);
    }

    [Fact]
    public void SprzedazWladanejTarczyNieDziala() {
        Bag.Items[TBagItem.SmallShield] = 2;
        Bag.Count = 2;
        Player.Money = 0;
        Gear.Values[TGearItem.ShieldKind] = 0;
        Gear.Values[TGearItem.Armor] = 0;
        Gear.Values[TGearItem.WornCount] = 0;
        Assert.True(Equip(TBagItem.SmallShield));
        Assert.False(Sell(TBagItem.SmallShield));
        Assert.Equal(1, Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(1, Bag.Count);
        Assert.Equal(0, Player.Money);
        Assert.True(Unequip(TBagItem.SmallShield));
        Assert.Equal(2, Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(2, Bag.Count);
    }

    [Fact]
    public void SprzedazWladanejTarczyBezSztukiNiePlaci() {
        Bag.Items[TBagItem.SmallShield] = 1;
        Bag.Count = 1;
        Player.Money = 0;
        Gear.Values[TGearItem.ShieldKind] = 0;
        Gear.Values[TGearItem.Armor] = 0;
        Gear.Values[TGearItem.WornCount] = 0;
        Assert.True(Equip(TBagItem.SmallShield));
        Assert.False(Sell(TBagItem.SmallShield));
        Assert.Equal(0, Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(0, Bag.Count);
        Assert.Equal(0, Player.Money);
        Assert.True(Unequip(TBagItem.SmallShield));
    }

    [Fact]
    public void SprzedazZalozonegoKompletuNieDziala() {
        Bag.Items[TBagItem.Tracksuit] = 1;
        Bag.Count = 1;
        Player.Money = 0;
        Gear.WornGarment = Texts.TxtEmpty;
        Gear.Values[TGearItem.Armor] = 0;
        Gear.Values[TGearItem.Wearing] = 0;
        Assert.True(Equip(TBagItem.Tracksuit));
        Assert.False(Sell(TBagItem.Tracksuit));
        Assert.Equal(0, Bag.Items[TBagItem.Tracksuit]);
        Assert.Equal(0, Bag.Count);
        Assert.Equal(0, Player.Money);
        Assert.True(Unequip(TBagItem.Tracksuit));
        Assert.Equal(1, Bag.Items[TBagItem.Tracksuit]);
        Assert.Equal(1, Bag.Count);
    }

    [Fact]
    public void SprzedazZalozonegoGarnituruNieDziala() {
        Bag.Items[TBagItem.SpikedSuit] = 1;
        Bag.Count = 1;
        Player.Money = 0;
        Gear.WornGarment = Texts.TxtEmpty;
        Gear.Values[TGearItem.WornCount] = 0;
        Gear.Values[TGearItem.Armor] = 0;
        Gear.Values[TGearItem.LuckRoll] = 0;
        Assert.True(Equip(TBagItem.SpikedSuit));
        Assert.False(Sell(TBagItem.SpikedSuit));
        Assert.Equal(0, Bag.Items[TBagItem.SpikedSuit]);
        Assert.Equal(0, Bag.Count);
        Assert.Equal(0, Player.Money);
        Assert.True(Unequip(TBagItem.SpikedSuit));
        Assert.Equal(1, Bag.Items[TBagItem.SpikedSuit]);
        Assert.Equal(1, Bag.Count);
    }

    [Fact]
    public void KupnoMieczaDzialaPrzyPosiadanym() {
        Bag.Items[TBagItem.OldSword] = 1;
        Bag.Count = 1;
        Gear.Values[TGearItem.WeaponKind] = 0;
        Player.Stats[TStat.Strength] = 0;
        Gear.Values[TGearItem.LuckRoll] = 0;
        Player.Money = 200;
        Assert.True(Equip(TBagItem.OldSword));
        Assert.True(Buy(TBagItem.OldSword));
        Assert.Equal(130, Player.Money);
        Assert.Equal(1, Bag.Items[TBagItem.OldSword]);
        Assert.Equal(1, Bag.Count);
        Assert.True(Buy(TBagItem.OldSword));
        Assert.Equal(60, Player.Money);
        Assert.Equal(2, Bag.Items[TBagItem.OldSword]);
        Assert.Equal(2, Bag.Count);
        Assert.True(Unequip(TBagItem.OldSword));
        Assert.Equal(3, Bag.Items[TBagItem.OldSword]);
        Assert.Equal(3, Bag.Count);
    }

    [Fact]
    public void KupnoMalaButelkaManyPlaciCeneMimoNizszegoProgu() {
        Bag.Items[TBagItem.ManaBottle] = 0;
        Bag.Count = 0;
        Player.Money = 16;
        Assert.True(Buy(TBagItem.ManaBottle));
        Assert.Equal(-4, Player.Money);
        Assert.Equal(1, Bag.Items[TBagItem.ManaBottle]);
        Assert.Equal(1, Bag.Count);
    }

    [Fact]
    public void KupnoFajkiDodajeMadrosc() {
        Bag.Items[TBagItem.Pipe] = 0;
        Bag.Count = 0;
        Player.Money = 480;
        Player.Stats[TStat.Wisdom] = 10;
        Assert.True(Buy(TBagItem.Pipe));
        Assert.Equal(0, Player.Money);
        Assert.Equal(1, Bag.Items[TBagItem.Pipe]);
        Assert.Equal(11, Player.Stats[TStat.Wisdom]);
        Player.Stats[TStat.Wisdom] = 0;
    }

    [Fact]
    public void KupnoPaczkaPlaciCene() {
        Bag.Items[TBagItem.Doughnut] = 0;
        Bag.Count = 0;
        Player.Money = 8;
        Assert.True(Buy(TBagItem.Doughnut));
        Assert.Equal(0, Player.Money);
        Assert.Equal(1, Bag.Items[TBagItem.Doughnut]);
        Assert.Equal(1, Bag.Count);
    }

    [Fact]
    public void ZapisWczytujeLiczniki() {
        Player.Name = "COUNTS";
        Bag.Items[TBagItem.Beer] = 2;
        Bag.Items[TBagItem.Diploma] = 0;
        Bag.ItemRooms[TBagItem.Diploma] = 12;
        string path = SaveFileName();
        try {
            SaveCharacter();
            Bag.Items[TBagItem.Beer] = 0;
            Bag.ItemRooms[TBagItem.Diploma] = Rooms.RoomAny;
            LoadGame();
        } finally {
            File.Delete(path);
            Session.SavedFlag = false;
        }
        Assert.Equal(2, Bag.Items[TBagItem.Beer]);
        Assert.Equal(0, Bag.Items[TBagItem.Diploma]);
        Assert.Equal(12, Bag.ItemRooms[TBagItem.Diploma]);
        Assert.Equal(Rooms.RoomAny, Bag.ItemRooms[TBagItem.Beer]);
    }

    [Fact]
    public void BagLimitFollowsDexterity() {
        Player.Stats[TStat.Dexterity] = 10;
        Bag.Items[TBagItem.Backpack] = 0;
        RecalcBagLimit();
        Assert.Equal(10, Bag.Limit);
        Player.Stats[TStat.Dexterity] = 12;
        RecalcBagLimit();
        Assert.Equal(11, Bag.Limit);
        Player.Stats[TStat.Dexterity] = 25;
        RecalcBagLimit();
        Assert.Equal(16, Bag.Limit);
        Bag.Items[TBagItem.Backpack] = 1;
        RecalcBagLimit();
        Assert.Equal(20, Bag.Limit);
    }
}

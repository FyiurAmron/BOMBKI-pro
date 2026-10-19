using static Game;

namespace Bombki.Tests;

public class UnitsItemsTests {
    [Fact]
    public void BraniePicksUpSword() {
        Session.OuterRoom = 7;
        Bag.Items[TBagItem.OldSword] = 7;
        Bag.Count = 0;
        Command = "BIERZ STARY";
        TakeDrop();
        Assert.Equal(-10, Bag.Items[TBagItem.OldSword]);
        Assert.Equal(1, Bag.Count);
    }

    [Fact]
    public void BranieDropsSword() {
        Session.OuterRoom = 7;
        Bag.Items[TBagItem.OldSword] = -10;
        Bag.Count = 1;
        Command = "ODRZUC STARY";
        TakeDrop();
        Assert.Equal(7, Bag.Items[TBagItem.OldSword]);
        Assert.Equal(0, Bag.Count);
    }

    [Fact]
    public void BranieIgnoresMissingSword() {
        Session.OuterRoom = 7;
        Bag.Items[TBagItem.OldSword] = 0;
        Bag.Count = 0;
        Command = "BIERZ STARY";
        TakeDrop();
        Assert.Equal(0, Bag.Items[TBagItem.OldSword]);
        Assert.Equal(0, Bag.Count);
    }

    [Fact]
    public void BraniePicksUpBluszczItems() {
        Session.OuterRoom = 7;
        Bag.Items[TBagItem.SmallShield] = 7;
        Bag.Items[TBagItem.Heart] = 7;
        Bag.Items[TBagItem.Diploma] = 7;
        Bag.Items[TBagItem.Pipe] = 7;
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
        Assert.Equal(-10, Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(-10, Bag.Items[TBagItem.Heart]);
        Assert.Equal(-10, Bag.Items[TBagItem.Diploma]);
        Assert.Equal(-10, Bag.Items[TBagItem.Pipe]);
        Assert.Equal(4, Bag.Count);
        Assert.Equal(55, Player.MaxEnergy);
        Assert.Equal(1, Player.Stats[TStat.Wisdom]);
    }

    [Fact]
    public void BranieDropsBluszczItems() {
        Session.OuterRoom = 7;
        Bag.Items[TBagItem.SmallShield] = -10;
        Bag.Items[TBagItem.Heart] = -10;
        Bag.Items[TBagItem.Diploma] = -10;
        Bag.Items[TBagItem.Pipe] = -10;
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
        Assert.Equal(7, Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(7, Bag.Items[TBagItem.Heart]);
        Assert.Equal(7, Bag.Items[TBagItem.Diploma]);
        Assert.Equal(7, Bag.Items[TBagItem.Pipe]);
        Assert.Equal(0, Bag.Count);
        Assert.Equal(50, Player.MaxEnergy);
        Assert.Equal(0, Player.Stats[TStat.Wisdom]);
    }

    [Fact]
    public void UzywaSerce() {
        Bag.Items[TBagItem.Heart] = -10;
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
        Bag.Items[TBagItem.Heart] = -10;
        Player.Energy = 48;
        Player.MaxEnergy = 50;
        Bag.Count = 1;
        Command = "UZYJ SERCE";
        UseItems();
        Assert.Equal(50, Player.Energy);
    }

    [Fact]
    public void UzywaKomplet() {
        Bag.Items[TBagItem.Tracksuit] = -10;
        Gear.WornGarment = "";
        Gear.Values[TGearItem.Armor] = 0;
        Gear.Values[TGearItem.Wearing] = 0;
        Command = "UZYJ KOMPLET";
        UseItems();
        Assert.Equal(7, Gear.Values[TGearItem.Armor]);
        Assert.Equal(1, Gear.Values[TGearItem.Wearing]);
        Assert.Equal("SYF", Gear.WornGarment);
        Command = "ODLOZ KOMPLET";
        UseItems();
        Assert.Equal(0, Gear.Values[TGearItem.Armor]);
        Assert.Equal(0, Gear.Values[TGearItem.Wearing]);
        Assert.Equal("", Gear.WornGarment);
    }

    [Fact]
    public void UzywaKompletWorn() {
        Bag.Items[TBagItem.Tracksuit] = -10;
        Gear.WornGarment = "SYF";
        Gear.Values[TGearItem.Armor] = 0;
        Gear.Values[TGearItem.Wearing] = 1;
        Command = "UZYJ KOMPLET";
        UseItems();
        Assert.Equal(0, Gear.Values[TGearItem.Armor]);
        Assert.Equal(1, Gear.Values[TGearItem.Wearing]);
        Assert.Equal("SYF", Gear.WornGarment);
    }

    [Fact]
    public void UzywaGarnitur() {
        Bag.Items[TBagItem.SpikedSuit] = -10;
        Gear.WornGarment = "";
        Gear.Values[TGearItem.WornCount] = 0;
        Gear.Values[TGearItem.Armor] = 0;
        Gear.Values[TGearItem.LuckRoll] = 0;
        Command = "UZYJ GARNITUR";
        UseItems();
        Assert.Equal(10, Gear.Values[TGearItem.Armor]);
        Assert.Equal(1, Gear.Values[TGearItem.WornCount]);
        Assert.Equal(15, Gear.Values[TGearItem.LuckRoll]);
        Assert.Equal("GARNITUR", Gear.WornGarment);
        Command = "ODLOZ GARNITUR";
        UseItems();
        Assert.Equal(0, Gear.Values[TGearItem.Armor]);
        Assert.Equal(0, Gear.Values[TGearItem.WornCount]);
        Assert.Equal(0, Gear.Values[TGearItem.LuckRoll]);
        Assert.Equal("", Gear.WornGarment);
    }

    [Fact]
    public void UzywaButelkaMany() {
        Bag.Items[TBagItem.ManaBottle] = -10;
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
        Bag.Items[TBagItem.ManaBottle] = -10;
        Player.Mana = 45;
        Player.MaxMana = 50;
        Bag.Count = 1;
        Command = "UZYJ MALA BUTELKA MANY";
        UseItems();
        Assert.Equal(50, Player.Mana);
    }

    [Fact]
    public void NiszczyPrzepustke() {
        Bag.Permit = -10;
        Bag.Count = 0;
        Command = "ZNISZCZ PRZEPUSTKA";
        UseItems();
        Assert.Equal(0, Bag.Permit);
        Assert.Equal(1, Bag.Count);
    }

    [Fact]
    public void PatrzPrzepustke() {
        Bag.Permit = -10;
        Bag.Count = 0;
        Command = "PATRZ PRZEPUSTKA";
        UseItems();
        Assert.Equal(-10, Bag.Permit);
        Assert.Equal(0, Bag.Count);
    }

    [Fact]
    public void UzywaDyplom() {
        Bag.Items[TBagItem.Diploma] = -10;
        Command = "UZYJ DYPLOM";
        UseItems();
        Assert.Equal(-10, Bag.Items[TBagItem.Diploma]);
    }

    static void EatSetup(TBagItem item) {
        Bag.Items[item] = -10;
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
        Bag.Items[TBagItem.TeleportPill] = -10;
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
        Bag.Items[TBagItem.TeleportPill] = -10;
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
        Bag.Items[TBagItem.TeleportPill] = -10;
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
        Bag.Items[TBagItem.SpikedSuit] = 50;
        Bag.Count = 0;
        SuitGain();
        Assert.True(Bag.Items[TBagItem.SpikedSuit] == 50
            || Bag.Items[TBagItem.SpikedSuit] == 40);
        Assert.True(Bag.Count == 0 || Bag.Count == 1);
    }

    [Fact]
    public void PigulkaZyskInvariant() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.TeleportPill] = 50;
        Bag.Count = 0;
        PillGain();
        Assert.True(Bag.Items[TBagItem.TeleportPill] == 50
            || Bag.Items[TBagItem.TeleportPill] == 40);
        Assert.True(Bag.Count == 0 || Bag.Count == 1);
    }

    [Fact]
    public void KasetaZyskInvariant() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.Cassette] = 50;
        Player.MaxEnergy = 0;
        Gear.Values[TGearItem.Armor] = 0;
        Bag.Count = 0;
        Player.Stats[TStat.Dexterity] = 0;
        Session.Room = 20;
        CassetteGain();
        Assert.True(Bag.Items[TBagItem.Cassette] == 50
            || Bag.Items[TBagItem.Cassette] == 40);
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
        Bag.Items[TBagItem.LuckyLeaf] = 50;
        Bag.Count = 0;
        Player.MaxMana = 0;
        Session.Room = 20;
        LeafGain();
        Assert.True(Bag.Items[TBagItem.LuckyLeaf] == 50
            || Bag.Items[TBagItem.LuckyLeaf] == 40);
        Assert.True(Bag.Count == 0 || Bag.Count == 1);
        Assert.True(Player.MaxMana == 0 || Player.MaxMana == 40);
    }

    [Fact]
    public void ScrollPorZyskInvariant() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.CompareScroll] = 50;
        Bag.Count = 0;
        Session.Room = 20;
        ScrollGain();
        Assert.True(Bag.Items[TBagItem.CompareScroll] == 50
            || Bag.Items[TBagItem.CompareScroll] == 40);
        Assert.True(Bag.Count == 0 || Bag.Count == 1);
    }

    [Fact]
    public void GarniturZyskGain() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.SpikedSuit] = 100;
        Bag.Count = 0;
        int i = 0;
        while (Bag.Count == 0 && i < 2000) {
            SuitGain();
            i++;
        }
        Assert.True(Bag.Count > 0);
        Assert.Equal(90, Bag.Items[TBagItem.SpikedSuit]);
    }

    [Fact]
    public void PigulkaZyskGain() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.TeleportPill] = 100;
        Bag.Count = 0;
        int i = 0;
        while (Bag.Count == 0 && i < 2000) {
            PillGain();
            i++;
        }
        Assert.True(Bag.Count > 0);
        Assert.Equal(90, Bag.Items[TBagItem.TeleportPill]);
    }

    [Fact]
    public void KasetaZyskGain() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.Cassette] = 100;
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
        Assert.Equal(90, Bag.Items[TBagItem.Cassette]);
        Assert.Equal(5, Player.MaxEnergy);
        Assert.Equal(-8, Gear.Values[TGearItem.Armor]);
        Assert.Equal(1, Player.Stats[TStat.Dexterity]);
    }

    [Fact]
    public void ListekZyskGain() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.LuckyLeaf] = 100;
        Bag.Count = 0;
        Player.MaxMana = 0;
        Session.Room = 20;
        int i = 0;
        while (Bag.Count == 0 && i < 2000) {
            LeafGain();
            i++;
        }
        Assert.True(Bag.Count > 0);
        Assert.Equal(90, Bag.Items[TBagItem.LuckyLeaf]);
        Assert.Equal(40, Player.MaxMana);
    }

    [Fact]
    public void ScrollPorZyskGain() {
        UnitsSeed.Reseed();
        Bag.Items[TBagItem.CompareScroll] = 100;
        Bag.Count = 0;
        Session.Room = 20;
        int i = 0;
        while (Bag.Count == 0 && i < 2000) {
            ScrollGain();
            i++;
        }
        Assert.True(Bag.Count > 0);
        Assert.Equal(90, Bag.Items[TBagItem.CompareScroll]);
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
        Bag.Items[TBagItem.Backpack] = -ItemUnit;
        RecalcBagLimit();
        Assert.Equal(20, Bag.Limit);
    }
}

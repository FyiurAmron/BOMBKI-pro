
namespace Bombki.Tests;

public class UnitsItemsTests {
    readonly Game game = new();
    [Fact]
    public void BraniePicksUpSword() {
        game.Session.OuterRoom = 7;
        game.Bag.Items[TBagItem.OldSword] = 0;
        game.Bag.AddGroundItem(TBagItem.OldSword, 7);
        game.Bag.Count = 0;
        game.Command = "BIERZ STARY";
        game.Inventory.TakeDrop();
        Assert.Equal(1, game.Bag.Items[TBagItem.OldSword]);
        Assert.Empty(game.Bag.ItemRooms[TBagItem.OldSword]);
        Assert.Equal(1, game.Bag.Count);
    }

    [Fact]
    public void BranieDropsSword() {
        game.Session.OuterRoom = 7;
        game.Bag.Items[TBagItem.OldSword] = 1;
        game.Bag.Count = 1;
        game.Command = "ODRZUC STARY";
        game.Inventory.TakeDrop();
        Assert.Equal(0, game.Bag.Items[TBagItem.OldSword]);
        Assert.Contains(7, game.Bag.ItemRooms[TBagItem.OldSword]);
        Assert.Equal(0, game.Bag.Count);
    }

    [Fact]
    public void OdrzucanieNieDzialaNaWladanymMieczu() {
        game.Session.OuterRoom = 7;
        game.Bag.Items[TBagItem.OldSword] = 2;
        game.Bag.ItemRooms[TBagItem.OldSword].Clear();
        game.Bag.Count = 2;
        game.Gear.Values[TGearItem.WeaponKind] = 0;
        game.Player.Stats[TStat.Strength] = 0;
        game.Gear.Values[TGearItem.LuckRoll] = 0;
        Assert.True(game.Inventory.Equip(TBagItem.OldSword));
        game.Command = "ODRZUC STARY";
        game.Inventory.TakeDrop();
        Assert.Equal(1, game.Bag.Items[TBagItem.OldSword]);
        Assert.Empty(game.Bag.ItemRooms[TBagItem.OldSword]);
        Assert.Equal(1, game.Bag.Count);
        Assert.True(game.Inventory.Unequip(TBagItem.OldSword));
        Assert.Equal(2, game.Bag.Items[TBagItem.OldSword]);
        Assert.Equal(2, game.Bag.Count);
    }

    [Fact]
    public void OdrzucanieNieDzialaNaWladanejTarczy() {
        game.Session.OuterRoom = 7;
        game.Bag.Items[TBagItem.SmallShield] = 2;
        game.Bag.ItemRooms[TBagItem.SmallShield].Clear();
        game.Bag.Count = 2;
        game.Gear.Values[TGearItem.ShieldKind] = 0;
        game.Gear.Values[TGearItem.Armor] = 0;
        game.Gear.Values[TGearItem.WornCount] = 0;
        Assert.True(game.Inventory.Equip(TBagItem.SmallShield));
        game.Command = "ODRZUC MALA";
        game.Inventory.TakeDrop();
        Assert.Equal(1, game.Bag.Items[TBagItem.SmallShield]);
        Assert.Empty(game.Bag.ItemRooms[TBagItem.SmallShield]);
        Assert.Equal(1, game.Bag.Count);
        Assert.True(game.Inventory.Unequip(TBagItem.SmallShield));
        Assert.Equal(2, game.Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(2, game.Bag.Count);
    }

    [Fact]
    public void OdrzucanieBezSztukiNieDziala() {
        game.Session.OuterRoom = 7;
        game.Bag.Items[TBagItem.OldSword] = 0;
        game.Bag.ItemRooms[TBagItem.OldSword].Clear();
        game.Gear.Values[TGearItem.WeaponKind] = 0;
        game.Bag.Count = 0;
        game.Command = "ODRZUC STARY";
        game.Inventory.TakeDrop();
        Assert.Equal(0, game.Bag.Items[TBagItem.OldSword]);
        Assert.Empty(game.Bag.ItemRooms[TBagItem.OldSword]);
        Assert.Equal(0, game.Bag.Count);
    }

    [Fact]
    public void OdrzucanieStosuZostawiaReszte() {
        game.Session.OuterRoom = 7;
        game.Bag.Items[TBagItem.OldSword] = 2;
        game.Bag.ItemRooms[TBagItem.OldSword].Clear();
        game.Gear.Values[TGearItem.WeaponKind] = 0;
        game.Bag.Count = 2;
        game.Command = "ODRZUC STARY";
        game.Inventory.TakeDrop();
        Assert.Equal(1, game.Bag.Items[TBagItem.OldSword]);
        Assert.Contains(7, game.Bag.ItemRooms[TBagItem.OldSword]);
        Assert.Equal(1, game.Bag.Count);
    }

    [Fact]
    public void BranieJednejSztukiZostawiaInne() {
        game.Session.OuterRoom = 7;
        game.Bag.Items[TBagItem.OldSword] = 0;
        game.Bag.Count = 0;
        game.Bag.ItemRooms[TBagItem.OldSword].Clear();
        game.Bag.AddGroundItem(TBagItem.OldSword, 7);
        game.Bag.AddGroundItem(TBagItem.OldSword, 7);
        game.Command = "BIERZ STARY";
        game.Inventory.TakeDrop();
        Assert.Equal(1, game.Bag.Items[TBagItem.OldSword]);
        Assert.Equal(1, game.Bag.Count);
        Assert.Single(game.Bag.ItemRooms[TBagItem.OldSword]);
        game.Inventory.TakeDrop();
        Assert.Equal(2, game.Bag.Items[TBagItem.OldSword]);
        Assert.Equal(2, game.Bag.Count);
        Assert.Empty(game.Bag.ItemRooms[TBagItem.OldSword]);
    }

    [Fact]
    public void OdrzucanieWieluSztuk() {
        game.Session.OuterRoom = 7;
        game.Bag.Items[TBagItem.OldSword] = 2;
        game.Bag.Count = 2;
        game.Bag.ItemRooms[TBagItem.OldSword].Clear();
        game.Gear.Values[TGearItem.WeaponKind] = 0;
        game.Command = "ODRZUC STARY";
        game.Inventory.TakeDrop();
        game.Inventory.TakeDrop();
        Assert.Equal(0, game.Bag.Items[TBagItem.OldSword]);
        Assert.Equal(0, game.Bag.Count);
        Assert.Equal(2, game.Bag.ItemRooms[TBagItem.OldSword].Count);
        Assert.All(game.Bag.ItemRooms[TBagItem.OldSword], room => Assert.Equal(7, room));
        game.Bag.ItemRooms[TBagItem.OldSword].Clear();
    }

    [Fact]
    public void WladaMieczZeStosu() {
        game.Bag.Items[TBagItem.OldSword] = 2;
        game.Bag.Count = 2;
        game.Gear.Values[TGearItem.WeaponKind] = 0;
        game.Player.Stats[TStat.Strength] = 10;
        game.Gear.Values[TGearItem.LuckRoll] = 0;
        game.CmdWieldOldSword();
        Assert.Equal(1, game.Gear.Values[TGearItem.WeaponKind]);
        Assert.Equal(1, game.Bag.Items[TBagItem.OldSword]);
        Assert.Equal(1, game.Bag.Count);
        game.CmdUnwieldOldSword();
        Assert.Equal(0, game.Gear.Values[TGearItem.WeaponKind]);
        Assert.Equal(2, game.Bag.Items[TBagItem.OldSword]);
        Assert.Equal(2, game.Bag.Count);
        game.Player.Stats[TStat.Strength] = 0;
        game.Gear.Values[TGearItem.LuckRoll] = 0;
    }

    [Fact]
    public void WladaTarczePrzenosiJaDoZalozenia() {
        game.Bag.Items[TBagItem.SmallShield] = 1;
        game.Bag.Count = 1;
        game.Gear.Values[TGearItem.ShieldKind] = 0;
        game.Gear.Values[TGearItem.Armor] = 0;
        game.Gear.Values[TGearItem.WornCount] = 0;
        game.CmdWieldSmallShield();
        Assert.Equal(1, game.Gear.Values[TGearItem.ShieldKind]);
        Assert.Equal(0, game.Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(0, game.Bag.Count);
        Assert.Equal(15, game.Gear.Values[TGearItem.Armor]);
        game.CmdUnwieldSmallShield();
        Assert.Equal(0, game.Gear.Values[TGearItem.ShieldKind]);
        Assert.Equal(1, game.Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(1, game.Bag.Count);
        Assert.Equal(0, game.Gear.Values[TGearItem.Armor]);
        Assert.Equal(0, game.Gear.Values[TGearItem.WornCount]);
    }

    [Fact]
    public void LootDropsWhenTheItemIsAlreadyCarried() {
        game.Bag.Items[TBagItem.OldSword] = 1;
        game.Bag.Count = 1;
        game.Bag.ItemRooms[TBagItem.OldSword].Clear();
        game.Session.Room = 7;
        game.Combat.TryLoot(TBagItem.OldSword, 100, "");
        Assert.Contains(7, game.Bag.ItemRooms[TBagItem.OldSword]);
        game.Bag.Items[TBagItem.OldSword] = 0;
        game.Bag.Count = 0;
        game.Bag.ItemRooms[TBagItem.OldSword].Clear();
    }

    [Fact]
    public void LootDodajeKolejnyPrzedmiotNaZiemi() {
        game.Session.Room = 7;
        game.Bag.ItemRooms[TBagItem.Heart].Clear();
        game.Bag.AddGroundItem(TBagItem.Heart, 7);
        game.Bag.AddGroundItem(TBagItem.Heart, 8);
        game.Combat.TryLoot(TBagItem.Heart, 100, "");
        Assert.Equal(3, game.Bag.ItemRooms[TBagItem.Heart].Count);
        Assert.Equal(2, game.Bag.ItemRooms[TBagItem.Heart].Count(room => room == 7));
        Assert.Contains(8, game.Bag.ItemRooms[TBagItem.Heart]);
        game.Bag.ItemRooms[TBagItem.Heart].Clear();
    }

    [Fact]
    public void BranieIgnoresMissingSword() {
        game.Session.OuterRoom = 7;
        game.Bag.Items[TBagItem.OldSword] = 0;
        game.Bag.ItemRooms[TBagItem.OldSword].Clear();
        game.Bag.Count = 0;
        game.Command = "BIERZ STARY";
        game.Inventory.TakeDrop();
        Assert.Equal(0, game.Bag.Items[TBagItem.OldSword]);
        Assert.Equal(0, game.Bag.Count);
    }

    [Fact]
    public void BranieIgnoresStartRoomWhenNothingLiesThere() {
        game.Session.OuterRoom = Rooms.RoomStart;
        game.Bag.Items[TBagItem.Diploma] = 0;
        game.Bag.ItemRooms[TBagItem.Diploma].Clear();
        game.Bag.Count = 0;
        game.Command = "BIERZ DYPLOM";
        game.Inventory.TakeDrop();
        Assert.Equal(0, game.Bag.Items[TBagItem.Diploma]);
        Assert.Empty(game.Bag.ItemRooms[TBagItem.Diploma]);
        Assert.Equal(0, game.Bag.Count);
    }

    [Fact]
    public void BraniePicksUpBluszczItems() {
        game.Session.OuterRoom = 7;
        game.Bag.Items[TBagItem.SmallShield] = 0;
        game.Bag.Items[TBagItem.Heart] = 0;
        game.Bag.Items[TBagItem.Diploma] = 0;
        game.Bag.Items[TBagItem.Pipe] = 0;
        game.Bag.AddGroundItem(TBagItem.SmallShield, 7);
        game.Bag.AddGroundItem(TBagItem.Heart, 7);
        game.Bag.AddGroundItem(TBagItem.Diploma, 7);
        game.Bag.AddGroundItem(TBagItem.Pipe, 7);
        game.Bag.Count = 0;
        game.Player.Stats[TStat.Wisdom] = 0;
        game.Player.Stats[TStat.MaxWisdom] = 5;
        game.Player.MaxEnergy = 50;
        game.Command = "BIERZ MALA";
        game.Inventory.TakeDrop();
        game.Command = "BIERZ SERCE";
        game.Inventory.TakeDrop();
        game.Command = "BIERZ DYPLOM";
        game.Inventory.TakeDrop();
        game.Command = "BIERZ FAJKA";
        game.Inventory.TakeDrop();
        Assert.Equal(1, game.Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(1, game.Bag.Items[TBagItem.Heart]);
        Assert.Equal(1, game.Bag.Items[TBagItem.Diploma]);
        Assert.Equal(1, game.Bag.Items[TBagItem.Pipe]);
        Assert.Equal(4, game.Bag.Count);
        Assert.Equal(55, game.Player.MaxEnergy);
        Assert.Equal(1, game.Player.Stats[TStat.Wisdom]);
    }

    [Fact]
    public void BranieDropsBluszczItems() {
        game.Session.OuterRoom = 7;
        game.Bag.Items[TBagItem.SmallShield] = 1;
        game.Bag.Items[TBagItem.Heart] = 1;
        game.Bag.Items[TBagItem.Diploma] = 1;
        game.Bag.Items[TBagItem.Pipe] = 1;
        game.Bag.Count = 4;
        game.Player.Stats[TStat.Wisdom] = 1;
        game.Player.MaxEnergy = 55;
        game.Command = "ODRZUC FAJKA";
        game.Inventory.TakeDrop();
        game.Command = "ODRZUC MALA";
        game.Inventory.TakeDrop();
        game.Command = "ODRZUC SERCE";
        game.Inventory.TakeDrop();
        game.Command = "ODRZUC DYPLOM";
        game.Inventory.TakeDrop();
        Assert.Equal(0, game.Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(0, game.Bag.Items[TBagItem.Heart]);
        Assert.Equal(0, game.Bag.Items[TBagItem.Diploma]);
        Assert.Equal(0, game.Bag.Items[TBagItem.Pipe]);
        Assert.Contains(7, game.Bag.ItemRooms[TBagItem.SmallShield]);
        Assert.Contains(7, game.Bag.ItemRooms[TBagItem.Heart]);
        Assert.Contains(7, game.Bag.ItemRooms[TBagItem.Diploma]);
        Assert.Contains(7, game.Bag.ItemRooms[TBagItem.Pipe]);
        Assert.Equal(0, game.Bag.Count);
        Assert.Equal(50, game.Player.MaxEnergy);
        Assert.Equal(0, game.Player.Stats[TStat.Wisdom]);
    }

    [Fact]
    public void PipeModifiersRoundTripAtWisdomCap() {
        game.Session.OuterRoom = 7;
        game.Bag.Items[TBagItem.Pipe] = 0;
        game.Bag.ItemRooms[TBagItem.Pipe].Clear();
        game.Bag.Count = 0;
        game.Bag.AddGroundItem(TBagItem.Pipe, 7);
        game.Player.Stats[TStat.Wisdom] = 5;
        game.Player.Stats[TStat.MaxWisdom] = 5;
        game.Command = "BIERZ FAJKA";
        game.Inventory.TakeDrop();
        Assert.Equal(6, game.Player.Stats[TStat.Wisdom]);
        Assert.Equal(1, game.Bag.Count);
        game.Command = "ODRZUC FAJKA";
        game.Inventory.TakeDrop();
        Assert.Equal(5, game.Player.Stats[TStat.Wisdom]);
        Assert.Equal(0, game.Bag.Count);
        game.Bag.ItemRooms[TBagItem.Pipe].Clear();
        game.Player.Stats[TStat.Wisdom] = 0;
        game.Player.Stats[TStat.MaxWisdom] = 0;
    }

    [Fact]
    public void PipeBonusAppliesOnceForAStack() {
        game.Bag.Items[TBagItem.Pipe] = 0;
        game.Bag.Count = 0;
        game.Player.Stats[TStat.Wisdom] = 5;
        game.Player.Stats[TStat.MaxWisdom] = 5;
        game.Inventory.AcquireItem(TBagItem.Pipe, 2);
        Assert.Equal(2, game.Bag.Items[TBagItem.Pipe]);
        Assert.Equal(2, game.Bag.Count);
        Assert.Equal(6, game.Player.Stats[TStat.Wisdom]);
        Assert.True(game.Inventory.LoseItem(TBagItem.Pipe));
        Assert.Equal(1, game.Bag.Items[TBagItem.Pipe]);
        Assert.Equal(1, game.Bag.Count);
        Assert.Equal(6, game.Player.Stats[TStat.Wisdom]);
        Assert.True(game.Inventory.LoseItem(TBagItem.Pipe));
        Assert.Equal(0, game.Bag.Items[TBagItem.Pipe]);
        Assert.Equal(0, game.Bag.Count);
        Assert.Equal(5, game.Player.Stats[TStat.Wisdom]);
        game.Player.Stats[TStat.Wisdom] = 0;
        game.Player.Stats[TStat.MaxWisdom] = 0;
    }

    [Fact]
    public void UzywaSerce() {
        game.Bag.Items[TBagItem.Heart] = 1;
        game.Player.Energy = 10;
        game.Player.MaxEnergy = 50;
        game.Bag.Count = 1;
        game.Command = "UZYJ SERCE";
        game.Inventory.UseItems();
        Assert.Equal(0, game.Bag.Items[TBagItem.Heart]);
        Assert.Equal(15, game.Player.Energy);
        Assert.Equal(0, game.Bag.Count);
    }

    [Fact]
    public void UzywaSerceClampsAtMaxE() {
        game.Bag.Items[TBagItem.Heart] = 1;
        game.Player.Energy = 48;
        game.Player.MaxEnergy = 50;
        game.Bag.Count = 1;
        game.Command = "UZYJ SERCE";
        game.Inventory.UseItems();
        Assert.Equal(50, game.Player.Energy);
    }

    [Fact]
    public void UzywaSerceZuzywaJedno() {
        game.Bag.Items[TBagItem.Heart] = 2;
        game.Player.Energy = 10;
        game.Player.MaxEnergy = 50;
        game.Bag.Count = 2;
        game.Command = "UZYJ SERCE";
        game.Inventory.UseItems();
        Assert.Equal(1, game.Bag.Items[TBagItem.Heart]);
        Assert.Equal(15, game.Player.Energy);
        Assert.Equal(1, game.Bag.Count);
    }

    [Fact]
    public void UzywaKomplet() {
        game.Bag.Items[TBagItem.Tracksuit] = 1;
        game.Bag.Count = 1;
        game.Gear.WornGarment = Texts.TxtEmpty;
        game.Gear.Values[TGearItem.Armor] = 0;
        game.Gear.Values[TGearItem.Wearing] = 0;
        game.Command = "UZYJ KOMPLET";
        game.Inventory.UseItems();
        Assert.Equal(7, game.Gear.Values[TGearItem.Armor]);
        Assert.Equal(1, game.Gear.Values[TGearItem.Wearing]);
        Assert.Equal(Texts.TxtWornSyf, game.Gear.WornGarment);
        Assert.Equal(0, game.Bag.Items[TBagItem.Tracksuit]);
        Assert.Equal(0, game.Bag.Count);
        game.Command = "ODLOZ KOMPLET";
        game.Inventory.UseItems();
        Assert.Equal(0, game.Gear.Values[TGearItem.Armor]);
        Assert.Equal(0, game.Gear.Values[TGearItem.Wearing]);
        Assert.Equal(Texts.TxtEmpty, game.Gear.WornGarment);
        Assert.Equal(1, game.Bag.Items[TBagItem.Tracksuit]);
        Assert.Equal(1, game.Bag.Count);
    }

    [Fact]
    public void UzywaKompletWorn() {
        game.Bag.Items[TBagItem.Tracksuit] = 1;
        game.Bag.Count = 1;
        game.Gear.WornGarment = Texts.TxtWornGarnitur;
        game.Gear.Values[TGearItem.Armor] = 0;
        game.Gear.Values[TGearItem.Wearing] = 1;
        game.Command = "UZYJ KOMPLET";
        game.Inventory.UseItems();
        Assert.Equal(0, game.Gear.Values[TGearItem.Armor]);
        Assert.Equal(1, game.Gear.Values[TGearItem.Wearing]);
        Assert.Equal(Texts.TxtWornGarnitur, game.Gear.WornGarment);
        Assert.Equal(1, game.Bag.Items[TBagItem.Tracksuit]);
        Assert.Equal(1, game.Bag.Count);
        game.Gear.WornGarment = Texts.TxtEmpty;
        game.Gear.Values[TGearItem.Wearing] = 0;
    }

    [Fact]
    public void UzywaGarnitur() {
        game.Bag.Items[TBagItem.SpikedSuit] = 1;
        game.Bag.Count = 1;
        game.Gear.WornGarment = Texts.TxtEmpty;
        game.Gear.Values[TGearItem.WornCount] = 0;
        game.Gear.Values[TGearItem.Armor] = 0;
        game.Gear.Values[TGearItem.LuckRoll] = 0;
        game.Command = "UZYJ GARNITUR";
        game.Inventory.UseItems();
        Assert.Equal(10, game.Gear.Values[TGearItem.Armor]);
        Assert.Equal(1, game.Gear.Values[TGearItem.WornCount]);
        Assert.Equal(15, game.Gear.Values[TGearItem.LuckRoll]);
        Assert.Equal(Texts.TxtWornGarnitur, game.Gear.WornGarment);
        Assert.Equal(0, game.Bag.Items[TBagItem.SpikedSuit]);
        Assert.Equal(0, game.Bag.Count);
        game.Command = "ODLOZ GARNITUR";
        game.Inventory.UseItems();
        Assert.Equal(0, game.Gear.Values[TGearItem.Armor]);
        Assert.Equal(0, game.Gear.Values[TGearItem.WornCount]);
        Assert.Equal(0, game.Gear.Values[TGearItem.LuckRoll]);
        Assert.Equal(Texts.TxtEmpty, game.Gear.WornGarment);
        Assert.Equal(1, game.Bag.Items[TBagItem.SpikedSuit]);
        Assert.Equal(1, game.Bag.Count);
    }

    [Fact]
    public void UzywaButelkaMany() {
        game.Bag.Items[TBagItem.ManaBottle] = 1;
        game.Player.Mana = 10;
        game.Player.MaxMana = 50;
        game.Bag.Count = 1;
        game.Command = "UZYJ MALA BUTELKA MANY";
        game.Inventory.UseItems();
        Assert.Equal(40, game.Player.Mana);
        Assert.Equal(0, game.Bag.Count);
    }

    [Fact]
    public void UzywaButelkaManyClampsAtMaxMana() {
        game.Bag.Items[TBagItem.ManaBottle] = 1;
        game.Player.Mana = 45;
        game.Player.MaxMana = 50;
        game.Bag.Count = 1;
        game.Command = "UZYJ MALA BUTELKA MANY";
        game.Inventory.UseItems();
        Assert.Equal(50, game.Player.Mana);
    }

    [Fact]
    public void PorownanieScrollNieDzialaBezMany() {
        game.Bag.Items[TBagItem.CompareScroll] = 1;
        game.Bag.Count = 1;
        game.Player.MaxMana = 100;
        game.Player.Mana = 0;
        game.CmdUseCompareScroll();
        Assert.Equal(0, game.Player.Mana);
        Assert.Equal(1, game.Bag.Items[TBagItem.CompareScroll]);
        Assert.Equal(1, game.Bag.Count);
    }

    [Fact]
    public void ScrollPowrotNieDzialaBezMany() {
        game.Bag.Items[TBagItem.ReturnScroll] = 1;
        game.Bag.Count = 1;
        game.Player.MaxMana = 100;
        game.Player.Mana = 0;
        game.Session.Room = Rooms.RoomCityCentre;
        game.Command = "UZYJ SCROLL POWROT";
        game.CmdOpenPortal();
        Assert.Equal(Rooms.RoomCityCentre, game.Session.Room);
        Assert.Equal(0, game.Player.Mana);
        Assert.Equal(1, game.Bag.Items[TBagItem.ReturnScroll]);
        Assert.Equal(1, game.Bag.Count);
    }

    [Fact]
    public void NiszczyPrzepustke() {
        game.Bag.Permit = 1;
        game.Bag.Count = 1;
        game.Command = "ZNISZCZ PRZEPUSTKA";
        game.Inventory.UseItems();
        Assert.Equal(0, game.Bag.Permit);
        Assert.Equal(0, game.Bag.Count);
    }

    [Fact]
    public void PrzepustkiStackAndEachUsesOneBagSlot() {
        game.Bag.Permit = 0;
        game.Bag.Count = 0;
        game.Inventory.AcquirePermit();
        game.Inventory.AcquirePermit();
        Assert.Equal(2, game.Bag.Permit);
        Assert.Equal(2, game.Bag.Count);
        Assert.True(game.Inventory.LosePermit());
        Assert.Equal(1, game.Bag.Permit);
        Assert.Equal(1, game.Bag.Count);
        Assert.True(game.Inventory.LosePermit());
        Assert.Equal(0, game.Bag.Permit);
        Assert.Equal(0, game.Bag.Count);
        Assert.False(game.Inventory.LosePermit());
    }

    [Fact]
    public void PatrzPrzepustke() {
        game.Bag.Permit = 1;
        game.Bag.Count = 1;
        game.Command = "PATRZ PRZEPUSTKA";
        game.Inventory.UseItems();
        Assert.Equal(1, game.Bag.Permit);
        Assert.Equal(1, game.Bag.Count);
        game.Bag.Permit = 0;
        game.Bag.Count = 0;
    }

    [Fact]
    public void UzywaDyplom() {
        game.Bag.Items[TBagItem.Diploma] = 1;
        game.Command = "UZYJ DYPLOM";
        game.Inventory.UseItems();
        Assert.Equal(1, game.Bag.Items[TBagItem.Diploma]);
    }

    void EatSetup(TBagItem item) {
        game.Bag.Items[item] = 1;
        game.Player.Energy = 10;
        game.Player.MaxEnergy = 50;
        game.Bag.Count = 1;
    }

    [Fact]
    public void UzywaPaczek() {
        EatSetup(TBagItem.Doughnut);
        game.Command = "UZYJ PACZEK";
        game.Inventory.UseItems();
        Assert.Equal(0, game.Bag.Items[TBagItem.Doughnut]);
        Assert.Equal(18, game.Player.Energy);
        Assert.Equal(0, game.Bag.Count);
    }

    [Fact]
    public void UzywaCiastko() {
        EatSetup(TBagItem.Cookie);
        game.Command = "UZYJ CIASTKO";
        game.Inventory.UseItems();
        Assert.Equal(0, game.Bag.Items[TBagItem.Cookie]);
        Assert.Equal(22, game.Player.Energy);
    }

    [Fact]
    public void UzywaSuchaRacja() {
        EatSetup(TBagItem.DryRation);
        game.Command = "UZYJ SUCHA RACJA";
        game.Inventory.UseItems();
        Assert.Equal(0, game.Bag.Items[TBagItem.DryRation]);
        Assert.Equal(26, game.Player.Energy);
    }

    [Fact]
    public void UzywaBulka() {
        EatSetup(TBagItem.BreadRoll);
        game.Command = "UZYJ BULKA";
        game.Inventory.UseItems();
        Assert.Equal(0, game.Bag.Items[TBagItem.BreadRoll]);
        Assert.Equal(30, game.Player.Energy);
    }

    [Fact]
    public void UzywaChleb() {
        EatSetup(TBagItem.Bread);
        game.Command = "UZYJ CHLEB";
        game.Inventory.UseItems();
        Assert.Equal(0, game.Bag.Items[TBagItem.Bread]);
        Assert.Equal(36, game.Player.Energy);
    }

    [Fact]
    public void UzyjaWeka() {
        EatSetup(TBagItem.Sausage);
        game.Command = "UZYJ WEKA";
        game.Inventory.UseItems();
        Assert.Equal(0, game.Bag.Items[TBagItem.Sausage]);
        Assert.Equal(44, game.Player.Energy);
    }

    [Fact]
    public void UzywaBigos() {
        EatSetup(TBagItem.Bigos);
        game.Command = "UZYJ BIGOS";
        game.Inventory.UseItems();
        Assert.Equal(0, game.Bag.Items[TBagItem.Bigos]);
        Assert.Equal(30, game.Player.Energy);
    }

    [Fact]
    public void UzywaPigulkaLow() {
        game.Bag.Items[TBagItem.TeleportPill] = 1;
        game.Player.Stats[TStat.Wisdom] = 5;
        game.Player.MaxEnergy = 50;
        game.Player.Energy = 40;
        game.Player.Experience = 100;
        game.Command = "UZYJ PIGULKA";
        game.Inventory.UseItems();
        Assert.Equal(0, game.Bag.Items[TBagItem.TeleportPill]);
        Assert.Equal(1, game.Player.Energy);
        Assert.Equal(49, game.Player.MaxEnergy);
        Assert.Equal(50, game.Player.Experience);
    }

    [Fact]
    public void UzywaPigulkaMid() {
        game.Bag.Items[TBagItem.TeleportPill] = 1;
        game.Player.Stats[TStat.Wisdom] = 12;
        game.Player.MaxEnergy = 100;
        game.Player.Energy = 100;
        game.Player.Experience = 100;
        game.Command = "UZYJ PIGULKA";
        game.Inventory.UseItems();
        Assert.Equal(60, game.Player.Energy);
        Assert.Equal(70, game.Player.Experience);
    }

    [Fact]
    public void UzywaPigulkaHigh() {
        game.Bag.Items[TBagItem.TeleportPill] = 1;
        game.Bag.Count = 1;
        game.Player.Stats[TStat.Wisdom] = 20;
        game.Player.MaxEnergy = 100;
        game.Player.Energy = 100;
        game.Player.Experience = 100;
        game.Command = "UZYJ PIGULKA";
        game.Inventory.UseItems();
        Assert.Equal(0, game.Bag.Items[TBagItem.TeleportPill]);
        Assert.Equal(0, game.Bag.Count);
        Assert.Equal(100, game.Player.Energy);
        Assert.Equal(100, game.Player.Experience);
    }

    [Fact]
    public void GarniturZyskInvariant() {
        UnitsSeed.Reseed(game);
        game.Bag.Items[TBagItem.SpikedSuit] = 0;
        game.Bag.Count = 0;
        game.Inventory.SuitGain();
        Assert.True(game.Bag.Items[TBagItem.SpikedSuit] == 0
            || game.Bag.Items[TBagItem.SpikedSuit] == 1);
        Assert.True(game.Bag.Count == 0 || game.Bag.Count == 1);
    }

    [Fact]
    public void PigulkaZyskInvariant() {
        UnitsSeed.Reseed(game);
        game.Bag.Items[TBagItem.TeleportPill] = 0;
        game.Bag.Count = 0;
        game.Inventory.PillGain();
        Assert.True(game.Bag.Items[TBagItem.TeleportPill] == 0
            || game.Bag.Items[TBagItem.TeleportPill] == 1);
        Assert.True(game.Bag.Count == 0 || game.Bag.Count == 1);
    }

    [Fact]
    public void KasetaZyskInvariant() {
        UnitsSeed.Reseed(game);
        game.Bag.Items[TBagItem.Cassette] = 0;
        game.Player.MaxEnergy = 0;
        game.Gear.Values[TGearItem.Armor] = 0;
        game.Bag.Count = 0;
        game.Player.Stats[TStat.Dexterity] = 0;
        game.Session.Room = 20;
        game.Inventory.CassetteGain();
        Assert.True(game.Bag.Items[TBagItem.Cassette] == 0
            || game.Bag.Items[TBagItem.Cassette] == 1);
        Assert.True(game.Player.MaxEnergy == 0 || game.Player.MaxEnergy == 5);
        Assert.True(game.Gear.Values[TGearItem.Armor] == 0
            || game.Gear.Values[TGearItem.Armor] == -8);
        Assert.True(game.Bag.Count == 0 || game.Bag.Count == 1);
        Assert.True(game.Player.Stats[TStat.Dexterity] == 0
            || game.Player.Stats[TStat.Dexterity] == 1);
    }

    [Fact]
    public void ListekZyskInvariant() {
        UnitsSeed.Reseed(game);
        game.Bag.Items[TBagItem.LuckyLeaf] = 0;
        game.Bag.Count = 0;
        game.Player.MaxMana = 0;
        game.Session.Room = 20;
        game.Inventory.LeafGain();
        Assert.True(game.Bag.Items[TBagItem.LuckyLeaf] == 0
            || game.Bag.Items[TBagItem.LuckyLeaf] == 1);
        Assert.True(game.Bag.Count == 0 || game.Bag.Count == 1);
        Assert.True(game.Player.MaxMana == 0 || game.Player.MaxMana == 40);
    }

    [Fact]
    public void ScrollPorZyskInvariant() {
        UnitsSeed.Reseed(game);
        game.Bag.Items[TBagItem.CompareScroll] = 0;
        game.Bag.Count = 0;
        game.Session.Room = 20;
        game.Inventory.ScrollGain();
        Assert.True(game.Bag.Items[TBagItem.CompareScroll] == 0
            || game.Bag.Items[TBagItem.CompareScroll] == 1);
        Assert.True(game.Bag.Count == 0 || game.Bag.Count == 1);
    }

    [Fact]
    public void GarniturZyskGain() {
        UnitsSeed.Reseed(game);
        game.Bag.Items[TBagItem.SpikedSuit] = 0;
        game.Bag.Count = 0;
        int i = 0;
        while (game.Bag.Count == 0 && i < 2000) {
            game.Inventory.SuitGain();
            i++;
        }
        Assert.True(game.Bag.Count > 0);
        Assert.Equal(1, game.Bag.Items[TBagItem.SpikedSuit]);
    }

    [Fact]
    public void PigulkaZyskGain() {
        UnitsSeed.Reseed(game);
        game.Bag.Items[TBagItem.TeleportPill] = 0;
        game.Bag.Count = 0;
        int i = 0;
        while (game.Bag.Count == 0 && i < 2000) {
            game.Inventory.PillGain();
            i++;
        }
        Assert.True(game.Bag.Count > 0);
        Assert.Equal(1, game.Bag.Items[TBagItem.TeleportPill]);
    }

    [Fact]
    public void KasetaZyskGain() {
        UnitsSeed.Reseed(game);
        game.Bag.Items[TBagItem.Cassette] = 0;
        game.Player.MaxEnergy = 0;
        game.Gear.Values[TGearItem.Armor] = 0;
        game.Bag.Count = 0;
        game.Player.Stats[TStat.Dexterity] = 0;
        game.Session.Room = 20;
        int i = 0;
        while (game.Bag.Count == 0 && i < 2000) {
            game.Inventory.CassetteGain();
            i++;
        }
        Assert.True(game.Bag.Count > 0);
        Assert.Equal(1, game.Bag.Items[TBagItem.Cassette]);
        Assert.Equal(5, game.Player.MaxEnergy);
        Assert.Equal(-8, game.Gear.Values[TGearItem.Armor]);
        Assert.Equal(1, game.Player.Stats[TStat.Dexterity]);
    }

    [Fact]
    public void ListekZyskGain() {
        UnitsSeed.Reseed(game);
        game.Bag.Items[TBagItem.LuckyLeaf] = 0;
        game.Bag.Count = 0;
        game.Player.MaxMana = 0;
        game.Session.Room = 20;
        int i = 0;
        while (game.Bag.Count == 0 && i < 2000) {
            game.Inventory.LeafGain();
            i++;
        }
        Assert.True(game.Bag.Count > 0);
        Assert.Equal(1, game.Bag.Items[TBagItem.LuckyLeaf]);
        Assert.Equal(40, game.Player.MaxMana);
    }

    [Fact]
    public void ScrollPorZyskGain() {
        UnitsSeed.Reseed(game);
        game.Bag.Items[TBagItem.CompareScroll] = 0;
        game.Bag.Count = 0;
        game.Session.Room = 20;
        int i = 0;
        while (game.Bag.Count == 0 && i < 2000) {
            game.Inventory.ScrollGain();
            i++;
        }
        Assert.True(game.Bag.Count > 0);
        Assert.Equal(1, game.Bag.Items[TBagItem.CompareScroll]);
    }

    [Fact]
    public void SprzedazMieczaZeStosu() {
        game.Bag.Items[TBagItem.OldSword] = 2;
        game.Bag.Count = 2;
        game.Gear.Values[TGearItem.WeaponKind] = 0;
        game.Player.Money = 0;
        Assert.True(game.World.Sell(TBagItem.OldSword));
        Assert.Equal(1, game.Bag.Items[TBagItem.OldSword]);
        Assert.Equal(1, game.Bag.Count);
        Assert.Equal(30, game.Player.Money);
    }

    [Fact]
    public void SprzedazTarczyZeStosu() {
        game.Bag.Items[TBagItem.SmallShield] = 2;
        game.Bag.Count = 2;
        game.Gear.Values[TGearItem.ShieldKind] = 0;
        game.Player.Money = 0;
        Assert.True(game.World.Sell(TBagItem.SmallShield));
        Assert.Equal(1, game.Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(1, game.Bag.Count);
        Assert.Equal(8, game.Player.Money);
    }

    [Fact]
    public void SprzedazTarczyBezSztukiNieDziala() {
        game.Bag.Items[TBagItem.SmallShield] = 0;
        game.Bag.Count = 0;
        game.Player.Money = 0;
        game.Gear.Values[TGearItem.ShieldKind] = 0;
        Assert.False(game.World.Sell(TBagItem.SmallShield));
        Assert.Equal(0, game.Bag.Count);
        Assert.Equal(0, game.Player.Money);
    }

    [Fact]
    public void SprzedazPigulkiBezSztukiNieDziala() {
        game.Bag.Items[TBagItem.TeleportPill] = 0;
        game.Bag.Count = 0;
        game.Player.Money = 0;
        Assert.False(game.World.Sell(TBagItem.TeleportPill));
        Assert.Equal(0, game.Bag.Count);
        Assert.Equal(0, game.Player.Money);
    }

    [Fact]
    public void SprzedazFajkiZeStosu() {
        game.Bag.Items[TBagItem.Pipe] = 2;
        game.Bag.Count = 2;
        game.Player.Money = 0;
        game.Player.Stats[TStat.Wisdom] = 10;
        Assert.True(game.World.Sell(TBagItem.Pipe));
        Assert.Equal(1, game.Bag.Items[TBagItem.Pipe]);
        Assert.Equal(1, game.Bag.Count);
        Assert.Equal(120, game.Player.Money);
        Assert.Equal(10, game.Player.Stats[TStat.Wisdom]);
        Assert.True(game.World.Sell(TBagItem.Pipe));
        Assert.Equal(0, game.Bag.Items[TBagItem.Pipe]);
        Assert.Equal(0, game.Bag.Count);
        Assert.Equal(240, game.Player.Money);
        Assert.Equal(9, game.Player.Stats[TStat.Wisdom]);
        game.Player.Stats[TStat.Wisdom] = 0;
        game.Bag.Permit = 0;
        game.Bag.Count = 0;
        game.Player.Money = 0;
    }

    [Fact]
    public void SprzedazListkaWymagaListka() {
        game.Bag.Items[TBagItem.LuckyLeaf] = 0;
        game.Bag.Count = 0;
        game.Player.Money = 1000;
        game.Player.MaxMana = 40;
        Assert.False(game.World.Sell(TBagItem.LuckyLeaf));
        game.Bag.Items[TBagItem.LuckyLeaf] = 1;
        game.Bag.Count = 1;
        Assert.True(game.World.Sell(TBagItem.LuckyLeaf));
        Assert.Equal(1400, game.Player.Money);
        Assert.Equal(0, game.Player.MaxMana);
    }

    [Fact]
    public void SprzedazKasetyRevertsCarryModifiers() {
        game.Bag.Items[TBagItem.Cassette] = 1;
        game.Bag.Count = 1;
        game.Player.Money = 0;
        game.Player.MaxEnergy = 55;
        game.Player.Stats[TStat.Dexterity] = 11;
        game.Gear.Values[TGearItem.Armor] = -8;
        Assert.True(game.World.Sell(TBagItem.Cassette));
        Assert.Equal(0, game.Bag.Items[TBagItem.Cassette]);
        Assert.Equal(0, game.Bag.Count);
        Assert.Equal(1000, game.Player.Money);
        Assert.Equal(50, game.Player.MaxEnergy);
        Assert.Equal(10, game.Player.Stats[TStat.Dexterity]);
        Assert.Equal(0, game.Gear.Values[TGearItem.Armor]);
    }

    [Fact]
    public void SprzedazGarnituruWymagaGarnituru() {
        game.Bag.Items[TBagItem.SpikedSuit] = 0;
        game.Bag.Count = 0;
        game.Gear.WornGarment = Texts.TxtEmpty;
        Assert.False(game.World.Sell(TBagItem.SpikedSuit));
        game.Bag.Items[TBagItem.SpikedSuit] = 1;
        game.Bag.Count = 1;
        Assert.True(game.World.Sell(TBagItem.SpikedSuit));
        Assert.Equal(0, game.Bag.Items[TBagItem.SpikedSuit]);
        Assert.Equal(0, game.Bag.Count);
    }

    [Fact]
    public void SprzedazWladanegoMieczaNieDziala() {
        game.Bag.Items[TBagItem.OldSword] = 2;
        game.Bag.Count = 2;
        game.Player.Money = 0;
        game.Gear.Values[TGearItem.WeaponKind] = 0;
        game.Player.Stats[TStat.Strength] = 0;
        game.Gear.Values[TGearItem.LuckRoll] = 0;
        Assert.True(game.Inventory.Equip(TBagItem.OldSword));
        Assert.False(game.World.Sell(TBagItem.OldSword));
        Assert.Equal(1, game.Bag.Items[TBagItem.OldSword]);
        Assert.Equal(1, game.Bag.Count);
        Assert.Equal(0, game.Player.Money);
        Assert.True(game.Inventory.Unequip(TBagItem.OldSword));
        Assert.Equal(2, game.Bag.Items[TBagItem.OldSword]);
        Assert.Equal(2, game.Bag.Count);
    }

    [Fact]
    public void SprzedazWladanejTarczyNieDziala() {
        game.Bag.Items[TBagItem.SmallShield] = 2;
        game.Bag.Count = 2;
        game.Player.Money = 0;
        game.Gear.Values[TGearItem.ShieldKind] = 0;
        game.Gear.Values[TGearItem.Armor] = 0;
        game.Gear.Values[TGearItem.WornCount] = 0;
        Assert.True(game.Inventory.Equip(TBagItem.SmallShield));
        Assert.False(game.World.Sell(TBagItem.SmallShield));
        Assert.Equal(1, game.Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(1, game.Bag.Count);
        Assert.Equal(0, game.Player.Money);
        Assert.True(game.Inventory.Unequip(TBagItem.SmallShield));
        Assert.Equal(2, game.Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(2, game.Bag.Count);
    }

    [Fact]
    public void SprzedazWladanejTarczyBezSztukiNiePlaci() {
        game.Bag.Items[TBagItem.SmallShield] = 1;
        game.Bag.Count = 1;
        game.Player.Money = 0;
        game.Gear.Values[TGearItem.ShieldKind] = 0;
        game.Gear.Values[TGearItem.Armor] = 0;
        game.Gear.Values[TGearItem.WornCount] = 0;
        Assert.True(game.Inventory.Equip(TBagItem.SmallShield));
        Assert.False(game.World.Sell(TBagItem.SmallShield));
        Assert.Equal(0, game.Bag.Items[TBagItem.SmallShield]);
        Assert.Equal(0, game.Bag.Count);
        Assert.Equal(0, game.Player.Money);
        Assert.True(game.Inventory.Unequip(TBagItem.SmallShield));
    }

    [Fact]
    public void SprzedazZalozonegoKompletuNieDziala() {
        game.Bag.Items[TBagItem.Tracksuit] = 1;
        game.Bag.Count = 1;
        game.Player.Money = 0;
        game.Gear.WornGarment = Texts.TxtEmpty;
        game.Gear.Values[TGearItem.Armor] = 0;
        game.Gear.Values[TGearItem.Wearing] = 0;
        Assert.True(game.Inventory.Equip(TBagItem.Tracksuit));
        Assert.False(game.World.Sell(TBagItem.Tracksuit));
        Assert.Equal(0, game.Bag.Items[TBagItem.Tracksuit]);
        Assert.Equal(0, game.Bag.Count);
        Assert.Equal(0, game.Player.Money);
        Assert.True(game.Inventory.Unequip(TBagItem.Tracksuit));
        Assert.Equal(1, game.Bag.Items[TBagItem.Tracksuit]);
        Assert.Equal(1, game.Bag.Count);
    }

    [Fact]
    public void SprzedazZalozonegoGarnituruNieDziala() {
        game.Bag.Items[TBagItem.SpikedSuit] = 1;
        game.Bag.Count = 1;
        game.Player.Money = 0;
        game.Gear.WornGarment = Texts.TxtEmpty;
        game.Gear.Values[TGearItem.WornCount] = 0;
        game.Gear.Values[TGearItem.Armor] = 0;
        game.Gear.Values[TGearItem.LuckRoll] = 0;
        Assert.True(game.Inventory.Equip(TBagItem.SpikedSuit));
        Assert.False(game.World.Sell(TBagItem.SpikedSuit));
        Assert.Equal(0, game.Bag.Items[TBagItem.SpikedSuit]);
        Assert.Equal(0, game.Bag.Count);
        Assert.Equal(0, game.Player.Money);
        Assert.True(game.Inventory.Unequip(TBagItem.SpikedSuit));
        Assert.Equal(1, game.Bag.Items[TBagItem.SpikedSuit]);
        Assert.Equal(1, game.Bag.Count);
    }

    [Fact]
    public void KupnoMieczaDzialaPrzyPosiadanym() {
        game.Bag.Items[TBagItem.OldSword] = 1;
        game.Bag.Count = 1;
        game.Gear.Values[TGearItem.WeaponKind] = 0;
        game.Player.Stats[TStat.Strength] = 0;
        game.Gear.Values[TGearItem.LuckRoll] = 0;
        game.Player.Money = 200;
        Assert.True(game.Inventory.Equip(TBagItem.OldSword));
        Assert.True(game.World.Buy(TBagItem.OldSword));
        Assert.Equal(130, game.Player.Money);
        Assert.Equal(1, game.Bag.Items[TBagItem.OldSword]);
        Assert.Equal(1, game.Bag.Count);
        Assert.True(game.World.Buy(TBagItem.OldSword));
        Assert.Equal(60, game.Player.Money);
        Assert.Equal(2, game.Bag.Items[TBagItem.OldSword]);
        Assert.Equal(2, game.Bag.Count);
        Assert.True(game.Inventory.Unequip(TBagItem.OldSword));
        Assert.Equal(3, game.Bag.Items[TBagItem.OldSword]);
        Assert.Equal(3, game.Bag.Count);
    }

    [Fact]
    public void KupnoMalaButelkaManyPlaciCeneMimoNizszegoProgu() {
        game.Bag.Items[TBagItem.ManaBottle] = 0;
        game.Bag.Count = 0;
        game.Player.Money = 16;
        Assert.True(game.World.Buy(TBagItem.ManaBottle));
        Assert.Equal(-4, game.Player.Money);
        Assert.Equal(1, game.Bag.Items[TBagItem.ManaBottle]);
        Assert.Equal(1, game.Bag.Count);
    }

    [Fact]
    public void KupnoFajkiDodajeMadrosc() {
        game.Bag.Items[TBagItem.Pipe] = 0;
        game.Bag.Count = 0;
        game.Player.Money = 480;
        game.Player.Stats[TStat.Wisdom] = 10;
        game.Player.Stats[TStat.MaxWisdom] = 10;
        Assert.True(game.World.Buy(TBagItem.Pipe));
        Assert.Equal(0, game.Player.Money);
        Assert.Equal(1, game.Bag.Items[TBagItem.Pipe]);
        Assert.Equal(11, game.Player.Stats[TStat.Wisdom]);
        Assert.Equal(1, game.Bag.Count);
        game.Player.Stats[TStat.Wisdom] = 0;
        game.Player.Stats[TStat.MaxWisdom] = 0;
        game.Bag.Items[TBagItem.Pipe] = 0;
        game.Bag.Count = 0;
        game.Player.Money = 0;
    }

    [Fact]
    public void KupnoPaczkaPlaciCene() {
        game.Bag.Items[TBagItem.Doughnut] = 0;
        game.Bag.Count = 0;
        game.Player.Money = 8;
        Assert.True(game.World.Buy(TBagItem.Doughnut));
        Assert.Equal(0, game.Player.Money);
        Assert.Equal(1, game.Bag.Items[TBagItem.Doughnut]);
        Assert.Equal(1, game.Bag.Count);
    }

    [Fact]
    public void ZapisWczytujeLiczniki() {
        game.Player.Name = "COUNTS";
        game.Bag.Items[TBagItem.Beer] = 2;
        game.Bag.Items[TBagItem.Diploma] = 0;
        game.Bag.ItemRooms[TBagItem.Diploma].Clear();
        game.Bag.AddGroundItem(TBagItem.Diploma, 12);
        game.Bag.AddGroundItem(TBagItem.Diploma, 14);
        string path = game.SaveFileName();
        try {
            game.SaveCharacter();
            game.Bag.Items[TBagItem.Beer] = 0;
            game.Bag.ItemRooms[TBagItem.Diploma].Clear();
            game.LoadGame();
        } finally {
            File.Delete(path);
            game.Session.SavedFlag = false;
        }
        Assert.Equal(2, game.Bag.Items[TBagItem.Beer]);
        Assert.Equal(0, game.Bag.Items[TBagItem.Diploma]);
        Assert.Equal(2, game.Bag.ItemRooms[TBagItem.Diploma].Count);
        Assert.Contains(12, game.Bag.ItemRooms[TBagItem.Diploma]);
        Assert.Contains(14, game.Bag.ItemRooms[TBagItem.Diploma]);
        Assert.Empty(game.Bag.ItemRooms[TBagItem.Beer]);
    }

    [Fact]
    public void BagLimitFollowsDexterity() {
        game.Player.Stats[TStat.Dexterity] = 10;
        game.Bag.Items[TBagItem.Backpack] = 0;
        game.Inventory.RecalcBag();
        Assert.Equal(10, game.Bag.Limit);
        game.Player.Stats[TStat.Dexterity] = 12;
        game.Inventory.RecalcBag();
        Assert.Equal(11, game.Bag.Limit);
        game.Player.Stats[TStat.Dexterity] = 25;
        game.Inventory.RecalcBag();
        Assert.Equal(16, game.Bag.Limit);
        game.Bag.Items[TBagItem.Backpack] = 1;
        game.Inventory.RecalcBag();
        Assert.Equal(20, game.Bag.Limit);
    }

    [Fact]
    public void RecalcBagRecomputesCount() {
        Enum.GetValues<TBagItem>().ToList().ForEach(item => game.Bag.Items[item] = 0);
        game.Player.Stats[TStat.Dexterity] = 10;
        game.Bag.Permit = 0;
        game.Bag.Items[TBagItem.Beer] = 2;
        game.Bag.Items[TBagItem.Backpack] = 1;
        game.Bag.Count = 99;
        game.Inventory.RecalcBag();
        Assert.Equal(3, game.Bag.Count);
        Assert.Equal(14, game.Bag.Limit);
    }

    [Fact]
    public void WczytaniePrzeliczaPlecak() {
        game.Player.Name = "RECALC";
        Enum.GetValues<TBagItem>().ToList().ForEach(item => game.Bag.Items[item] = 0);
        game.Bag.Items[TBagItem.Beer] = 2;
        game.Bag.Permit = 1;
        game.Bag.Count = 0;
        string path = game.SaveFileName();
        try {
            game.SaveCharacter();
            game.Bag.Count = 99;
            game.LoadGame();
        } finally {
            File.Delete(path);
            game.Session.SavedFlag = false;
        }
        Assert.Equal(3, game.Bag.Count);
    }
}

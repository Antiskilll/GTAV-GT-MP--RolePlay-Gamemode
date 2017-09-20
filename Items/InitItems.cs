using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Shared;
using System.Linq;
using LSRP_VFR.Players;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using static LSRP_VFR.Mysql.DBPlayers;
namespace LSRP_VFR.Items
{
    public class Items : Script
    {
        public Items()
        {
            API.onResourceStart += OnStart;
        }

        public const int ITEM_ID_SNIPERRIFLE = 1;
        public const int ITEM_ID_FIREEXTINGUISHER = 2;
        public const int ITEM_ID_VINTAGEPISTOL = 3;
        public const int ITEM_ID_COMBATPDW = 4;
        public const int ITEM_ID_HEAVYSNIPER = 5;
        public const int ITEM_ID_MICROSMG = 6;
        public const int ITEM_ID_PISTOL = 7;
        public const int ITEM_ID_PUMPSHOTGUN = 8;
        public const int ITEM_ID_MOLOTOV = 9;
        public const int ITEM_ID_SMG = 10;
        public const int ITEM_ID_PETROLCAN = 11;
        public const int ITEM_ID_STUNGUN = 12;
        public const int ITEM_ID_DOUBLEBARRELSHOTGUN = 13;
        public const int ITEM_ID_GOLFCLUB = 15;
        public const int ITEM_ID_HAMMER = 16;
        public const int ITEM_ID_COMBATPISTOL = 17;
        public const int ITEM_ID_GUSENBERG = 18;
        public const int ITEM_ID_NIGHTSTICK = 19;
        public const int ITEM_ID_SAWNOFFSHOTGUN = 20;
        public const int ITEM_ID_CARBINERIFLE = 21;
        public const int ITEM_ID_CROWBAR = 22;
        public const int ITEM_ID_FLASHLIGHT = 23;
        public const int ITEM_ID_DAGGER = 24;
        public const int ITEM_ID_BAT = 25;
        public const int ITEM_ID_KNIFE = 26;
        public const int ITEM_ID_BZGAS = 27;
        public const int ITEM_ID_MUSKET = 28;
        public const int ITEM_ID_SNSPISTOL = 29;
        public const int ITEM_ID_ASSUALTRIFLE = 30;
        public const int ITEM_ID_REVOLVER = 31;
        public const int ITEM_ID_HEAVYPISTOL = 32;
        public const int ITEM_ID_KNUCKLEDUSTER = 33;
        public const int ITEM_ID_MARKSMANPISTOL = 34;
        public const int ITEM_ID_MACHETE = 35;
        public const int ITEM_ID_SWITCHBLADE = 36;
        public const int ITEM_ID_HATCHET = 37;
        public const int ITEM_ID_BOTTLE = 38;
        public const int ITEM_ID_SMOKEGRENADE = 39;
        public const int ITEM_ID_PARACHUTE = 40;
        public const int ITEM_ID_MORPHINE = 41;
        public const int ITEM_ID_HEROIN = 42;
        public const int ITEM_ID_SMALLBANDAGE = 43;
        public const int ITEM_ID_LARGEBANDAGE = 44;
        public const int ITEM_ID_COPBADGE = 45;
        public const int ITEM_ID_WEED28 = 46;
        public const int ITEM_ID_WEED = 47;
        public const int ITEM_ID_PLICENSE = 48;
        public const int ITEM_ID_FLICENSE = 49;
        public const int ITEM_ID_AMMOPISTOL = 49;
        public const int ITEM_ID_AMMOSMG = 50;
        public const int ITEM_ID_AMMOASSAULT = 51;
        public const int ITEM_ID_AMMOSNIPER = 52;
        public const int ITEM_ID_AMMOSHOTGUN = 53;
        public const int ITEM_ID_SPIKESTRIP = 54;
        public const int ITEM_ID_SPARETIRE = 55;
        public const int ITEM_ID_SPRUNKPACK = 56;
        public const int ITEM_ID_COLAPACK = 57;
        public const int ITEM_ID_WATERPACK = 58;
        public const int ITEM_ID_COPKEY = 60;
        public const int ITEM_ID_MILJETKEY = 61;
        public const int ITEM_ID_MILHELIKEY = 62;
        public const int ITEM_ID_SPRUNK = 63;
        public const int ITEM_ID_ECOLA = 64;
        public const int ITEM_ID_EWATER = 65;
        public const int ITEM_ID_REPAIRKIT = 66;
        public const int ITEM_ID_COMP_SNIPERBARREL = 67;
        public const int ITEM_ID_COMP_SNIPERSTOCK = 68;
        public const int ITEM_ID_COMP_SNIPERRECIEVER = 69;
        public const int ITEM_ID_COMP_MICROSMGBARREL = 70;
        public const int ITEM_ID_COMP_MICROSMGSTOCK = 71;
        public const int ITEM_ID_COMP_MICROSMGRECIEVER = 72;
        public const int ITEM_ID_COMP_SMGBARREL = 73;
        public const int ITEM_ID_COMP_SMGSTOCK = 74;
        public const int ITEM_ID_COMP_SMGRECIEVER = 75;
        public const int ITEM_ID_COMP_DOUBLEBARRELSHOTGUNBARREL = 76;
        public const int ITEM_ID_COMP_DOUBLEBARRELSHOTGUNSTOCK = 77;
        public const int ITEM_ID_COMP_DOUBLEBARRELSHOTGUNRECIEVER = 78;
        public const int ITEM_ID_COMP_AKBARREL = 79;
        public const int ITEM_ID_COMP_AKSTOCK = 80;
        public const int ITEM_ID_COMP_AKRECIEVER = 81;
        public const int ITEM_ID_COMP_SAWNOFFBARREL = 82;
        public const int ITEM_ID_COMP_SAWNOFFSTOCK = 83;
        public const int ITEM_ID_COMP_SAWNOFFRECIEVER = 84;
        public const int ITEM_ID_COMP_HEAVYPISTOLBARREL = 82;
        public const int ITEM_ID_COMP_HEAVYPISTOLSTOCK = 83;
        public const int ITEM_ID_COMP_HEAVYPISTOLRECIEVER = 84;
        public const int ITEM_ID_COMP_50CALBARREL = 82;
        public const int ITEM_ID_COMP_50CALSTOCK = 83;
        public const int ITEM_ID_COMP_50CALRECIEVER = 84;
        public const int ITEM_ID_MineraiOreCopper = 85;
        public const int ITEM_ID_OreCopper = 86;
        public const int ITEM_ID_BUNCH_OF_GRAPES = 87;
        public const int ITEM_ID_GRAPES = 88;
        public const int ITEM_ID_EATCHICKEN = 89;
        public const int ITEM_ID_WEEDNONTRAITER = 90;
        public const int ITEM_ID_WEEDTRAITER = 91;
        public const int ITEM_ID_DEFIBRILLATOR = 92;
        public const int ITEM_ID_POCHESANG = 93;
        public const int ITEM_ID_PETROLEBRUT = 94;
        public const int ITEM_ID_PETROLERAFFINE = 95;
        public const int ITEM_ID_SANDBRUT = 96;
        public const int ITEM_ID_SANDRAFFINE = 97;
        public const int ITEM_ID_PIZZAF = 98;
        public const int ITEM_ID_PIZZAB = 99;
        public const int ITEM_ID_PIZZAV = 100;

        public static List<Item> items = new List<Item>();

        public void OnStart()
        {
            PopulateItems();
        }

        public void PopulateItems()
        {
            try
            {
                items.Add(new Weapon(ITEM_ID_SNIPERRIFLE, "Sniper Rifle", "A long range rifle equiped with a high magnification scope.", 1, "SniperHash"));
                items.Add(new Weapon(ITEM_ID_FIREEXTINGUISHER, "Fire Extinguisher", "For putting out fires.", 1, "FireExtinguisher"));
                items.Add(new Weapon(ITEM_ID_COMBATPDW, "Combat PDW", "A tactical Sub-machine gun. A favourite of law enforcement.", 1, "CombatPDW"));
                items.Add(new Weapon(ITEM_ID_HEAVYSNIPER, ".50 Sniper Rifle", "A High calibre sniper rifle for armor penetration at long range", 1, "HeavySniper"));
                items.Add(new Weapon(ITEM_ID_MICROSMG, "Micro SMG", "A compact Sub-machine gun.", 1, "MicroSMG"));
                items.Add(new Weapon(ITEM_ID_PISTOL, "Pistol", "A generic handgun.", 1, "Pistol"));
                items.Add(new Weapon(ITEM_ID_PUMPSHOTGUN, "Pump Shotgun", "A pump action shotgun. A favourite among sport shooters.", 1, "PumpShotgun"));
                items.Add(new Weapon(ITEM_ID_MOLOTOV, "Molotov Cocktail", "The hand grenade of revolutionaries.", 1, "Molotov"));
                items.Add(new Weapon(ITEM_ID_SMG, "MP-5 SMG", "An SMG", 1, "SMG"));
                items.Add(new Weapon(ITEM_ID_PETROLCAN, "Petrol can", "Warning: Flamable", 1, "PetrolCan"));
                items.Add(new Weapon(ITEM_ID_STUNGUN, "Taser", "Standard Issue law enforcement taser", 1, "Stungun"));
                items.Add(new Weapon(ITEM_ID_DOUBLEBARRELSHOTGUN, "Double Barrel shotgun", "An illeagally modified shotgun", 1, "DoubleBarrelShotgun"));
                items.Add(new Weapon(ITEM_ID_GOLFCLUB, "Golf club", "For hitting golf balls... or heads.", 1, "GolfClub"));
                items.Add(new Weapon(ITEM_ID_HAMMER, "Hammer", "A useful workman's tool", 1, "Hammer"));
                items.Add(new Weapon(ITEM_ID_COMBATPISTOL, "Combat Pistol", "A combat pistol. For killing.", 1, "CombatPistol"));
                items.Add(new Weapon(ITEM_ID_GUSENBERG, "Gusenberg", "A Gusenberg Sweeper. A favourite of the Italian mob", 1, "Gusenberg"));
                items.Add(new Weapon(ITEM_ID_NIGHTSTICK, "Nightstick", "Standard issue law enforcement nightstick", 1, "Nightstick"));
                items.Add(new Weapon(ITEM_ID_SAWNOFFSHOTGUN, "Sawnoff Shotgun", "An illeagally modified shotgun", 1, "SawnoffShotgun"));
                items.Add(new Weapon(ITEM_ID_CARBINERIFLE, "M4-A1 Rifle", "A high powered rifle, a favourite of American military and law enforcement.", 1, "CarbineRifle"));
                items.Add(new Weapon(ITEM_ID_CROWBAR, "Crowbar", "For smashing headcrabs", 1, "Crowbar"));
                items.Add(new Weapon(ITEM_ID_FLASHLIGHT, "Flashlight", "Can be used as a weapon", 1, "Flashlight"));
                items.Add(new Weapon(ITEM_ID_DAGGER, "Dagger", "An ancient dagger", 1, "Dagger"));
                items.Add(new Weapon(ITEM_ID_BAT, "Baseball bat", "For hitting baseballs", 1, "Bat"));
                items.Add(new Weapon(ITEM_ID_KNIFE, "Knife", "A knife", 1, "Knife"));
                items.Add(new Weapon(ITEM_ID_BZGAS, "BZGAS", "Law enforcement BZGAS", 1, "BZGas"));
                items.Add(new Weapon(ITEM_ID_MUSKET, "Musket", "A musket from simpler times", 1, "Musket"));
                items.Add(new Weapon(ITEM_ID_SNSPISTOL, "SNS Pistol", "An SNS Pistol. easy to conceal.", 1, "SNSPistol"));
                items.Add(new Weapon(ITEM_ID_ASSUALTRIFLE, "AK-47", "The classic AK-47 used by freedom fighters for more than 60 years", 1, "AssaultRifle"));
                items.Add(new Weapon(ITEM_ID_REVOLVER, "Revolver", "A revolver", 1, "Revolver"));
                items.Add(new Weapon(ITEM_ID_HEAVYPISTOL, "Heavy Pistol", "Packs a slightly heavier punch than your standard 9mm", 1, "HeavyPistol"));
                items.Add(new Weapon(ITEM_ID_KNUCKLEDUSTER, "Knuckle Duster", "For hitting people. Easy to conceal", 1, "KnuckleDuster"));
                items.Add(new Weapon(ITEM_ID_MARKSMANPISTOL, "Marksman Pistol", "An extremely accurate pistol", 1, "MarksmanPistol"));
                items.Add(new Weapon(ITEM_ID_MACHETE, "Machete", "A machete. Useful in dense vegitation", 1, "Machete"));
                items.Add(new Weapon(ITEM_ID_SWITCHBLADE, "Switchblade", "Easy to conceal weapon", 1, "SwitchBlade"));
                items.Add(new Weapon(ITEM_ID_HATCHET, "Hatchet", "EMS standard issue hatchet", 1, "Hatchet"));
                items.Add(new Weapon(ITEM_ID_BOTTLE, "Bottle", "Break to use as a weapon", 1, "Bottle"));
                items.Add(new Weapon(ITEM_ID_PARACHUTE, "Parachute", "A parachute", 1, "Parachute"));

                items.Add(new Narcotic(ITEM_ID_MORPHINE, "Morphine", "Fournit un soulagement temporaire de la douleur. Addictif.", 1, 20, 2));
                items.Add(new Narcotic(ITEM_ID_HEROIN, "Heroin", "Provides temporary pain relief. Extremely addictive.", 1, 70, 5));

                items.Add(new Health(ITEM_ID_SMALLBANDAGE, "Bandage ", " Soigne légérement.", 1, 30));
                items.Add(new Health(ITEM_ID_LARGEBANDAGE, "Large bandage", "Heals the character a large amount", 1, 50));
                items.Add(new Health(ITEM_ID_POCHESANG, "Poche de sang ", " Soigne totalement.", 1, 100));
                items.Add(new Defibrillator(ITEM_ID_DEFIBRILLATOR, "Défibrillateur", "A utiliser pour réanimer un patient.", 1));

                items.Add(new Ammo(ITEM_ID_AMMOPISTOL, "9mm Pistol Ammo x 30", "30 Rounds of 9mm ammo", 1, 0x1B06D571, 30));
                items.Add(new Ammo(ITEM_ID_AMMOSHOTGUN, "Shotgun Cartridges x 15", "15 Shotgun cartridges", 1, 0x1D073A89, 15));
                items.Add(new Ammo(ITEM_ID_AMMOSNIPER, "Sniper Rifle Ammo x 10", "10 Sniper rifle rounds", 1, 0x05FC3C11, 10));
                items.Add(new Ammo(ITEM_ID_AMMOSMG, "Sub-Machine Gun Ammo x 30", "30 SMG rounds", 1, 0x13532244, 30));
                items.Add(new Ammo(ITEM_ID_AMMOASSAULT, "Assault Rifle Ammo x 30", "30 Assault rifle rounds", 1, 0x83BF0278, 30));
                items.Add(new Tire(ITEM_ID_SPARETIRE, "Spare Tire", "Repairs a vehicle's popped tires", 1));

                items.Add(new Drink(ITEM_ID_SPRUNK, "Canette de Sprunk", "Peut-être radioactif...", 1, 30));
                items.Add(new Drink(ITEM_ID_ECOLA, "Canette de E-Cola", "Avertissement: dangereux pour la santé", 1, 30));
                items.Add(new Drink(ITEM_ID_EWATER, "Bouteille d'eau", "Eau saine améliorée avec des e-vitamines", 1, 60));
                items.Add(new Eat(ITEM_ID_EATCHICKEN, "Pilons de poulet", "Morceau de poulet bien gras", 1, 30));
                items.Add(new Eat(ITEM_ID_PIZZAF, "Pizza 4 fromage", "Attention votre ligne...", 1, 80));
                items.Add(new Eat(ITEM_ID_PIZZAB, "Pizza bolognese", "Attention votre ligne...", 1, 80));
                items.Add(new Eat(ITEM_ID_PIZZAV, "Pizza végétarienne", "Attention votre ligne...", 1, 70));

                items.Add(new MineraiCuivre(ITEM_ID_MineraiOreCopper, "Minerai de cuivre", "Inutilisable", 2));
                items.Add(new MineraiCuivre(ITEM_ID_OreCopper, "Cuivre", "Inutilisable", 1));
                items.Add(new Raisin(ITEM_ID_BUNCH_OF_GRAPES, "Grappe(s) de raisin", "Inutilisable", 2));
                items.Add(new Raisin(ITEM_ID_GRAPES, "Raisin(s)", "Inutilisable", 1));
                items.Add(new PetrolBaril(ITEM_ID_PETROLEBRUT, "Barile de Pétrole", "Inutilisable", 2));
                items.Add(new PetrolBaril(ITEM_ID_PETROLERAFFINE, "Barile d'essence", "Inutilisable", 1));
                items.Add(new Sable(ITEM_ID_SANDBRUT, "Sac de sable", "Inutilisable", 2));
                items.Add(new Sable(ITEM_ID_SANDRAFFINE, "Caisse de verre", "Inutilisable", 1));

                items.Add(new ItemNonTraiter(ITEM_ID_WEEDNONTRAITER, "Pied de Cannabis", "Inutilisable", 3));
                items.Add(new ItemWeed(ITEM_ID_WEEDTRAITER, "Cannabis", "De la bonne beuh sa mère!", 2));

                
            }
            catch (Exception e)
            {
                API.consoleOutput("~r~[ERROR][INITITEM] : ~s~" + e.ToString());
            }
        }

        public class Item
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int Weight { get; set; }

            public Item(int id, string name, string description, int weight)
            {
                ID = id;
                Name = name;
                Description = description;
                Weight = weight;
            }

            public virtual void Use(Client c)
            {
            }

        }

        public void ItemSelected(Client sender, int index)
        {
            try
            {
                var Products = API.getEntityData(sender, "ProductsOfUsingShop");
                var item = ItemByID(Products[index].Key);
                var price = Products[index].Value;
                API.resetEntityData(sender, "ProductsOfUsingShop");
                if (Money.TakeMoney(sender, price))
                {
                    InventoryHolder ih = API.shared.getEntityData(sender, "InventoryHolder");
                    ih.AddItemToInventory(item, 1);
                    UpdatePlayerMoney(sender);
                    API.shared.triggerClientEvent(sender, "display_subtitle", "Item ajouté à votre inventaire", 3000);
                }
                else
                {
                    API.shared.triggerClientEvent(sender, "display_subtitle", "Désolé, vous n'avez pas assez d'argent", 3000);
                }
            }
            catch (Exception e)
            {
                API.consoleOutput("~r~[ERROR][INITITEM] : ~s~" + e.ToString());
            }
        }

        public static Item ItemByID(int id)
        {
            foreach (Item item in items)
            {
                if (item.ID == id)
                {
                    return item;
                }
            }
            return null;
        }

        public class InventoryItem
        {
            public Item Details { get; set; }
            public int Quantity { get; set; }

            public InventoryItem(Item details, int quantity)
            {
                Details = details;
                Quantity = quantity;
            }
        }

        public class InventoryHolder
        {
            public List<InventoryItem> Inventory { get; set; }
            public NetHandle Owner { get; set; }

            public InventoryHolder()
            {
                Inventory = new List<InventoryItem>();
            }

            public void AddItemToInventory(Item itemAdd, int Qt = 1)
            {
                try
                {
                    if (itemAdd != null && CheckWeightInventory(itemAdd, Qt))
                    {

                        int weightactual = API.shared.getEntityData(Owner, "weight");
                        int qtyweight = itemAdd.Weight * Qt;
                        int new_weight = weightactual + qtyweight;
                        API.shared.setEntityData(Owner, "weight", new_weight);

                        foreach (InventoryItem ii in Inventory)
                        {
                            if (ii.Details.ID == itemAdd.ID)
                            {
                                ii.Quantity = (ii.Quantity + Qt);
                                return;
                            }
                        }
                        Inventory.Add(new InventoryItem(itemAdd, Qt));

                    }
                }
                catch (Exception e)
                {
                    API.shared.consoleOutput("~r~[ERROR][INITITEM] : ~s~" + e.ToString());
                }
            }

            public bool CheckWeightInventory(Item itemAdd, int quantite = 1)
            {
                try
                {
                    int actuel = API.shared.getEntityData(Owner, "weight");
                    int max = API.shared.getEntityData(Owner, "weight_max");
                    int weight_total = (itemAdd.Weight * quantite);
                    if ((actuel + weight_total) <= max)
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception e)
                {
                    API.shared.consoleOutput("~r~[ERROR][INITITEM] : ~s~" + e.ToString());
                    return false;
                }

            }

            public void RemoveItemFromInventory(Item itemToDel, int itemnumber = 1)
            {
                try
                {
                    InventoryItem item = Inventory.SingleOrDefault(ii => ii.Details.ID == itemToDel.ID);
                    if (item == null) { return; }
                    item.Quantity = item.Quantity - itemnumber;

                    int weightactual = API.shared.getEntityData(Owner, "weight");
                    int qtyweight = itemToDel.Weight * itemnumber;
                    int new_weight = weightactual - qtyweight;

                    API.shared.setEntityData(Owner, "weight", new_weight);
                    // check if need delete item in inventory
                    if (item.Quantity <= 0)
                    {
                        Inventory.Remove(item);
                    }
                }
                catch (Exception e)
                {
                    API.shared.consoleOutput("~r~[ERROR][INITITEM] : ~s~" + e.ToString());
                }
            }
        }
    }
}

/*
            try
            {

            }
            catch (Exception e)
            {
                API.consoleOutput("~r~[ERROR][INITITEM] : ~s~" + e.ToString());
            }
*/
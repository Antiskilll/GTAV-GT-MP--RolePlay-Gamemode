using System;
using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Shared;
using static LSRP_VFR.Items.Items;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Server.Managers;

namespace LSRP_VFR.Faction.EMS
{
    public class Hospital: Script
    {
        private Ped pnj;
        private Ped pnjservice;
        private Blip blip;
        private Vector3 hospos = new Vector3(340.6274, -1399.797, 32.50922);
        private Vector3 pnjservicepos = new Vector3(294.9087, -1447.724, 29.96659);
        private Vector3 parkingpos = new Vector3(331.3084, -1478.712, 29.77438);
        private SphereColShape ParkingCol;
        private Marker ParkingMarker;
        private static List<KeyValuePair<int, int>> products = new List<KeyValuePair<int, int>>();
        private static List<KeyValuePair<int, int>> superetteProducts = new List<KeyValuePair<int, int>>();

        public Hospital()
        {
            API.onResourceStart += OnResourceStart;
            API.onResourceStop += OnResourceStop;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        private void OnResourceStart()
        {
            SpawnHospital();
            SpawnParking();

            products.Add(new KeyValuePair<int, int>(ITEM_ID_DEFIBRILLATOR, 0));
            products.Add(new KeyValuePair<int, int>(ITEM_ID_SMALLBANDAGE, 0));
            products.Add(new KeyValuePair<int, int>(ITEM_ID_POCHESANG, 0));

            //Ajout dans la supérette :
            superetteProducts.Add(new KeyValuePair<int, int>(ITEM_ID_SPRUNK, 0));
            superetteProducts.Add(new KeyValuePair<int, int>(ITEM_ID_ECOLA, 0));
            superetteProducts.Add(new KeyValuePair<int, int>(ITEM_ID_EWATER, 0));
            superetteProducts.Add(new KeyValuePair<int, int>(ITEM_ID_EATCHICKEN, 0));
        }

        private void SpawnParking()
        {
            ParkingCol = API.createSphereColShape(parkingpos, 1f);
            ParkingMarker = API.createMarker(1, parkingpos - new Vector3(0, 0, 1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 100, 255, 255, 255);
            ParkingCol.onEntityEnterColShape += ParkingCol_onEntityEnterColShape;

        }

        private void ParkingCol_onEntityEnterColShape(ColShape shape, NetHandle entity)
        {
            if (entity == null) return;
            if (API.getEntityType(entity) != EntityType.Player) return;
            Client player = API.getPlayerFromHandle(entity);
            if (!IsMedic(player)) { return; }
            if (API.isPlayerInAnyVehicle(player) && (player.vehicle.model == (int)VehicleHash.Ambulance))
            {
                List<string> Actions = new List<string>();
                Actions.Add("Ranger le véhicule");
                API.shared.triggerClientEvent(player, "bettermenuManager", 451, "Parking EMS", "", false, Actions);
            }
        }

        private void OnResourceStop()
        {
            API.deleteEntity(pnj);
            API.deleteEntity(pnjservice);
        }

        private void SpawnHospital() {
            try
            {
                
                blip = API.createBlip(hospos);
                API.setBlipName(blip, "Hôpital");
                API.setBlipSprite(blip, 61);
                API.setBlipShortRange(blip, true);
                pnj = API.createPed((PedHash)(-1420211530), hospos, 37.54668f, 0);
                pnj.setSyncedData("Interaction", "Hospital");
                API.playPedScenario(pnj, "WORLD_HUMAN_SMOKING");

                pnjservice = API.createPed((PedHash)(-1286380898), pnjservicepos, 42.87561f, 0);
                pnjservice.setSyncedData("Interaction", "EMS");
            }
            catch (Exception e)
            {
                API.consoleOutput("~r~[ERROR][Hospital] : ~s~" + e.ToString());
            }
        }


        public static void OpenMenuHospital(Client sender)
        {
            List<string> Actions = new List<string>();
            List<string> label = new List<string>();
            Actions.Add("Se Soigner");
            label.Add("500$");
            Actions.Add("Quitter");
            label.Add("");
            API.shared.triggerClientEvent(sender, "bettermenuManager", 34, "Hopital", "Menu Hopital: ", false, Actions, label);
        }

        public static void OpenMenuServiceEMS(Client sender)
        {
            if ((API.shared.getEntitySyncedData(sender, "EMSrank")) == 0) { API.shared.sendNotificationToPlayer(sender,"Vous n'êtes pas médecin."); return; }
            List<string> Actions = new List<string>();
            if (!IsMedic(sender))
            {
                Actions.Add("Prendre son service");
            } else
            {
                Actions.Add("Quitter son service");
                Actions.Add("Sortir un véhicule");
                Actions.Add("Pharmacie");
                Actions.Add("Supérette EMS");
            }
            API.shared.triggerClientEvent(sender, "bettermenuManager", 450, "EMS", "Prise de service: ", false, Actions);
        }

        private void OnClientEventTrigger(Client sender, string eventName, object[] arguments)
        {
            if (eventName == "menu_handler_select_item")
            {
                if ((int)arguments[0] == 34) {
                    if (Players.Money.TakeMoney(sender, 500)) { 
                        API.setPlayerHealth(sender, 100);
                        API.sendNotificationToPlayer(sender, "Vous avez été soigné.");
                    } else {
                        API.sendNotificationToPlayer(sender, "Vous n'avez pas assez d'argent sur vous.");
                    }
                }

                else if ((int)arguments[0] == 450 && (int)arguments[1] == 0)
                {
                    if (!IsMedic(sender))
                    {
                        API.shared.setEntitySyncedData(sender, "IsMedic", true);
                        API.sendNotificationToPlayer(sender, "Vous avez pris votre service.");
                        PriseServiceEMS(sender);
                    } else {
                        API.shared.setEntitySyncedData(sender, "IsMedic", false);
                        API.sendNotificationToPlayer(sender, "Vous avez quitté votre service.");
                        QuitterServiceEMS(sender);
                    }
                }
                else if ((int)arguments[0] == 450 && (int)arguments[1] == 1)
                {
                    if (!IsMedic(sender)) { return; }
                    Vehicle EMS = API.createVehicle(VehicleHash.Ambulance, new Vector3(331.3084, -1478.712, 29.77438), new Vector3(0, 0, -59.07445), 0, 0, 0);
                    API.setVehicleNumberPlate(EMS, "LS-EMS");
                    API.setEntitySyncedData(EMS, "Owner", "EMS");
                    Items.Items.InventoryHolder ih = new Items.Items.InventoryHolder();
                    ih.Owner = EMS.handle;
                    API.setEntityData(EMS, "InventoryHolder", ih);
                    API.setEntitySyncedData(EMS, "VEHICLE_FUEL", 100);
                    API.setEntitySyncedData(EMS, "VEHICLE_FUEL_MAX", 100);
                    API.setEntityData(EMS, "weight", 0);
                    API.setEntityData(EMS, "weight_max", 0);
                    API.setVehicleLocked(EMS, true);
                    API.setEntitySyncedData(EMS, "Locked", true);

                }
                else if ((int)arguments[0] == 450 && (int)arguments[1] == 2)
                {

                    List<string> Actions = new List<string>();
                    List<string> label = new List<string>();
                    foreach (KeyValuePair<int, int> entry in products)
                    {
                        Item item = ItemByID(entry.Key);
                        Actions.Add(item.Name);
                        label.Add("Prix: $" + entry.Value);
                    }
                    API.shared.triggerClientEvent(sender, "bettermenuManager", 454, "Pharmacie", "Sélectionner un item:", false, Actions, label);
                    API.shared.setEntityData(sender, "ProductsOfUsingShop", products);

                }
                else if ((int)arguments[0] == 450 && (int)arguments[1] == 3)
                {

                    List<string> Actions = new List<string>();
                    List<string> label = new List<string>();
                    foreach (KeyValuePair<int, int> entry in superetteProducts)
                    {
                        Item item = ItemByID(entry.Key);
                        Actions.Add(item.Name);
                        label.Add("Prix: $" + entry.Value);
                    }
                    API.shared.triggerClientEvent(sender, "bettermenuManager", 454, "Supérette EMS", "Sélectionner un item:", false, Actions, label);
                    
                    API.shared.setEntityData(sender, "ProductsOfUsingShop", superetteProducts);
                }

                else if ((int)arguments[0] == 451 && (int)arguments[1] == 0)
                {
                    if (sender.vehicle == null) return;
                    API.deleteEntity(sender.vehicle);
                    API.sendNotificationToPlayer(sender, "Vous avez ranger votre véhicule.");
                }

                else if ((int)arguments[0] == 454)
                {
                    var Products = API.getEntityData(sender, "ProductsOfUsingShop");
                    var item = ItemByID(Products[(int)arguments[1]].Key);
                    API.resetEntityData(sender, "ProductsOfUsingShop");
                    InventoryHolder ih = API.getEntityData(sender, "InventoryHolder");
                    if (ih.CheckWeightInventory(item, 1))
                    {
                        ih.AddItemToInventory(item, 1);
                        API.triggerClientEvent(sender, "display_subtitle", "Item ajouté à votre inventaire", 3000);
                    }
                    else
                    {
                        API.triggerClientEvent(sender, "display_subtitle", "Désolé, vous n'avez pas assez de place dans votre inventaire.", 3000);
                    }
                }

            }
        }

        private static void PriseServiceEMS(Client player)
        {
            API.shared.setPlayerClothes(player, 11, 26, 0); //Survet
            API.shared.setPlayerClothes(player, 3, 92, 0); // Bras
            API.shared.setPlayerClothes(player, 8, 15, 0); // Chemise
            API.shared.setPlayerClothes(player, 6, 0, 0); // Chaussure
            API.shared.setPlayerClothes(player, 4, 13, 0); // Pantalon
        }

        private static void QuitterServiceEMS(Client player)
        {
            API.shared.setPlayerClothes(player, 4, player.getData("Pants"), 0);
            API.shared.setPlayerClothes(player, 8, player.getData("Chemise"), 0);
            API.shared.setPlayerClothes(player, 11, player.getData("Survet"), 0);
            API.shared.setPlayerClothes(player, 6, player.getData("Chaussures"), 0);
            API.shared.setPlayerClothes(player, 3, player.getData("Bras"), 0);
        }

        public static bool IsMedic(Client player)
        {
            return API.shared.getEntitySyncedData(player, "IsMedic");
        }


        public static void CallMedic(Client sender, string msg)
        {
            var players = API.shared.getAllPlayers();
            var i = 0;
            foreach (var player in players)
            {
                if (Faction.EMS.Hospital.IsMedic(player))
                {
                    i += 1;
                    API.shared.triggerClientEvent(player, "display_subtitle", "~r~~h~[APPEL D'URGENCE] de " + API.shared.getEntitySyncedData(sender, "Nom_Prenom") + "~h~~y~ : " + msg + "\n ~w~ Position transmise!", 13000);
                    API.shared.sendNativeToPlayer(player, Hash.SET_NEW_WAYPOINT, sender.position.X, sender.position.Y);
                    
                }
            }
            if (i == 0)
            {
                API.shared.sendNotificationToPlayer(sender, "Aucun médic n'est actuellement en service.");
            } else
            {
                API.shared.sendNotificationToPlayer(sender, "Vous avez contacté les secours.");
            }
        }
    }
}

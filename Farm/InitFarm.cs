using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Server.Constant;
using static LSRP_VFR.Items.Items;
using System.Linq;
using GrandTheftMultiplayer.Shared;
using System;
using static LSRP_VFR.Mysql.DBPlayers;
namespace LSRP_VFR.Farm
{
    public class InitFarm : Script
    {

        public InitFarm()
        {
            API.consoleOutput("[Farm][INFO] Initialisation des farms!");
            API.onResourceStart += OnResourceStart;
            API.onEntityEnterColShape += OnEntityEnterColShape;
            API.onEntityExitColShape += OnEntityExitColShape;
        }

        private void OnResourceStart()
        {
            FarmZone();
        }

        public void FarmZone()
        {
            try
            {
                // resourceName - The name of the item, will be displayed in inventy with the same name | String
                // itemPrice - The price for the item at the trader | int
                // unprocessedItemID - The item ID before it was processed, see initItem.cs | int
                // processedItemID - The item ID before it was processed, see initItem.cs | int
                // farmZoneName - The name of the farm zone on the map | String
                // processName - The name of the process zone on the map | String
                // traderName - The name of the trader PNJ on the map | String
                // farmZonePos - The position of the farm zone on the map | Vector3
                // processPos - The position of the process zone on the map | Vector3
                // traderPos - The position of the trader PNJ on the map | Vector3
                // processPnjRotation - Determinate the direction the pnj will be | float
                // traderPnjRotation - Determinate the direction the pnj will be | float
                // blip - the blip type | int
                // blipColor - Determinate the color of the blip | int
                // legal - set true if the resource is legal, else the blip is disabled | bool
                // farmZoneScale - set the zone range | float
                // skin traitetement | PedHash
                // skin trader | PedHash
                var farm = new object[5, 18] {
                    { "Cuivre", 190, 85, 86, "Mine de cuivre", "Fonderie de cuivre", "Vendeur de Cuivre", new Vector3(2684.724, 2866.268, 33), new Vector3(741.797, -972.2991, 24.50507), new Vector3(606, -3073.102, 6.06), -94.82858f, -11.52882f, 85, 64, true, 100f,-973145378,797459875},
                    { "Raisin", 215, 87, 88, "Vignes", "Traitement de Raisin", "Vendeur de Raisin", new Vector3(-1791, 2146, 30), new Vector3(-67.01217, 1909.044, 196.1046), new Vector3(-1341.587, -1078.23, 6.938033), -94.82858f, -155.4158f, 85, 25, true, 100f,-973145378,797459875},
                    { "Weed", 532, 90, 91, "Weed", "Traitement de Weed", "Dealer de Weed", new Vector3(2224.306, 5577.014, 53.85302), new Vector3(709.16, 4185.644, 40.70919), new Vector3(-1156.675, -1574.621, 8.344103), -90f, -151f, 85, 27, false, 10f,-1835459726,-459818001},
                    { "Pétrole", 192, 94, 95, "Pétrole Brut", "Raffinerie de Pétrole", "Dépot d'essence", new Vector3(600.788, 2859.656, 39.58144), new Vector3(826.7706, -1992.382, 29.30135), new Vector3(580.2372, -2805.223, 6.059413), -21f, -126f, 361, 85, true, 50f,-1835459726,-459818001},
                    { "Sable", 192, 96, 97, "Carrière de Sable", "Traitement de Sable", "Vendeur de verre", new Vector3(232.136, 7009.544, 1.883199), new Vector3(973.6821, -1942.985, 31.06884), new Vector3(575.4752, 137.3664, 99.47485), -178f, -179f, 85, 81, true, 50f,-1835459726,-459818001}
                };

                int v = farm.GetLength(0);
                for (int i = 0; i < v; i++)
                {
                    string resourceName = (string)farm[i, 0];
                    int itemPrice = (int)farm[i, 1];
                    int unprocessedItemID = (int)farm[i, 2];
                    int processedItemID = (int)farm[i, 3];
                    string farmZoneName = (string)farm[i, 4];
                    string processName = (string)farm[i, 5];
                    string traderName = (string)farm[i, 6];
                    Vector3 farmZonePos = (Vector3)farm[i, 7];
                    Vector3 processPos = (Vector3)farm[i, 8];
                    Vector3 traderPos = (Vector3)farm[i, 9];
                    float processPnjRotation = (float)farm[i, 10];
                    float traderPnjRotation = (float)farm[i, 11];
                    int blip = (int)farm[i, 12];
                    int blipColor = (int)farm[i, 13];
                    bool legal = (bool)farm[i, 14];
                    float farmZoneScale = (float)farm[i, 15];
                    PedHash traitementhash = (PedHash)farm[i, 16];
                    PedHash traiderhash = (PedHash)farm[i, 17];
                    API.consoleOutput("[FARM] Création de la zone: " + resourceName);

                    SphereColShape colshape = API.createSphereColShape(farmZonePos, farmZoneScale);
                    //Marker marker = API.createMarker(1, farmZonePos - new Vector3(0, 0, 1f), new Vector3(), new Vector3(), new Vector3(200f, 200f, 250), 30, 255, 255, 255);
                    colshape.setData("FarmValue", unprocessedItemID);

                    Ped pnjtraitement = API.createPed((PedHash)(traitementhash), processPos, processPnjRotation);
                    API.setEntityRotation(pnjtraitement, new Vector3(0, 0, processPnjRotation));
                    API.setEntitySyncedData(pnjtraitement, "Interaction", "Traitement");
                    API.setEntitySyncedData(pnjtraitement, "unprocessedItemID", unprocessedItemID);
                    API.setEntitySyncedData(pnjtraitement, "processedItemID", processedItemID);

                    Ped pnjtraider = API.createPed((PedHash)traiderhash, traderPos, traderPnjRotation);
                    API.setEntityRotation(pnjtraitement, new Vector3(0, 0, traderPnjRotation));
                    API.setEntitySyncedData(pnjtraider, "Interaction", "Trader");
                    API.setEntitySyncedData(pnjtraider, "Trader", resourceName);
                    API.setEntitySyncedData(pnjtraider, "ItemPrice", itemPrice);
                    API.setEntitySyncedData(pnjtraider, "processedItemID", processedItemID);

                    if (legal)
                    {
                        // Farm Zone Marker
                        Blip BlipFarm = API.createBlip(farmZonePos);
                        API.setBlipName(BlipFarm, farmZoneName);
                        API.setBlipColor(BlipFarm, blipColor);
                        API.setBlipShortRange(BlipFarm, true);
                        API.setBlipSprite(BlipFarm, 85);

                        // Traitement Zone Marker
                        Blip BlipTraitement = API.createBlip(processPos);
                        API.setBlipName(BlipTraitement, processName);
                        API.setBlipColor(BlipTraitement, blipColor);
                        API.setBlipShortRange(BlipTraitement, true);
                        API.setBlipSprite(BlipTraitement, 499);

                        // Trader Zone Marker
                        Blip BlipTraider = API.createBlip(traderPos);
                        API.setBlipName(BlipTraider, traderName);
                        API.setBlipColor(BlipTraider, blipColor);
                        API.setBlipShortRange(BlipTraider, true);
                        API.setBlipSprite(BlipTraider, 500);
                    }
                }
            }
            catch (Exception e)
            {
                API.consoleOutput("~r~[ERROR][INITFARM] : ~s~" + e.ToString());
            }
        }

        private void OnEntityEnterColShape(ColShape colshape, GrandTheftMultiplayer.Shared.NetHandle entity)
        {
            if (entity == null) return;
            Client player = API.getPlayerFromHandle(entity);
            if (player == null) return;
            API.setEntityData(player, "OnFarmZone", colshape.getData("FarmValue"));
        }

        private void OnEntityExitColShape(ColShape colshape, GrandTheftMultiplayer.Shared.NetHandle entity)
        {
            if (entity == null) return;
            Client player = API.getPlayerFromHandle(entity);
            if (player == null) return;
            API.resetEntityData(player, "OnFarmZone");
        }


        public static void StartFarming(Client sender)
        {
            try
            {
                if (Players.Player.IsOnProgress(sender)) { return; }
                InventoryHolder ih = API.shared.getEntityData(sender, "InventoryHolder");
                var unprocessedItemID = API.shared.getEntityData(sender, "OnFarmZone");
                Item itemfarm = ItemByID(unprocessedItemID);

                if (ih.CheckWeightInventory(itemfarm))
                {
                    Players.Player.OnProgress(sender);
                    API.shared.triggerClientEvent(sender, "display_subtitle", "Vous commencez à ramasser du ~r~" + itemfarm.Name, 5000);
                    bool _exit = false;
                    int count = 0;
                        
                    while (!_exit)
                    {
                        API.shared.sleep(10000);
                        if (!API.shared.hasEntityData(sender.handle, "OnFarmZone"))
                        {
                            API.shared.triggerClientEvent(sender, "display_subtitle", "Récolte interrompue : Veuillez retourner dans la zone pour récolter.", 60000); _exit = true;
                            return;
                        }
                        else if (!ih.CheckWeightInventory(itemfarm)) { _exit = true; }
                        else if (sender.vehicle !=null) { _exit = true; }  
                        count = count + 1;
                        ih.AddItemToInventory(itemfarm, 1);
                        API.shared.triggerClientEvent(sender, "display_subtitle", "Récolte en cours: Vous avez ramassé +1 ~r~" + itemfarm.Name, 5000);
                    }
                    API.shared.triggerClientEvent(sender, "display_subtitle", "Récolte terminée : Vous avez ramassé ~r~" + count.ToString() + " " + itemfarm.Name.ToString(), 60000);
                    Players.Player.OnProgress(sender, false);

                }
                else
                {
                    API.shared.triggerClientEvent(sender, "display_subtitle", "Vous inventaire est plein!", 60000);
                }
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][INITFARM] : ~s~" + e.ToString());
            }
        }

        public static void StartTraitement(Client sender, NetHandle pnj)
        {
            try
            {
                if (Players.Player.IsOnProgress(sender)) { return; }
                int idnontraite = API.shared.getEntitySyncedData(pnj, "unprocessedItemID");
                int idtraite = API.shared.getEntitySyncedData(pnj, "processedItemID");

                InventoryHolder ih = API.shared.getEntityData(sender, "InventoryHolder");
                Item itemnontraite = ItemByID(idnontraite);
                Item itemtraite = ItemByID(idtraite);

                InventoryItem items = ih.Inventory.SingleOrDefault(ii => ii.Details.ID == itemnontraite.ID);
                if (items != null)
                {
                    int qtitem = items.Quantity;
                    Players.Player.OnProgress(sender);
                    API.shared.triggerClientEvent(sender, "display_subtitle", "Vous traitez vos  ~r~" + qtitem.ToString() + " " + itemnontraite.Name.ToString(), 2000);

                    if (qtitem > 0)
                    {
                        API.shared.setEntitySyncedData(sender.handle, "InProgress", true);
                        API.shared.sleep(30000 + (1000 * qtitem));
                        ih.RemoveItemFromInventory(itemnontraite, qtitem);
                        ih.AddItemToInventory(itemtraite, qtitem);
                        API.shared.setEntitySyncedData(sender.handle, "InProgress", false);
                    }



                    /*
                    bool _exit = false;
                    var startTime = DateTime.UtcNow;
                    
                    while (!_exit || sender !=null) {
                        if(sender.vehicle != null || sender.position.DistanceToSquared(API.getEntityPosition(pnj)) > 60f || DateTime.UtcNow - startTime > TimeSpan.FromSeconds(15 * qtitem)) {
                            _exit = true;
                            ih.AddItemToInventory(itemnontraite, qtitem);
                            Players.Player.OnProgress(sender, false);
                            API.sendNotificationToPlayer(sender, "[TRAITEMENT] Abandonné.");
                            return;
                        }
                    }
                    
                    ih.AddItemToInventory(itemtraite, qtitem);
                    
                    API.setEntitySyncedData(sender.handle, "InProgress", false);
                    */
                    UpdatePlayerInfo(sender);
                    API.shared.triggerClientEvent(sender, "display_subtitle", "Vous avez traité vos ~r~" + itemnontraite.Name, 30000);
                    Players.Player.OnProgress(sender, false);
                }
                else
                {
                    API.shared.triggerClientEvent(sender, "display_subtitle", "Vous n'avez rien à traiter", 30000);
                }

            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][INITFARM] : ~s~" + e.ToString());
            }

        }

        public static void StartTraid(Client sender, NetHandle pnj)
        {
            try
            {
                int idtraite = API.shared.getEntitySyncedData(pnj, "processedItemID");
                int itemPrice = API.shared.getEntitySyncedData(pnj, "ItemPrice");
                InventoryHolder ih = API.shared.getEntityData(sender, "InventoryHolder");
                Item itemtraite = ItemByID(idtraite);

                InventoryItem items = ih.Inventory.SingleOrDefault(ii => ii.Details.ID == itemtraite.ID);
                if (items == null) { return; }
                int qtitem = items.Quantity;

                if (qtitem > 0)
                {
                    int price = qtitem * itemPrice;
                    ih.RemoveItemFromInventory(itemtraite, qtitem);
                    Players.Money.GiveMoney(sender, price);
                    API.shared.triggerClientEvent(sender, "display_subtitle", "Vous avez vendu votre ~r~" + itemtraite.Name.ToString() + " ~s~pour la somme de ~r~" + price.ToString() + "~s~$", 30000);
                }

            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][INITFARM] : ~s~" + e.ToString());
            }

        }
    }
}

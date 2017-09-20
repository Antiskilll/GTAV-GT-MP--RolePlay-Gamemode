using GrandTheftMultiplayer.Server.API;
using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using static LSRP_VFR.Items.Items;
using static LSRP_VFR.Mysql.DBPlayers;
namespace LSRP_VFR.Menu
{
    class MenuVehicle: Script
    {
        public MenuVehicle()
        {
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        public static void OpenMenuVehicle(Client sender)
        {
            NetHandle vehicle = Vehicles.Vehicle.GetVehicleInRange(sender, 3f);
            if (vehicle.IsNull) return;
            if (!Vehicles.Vehicle.LockVehicleState(vehicle) || sender.getSyncedData("Police") == true)
            {
                API.shared.setEntityData(sender, "NearbyList", vehicle);
                List<String> Actions = new List<string>();
                Actions.Add("Ouvrir l'inventaire");
                if (sender.getSyncedData("Police") == true)
                {
                    Actions.Add("Information du Véhicule");
                    Actions.Add("Mise en fourrière");
                    Actions.Add("Crocheter");
                }
                API.shared.triggerClientEvent(sender, "bettermenuManager", 143, "Menu Vehicule", "", false, Actions);
            }
            else
            {
                API.shared.sendNotificationToPlayer(sender, "Le véhicule est fermer.");
            }
        }

        private void OnClientEventTrigger(Client sender, string eventName, object[] arguments)
        {
            if (eventName == "menu_handler_select_item")
            {
                var car = API.getEntityData(sender, "NearbyList");
                if (car == null){return;}
                // Menu Vehicule
                if ((int)arguments[0] == 143 && (int)arguments[1] == 0)
                {
                    if (Vehicles.Vehicle.LockVehicleState(car)) { API.sendNotificationToPlayer(sender, "Le véhicule est vérrouillé."); return; }
                    List<String> Actions = new List<string>();
                    Items.Items.InventoryHolder ih = API.getEntityData(car, "InventoryHolder");
                    foreach (Items.Items.InventoryItem item in ih.Inventory)
                    {
                        Actions.Add(item.Details.Name + " :  " + item.Quantity);
                    }
                    API.triggerClientEvent(sender, "bettermenuManager", 144, "Inventaire Véhicule", "Selectionner l'item :               Poids : " + API.shared.getEntityData(car, "weight") + " / " + API.shared.getEntityData(car, "weight_max"), false, Actions);
                }

                // QUANTITE à Récupérer du coffre 
                if ((int)arguments[0] == 144 && (int)arguments[1] == 0)
                {
                    if (Vehicles.Vehicle.LockVehicleState(car)) { API.sendNotificationToPlayer(sender, "Le coffre du véhicule est vérrouillé."); return; }
                    InventoryHolder ih = API.getEntityData(car, "InventoryHolder");
                    var item = ih.Inventory[(int)arguments[1]];
                    API.setEntityData(sender, "LastSelectedItem", item);
                    API.shared.triggerClientEvent(sender, "get_user_input", 145, "", 3, null);
                }
                if ((int)arguments[0] == 143 && (int)arguments[1] == 1)
                {
                    API.call("Vehicle", "GetVehicleInfo", sender);
                }

                if ((int)arguments[0] == 143 && (int)arguments[1] == 2)
                {
                    Jobs.Fourriere.CallFourriere(sender);
                }

                if ((int)arguments[0] == 143 && (int)arguments[1] == 3)
                {
                    API.call("Vehicle", "CrocheteVehicle", sender);
                }
            }
            else if (eventName == "menu_handler_user_input")
            {
                // VALIDATION SORTIR DU COFFRE
                if ((int)arguments[0] == 145) {
                    try
                    {
                        var item = API.getEntityData(sender, "LastSelectedItem");
                        var reciever = API.getEntityData(sender, "NearbyList");
                        int qty = Convert.ToInt32(arguments[1]);
                        Items.Items.InventoryHolder invplayer = API.getEntityData(reciever, "InventoryHolder");
                        Items.Items.InventoryHolder invreciever = API.getEntityData(sender, "InventoryHolder");

                        int itemplayerqty = item.Quantity;
                        if (qty <= itemplayerqty)
                        {
                            if (invreciever.CheckWeightInventory(item.Details, qty))
                            {
                                invplayer.RemoveItemFromInventory(item.Details, qty);
                                invreciever.AddItemToInventory(item.Details, qty);
                                API.sendNotificationToPlayer(sender, "Vous avez récupéré " + qty.ToString() + " " + item.Details.Name + " du coffre du véhicule");
                                UpdatePlayerInfo(sender);
                                API.resetEntityData(sender, "LastSelectedItem");
                                API.resetEntityData(sender, "NearbyList");
                            }
                            else
                            {
                                
                                API.sendNotificationToPlayer(sender,"Vous n'avez pas la place dans votre inventaire!");
                            }
                        }
                        else
                        {
                            API.sendNotificationToPlayer(sender, "Vous en avez pas autant sur vous");
                        }
                    }
                    catch (FormatException)
                    {
                        API.sendNotificationToPlayer(sender, "ERREUR!");
                    }
                }
            }
        }
    }
}

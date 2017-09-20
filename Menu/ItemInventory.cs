using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using System;
using System.Collections.Generic;
using static LSRP_VFR.Items.Items;
using static LSRP_VFR.Mysql.DBPlayers;
namespace LSRP_VFR.Menu
{
    class ItemInventory : Script
    {
        private int qty = 0;

        public ItemInventory()
        {
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        private void OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "menu_handler_select_item")
            {
                if ((int)arguments[0] == 132)
                {
                    if (API.hasEntityData(sender, "InventoryHolder"))
                    {
                        InventoryHolder ih = API.getEntityData(sender, "InventoryHolder");
                        var item = ih.Inventory[(int)arguments[1]];
                        API.setEntityData(sender, "LastSelectedItem", item);
                        List<String> Actions = new List<string>();
                        Actions.Add("Utiliser /Equiper");
                        Actions.Add("Description");
                        Actions.Add("Donner");
                        Actions.Add("Ranger dans le coffre");
                        Actions.Add("Jeter");
                        API.triggerClientEvent(sender, "bettermenuManager", 133, API.getEntitySyncedData(sender, "Nom_Prenom"), "Selectionner l'item :               Poids : " + API.getEntityData(sender.handle, "weight") + " / " + API.getEntityData(sender.handle, "weight_max"), false, Actions);
                    }
                }
                if ((int)arguments[0] == 133)
                {
                    // Utilise Equiper
                    InventoryItem item = API.getEntityData(sender, "LastSelectedItem");
                    if ((int)arguments[1] == 0)
                    {
                        item.Details.Use(sender);
                        API.resetEntityData(sender, "LastSelectedItem");
                    }
                    // Description
                    if ((int)arguments[1] == 1)
                    {
                        object[] ar = new object[2];
                        ar[0] = item.Details.Description;
                        ar[1] = 2000;
                        API.triggerClientEvent(sender, "display_subtitle", ar);
                    }
                    // Donner au joueur
                    if ((int)arguments[1] == 2)
                    {
                        var peopleNearby = API.getPlayersInRadiusOfPlayer(4, sender);
                        peopleNearby.Remove(sender);
                        API.setEntityData(sender, "NearbyList", peopleNearby);
                        List<String> Actions = new List<string>();
                        foreach (Client player in peopleNearby)
                        {
                            Actions.Add(API.getEntitySyncedData(player, "Nom_Prenom"));
                        }
                        API.triggerClientEvent(sender, "bettermenuManager", 134, API.getEntitySyncedData(sender, "Nom_Prenom"), "Donner l'object au ~g~joueur:", false, Actions);
                    }
                    //Mettre en coffre
                    if ((int)arguments[1] == 3)
                    {
                        API.shared.triggerClientEvent(sender, "get_user_input", 137, "", 3, null);
                    }
                    if((int)arguments[1] == 4)
                    {
                        var itemS = API.getEntityData(sender, "LastSelectedItem");
                        Items.Items.InventoryHolder invplayer = API.getEntityData(sender, "InventoryHolder");
                        invplayer.RemoveItemFromInventory(item.Details, 1);
                        API.resetEntityData(sender, "LastSelectedItem");
                    }
                }
                // QUANTITE à Donner au Joueur 
                if ((int)arguments[0] == 134)
                {
                    API.shared.triggerClientEvent(sender, "get_user_input", 135, "", 3, (int)arguments[1]);
                }
            }
            
            if (eventName == "menu_handler_user_input")
            {
                // Donner au Joueur VALIDATION
                if ((int)arguments[0] == 135)
                {
                    var item = API.getEntityData(sender, "LastSelectedItem");
                    var nearbylist = API.getEntityData(sender, "NearbyList");
                    var reciever = nearbylist[(int)arguments[2]];


                    bool result = Int32.TryParse(arguments[1].ToString(), out int number);
                    if (!result) { API.sendNotificationToPlayer(sender, "Vous devez rentrer exlusivement un nombre."); return; }

                    InventoryHolder invplayer = API.getEntityData(sender, "InventoryHolder");
                    InventoryHolder invreciever = API.getEntityData(reciever, "InventoryHolder");

                    var itemplayerqty = item.Quantity;
                    if (qty <= itemplayerqty) { 
                        if (invreciever.CheckWeightInventory(item.Details, qty))
                        {
                            invplayer.RemoveItemFromInventory(item.Details, qty);
                            invreciever.AddItemToInventory(item.Details, qty);

                            API.sendNotificationToPlayer(sender, "Vous avez donner " + qty.ToString() + " " + item.Details.Name + " à " + (API.getEntitySyncedData(reciever, "Nom_Prenom")).ToString());
                            API.sendNotificationToPlayer(reciever, "Vous avez reçu " + qty.ToString() + " " + item.Details.Name + " de " + (API.getEntitySyncedData(sender, "Nom_Prenom")).ToString());
                            UpdatePlayerInfo(sender);
                            UpdatePlayerInfo(reciever); 
                            API.resetEntityData(sender, "LastSelectedItem");
                            API.resetEntityData(sender, "NearbyList");
                        } else
                        {
                            API.sendNotificationToPlayer(sender, (API.getEntitySyncedData(reciever, "Nom_Prenom")).ToString() + " n'a pas la place dans sont inventaire");
                        }
                    }else
                    {
                        API.sendNotificationToPlayer(sender, "Vous en avez pas autant sur vous");
                    }
                }
                // Mettre en coffre VALIDATION
                if ((int)arguments[0] == 137)
                {
                    try
                    {
                        InventoryItem item = API.getEntityData(sender, "LastSelectedItem");
                        NetHandle reciever = Vehicles.Vehicle.GetVehicleInRange(sender, 3f);

                        if (Vehicles.Vehicle.LockVehicleState(reciever))
                        {
                            API.sendNotificationToPlayer(sender, "Le véhicule est fermer!");
                            return;
                        }

                        if (item == null || reciever == null) { API.sendNotificationToPlayer(sender, "~r~[ERROR]"); return; } 
                        if (API.getEntitySyncedData(reciever, "Locked") == true)
                        {
                            API.sendNotificationToPlayer(sender, "Le véhicule est fermer!");
                        }
                        else
                        {
                            Items.Items.InventoryHolder invplayer = API.getEntityData(sender, "InventoryHolder");
                            Items.Items.InventoryHolder invreciever = API.getEntityData(reciever, "InventoryHolder");
                            string plate = API.getEntitySyncedData(reciever, "Plate");
                            qty = Convert.ToInt32(arguments[1]);
                            if(qty <= 0) { API.sendNotificationToPlayer(sender, "ERREUR!"); return; }
                            var itemplayerqty = item.Quantity;
                            if (qty <= itemplayerqty)
                            {
                                if (invreciever.CheckWeightInventory(item.Details, qty))
                                {
                                    invplayer.RemoveItemFromInventory(item.Details, qty);
                                    invreciever.AddItemToInventory(item.Details, qty);

                                    API.sendNotificationToPlayer(sender, "Vous avez mis  ~r~" + qty.ToString() + " " + item.Details.Name + " ~s~dans le coffre du véhicule: " + plate);
                                }
                                else
                                {
                                    API.sendNotificationToPlayer(sender, plate + " n'a pas la place dans le coffre.");
                                }
                            }
                            else
                            {
                                API.sendNotificationToPlayer(sender, "Vous en avez pas autant sur vous");
                            }
                            API.resetEntityData(sender, "LastSelectedItem");
                            API.resetEntityData(sender, "NearbyList");
                            UpdatePlayerInfo(sender);
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

using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using System.Data;
using static LSRP_VFR.Items.Items;
using static LSRP_VFR.Mysql.DBPlayers;
namespace LSRP_VFR.Vehicles
{
    class Parking : Script
    {
        public Parking()
        {
            API.onResourceStart += OnResourceStart;
            API.onEntityEnterColShape += OnEntityEnterColShape;
            API.onEntityExitColShape += OnEntityExitColShape;
            API.onClientEventTrigger += ScriptEvent;
        }

        private void ScriptEvent(Client sender, string eventName, object[] arg)
        {
            try
            {
                if (eventName == "menu_handler_select_item")
                {
                    if ((int)arg[0] == 220) { SaveVehicle(sender); }
                    else if ((int)arg[0] == 221)
                    {
                        if (Players.Money.TakeMoney(sender, 250))
                        {
                            API.sendNotificationToPlayer(sender, "Vous avez rangé votre véhicule dans le garage.");
                            UpdatePlayerMoney(sender);
                            string plate = (String)API.getEntitySyncedData(sender.vehicle, "Plate");
                            Mysql.DBVehicles.SetNotActiveCar(plate);
                            API.deleteEntity(sender.vehicle);
                        }
                        else
                        {
                            API.sendNotificationToPlayer(sender, "Vous n'avez pas assez d'argent sur vous.");
                        }
                    }
                    else if ((int)arg[0] == 222)
                    {
                        DataTable result = Mysql.DBVehicles.GetVehicleInfo(sender);
                        if (result.Rows.Count != 0)
                        {
                            List<string> Actions = new List<string>();
                            List<string> label = new List<string>();
                            foreach (DataRow row in result.Rows)
                            {
                                Actions.Add(row["classname"].ToString());
                                label.Add((row["plate"]).ToString());
                            }
                            API.triggerClientEvent(sender, "bettermenuManager", 223, "Parking", "Menu parking: ", result, Actions, label);
                        }
                        else
                        {
                            API.sendNotificationToPlayer(sender, "Vous n'avez aucun véhicule dans le garage");
                        }
                    }
                    else if ((int)arg[0] == 223)
                    {
                        DataTable result = Mysql.DBVehicles.GetVehicleInfo(sender);
                        for (int i = 0; i < result.Rows.Count; i++)
                        {
                            if (i == (int)arg[1])
                            {
                                if (Players.Money.TakeMoney(sender, 250))
                                {
                                    DataRow myRow = result.Rows[i];
                                    VehicleHash vehicleHash = API.vehicleNameToModel(myRow["classname"].ToString());
                                    Vector3 playerpos = sender.position;
                                    var spawncar = API.createVehicle(vehicleHash, new Vector3(playerpos.X, playerpos.Y + 4f, playerpos.Z), new Vector3(0, 0, 0), 111, 111);

                                    InventoryHolder ivh = new InventoryHolder();
                                    ivh.Owner = spawncar.handle;
                                    API.shared.setEntityData(spawncar, "InventoryHolder", ivh);
                                    API.setEntityData(spawncar, "weight", 0);
                                    API.setEntityData(spawncar, "weight_max", Vehicles.Vehicle.GetVehicleWeight(vehicleHash));

                                    if (!((myRow["inventory"]).Equals("[]")))
                                    {
                                        var inventairerow = Convert.ToString(myRow["inventory"]);
                                        var inventaire = inventairerow.Split(new[] { "],[" }, StringSplitOptions.None);
                                        foreach (var I in inventaire)
                                        {
                                            var I2 = I.ToString().Replace("[", "").Replace("]", "");
                                            var I3 = I2.Split(new[] { "," }, StringSplitOptions.None);
                                            Item item = ItemByID(Convert.ToInt16(I3[0]));
                                            ivh.AddItemToInventory(item, Convert.ToInt16(I3[1]));
                                        }
                                    }

                                    string plate = (myRow["plate"]).ToString();
                                    API.setEntitySyncedData(spawncar, "VEHICLE_FUEL", Convert.ToDecimal(myRow["fuel"]));
                                    API.setVehicleNumberPlate(spawncar, plate);
                                    API.setEntitySyncedData(spawncar, "Plate", plate);
                                    API.setEntitySyncedData(spawncar, "Owner", (myRow["pid"]).ToString());
                                    API.setVehicleLocked(spawncar, true);
                                    API.setEntitySyncedData(spawncar, "Locked", true);
                                    API.setEntitySyncedData(spawncar, "VEHICLE_FUEL", 100);
                                    API.setEntitySyncedData(spawncar, "VEHICLE_FUEL_MAX", 100);
                                    int color = Convert.ToInt32(myRow["color"]);
                                    API.setVehiclePrimaryColor(spawncar, color);
                                    Mysql.DBVehicles.SetActiveCar(plate);
                                    UpdatePlayerMoney(sender);
                                    API.setVehicleEngineStatus(spawncar, false);
                                }
                                else
                                {
                                    API.sendNotificationToPlayer(sender, "Vous n'avez pas assez d'argent sur vous.");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                API.consoleOutput("~r~[ERROR][EVENT] : ~s~" + e.ToString());
            }
        }

        private void OnEntityEnterColShape(ColShape colshape, NetHandle entity)
        {
            if (colshape.hasData("Parking"))
            {
                Client player = API.getPlayerFromHandle(entity);
                if (player != null)
                {
                    if (API.isPlayerInAnyVehicle(player))
                    {
                        List<string> Actions = new List<string>();
                        List<string> label = new List<string>();
                        Actions.Add("Ranger le véhicule");
                        label.Add("250$");
                        API.triggerClientEvent(player, "bettermenuManager", 220, "Parking", "Menu parking: ", false, Actions, label);
                    }
                    else
                    {
                        List<string> Actions = new List<string>();
                        List<string> label = new List<string>();
                        Actions.Add("Sortir un véhicule");
                        label.Add("250$");
                        API.triggerClientEvent(player, "bettermenuManager", 222, "Parking", "Menu parking: ", false, Actions, label);
                    }
                }
            }
        }

        private void OnEntityExitColShape(ColShape colshape, NetHandle entity)
        {
            if (colshape.hasData("Parking"))
            {
                var player = API.getPlayerFromHandle(entity);
                if (player == null) return;
                API.triggerClientEvent(player, "menu_handler_close_menu");
            }
        }

        private void OnResourceStart()
        {
            try
            {
                List<Vector3> parkingpos = new List<Vector3>();
                parkingpos.Add(new Vector3(-891.0928, -344.4663, 34.53425));
                parkingpos.Add(new Vector3(323.0428, -683.402, 28.67723));
                int count = 0;
                foreach (Vector3 pos in parkingpos)
                {
                    var blip = API.createBlip(pos);
                    API.setBlipSprite(blip, 50);
                    API.setBlipShortRange(blip, true);
                    API.setBlipName(blip, "Parking");
                    var ColMarkVehicule = API.createCylinderColShape(pos, 3.0f, 1.0f);
                    var MarkVeh = API.createMarker(1, pos - new Vector3(0, 0, 1f), new Vector3(), new Vector3(), new Vector3(3f, 3f, 1f), 100, 255, 255, 255);
                    ColMarkVehicule.setData("Parking", count);
                    count = count + 1;
                }
            }
            catch (Exception e)
            {
                API.consoleOutput("~r~[ERROR][VEHICLE] : ~s~" + e.ToString());
            }
        }

        public static void SaveVehicle(NetHandle entity)
        {
            try
            {
                Client player = API.shared.getPlayerFromHandle(entity);
                if (player != null)
                {
                    if (API.shared.isPlayerInAnyVehicle(player))
                    {
                        NetHandle veh = player.vehicle;
                        if (player.vehicle != null)
                        {
                            string idplate = API.shared.getEntitySyncedData(veh, "Plate");
                            string owner = API.shared.getEntitySyncedData(veh, "Owner");
                            if (owner == player.socialClubName)
                            {
                                List<string> Actions = new List<string>();
                                List<string> label = new List<string>();
                                Actions.Add("Valider");
                                label.Add("Prix: " + 250 + "$");
                                API.shared.triggerClientEvent(player, "bettermenuManager", 221, "Parking", "Mettre au parking: " + idplate, veh, Actions, label);
                            }
                            else
                            {
                                API.shared.sendNotificationToPlayer(player, "Ce véhicule ne vous appartient pas!");
                            }
                        }
                        else
                        {
                            API.shared.sendNotificationToPlayer(player, "Vous devait être dans le véhicule!");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][VEHICLE] : ~s~" + e.ToString());
            }
        }
    }
}

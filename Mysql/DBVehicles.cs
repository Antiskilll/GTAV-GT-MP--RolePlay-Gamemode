using System;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.Data;
using static LSRP_VFR.Items.Items;
using static LSRP_VFR.Mysql.Database;
using static LSRP_VFR.Mysql.DBPlayers;
namespace LSRP_VFR.Mysql
{
    class DBVehicles : Script
    {
        public DBVehicles()
        {
            API.onPlayerExitVehicle += OnPlayerExitVehicle;
            API.onResourceStop += OnResourceStop;      
        }

        public static DataTable GetConcessVehicles(string concess)
        {
            try
            {
                DataTable result = GetQuery("SELECT vehiclehash, name, price, poids  FROM concessionnaire WHERE nameconcess='" + concess + "'");
                return result;
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR] : ~s~" + e.ToString());
                return null;
            }
        }

        public static void GetCarSpawn()
        {
            try
            {
                API.shared.consoleOutput("[VEHICLE] Spawn des véhicules sur la map");
                Vector3 spawncarPos;
                Vector3 spawncarRot;
                DataTable result = GetQuery("SELECT classname, pid, plate, inventory ,position, rotation, color, fuel FROM vehicles WHERE active='1'");
                if (result.Rows.Count != 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        if (!(row["position"]).Equals("[]") || !(row["position"]).Equals(0))
                        {
                            var position0 = Convert.ToString(row["position"]);
                            var position1 = position0.Split(new[] { "],[" }, StringSplitOptions.None);

                            var posX0 = position1[0].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");
                            var posY0 = position1[1].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");
                            var posZ0 = position1[2].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");

                            float posX1 = Convert.ToSingle(posX0);
                            float posY1 = Convert.ToSingle(posY0);
                            float posZ1 = Convert.ToSingle(posZ0);
                            spawncarPos = new Vector3(posX1, posY1, posZ1 + 0.5f);
                        }
                        else
                        {
                            spawncarPos = new Vector3(408, -1636, 28);
                        }

                        if (!(row["rotation"]).Equals("[]") || !(row["rotation"]).Equals(0))
                        {
                            var rot0 = Convert.ToString(row["rotation"]);
                            var rot1 = rot0.Split(new[] { "],[" }, StringSplitOptions.None);

                            var rotX0 = rot1[0].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");
                            var rotY0 = rot1[1].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");
                            var rotZ0 = rot1[2].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");

                            float rotX1 = Convert.ToSingle(rotX0);
                            float rotY1 = Convert.ToSingle(rotY0);
                            float rotZ1 = Convert.ToSingle(rotZ0);
                            spawncarRot = new Vector3(rotX1, rotY1, rotZ1);
                        }
                        else
                        {
                            spawncarRot = new Vector3(0, 0, 0);
                        }

                        VehicleHash vehicleHash = API.shared.vehicleNameToModel(row["classname"].ToString());
                        var spawncar = API.shared.createVehicle(vehicleHash, spawncarPos, spawncarRot, 111, 111);
                        InventoryHolder ivh = new InventoryHolder();
                        ivh.Owner = spawncar.handle;
                        API.shared.setEntityData(spawncar, "InventoryHolder", ivh);
                        API.shared.setEntityData(spawncar, "weight", 0);
                        API.shared.setEntityData(spawncar, "weight_max", Vehicles.Vehicle.GetVehicleWeight(vehicleHash));
                        if (!((row["inventory"]).Equals("[]")))
                        {
                            var inventairerow = Convert.ToString(row["inventory"]);
                            var inventaire = inventairerow.Split(new[] { "],[" }, StringSplitOptions.None);
                            foreach (var I in inventaire)
                            {
                                var I2 = I.ToString().Replace("[", "").Replace("]", "");
                                var I3 = I2.Split(new[] { "," }, StringSplitOptions.None);
                                Item item = ItemByID(Convert.ToInt16(I3[0]));
                                ivh.AddItemToInventory(item, Convert.ToInt16(I3[1]));
                            }
                        }

                        int color = Convert.ToInt32(row["color"]);
                        API.shared.setVehiclePrimaryColor(spawncar, color);
                        string plate = (row["plate"]).ToString();
                        API.shared.setVehicleNumberPlate(spawncar, plate);
                        API.shared.setEntitySyncedData(spawncar, "Plate", plate);
                        API.shared.setEntitySyncedData(spawncar, "Owner", (row["pid"]).ToString());
                        
                        API.shared.setVehicleLocked(spawncar, true);
                        API.shared.setEntitySyncedData(spawncar, "Locked", true);
                        API.shared.setEntitySyncedData(spawncar, "VEHICLE_FUEL", Convert.ToInt32(row["fuel"]));
                        API.shared.setEntitySyncedData(spawncar, "VEHICLE_FUEL_MAX", 100);
                        API.shared.setVehicleEngineStatus(spawncar, false);
                    }
                }
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR] : ~s~" + e.ToString());
            }
        }

        public static void DeleteVehicleDB(string plate)
        {
            try
            {
                String sql = String.Format("DELETE FROM vehicles WHERE plate = '{0}'", plate);
                InsertQuery(sql);
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR] : ~s~" + e.ToString());
            }
        }

        private static void OnPlayerExitVehicle(Client player, NetHandle vehicle)
        {
            try
            {
                UpdatePlayerInfo(player);
                SaveVehicle(vehicle);
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR] : ~s~" + e.ToString());
            }
        }


        public static Boolean GetPlate(String plate)
        {
            try
            {
                if (plate != null)
                {
                    DataTable result = GetQuery("SELECT * FROM vehicles WHERE plate='" + plate + "'");
                    if (result.Rows.Count != 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR] : ~s~" + e.ToString());
                return false;
            }
        }

        public static void InsertVehicle(Client player, VehicleHash model, string plate, Vector3 position, int color)
        {
            try
            {
                if (player != null || plate != null || position != null)
                {
                    string pos = "[[" + position.X.ToString() + "],[" + position.Y.ToString() + "],[" + position.Z.ToString() + "]]"; ;
                    InsertQuery(String.Format("INSERT INTO vehicles ( side, classname, type, pid, plate, color, inventory, fuel, position, rotation, active) VALUES ('CIV','{0}','ground','{1}','{2}','{4}','[]','100','{3}','[[0],[0],[0]]','1')", model.ToString(), player.socialClubName, plate, pos, color));
                }
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR] : ~s~" + e.ToString());
            }
        }

        public static void SaveVehicle(NetHandle vehicle)
        {
            try
            {
                string plate = API.shared.getEntitySyncedData(vehicle, "Plate");
                if (plate == null) return;
                InventoryHolder inventory = API.shared.getEntityData(vehicle, "InventoryHolder");
                Vector3 VehiclePos = API.shared.getEntityPosition(vehicle);
                Vector3 VehicleRot = API.shared.getEntityRotation(vehicle);
                string pos = "[[" + VehiclePos.X.ToString() + "],[" + VehiclePos.Y.ToString() + "],[" + VehiclePos.Z.ToString() + "]]";
                string rot = "[[" + VehicleRot.X.ToString() + "],[" + VehicleRot.Y.ToString() + "],[" + VehicleRot.Z.ToString() + "]]";
                var invs = "";
                foreach (InventoryItem ii in inventory.Inventory)
                {
                    invs += "[" + ii.Details.ID.ToString() + "," + ii.Quantity.ToString() + "],";
                }
                char[] car = { ',' };
                string inventaires = "[" + invs.TrimEnd(car) + "]";
                string fuel = Convert.ToString(API.shared.getEntitySyncedData(vehicle, "VEHICLE_FUEL"));
                string query = String.Format("UPDATE vehicles SET position='{0}', rotation='{1}', inventory='{2}', fuel='{3}' WHERE plate='{4}'",
                        pos,
                        rot,
                        inventaires,
                        fuel,
                        plate
                    );
                InsertQuery(query);

            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR] : ~s~" + e.ToString());
            }
        }

        public static void SetActiveCar(String plate)
        {
            try
            {
                InsertQuery(String.Format("UPDATE vehicles SET active='1' WHERE plate='{0}'", plate));
            }
            catch (Exception ex)
            {
                API.shared.consoleOutput(ex.Message.ToString());
            }
        }

        public static void SetNotActiveCar(String plate)
        {
            try
            {
                InsertQuery(String.Format("UPDATE vehicles SET active='0' WHERE plate='{0}'", plate));
            }
            catch (Exception ex)
            {
                API.shared.consoleOutput(ex.Message.ToString());
            }
        }

        public static DataTable GetVehicleInfo(Client player)
        {
            try
            {
                DataTable result = GetQuery(String.Format("SELECT classname, pid, plate, inventory ,position, rotation, color, fuel FROM vehicles WHERE active='0' AND pid='{0}'", player.socialClubName));
                return result;
            }
            catch (Exception ex)
            {
                API.shared.consoleOutput(ex.Message.ToString());
                return null;
            }
        }

        private static void OnResourceStop()
        {
            var AllVehicle = API.shared.getAllVehicles();
            foreach (NetHandle vehicle in AllVehicle)
            {
                SaveVehicle(vehicle);
                API.shared.sleep(100);
            }
        }
    }
}

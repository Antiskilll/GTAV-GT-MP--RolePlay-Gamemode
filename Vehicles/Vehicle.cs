using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System;
using LSRP_VFR.Faction.EMS;
using LSRP_VFR.Faction.LSPD;

namespace LSRP_VFR.Vehicles
{
    public class Vehicle : Script
    {

        public Vehicle()
        {
            API.onVehicleDeath += OnVehicleDeath;
        }

        public static string RandomPlate()
        {
            try
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var stringChars = new char[8];
                var random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }
                string resultat = new String(stringChars);

                if (resultat.Equals(Mysql.DBVehicles.GetPlate(resultat)))
                {
                    RandomPlate();
                }
                return resultat;
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][VEHICLE] : ~s~" + e.ToString());
                return "";
            }
        }

        public static int GetVehicleWeight(VehicleHash vehicle)
        {
            try
            {
                int weight = 0;
                int switchExpression = API.shared.getVehicleClass(vehicle);
                switch (switchExpression)
                {
                    // TODO: Si le type de véhicule n'est pas listé, la fonction return 0 de poid d'inventaire.
                    case 0:
                        weight = 30; // Compacts
                        break;
                    case 2:
                        weight = 70; // SUV
                        break;
                    case 3:
                        weight = 60; // Coupes
                        break;
                    case 4:
                        weight = 60; // Muscle Car
                        break;
                    case 5:
                        weight = 30; // Super Car1
                        break;
                    case 6:
                        weight = 30; // Super Car2
                        break;
                    case 12:
                        weight = 90; // Vans
                        break;
                    case 18:
                        weight = 60; // Emergency
                        break;
                    case 20:
                        weight = 200; // Commercial
                        break;
                    default:
                        weight = 0;
                        break;
                }
                return weight;
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][VEHICLE] : ~s~" + e.ToString());
                return 0;
            }
        }

        private void OnVehicleDeath(NetHandle entity)
        {
            try
            {
                string idplate = API.getEntitySyncedData(entity, "Plate");
                string owner = API.getEntitySyncedData(entity, "Owner");
                //API.call("Database", "DeleteVehicleDB", idplate);
                Mysql.DBVehicles.SetNotActiveCar(idplate);
                API.sleep(64000);
                API.deleteEntity(entity);
                Client carowner = Players.Player.GetClientPlayerByName(owner);
                if (carowner != null)
                {
                    string message = "Votre véhicule immatriculé: " + idplate + " a été retrouvé détruit.";
                    API.sendNotificationToPlayer(carowner, "~r~SMS reçu: ~s~'" + message + "'");
                    API.playSoundFrontEnd(carowner, "Menu_Accept", "Phone_SoundSet_Default");
                }
            }
            catch (Exception e)
            {
                API.consoleOutput("~r~[ERROR][VEHICLE] : ~s~" + e.ToString());
            }

        }

        public static void LockUnlockVehOwner(Client sender)
        {
            try
            {
                NetHandle veh = GetVehicleInRange(sender, 3f);
                if (veh == null) return;
                if (!API.shared.hasEntitySyncedData(veh, "Owner")) return;
                if (IsOwnerVehicle(veh, sender) || CopVehicle(veh) && LSPD_Service.IsCop(sender) || EMSVehicle(veh) && Hospital.IsMedic(sender))
                {
                    bool locked = LockVehicleState(veh);
                    if (locked)
                    {
                        UnlockVehicle(veh);
                        API.shared.sendNotificationToPlayer(sender, "~r~[VEHICULE] ~s~Portes déverrouillées !");
                    } else {
                        LockVehicle(veh);
                        API.shared.sendNotificationToPlayer(sender, "~r~[VEHICULE] ~s~Portes verrouillées !");
                    }
                }
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][VEHICLE] : ~s~" + e.ToString());
            }
        }

        public static bool CopVehicle(NetHandle veh)
        {
            try
            {
                return (API.shared.getEntitySyncedData(veh, "Owner") == "Cops");
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][VEHICLE] : ~s~" + e.ToString());
                return false;

            }
        }

        public static bool EMSVehicle(NetHandle veh)
        {
            try
            {
                return (API.shared.getEntitySyncedData(veh, "Owner") == "EMS");
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][VEHICLE] : ~s~" + e.ToString());
                return false;

            }
        }

        public static void LockVehicle(NetHandle veh)
        {
            try
            {
                if (LockVehicleState(veh)) { return; }
                API.shared.setEntitySyncedData(veh, "Locked", true);
                API.shared.setVehicleLocked(veh, true);
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][VEHICLE] : ~s~" + e.ToString());

            }
        }

        public static void UnlockVehicle(NetHandle veh)
        {
            try
            {
                if (!LockVehicleState(veh)) { return; }
                API.shared.setEntitySyncedData(veh, "Locked", false);
                API.shared.setVehicleLocked(veh, false);
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][VEHICLE] : ~s~" + e.ToString());

            }
        }

        public static bool LockVehicleState(NetHandle veh)
        {
            
            try
            {
                return API.shared.getEntitySyncedData(veh, "Locked");
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][VEHICLE] : ~s~" + e.ToString());
                return false;
            }
            
        }

        public static bool IsOwnerVehicle(NetHandle veh, Client sender)
        {
            return (API.shared.getEntitySyncedData(veh, "Owner") == sender.socialClubName);
        }

        public void CrocheteVehicle(Client sender)
        {
            try { 
                NetHandle vehicle = GetVehicleInRange(sender, 3f);
                API.triggerClientEvent(sender, "display_subtitle", "Crochetage du véhicule ...", 5000);
                API.delay(5000, true, () => {
                    API.sendNotificationToPlayer(sender, "~r~[VEHICULE] ~s~Portes dévérouillées !");
                    API.setVehicleLocked(vehicle, false);
                    API.setEntitySyncedData(vehicle, "Locked", false);
                });
            }catch(Exception e)
            {
                API.consoleOutput("~r~[ERROR][VEHICLE] : ~s~" + e.ToString());
            }
        }

        public void GetVehicleInfo(Client sender)
        {
            try
            {
                NetHandle vehicle = GetVehicleInRange(sender, 3f);
                API.triggerClientEvent(sender, "display_subtitle", "Questionnement de la base de donnée...", 5000);
                var owner = API.getEntitySyncedData(vehicle, "Owner");
                //Client carowner = (Client)API.call("Player", "GetClientPlayerByName", owner);
                Client carowner = Players.Player.GetClientPlayerByName(owner);
                if (carowner != null)
                {
                    string carownername = API.shared.getEntitySyncedData(carowner, "Nom_Prenom");
                    API.delay(5000, true, () => { API.triggerClientEvent(sender, "display_subtitle", "Le véhicule: " + API.getEntitySyncedData(vehicle, "Plate") + " appartient à " + carownername, 3000); });
                }
                else
                {
                    API.delay(5000, true, () => { API.triggerClientEvent(sender, "display_subtitle", "Le véhicule: " + API.getEntitySyncedData(vehicle, "Plate") + " est abandonné"); });
                }
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][VEHICLE] : ~s~" + e.ToString());
            }
        }

        public static NetHandle GetVehicleInRange(Client sender, float distance = 1000.0f)
        {
            NetHandle handleReturned = new NetHandle();
            foreach (var veh in API.shared.getAllVehicles())
            {
                Vector3 vehPos = API.shared.getEntityPosition(veh);
                float distanceVehicleToPlayer = sender.position.DistanceTo(vehPos);
                if (distanceVehicleToPlayer < distance)
                {
                    distance = distanceVehicleToPlayer;
                    handleReturned = veh;
                }
            }
            return handleReturned;
        }
    }
}

/*
            try
            {

            }
            catch (Exception e)
            {
                API.consoleOutput("~r~[ERROR][VEHICLE] : ~s~" + e.ToString());
            }
*/

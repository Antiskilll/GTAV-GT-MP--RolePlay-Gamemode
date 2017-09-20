using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System.Collections.Generic;
using System;

namespace LSRP_VFR.Jobs
{
    public class Fourriere : Script
    {
        private Vector3 jobspos = new Vector3(370.3964, -1608.694, 29.29194);
        private Vector3 spawnpos = new Vector3(401.0005, -1633.698, 29.29196);
        private List<Vehicle> vehicle = new List<Vehicle>();

        public Fourriere()
        {
            API.onResourceStart += OnResourceStart;
            API.onPlayerEnterVehicle += OnPlayerEnterVehicle;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        private void OnPlayerEnterVehicle(Client player, NetHandle vehicle)
        {
            if (OnTowVehicle(player) && IsFourriereMan(player))
            {
                API.sendNotificationToPlayer(player, "~r~[JOB] ~s~Appuyer sur la touche ~r~Entrée ~s~pour remorquer un véhicule");
            }
        }

        public static void TowVehicle(Client player)
        {
            if (OnTowVehicle(player) && IsFourriereMan(player))
            {
                foreach (var veh in API.shared.getAllVehicles())
                {
                    float distance = 5f;
                    Vector3 vehPos = API.shared.getEntityPosition(veh);
                    float distanceVehicleToPlayer = player.position.DistanceTo(vehPos);
                    if (distanceVehicleToPlayer < distance && veh != player.vehicle)
                    {
                        if(API.shared.getEntityData(veh, "Towed") == true)
                        {
                            DetachVehicleToTowTruck(player, player.vehicle.handle, veh);
                        }else
                        {
                            AttachVehicleToTowTruck(player, player.vehicle.handle, veh, -1, 0.0, 1.5, 0.0);
                            API.shared.sendNotificationToPlayer(player, "~r~[JOB] ~s~Appuyer sur la touche ~r~Entrée ~s~pour détacher un véhicule");
                        }
                    }
                }
            }
        }

        private void OnResourceStart()
        {

            Marker marker = API.createMarker(1, jobspos - new Vector3(0, 0, 1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 100, 255, 255, 255);
            CylinderColShape colshape = API.createCylinderColShape(jobspos, 1f, 1f);
            Blip myBlip = API.createBlip(jobspos);
            API.setBlipShortRange(myBlip, true);
            API.setBlipSprite(myBlip, 88);
            API.setBlipName(myBlip, "Fourrière");
            CylinderColShape f_colShape = API.createCylinderColShape(jobspos, 1.0f, 1.0f);
            f_colShape.onEntityEnterColShape += (shape, entity) =>
            {
                var players = API.getPlayerFromHandle(entity);
                if (players == null)
                {
                    return;
                }
                else
                {
                    List<String> Actions = new List<string>();
                    Actions.Add("Demarrer Mission");
                    Actions.Add("Quitter la Mission");
                    Actions.Add("Quitter le menu");
                    API.triggerClientEvent(players, "bettermenuManager", 150, "Menu fourrière", "", false, Actions);
                }

            };


            int[] doors = {
                API.exported.doormanager.registerDoor(-1483471451, new Vector3(413.3649, -1620.036, 28.34509)),
                API.exported.doormanager.registerDoor(-1483471451, new Vector3(418.2911, -1651.395, 28.29333)),
            };
            foreach (int door in doors)
            {
                API.exported.doormanager.setDoorState(door, true, 1);
            }
        }


        private void OnClientEventTrigger(Client sender, string eventName, params object[] args)
        {
            if (eventName == "menu_handler_select_item")
            {
                if ((int)args[0] == 150)
                {
                    if ((int)args[1] == 0)
                    {
                        StartMission(sender);
                    }
                    else if ((int)args[1] == 1)
                    {
                        QuitterMission(sender);
                    }
                }
            }
        }

        private void QuitterMission(Client sender)
        {

            if (IsFourriereMan(sender) && sender.getData("IS_FOURRIERE") == true)
            {
                var vehicleFourriere = vehicle.Find(x => x.getData("Owner") == sender.socialClubName);
                API.deleteEntity(vehicleFourriere);
                vehicle.Remove(vehicleFourriere);
                API.sendNotificationToPlayer(sender, "~r~[JOB] ~s~Vous avez quitté votre travail dépanneur.");
                API.sendNotificationToPlayer(sender, "~r~[JOB] ~s~Nous avons ranger votre véhicule");
                sender.setData("IS_FOURRIERE", false);
            }
        }

        private void StartMission(Client sender)
        {

            try
            {
                if (IsFourriereMan(sender))
                {
                    API.sendNotificationToPlayer(sender, "~r~[JOB] ~s~Vous êtes dorénavant dépanneur.");
                    API.sendNotificationToPlayer(sender, "~r~[JOB] ~s~Votre véhicule vous attend sur le parking.");

                    //Vehicle towtruck = API.createVehicle(VehicleHash.TowTruck2, spawnpos, new Vector3(0, 0, -51.54338), 0, 0, 0);
                    Vehicle towtruck = API.createVehicle(VehicleHash.Flatbed, spawnpos, new Vector3(0, 0, -51.54338), 0, 0, 0);
                    sender.setData("IS_FOURRIERE", true);
                    API.setEntitySyncedData(towtruck, "VEHICLE_FUEL", 100);
                    API.setEntitySyncedData(towtruck, "VEHICLE_FUEL_MAX", 100);
                    API.setEntityData(towtruck, "weight", 0);
                    API.setEntityData(towtruck, "weight_max", 0);
                    API.setVehicleNumberPlate(towtruck, "LSantos");
                    API.setEntitySyncedData(towtruck, "Locked", false);
                    vehicle.Add(towtruck);
                }else
                {
                    API.sendNotificationToPlayer(sender, "~r~[JOB] ~s~Vous n'avez pas de licence dépanneur.");
                }
            }
            catch (Exception e)
            {
                API.consoleOutput("~r~[ERROR][FOURRIERE] : ~s~" + e.ToString());
            }
        }

        public static bool OnTowedVehicle(Client player, NetHandle towtruck, NetHandle vehtowed)
        {
            return API.shared.fetchNativeFromPlayer<bool>(player, Hash.IS_VEHICLE_ATTACHED_TO_TOW_TRUCK, towtruck, vehtowed);
        }

        public static bool OnTowVehicle(Client player)
        {
            if (player.vehicle == null) return false;
            if (player.vehicle.model == (Int32)VehicleHash.TowTruck2 || player.vehicle.model == (Int32)VehicleHash.Flatbed)
            {
                return true;
            }
            return false;
        }

        public static bool IsFourriereMan(Client player)
        {
            if (API.shared.getEntityData(player, "P_Fourriere") == true)
            {
                return true;
            }
            return false;
        }

        public static void CallFourriere(Client sender)
        {
            try
            {
                var players = API.shared.getAllPlayers();
                foreach (var player in players)
                {
                    if (API.shared.getEntityData(player, "IS_FOURRIERE") == true)
                    {
                        API.shared.triggerClientEvent(player, "display_subtitle", "~r~~h~[APPEL] de " + API.shared.getEntitySyncedData(sender, "Nom_Prenom") + "~h~~y~ : \n ~w~ Position transmise!", 13000);
                        API.shared.sendNativeToPlayer(player, Hash.SET_NEW_WAYPOINT, sender.position.X, sender.position.Y);
                    }
                }

            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][FOURRIERE] : ~s~" + e.ToString());
            }
        }

        public static void AttachVehicleToTowTruck(Client sender, NetHandle towtruck, NetHandle car, int p0 = 0, double p1 = 0, double p2 = 0, double p3 = 0)
        {
            if (!(bool)API.shared.getEntityData(towtruck, "UseTow"))
            {
                API.shared.setEntityData(car, "Towed", true);
                API.shared.attachEntityToEntity(car, towtruck, "0", new Vector3(0, -3f, 1f), new Vector3(0, 0, 0));
            }
        }

        public static void DetachVehicleToTowTruck(Client sender, NetHandle towtruck, NetHandle car)
        {
            if ((bool)API.shared.getEntityData(towtruck, "UseTow"))
            {
                API.shared.setEntityData(towtruck, "UseTow", false);
                API.shared.setEntityData(car, "Towed", false);
                API.shared.detachEntity(car);
                Vector3 towtruckPos = API.shared.getEntityPosition(car);
                API.shared.setEntityPosition(car, new Vector3(towtruckPos.X - 4f, towtruckPos.Y, towtruckPos.Z));
            }
            else
            {
                // ???
            }
        }
    }
}

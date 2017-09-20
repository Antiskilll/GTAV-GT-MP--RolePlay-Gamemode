using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using System.Threading;
using static LSRP_VFR.Mysql.DBPlayers;
namespace LSRP_VFR.Jobs
{
    public class Taxi : Script
    {
        private Vector3 PositionJob = new Vector3(895.4366, -179.6842, 74.70023);
        private Vector3 positionVeh = new Vector3(908.5834, -176.1449, 74.17666);
        private List<Vehicle> vehicle = new List<Vehicle>();
        private static CylinderColShape colshape;

        public Taxi()
        {
            var myBlip = API.createBlip(PositionJob);
            API.setBlipShortRange(myBlip, true);
            API.setBlipSprite(myBlip, 198);
            API.setBlipColor(myBlip, 46);
            API.setBlipName(myBlip, "Metier : Taxi");
            var m_colShape = API.createCylinderColShape(PositionJob, 1.0f, 1.0f);
            var marker = API.createMarker(1, PositionJob - new Vector3(0, 0, 1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 100, 255, 255, 255);
            m_colShape.onEntityEnterColShape += (shape, entity) =>
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
                    Actions.Add("Finir la Mission");
                    Actions.Add("Quitter le menu");
                    API.triggerClientEvent(players, "bettermenuManager", 103, "MenuTaxi", "", false, Actions);
                }

            };
            API.onClientEventTrigger += OnClientEventTrigger;
            API.onPlayerEnterVehicle += OnPlayerEnterVehicle;
        }

        private void OnPlayerEnterVehicle(Client player, NetHandle vehicle)
        {
            if (player.vehicleSeat != -1)
            {
                Vehicle playervehicle = player.vehicle;
                Client[] occupants = playervehicle.occupants;
                foreach(Client occupant in occupants)
                {
                    if(occupant.vehicleSeat == -1 && occupant.getData("Taxi") == true)
                    {
                        StartTransport(occupant, player);
                    }
                }
            } 
        }

        private void OnClientEventTrigger(Client sender, string eventName, params object[] args)
        {
            if (eventName == "menu_handler_select_item")
            {
                if ((int)args[0] == 103)
                {
                    if ((int)args[1] == 0)
                    {
                        if (IsTaxiMan(sender))
                        {
                            StartService(sender);
                        }
                        else
                        {
                            API.sendNotificationToPlayer(sender, "~r~[JOB] ~s~Vous n'avez pas de licence de chauffeur de taxi.");
                        }
                    }
                    if ((int)args[1] == 1)
                    {
                        StopMission(sender);
                    }
                }
                if ((int)args[0] == 104)
                {
                    if ((int)args[1] == 0)
                    {
                        AccepterAppel(sender);
                    }
                }
            }
        }
        private void AccepterAppel(Client player)
        {
            player.setData("Appel", true);
        }
        private void StartService(Client player)
        {

            if (player.getData("Taxi") == false || player.getData("Taxi") == null)
            {
                player.setData("Taxi", true);
                VehicleHash vehicleJob = API.vehicleNameToModel("Taxi");
                var vehicleTaxi = API.createVehicle(vehicleJob, positionVeh, new Vector3(), 88, 88);
                API.setEntitySyncedData(vehicleTaxi, "VEHICLE_FUEL", 100);
                API.setEntitySyncedData(vehicleTaxi, "VEHICLE_FUEL_MAX", 100);
                API.setEntityData(vehicleTaxi, "weight", 0);
                API.setEntityData(vehicleTaxi, "weight_max", 0);
                API.setVehicleNumberPlate(vehicleTaxi, "TAXI");
                vehicleTaxi.setData("Owner", player.socialClubName);
                vehicle.Add(vehicleTaxi);
            }
            else
            {
                API.sendNotificationToPlayer(player, "Vous avez déjà un taxi");
            }

        }
        public static void CallTaxi(Client sender)
        {
            List<Client> clients = EntityManager.GetClientList();
            List<Client> taxi = new List<Client>();
            clients.ForEach(delegate (Client client)
            {
                if(client.getData("Taxi") == true)
                {
                    taxi.Add(client);
                }
            });
            if(taxi.Count > 0)
            {
                if (MarkerClient(sender, taxi))
                {
                    API.shared.sendNotificationToPlayer(sender, "Un conducteur de taxi a reçu votre message");
                }
                else
                {
                    API.shared.sendNotificationToPlayer(sender, "Il semblerait qu'aucun taxi ne soit actuellement disponible");
                }
            }
            else
            {
                API.shared.sendNotificationToPlayer(sender, "Désolé mais aucun taxi n'est actuellement en service");
            }
        }
        public static bool MarkerClient(Client sender,List <Client> player)
        {
            player.ForEach(delegate (Client client)
            {
                List<String> Actions = new List<string>();
                Actions.Add("Accepter l'appel");
                Actions.Add("Refuser l'appel");
                API.shared.triggerClientEvent(client, "bettermenuManager", 104, "Appel Client", "", false, Actions);
            });
            bool AppelAccepter = false;
            int count = 0;
            while (AppelAccepter == false && count < 60)
            {
                player.ForEach(delegate (Client client)
                {
                    if(client.getData("Appel") == true)
                    {
                        AppelAccepter = true;
                        MarkerManager(client, sender.position);
                        client.setData("Appel", false);
                    }
                });
                Thread.Sleep(500);
                count++;
            }
            return AppelAccepter;
        }
        private void StartTransport(Client taxi,Client passager)
        {
            int cachPassager = passager.getSyncedData("Money"); 
            int cashTaxi = taxi.getSyncedData("Money");
            int tarif = 1;
            int montant = 0;
            int compteur = 0;
            while (passager.isInVehicle)
            {
                if(compteur%60 == 0)
                {
                    montant += tarif;
                }
                API.triggerClientEvent(taxi,"update_taxi_fare", true, tarif, montant, passager.socialClubName);
                API.triggerClientEvent(passager, "update_taxi_fare", true, tarif, montant, passager.socialClubName);
                Thread.Sleep(500);
            }
            API.triggerClientEvent(taxi, "update_taxi_fare", false, tarif, montant, passager.socialClubName);
            API.triggerClientEvent(passager, "update_taxi_fare", false, tarif, montant, passager.socialClubName);
            cashTaxi += montant;
            cachPassager -= montant;
            passager.setSyncedData("Money",cachPassager);
            taxi.setSyncedData("Money", cashTaxi);
            UpdatePlayerMoney(taxi);
            UpdatePlayerMoney(passager);
        }
        public static void MarkerManager(Client player, Vector3 position)
        {
            API.shared.triggerClientEvent(player, "removemarkerblip");
            API.shared.triggerClientEvent(player, "markerblip", position - new Vector3(0, 0, 1f));
            
            colshape = API.shared.createCylinderColShape(position, 1f, 1f);
            bool colision = false;
            colshape.onEntityEnterColShape += (shape, entity) =>
            {
                var players = API.shared.getPlayerFromHandle(entity);

                if (players == null)
                {
                    return;
                }
                else
                {
                    if (players.handle == player.handle)
                    {
                        API.shared.triggerClientEvent(player, "removemarkerblip");
                        colision = true;
                    }
                }

            };
            while (colision == false)
            {
                Thread.Sleep(1000);
            }
            API.shared.deleteColShape(colshape);
        }

        public void StopMission(Client player)
        {
            if (IsTaxiMan(player) && player.getData("Taxi") == true)
            {
                var vehicleTaxi = vehicle.Find(x => x.getData("Owner") == player.socialClubName);
                API.deleteEntity(vehicleTaxi);
                vehicle.Remove(vehicleTaxi);
                player.setData("Taxi", false);
                API.shared.deleteColShape(colshape);
            }
            else
            {
                API.sendNotificationToPlayer(player, "Vous n'êtes pas chauffeur de taxi.");
            }
        }

        public static bool IsTaxiMan(Client player)
        {
            if (API.shared.getEntityData(player, "P_Taxi") == true)
            {
                return true;
            }
            return false;
        }
    }
}

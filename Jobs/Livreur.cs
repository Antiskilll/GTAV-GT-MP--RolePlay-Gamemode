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
    public class Livreur : Script
    {
        private Vector3 PositionJob = new Vector3(156.4212, -1065.724, 30.05423);
        private Vector3 positionVeh = new Vector3(150.0459, -1075.73, 29.19238);

        public Livreur()
        {
            API.onResourceStart += OnResourceStart;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        private void OnResourceStart()
        {
            Blip myBlip = API.createBlip(PositionJob);
            API.setBlipShortRange(myBlip, true);
            API.setBlipSprite(myBlip, 67);
            API.setBlipName(myBlip, "Metier : Livreur");
            CylinderColShape m_colShape = API.createCylinderColShape(PositionJob, 1.0f, 1.0f);
            Marker marker = API.createMarker(1, PositionJob - new Vector3(0, 0, 1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 100, 255, 255, 255);
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
                    Actions.Add("Quitter le menu");
                    API.triggerClientEvent(players, "bettermenuManager", 102, "Menu livreur", "", false, Actions);
                }

            };
        }

        private void OnClientEventTrigger(Client sender, string eventName, params object[] args)
        {
            if (eventName == "menu_handler_select_item")
            {
                if ((int)args[0] == 102)
                {
                    if ((int)args[1] == 0)
                    {
                        StartMission(sender);
                    }
                }
            }
        }

        public void StartMission(Client player)
        {
            if (player.getData("Livreur") == false || player.getData("Livreur") == null)
            {
                Vector3[] JobMarkers =  { new Vector3(-155.9332, -1348.11, 29.92623),new Vector3(-64.1832, -1450.629, 32.52178), new Vector3(10.88392, -1669.231, 29.25884), new Vector3(-50.26782, -1783.394, 28.30124),
                  new Vector3(5.014607,-1884.653,23.69442), new Vector3(46.17117,-1864.312,23.28129),new Vector3(100.2306,-1913.019,21.16526),new Vector3(130.0086,-1853.98,25.00826),new Vector3(128.1205,-1896.221,23.6725),
                  new Vector3(170.7524,-1871.188,24.40022),new Vector3(374.0941,-1990.498,24.21575),new Vector3(345.6743,-2014.632,22.23506)};
                VehicleHash vehicleJob = API.vehicleNameToModel("Boxville4");
                player.setData("Livreur", true);

                Vehicle vehicle = API.createVehicle(vehicleJob, positionVeh, new Vector3(), 20, 20);
                API.setEntitySyncedData(vehicle, "VEHICLE_FUEL", 100);
                API.setEntitySyncedData(vehicle, "VEHICLE_FUEL_MAX", 100);
                API.setEntityData(vehicle, "weight", 0);
                API.setEntityData(vehicle, "weight_max", 0);
                string plate = Vehicles.Vehicle.RandomPlate().ToString();
                API.setVehicleNumberPlate(vehicle, plate);
                vehicle.setSyncedData("Livreur", true);
                EntityManager.Add(vehicle);

                API.sendNotificationToPlayer(player, "Monter dans un vehicule et rendez vous au differents markers");
                for (int i = 5; i > 0; i--)
                {
                    Random rnd = new Random();
                    int index = rnd.Next(0, JobMarkers.Length - 1);

                    if (i == 5)
                    {
                        API.sendNotificationToPlayer(player, "Vous avez " + i + "colis a livrer, les points de livraison se trouve sur les markers rouge");
                    }
                    else
                    {
                        API.sendNotificationToPlayer(player, "Vous avez encore " + i + "colis a livrer");
                    }
                    MarkerManager(player, JobMarkers[index]);

                }
                player.setData("Mission_finish", true);
                FinMission(player);
            }
            else
            {
                API.sendNotificationToPlayer(player, "Vous avez deja un vehicule de fonction");
            }
        }
        public void FinMission(Client player)
        {
            API.sendNotificationToPlayer(player, "Vous avez terminer vos mission retourner au depot pour recuperer votre paye");
            Vector3 position = new Vector3(135.8563, -1049.832, 29.15181);
            API.shared.triggerClientEvent(player, "markerblip", position - new Vector3(0, 0, 1f));
            ColShape colshape;
            colshape = API.createCylinderColShape(position, 10f, 1f);
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
                        if (player.isInVehicle)
                        {
                            Vehicle vehicle = player.vehicle;
                            if (vehicle.getSyncedData("Livreur"))
                            {
                                if (players.getData("Mission_finish"))
                                {
                                    API.sendNotificationToPlayer(player, "Vous avez terminer vos Livraison");
                                    API.deleteEntity(API.getPlayerVehicle(player));
                                    API.triggerClientEvent(player, "removemarkerblip");
                                    API.deleteColShape(colshape);
                                    Paye(player);
                                }else
                                {
                                    API.sendNotificationToPlayer(player, "Vous n'avez pas fini vos livraison");
                                }
                            }
                            else
                            {
                                API.sendNotificationToPlayer(player, "Vous devez être dans une vehicle de votre metier");
                            }
                        }
                        else
                        {
                            API.sendNotificationToPlayer(player, "Vous devez être dans une vehicle de votre metier");
                        }
                    }
                }

            };
            while (colision == false)
            {
                Thread.Sleep(1000);
            }
            
            //Paye(player);
        }
        public void Paye(Client player)
        {
            player.setData("Livreur", false);
            int money = player.getSyncedData("Money");
            Random rnd = new Random();
            int paye = rnd.Next(750, 1020);
            money += paye;
            player.setSyncedData("Money", money);
            API.sendNotificationToPlayer(player, "Vous venez de gagner  " + paye + " $");
            UpdatePlayerMoney(player);
        }
        public bool MarkerManager(Client player, Vector3 position)
        {
            API.shared.triggerClientEvent(player, "markerblip", position - new Vector3(0, 0, 1f));
            ColShape colshape;
            colshape = API.createCylinderColShape(position, 1f, 1f);
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
                        API.triggerClientEvent(player, "removemarkerblip");
                        colision = true;
                    }
                }

            };
            while (colision == false)
            {
                Thread.Sleep(1000);
            }
            API.deleteColShape(colshape);
            return false;
        }
    }
}
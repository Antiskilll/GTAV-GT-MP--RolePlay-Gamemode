using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using static LSRP_VFR.Mysql.DBPlayers;

namespace LSRP_VFR.Vehicles
{
    class LSCustom : Script
    {
        int[] doors = { API.shared.exported.doormanager.registerDoor((int)-550347177, new Vector3(-362.1167, -133.0148, 38.68042)), API.shared.exported.doormanager.registerDoor((int)270330101, new Vector3(717.8492, -1088.752, 22.35802)), API.shared.exported.doormanager.registerDoor((int)868499217, new Vector3(418.5713, -806.3979, 29.64108)) };
        Vector3 [] Positions = { new Vector3(731.825, -1088.583, 22.16902), new Vector3(-323.4939, -132.7638, 38.96248) }; 
        public LSCustom()
        {
            API.onClientEventTrigger += OnClientEventTrigger;
            foreach(Vector3 pos in Positions)
            {
                RepairPoisition(pos);
            }
            foreach(int door in doors)
            {
                API.exported.doormanager.refreshDoorState(door);
            }
        }



        private void OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if(eventName == "menu_handler_select_item")
            {
                switch ((int)arguments[0])
                {
                    case 300:
                        if ((int)arguments[1] == 0)
                        {
                            repair(sender);
                        }
                        break;

                }
            }
        }

        public void RepairPoisition(Vector3 position)
        {
            ColShape repair = API.createCylinderColShape(position, 3f, 2f);
            var myBlip = API.createBlip(position);
            API.setBlipShortRange(myBlip, true);
            API.setBlipSprite(myBlip, 446);
            API.setBlipColor(myBlip, 37);
            API.setBlipName(myBlip, "Garage");
            API.createMarker(1, position - new Vector3(0f, 0f, 1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 0, 255, 0);
            repair.onEntityEnterColShape += (shape, entity) =>
            {
                var players = API.getPlayerFromHandle(entity);
                if (players == null)
                {
                    return;
                }
                else
                {
                    List<String> Actions = new List<string>();
                    Actions.Add("Reparer");
                    Actions.Add("Annuler");
                    API.triggerClientEvent(players, "bettermenuManager", 300, "Mecanicien", "", false, Actions);
                }

            };
        }
        public void repair(Client player)
        {
            if (player.isInVehicle)
            {
                player.setSyncedData("Money", (int)player.getSyncedData("Money") - 100);
                player.vehicle.health = 1000;
                API.repairVehicle(player.vehicle);
                UpdatePlayerMoney(player);
            }
        }
    }
}

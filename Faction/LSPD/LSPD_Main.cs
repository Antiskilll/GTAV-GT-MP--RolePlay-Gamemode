using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using static LSRP_VFR.Main;
using static LSRP_VFR.Mysql.DBPlayers;

namespace LSRP_VFR.Faction
{
    public class LSPD_Main : Script
    {

        public LSPD_Main()
        {
            API.onResourceStart += API_onResourceStart;
        }

        private void API_onResourceStart()
        {
            var myBlip = API.createBlip(new Vector3(436.0187, -981.6024, 30.69861));
            API.setBlipShortRange(myBlip, true);
            API.setBlipSprite(myBlip, 487);
            API.setBlipColor(myBlip, 63);
            API.setBlipName(myBlip, "LSPD QG");
            API.onClientEventTrigger += OnClientEventTrigger;

            var pnj = API.createPed((PedHash)1581098148, new Vector3(441.1486, -978.8523, 30.68961), 169.0241f, 0);
            API.setEntitySyncedData(pnj, "Interaction", "Cops");

            Vector3 position = new Vector3(1717.663, 2531.471, 45.5649);
            ColShape JailShape = API.createCylinderColShape(position, 59f, 20f);
            JailShape.onEntityExitColShape += (shape, entity) =>
            {
                var players = API.getPlayerFromHandle(entity);
                if (players != null) {
                    if (players.getData("Jailed") == true)
                    {
                        players.position = position;
                    }
                }
            };
        }

        public static void OnOpenCopsMenu(Client sender)
        {
            if (sender.getSyncedData("Police") == true)
            {
                List<string> PoliceCar = new List<string>();
                PoliceCar.Add("Police");
                PoliceCar.Add("Police2");
                PoliceCar.Add("Police3");
                PoliceCar.Add("PoliceT");
                PoliceCar.Add("Sheriff2");
                List<string> Actions = new List<string>();
                Actions.Add("Valider");
                Actions.Add("Annuler");

                List<string>[] _ConteneurListe = new List<string>[1];
                _ConteneurListe[0] = PoliceCar;

                List<string> _NomListe = new List<string>();
                _NomListe.Add("Skin");

                API.shared.triggerClientEvent(sender, "lspdservice", 202, "Selection de vehicule", "", _NomListe, _ConteneurListe, Actions);
            }
        }

        public static Dictionary<Client, long> JailTimes = new Dictionary<Client, long>();

        private void OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "menu_handler_select_item")
            {
                if ((int)arguments[0] == 202)
                {
                    if ((int)arguments[1] == 1)
                    {
                        CarSpawn((string)arguments[2]);
                    }
                }
                if ((int)arguments[0] == 203)
                {
                    if ((string)arguments[2] != null)
                    {
                        //API.consoleOutput((string)arguments[2]);
                        Cuff((string)arguments[2]);
                    }
                }
                if ((int)arguments[0] == 204)
                {
                    if ((string)arguments[2] != null)
                    {
                        //API.consoleOutput((string)arguments[2]);
                        UnCuff((string)arguments[2]);
                    }
                }
                if ((int)arguments[0] == 205)
                {
                    if ((int)arguments[1]==3)
                    {
                        JailPlayer((string)arguments[2], Int32.Parse((string)arguments[3]), Int32.Parse((string)arguments[4]));
                    }
                }
                
                if ((int)arguments[0] == 206)
                {
                    if ((int)arguments[1] == 2 && arguments[2]!=null)
                    {
                        Amende(sender, arguments[2].ToString(), Int32.Parse((string)arguments[3]));
                    }
                }
                if ((int)arguments[0] == 207)
                {
                    if ((int)arguments[1] == 1)
                    {
                        Fouiller(sender, arguments[2].ToString());
                    }
                }
                if ((int)arguments[0] == 209)
                {
                    if ((int)arguments[1] == 1)
                    {
                        Client player = API.getPlayerFromHandle((NetHandle)arguments[2]);
                        if(player != null)
                        {
                            API.sendNotificationToPlayer(player, "La personne a payé l'amende");
                        }
                        sender.setSyncedData("Money", sender.getSyncedData("Money") - Int32.Parse((string)arguments[3]));
                        UpdatePlayerMoney(player);
                    }
                }
            }
            
        }

        public static void Cuff(string target)
        {
            Client player = EntityManager.GetClientName(target);
            if (Players.Player.IsArrested(player)) return;
            API.shared.playPlayerAnimation(player, (int)(AnimationFlags.Loop | AnimationFlags.AllowPlayerControl), "mp_arresting", "idle");
            API.shared.setEntitySyncedData(player, "Arrested", true);
        }

        public static void UnCuff(string target)
        {
            Client player = EntityManager.GetClientName(target);
            if (!Players.Player.IsArrested(player)) return;
            API.shared.stopPlayerAnimation(player);
            API.shared.setEntitySyncedData(player, "Arrested", false);
        }

        public void JailPlayer(string target, int minutes,int amende)
        {

            Client player = EntityManager.GetClientName(target);
            if(player != null)
            {
                Vector3 JailCenter = new Vector3(1707.69f, 2546.69f, 45.56f);
                int seconds = minutes * 60;
                player.setData("Jailed", true);
                API.setEntityData(player, "JailTime", seconds);
                API.removeAllPlayerWeapons(player);
                int money = player.getSyncedData("Money");
                money -= amende;
                player.setSyncedData("Money", money);
                UpdatePlayerMoney(player);
                player.position = JailCenter;
                API.shared.stopPlayerAnimation(player);
                API.sleep(seconds * 1000);
                FreePlayer(player);
            }
        }

        public void FreePlayer(Client player)
        {
            API.resetEntityData(player, "Jailed");
            API.resetEntityData(player, "JailTime");
            lock (JailTimes) JailTimes.Remove(player);
            API.setEntityPosition(player, new Vector3(1850.909, 2585.61, 45.67202));
        }
        public void CarSpawn(string model)
        {
            Vector3 posVeh = new Vector3(442.8555, -1024.834, 28.70241);
            Dictionary<string, VehicleHash> dict = new Dictionary<string, VehicleHash>();
            dict.Add("Police", (VehicleHash)(int)2046537925);
            dict.Add("Police2", (VehicleHash)(int)-1627000575);
            dict.Add("Police3", (VehicleHash)(int)1912215274);
            dict.Add("PoliceT", (VehicleHash)(int)456714581);
            dict.Add("Sheriff2",(VehicleHash)(int)1922257928);

            Vehicle veh = API.createVehicle(dict[model], posVeh, new Vector3(0.0f, 0.0f, -114.44), 111, 0);
            API.setEntitySyncedData(veh, "VEHICLE_FUEL", 100);
            API.setEntitySyncedData(veh, "VEHICLE_FUEL_MAX", 100);
            API.setEntityData(veh, "weight", 0);
            API.setEntityData(veh, "weight_max", 0);
            API.setVehicleNumberPlate(veh, "POLICE");
            API.setEntitySyncedData(veh, "Owner", "Cops");
            API.setEntitySyncedData(veh, "Locked", false);
        }

        public void Amende(Client sender,string target,int amount)
        {
            Client player = EntityManager.GetClientName(target);
            if(player != null)
            { 
                List<string> Action = new List<string>();
                Action.Add("Prix");
                Action.Add("Payer l'amende");
                Action.Add("Refuser l'amende");
                List<string> label = new List<string>();
                label.Add(amount.ToString());
                label.Add("Accepter");
                label.Add("Refuser");
                API.triggerClientEvent(player, "amendeenvoi", 209, "Amende", "", false, Action,label,player);
            }
        }
        public void Fouiller(Client sender,string target)
        {
            Client player = EntityManager.GetClientName(target);
            WeaponHash[] playerWeapons = API.getPlayerWeapons(player);
            List<string> weapons = new List<string>();
            foreach (WeaponHash weapon in playerWeapons)
            {
                weapons.Add(weapon.ToString());
            }
            API.triggerClientEvent(sender, "bettermenuManager", 208, "Fouille", "", false, weapons);
        }

    }
}

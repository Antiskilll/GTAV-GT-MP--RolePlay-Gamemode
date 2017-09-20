using GrandTheftMultiplayer.Server.API;
using System;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Server.Constant;
using System.Collections.Generic;
using static LSRP_VFR.Mysql.DBPlayers;

namespace LSRP_VFR.Players
{
    class Licence : Script
    {
        private Ped pnjlicence;
        private static List<KeyValuePair<int, int>> products = new List<KeyValuePair<int, int>>();

        public Licence()
        {
            API.onResourceStart += OnResourceStart;
            API.onResourceStop += OnResourceStop;
            API.onClientEventTrigger += OnClientEventTrigger;

        }

        private void OnResourceStop()
        {
            API.deleteEntity(pnjlicence);
        }

        private void OnResourceStart()
        {
            Vector3 pnjpos = new Vector3(-546.3825, -204.3235, 38.21516);
            pnjlicence = API.createPed((PedHash)(-1768198658), pnjpos, 0);
            API.setEntityRotation(pnjlicence, new Vector3(0, 0, -154.5915));
            API.setEntitySyncedData(pnjlicence, "Interaction", "Prefecture");
            API.playPedScenario(pnjlicence, "WORLD_HUMAN_SMOKING");
            Blip blip = API.createBlip(pnjpos);
            API.setBlipName(blip, "Préfecture");
            blip.shortRange = true;
            blip.sprite = 267;
        }

        public static void OpenMenuLicence(Client sender)
        {
            List<string> Actions = new List<string>();
            List<string> label = new List<string>();

            Actions.Add("Permis Automobile");
            label.Add("Prix: 2000$");
            Actions.Add("Permis Moto");
            label.Add("Prix: 5000$");
            Actions.Add("Permis Poids Lourd ");
            label.Add("Prix: 10000$");
            API.shared.triggerClientEvent(sender, "bettermenuManager", 40, "Licence", "Sélectionner une licence:", false, Actions, label);
        }

        private void OnClientEventTrigger(Client sender, string eventName, object[] arguments)
        {
            if (eventName == "menu_handler_select_item")
            {
                if ((int)arguments[0] == 40)
                {
                    if ((int)arguments[1] == 0)
                    {
                        if (API.shared.getEntityData(sender, "P_Voiture") == true) {
                            API.sendNotificationToPlayer(sender, "Vous avez déjà votre ~r~permis de voiture.");
                            return; };
                        if (Players.Money.TakeMoney(sender, 2000))
                        {
                            API.sendNotificationToPlayer(sender, "Vous avez obtenu votre ~r~permis de voiture.");
                            AddLicence(sender, "Voiture");
                        } else
                        {
                            API.sendNotificationToPlayer(sender, "Vous n'avez pas assez d'argent sur vous.");
                        }      
                    }
                    if ((int)arguments[1] == 1)
                    {
                        if (API.shared.getEntityData(sender, "P_Moto") == true)
                        {
                            API.sendNotificationToPlayer(sender, "Vous avez déjà votre ~r~permis de moto.");
                            return;
                        };
                        if (Players.Money.TakeMoney(sender, 5000))
                        {
                            API.sendNotificationToPlayer(sender, "Vous avez obtenu votre ~r~permis de moto.");
                            AddLicence(sender, "Moto");
                        }
                        else
                        {
                            API.sendNotificationToPlayer(sender, "Vous n'avez pas assez d'argent sur vous.");
                        }
                    }
                    if ((int)arguments[1] == 2)
                    {
                        if (API.shared.getEntityData(sender, "P_Camion") == true)
                        {
                            API.sendNotificationToPlayer(sender, "Vous avez déjà votre ~r~permis de poids lourd.");
                            return;
                        };
                        if (Players.Money.TakeMoney(sender, 10000))
                        {
                            API.sendNotificationToPlayer(sender, "Vous avez obtenu votre ~r~permis de poids lourd.");
                            AddLicence(sender, "Camion");
                        }
                        else
                        {
                            API.sendNotificationToPlayer(sender, "Vous n'avez pas assez d'argent sur vous.");
                        }
                    }
                }
            }
        }

        public static void AddLicence(Client player, string type)
        {
            switch (type)
            {
                case "Voiture":
                    API.shared.setEntityData(player, "P_Voiture", true);
                    break;
                case "Moto":
                    API.shared.setEntityData(player, "P_Moto", true);
                    break;
                case "Camion":
                    API.shared.setEntityData(player, "P_Camion", true);
                    break;
                default:
                    API.shared.sendNotificationToPlayer(player, "ERREUR!");
                    break;
            }
            UpdatePlayerLicences(player);
        }

        public static void RemoveLicence(Client player, string type)
        {
            switch (type)
            {
                case "Voiture":
                    API.shared.setEntityData(player, "P_Voiture", false);
                    break;
                case "Moto":
                    API.shared.setEntityData(player, "P_Moto", false);
                    break;
                case "Camion":
                    API.shared.setEntityData(player, "P_Camion", false);
                    break;
                default:
                    API.shared.sendNotificationToPlayer(player, "ERREUR!");
                    break;
            }
            UpdatePlayerLicences(player);
        }

    }
}

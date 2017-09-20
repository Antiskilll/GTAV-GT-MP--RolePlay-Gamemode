using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System;
using System.Collections.Generic;
using LSRP_VFR.Jobs;
using GrandTheftMultiplayer.Server.Constant;

namespace LSRP_VFR.Menu
{
    public class MenuTelephone : Script
    {
        public MenuTelephone()
        {
            API.onClientEventTrigger += OnClientEventTrigger; ;
        }

        public static void OpenMenuTelephone (Client sender)
        {
            List<String> Actions = new List<string>();
            Actions.Add("Appeler un Taxi");
            Actions.Add("Contacter le 911");
            Actions.Add("Contacter les Urgences");
            Actions.Add("Envoyer un SMS");
            API.shared.triggerClientEvent(sender, "bettermenuManager", 101, "Telephone", "Repertoire", false, Actions);
        }

        private void OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "menu_handler_select_item")
            {
                // APPELER UN TAXI
                if ((int)arguments[0] == 101 && (int)arguments[1] == 0)
                {
                    Taxi.CallTaxi(sender);
                }
                // CALL 911
                if ((int)arguments[0] == 101 && (int)arguments[1] == 1)
                {
                    API.shared.triggerClientEvent(sender, "get_user_input", 107, "", 144, null);
                }
                // URGENCES
                if ((int)arguments[0] == 101 && (int)arguments[1] == 2)
                {
                    API.shared.triggerClientEvent(sender, "get_user_input", 108, "", 144, null);
                }
                // SMS
                if ((int)arguments[0] == 101 && (int)arguments[1] == 3)
                {
                    var list = API.getAllPlayers();
                    List<String> Actions = new List<string>();
                    list.Remove(sender);
                    API.setEntityData(sender, "list", list);
                    foreach (Client player in list)
                    {
                        Actions.Add(API.getEntitySyncedData(player, "Nom_Prenom"));
                    }
                    API.triggerClientEvent(sender, "bettermenuManager", 106, API.getEntitySyncedData(sender, "Nom_Prenom"), "Envoyé un SMS au ~g~joueur:", false, Actions);
                }
                if ((int)arguments[0] == 106)
                {

                    API.triggerClientEvent(sender, "get_user_input", 109, "", 64, (int)arguments[1]);
                }
            }
            else if (eventName == "menu_handler_user_input")
            {
                if ((int)arguments[0] == 109)
                {
                    List<Client> list = API.getEntityData(sender, "list");
                    int index = (int)arguments[2];
                    Client recever = list[index];
                    String senderName = API.getEntitySyncedData(sender, "Nom_Prenom");
                    String message = (string)arguments[1];

                    API.sendNotificationToPlayer(recever, "~r~SMS reçu: ~s~'" + message + "' de: " + senderName);
                    API.playSoundFrontEnd(recever, "Menu_Accept", "Phone_SoundSet_Default");
                    API.resetEntityData(sender, "list");
                }
                // CALL 911
                else if ((int)arguments[0] == 107)
                {
                    var players = API.getAllPlayers();
                    foreach (var player in players)
                    {
                        if (API.getEntitySyncedData(player, "Police") == true)
                        {
                            API.triggerClientEvent(player, "display_subtitle", "~r~~h~[911 CALL] de " + sender.name + "~h~~y~ : " + (string)arguments[1] + "\n ~w~ Position transmise!", 13000);
                            float posX = sender.position.X;
                            float posY = sender.position.Y;
                            API.sendNativeToPlayer(player, Hash.SET_NEW_WAYPOINT, posX, posY);
                        }
                    }
                }
                // CALL URGENCE
                else if ((int)arguments[0] == 108)
                {
                    Faction.EMS.Hospital.CallMedic(sender, (string)arguments[1]);
                }
            }
        }
    } 
}

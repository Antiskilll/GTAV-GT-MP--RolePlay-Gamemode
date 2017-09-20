using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using static LSRP_VFR.Mysql.DBPlayers;
namespace LSRP_VFR.Menu
{
    class MainInventory : Script
    {

        public List<KeyValuePair<string, string>> AnimationList = new List<KeyValuePair<string, string>>()
        {
            {new KeyValuePair<string,string>("Doigt d'honneur", "mp_player_intfinger mp_player_int_finger")},
            {new KeyValuePair<string,string>("Jouer de la guitare", "anim@mp_player_intcelebrationmale@air_guitar air_guitar")},
            {new KeyValuePair<string,string>("Provocation baise", "anim@mp_player_intcelebrationmale@air_shagging air_shagging")},
            {new KeyValuePair<string,string>("Jouer du synthé", "anim@mp_player_intcelebrationmale@air_synth air_synth")},
            {new KeyValuePair<string,string>("Faire un bisou", "anim@mp_player_intcelebrationmale@blow_kiss blow_kiss")},
            {new KeyValuePair<string,string>("Je t'aime frère", "anim@mp_player_intcelebrationmale@bro_love bro_love")},
            {new KeyValuePair<string,string>("Faire la poule", "anim@mp_player_intcelebrationmale@chicken_taunt chicken_taunt")},
            {new KeyValuePair<string,string>("Se grater le menton", "anim@mp_player_intcelebrationmale@chin_brush chin_brush")},
            {new KeyValuePair<string,string>("Faire le DJ", "anim@mp_player_intcelebrationmale@dj dj")},
            {new KeyValuePair<string,string>("dock", "anim@mp_player_intcelebrationmale@dock dock")},
            {new KeyValuePair<string,string>("Face to Face", "anim@mp_player_intcelebrationmale@face_palm face_palm")},
            {new KeyValuePair<string,string>("Embrasser son doigt", "anim@mp_player_intcelebrationmale@finger_kiss finger_kiss")},
            {new KeyValuePair<string,string>("Flipper", "anim@mp_player_intcelebrationmale@freakout freakout")},
            {new KeyValuePair<string,string>("Joueur du jazz", "anim@mp_player_intcelebrationmale@jazz_hands jazz_hands")},
            {new KeyValuePair<string,string>("Avoir les mains liés", "anim@mp_player_intcelebrationmale@knuckle_crunch knuckle_crunch")},
            {new KeyValuePair<string,string>("Mettre son doit dans le nez", "anim@mp_player_intcelebrationmale@nose_pick nose_pick")},
            {new KeyValuePair<string,string>("Faire signe que non", "anim@mp_player_intcelebrationmale@no_way no_way")},
            {new KeyValuePair<string,string>("Peace mon frère", "anim@mp_player_intcelebrationmale@peace peace")},
            {new KeyValuePair<string,string>("Faire une photo", "anim@mp_player_intcelebrationmale@photography photography")},
            {new KeyValuePair<string,string>("Jouer du rock", "anim@mp_player_intcelebrationmale@rock rock")},
            {new KeyValuePair<string,string>("Faire Salut", "anim@mp_player_intcelebrationmale@salute salute")},
            {new KeyValuePair<string,string>("Chuuuut !", "anim@mp_player_intcelebrationmale@shush shush")},
            {new KeyValuePair<string,string>("Applaudir lentement", "anim@mp_player_intcelebrationmale@slow_clap slow_clap")},
            {new KeyValuePair<string,string>("Se rendre", "anim@mp_player_intcelebrationmale@surrender surrender")},
            {new KeyValuePair<string,string>("Lever le pouce", "anim@mp_player_intcelebrationmale@thumbs_up thumbs_up")},
            {new KeyValuePair<string,string>("Provoquer", "anim@mp_player_intcelebrationmale@thumb_on_ears thumb_on_ears")},
            {new KeyValuePair<string,string>("Signe de la victoire", "anim@mp_player_intcelebrationmale@v_sign v_sign")},
            {new KeyValuePair<string,string>("Signe de branlette", "anim@mp_player_intcelebrationmale@wank wank")},
            {new KeyValuePair<string,string>("Faire la vague", "anim@mp_player_intcelebrationmale@wave wave")},
            {new KeyValuePair<string,string>("Tu es dingue !", "anim@mp_player_intcelebrationmale@you_loco you_loco")},
            {new KeyValuePair<string,string>("Main", "missminuteman_1ig_2 handsup_base")},
        };

        public MainInventory()
        {
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        public static void OpenInventory(Client sender)
        {
            List<String> Actions = new List<string>();
            if (!API.shared.getEntityData(sender, "Jailed"))
            {
                Actions.Add("Inventaire");
                Actions.Add("Donner de l'argent");
                Actions.Add("Animations");
                Actions.Add("Donner les papiers");
            }
            if (sender.getSyncedData("Police") == true)
            {
                Actions.Add("Menotter");
                Actions.Add("Demenotter");
                Actions.Add("Emprisoner");
                Actions.Add("Amende");
                Actions.Add("Fouiller");
            }
            API.shared.triggerClientEvent(sender, "bettermenuManager", 121, API.shared.getEntitySyncedData(sender, "Nom_Prenom"), "Menu Joueur: ", false, Actions);
        }

        private void OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "menu_handler_select_item")
            {
                if ((int)arguments[0] == 128)
                {
                    Client player = EntityManager.GetClientName((string)arguments[2]);
                    string _txt = String.Format("Nom: {0}\nPrenom: {1}\nAge: {2}",
                         sender.getSyncedData("Nom"),
                         sender.getSyncedData("Prenom"),
                         sender.getSyncedData("Age")
                         );
                    API.sendNotificationToPlayer(player, _txt);
                }
                // INVENTAIRE
                if ((int)arguments[0] == 121 && (int)arguments[1] == 0)
                {
                    if (API.hasEntityData(sender, "InventoryHolder"))
                    {
                        List<String> Actions = new List<string>();
                        Items.Items.InventoryHolder ih = API.getEntityData(sender, "InventoryHolder");
                        foreach (Items.Items.InventoryItem item in ih.Inventory)
                        {
                            Actions.Add(item.Details.Name + " :  " + "Quantité: " + item.Quantity);
                        }
                        API.triggerClientEvent(sender, "bettermenuManager", 132, API.getEntitySyncedData(sender, "Nom_Prenom"), "Selectionner l'item :               Poids : " + API.getEntityData(sender.handle, "weight") + " / " + API.getEntityData(sender.handle, "weight_max"), false, Actions);
                    }
                }

                // DONNER DE L'ARGENT
                if ((int)arguments[0] == 121 && (int)arguments[1] == 1)
                {
                    var nearbylist = API.getPlayersInRadiusOfPlayer(10, sender);
                    API.setEntityData(sender, "NearbyList", nearbylist);
                    List<String> Actions = new List<string>();
                    nearbylist.Remove(sender);
                    foreach (Client player in nearbylist)
                    {
                        Actions.Add(API.getEntitySyncedData(player, "Nom_Prenom"));   
                    }
                    API.triggerClientEvent(sender, "bettermenuManager", 123, API.getEntitySyncedData(sender, "Nom_Prenom"), "Donner de l'argent au ~g~joueur:", false, Actions);
                }
                if ((int)arguments[0] == 123)
                {
                    API.shared.triggerClientEvent(sender, "get_user_input", 125, "", 144, (int)arguments[1]);
                }

                // ANIMATION
                if ((int)arguments[0] == 121 && (int)arguments[1] == 2)
                {
                    if (sender.vehicle == null) { 
                        var nearbylist = API.getPlayersInRadiusOfPlayer(10, sender);
                        List<String> Actions = new List<string>();
                        foreach (KeyValuePair<string, string> anim in AnimationList)
                        {
                            Actions.Add(anim.Key.ToString());
                        }
                        API.triggerClientEvent(sender, "bettermenuManager", 124, API.getEntitySyncedData(sender, "Nom_Prenom"), "Selectionner l'animation:", false, Actions);
                    }
                }
                if ((int)arguments[0] == 124)
                {
                    API.playPlayerAnimation(sender, 0, AnimationList[(int)arguments[1]].Value.Split()[0], AnimationList[(int)arguments[1]].Value.Split()[1]);
                }

                // Donner les papiers
                if ((int)arguments[0] == 121 && (int)arguments[1] == 3)
                {
                    var nearbylist = API.getPlayersInRadiusOfPlayer(10, sender);
                    List<string> Actions = new List<string>();
                    foreach (Client player in nearbylist)
                    {
                        if (!(player.name == sender.name))
                        {
                            Actions.Add(API.getEntitySyncedData(player, "Nom_Prenom"));
                        }
                    }
                    API.triggerClientEvent(sender, "bettermenuManager", 128, API.getEntitySyncedData(sender, "Nom_Prenom"), "Donner les papiers à:", false, Actions);
                }
                //Menoter
                if ((int)arguments[0] == 121 && (int)arguments[1] == 4)
                {
                    var nearbylist = API.getPlayersInRadiusOfPlayer(10, sender);
                    List<string> Actions = new List<string>();
                    foreach (Client player in nearbylist)
                    {
                        if (!(player.name == sender.name))
                        {
                            Actions.Add(API.getEntitySyncedData(player, "Nom_Prenom"));
                        }
                    }
                    API.triggerClientEvent(sender, "bettermenuManager", 203, API.getEntitySyncedData(sender, "Nom_Prenom"), "Menotter", false, Actions);
                }

                //DeMenoter
                if ((int)arguments[0] == 121 && (int)arguments[1] == 5)
                {
                    var nearbylist = API.getPlayersInRadiusOfPlayer(10, sender);
                    List<string> Actions = new List<string>();
                    foreach (Client player in nearbylist)
                    {
                        if (!(player.name == sender.name))
                        {
                            Actions.Add(API.getEntitySyncedData(player, "Nom_Prenom"));
                        }
                    }
                    API.triggerClientEvent(sender, "bettermenuManager", 204, API.getEntitySyncedData(sender, "Nom_Prenom"), "Demenotter", false, Actions);
                }

                //Jail
                if ((int)arguments[0] == 121 && (int)arguments[1] == 6)
                {
                    var nearbylist = API.getPlayersInRadiusOfPlayer(10, sender);
                    List<string> players = new List<string>();
                    foreach (Client player in nearbylist)
                    {
                        if (!(player.name == sender.name))
                        {
                            players.Add(API.getEntitySyncedData(player, "Nom_Prenom"));
                        }
                    }
                    List<string> _Duree = new List<string>();
                    for(int i = 0; i<50; i++)
                    {
                        _Duree.Add(i.ToString());
                    }
                    List<string> _Money = new List<string>();
                    for(int i = 0; i < 100000; i+=1000)
                    {
                        _Money.Add(i.ToString());
                    }
                    

                    List<string> Actions = new List<string>();
                    Actions.Add("Envoyer en prison");
                    Actions.Add("Annuler");

                    List<string>[] _ConteneurListe = new List<string>[3];
                    _ConteneurListe[0] = players;
                    _ConteneurListe[1] = _Duree;
                    _ConteneurListe[2] = _Money;

                    List<string> _NomListe = new List<string>();
                    _NomListe.Add("Joueur");
                    _NomListe.Add("Duree (minutes)");
                    _NomListe.Add("Prix");

                    API.triggerClientEvent(sender, "jailmenu", 205, "Menu prison", "", _NomListe, _ConteneurListe, Actions);
                }
                if ((int)arguments[0] == 121 && (int)arguments[1] == 7)
                {
                    var nearbylist = API.getPlayersInRadiusOfPlayer(10, sender);
                    List<string> players = new List<string>();
                    foreach (Client player in nearbylist)
                    {
                        if (!(player.name == sender.name))
                        {
                            players.Add(API.getEntitySyncedData(player, "Nom_Prenom"));
                        }
                    }
                    List<string> _Money = new List<string>();
                    for (int i = 0; i < 10000; i += 500)
                    {
                        _Money.Add(i.ToString());
                    }

                    List<string> Actions = new List<string>();
                    Actions.Add("Donner une amende");
                    Actions.Add("Annuler");

                    List<string>[] _ConteneurListe = new List<string>[2];
                    _ConteneurListe[0] = players;
                    _ConteneurListe[1] = _Money;

                    List<string> _NomListe = new List<string>();
                    _NomListe.Add("Joueur");
                    _NomListe.Add("Montant");

                    API.triggerClientEvent(sender, "amende", 206, "Menu Amende", "", _NomListe, _ConteneurListe, Actions);
                }
                if((int)arguments[0] == 121 && (int)arguments[1] == 8)
                {
                    var nearbylist = API.getPlayersInRadiusOfPlayer(10, sender);
                    List<string> players = new List<string>();
                    foreach (Client player in nearbylist)
                    {
                        if (!(player.name == sender.name))
                        {
                            players.Add(API.getEntitySyncedData(player, "Nom_Prenom"));
                        }
                    }

                    List<string> Actions = new List<string>();
                    Actions.Add("Fouiller");
                    Actions.Add("Annuler");

                    List<string>[] _ConteneurListe = new List<string>[1];
                    _ConteneurListe[0] = players;

                    List<string> _NomListe = new List<string>();
                    _NomListe.Add("Joueur");

                    API.triggerClientEvent(sender, "fouiller", 207, "Fouille", "", _NomListe, _ConteneurListe, Actions);
                }
            }
            
            // DONNER L'ARGENT
            if (eventName == "menu_handler_user_input")
            {
                if ((int)arguments[0] == 125)
                {
                    int currentmoney = API.getEntitySyncedData(sender, "Money");
                    
                    var nearbylist = API.getEntityData(sender, "NearbyList");
                    var reciever = nearbylist[(int)arguments[2]];
                    int amount = Convert.ToInt32(arguments[1]);
                    API.resetEntityData(sender, "NearbyList");
                    if (currentmoney >= amount)
                    {
                        API.sendNotificationToPlayer(reciever, "Vous avez reçu de la part de ~r~" + API.getEntitySyncedData(sender, "Nom_Prenom") + " ~s~la somme de ~r~" + amount + "$");
                        Players.Money.GiveMoneyToOther(sender, reciever, amount);
                        UpdatePlayerMoney(sender);
                        UpdatePlayerMoney(reciever);
                    } else {
                        API.sendNotificationToPlayer(sender, "Vous n'avez pas assez d'argent sur vous!");
                    }
                }
            }
        }
    }
}

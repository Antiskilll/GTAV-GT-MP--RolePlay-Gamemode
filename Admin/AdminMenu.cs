using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System;
using System.Collections.Generic;
using static LSRP_VFR.Mysql.DBPlayers;

namespace LSRP_VFR.Admin
{
    public class AdminMenu : Script
    {
        public AdminMenu()
        {
            API.onClientEventTrigger += OnClientEventTrigger;

        }

        private void OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "tptoplayer")
            {
                TpToPlayer(sender, (string)arguments[0]);
            }
            else if (eventName == "tpplayer")
            {
                TpPlayer(sender, (string)arguments[0]);
            }
            else if (eventName == "kickplayer")
            {
                KickPlayer(sender, (string)arguments[0]);
            }
            else if (eventName == "givemoney")
            {
                GiveMoney((string)arguments[0], Int32.Parse((string)arguments[1]));
            }
            else if (eventName == "setrank")
            {
                SetAdminRank((string)arguments[0], Int32.Parse((string)arguments[1]));
            }
            else if (eventName == "invisible")
            {
                SetInvisible(sender);
            }
            else if (eventName == "invincible")
            {
                SetInvincible(sender);
            }
        }

        public static void AdminCommand(Client player)
        {
            if (AdminRankCheck(player) > 0)
            {
                int rank = AdminRankCheck(player);



                //Liste des joueurs en ligne Generer via EntityManager et OnPlayerConnect
                List<String> _Joueur = new List<string>();
                foreach (var client in EntityManager.GetClientList())
                {
                    _Joueur.Add(client.socialClubName);
                }
                //Liste des montant pour Give
                List<string> _Money = new List<string>();
                int money = 0;
                for (int i = 0; i < 10; i++)
                {
                    money += 10000;
                    _Money.Add(money.ToString());
                }
                //Liste des rangs Admin
                List<string> _Rank = new List<string>();
                _Rank.Add("0");
                _Rank.Add("1");
                _Rank.Add("2");
                _Rank.Add("3");
                //Nom des liste A envoyer a La merde de javascript
                List<String> _NomListe = new List<String>();
                _NomListe.Add("Joueur");
                _NomListe.Add("Give");
                _NomListe.Add("AdminRank");
                //Array de liste qui contient les differente liste du menu
                List<String>[] _ConteneurListe = new List<String>[3];
                _ConteneurListe[0] = _Joueur;
                _ConteneurListe[1] = _Money;
                _ConteneurListe[2] = _Rank;
                //Autres Item du menu
                List<String> _MenuItem = new List<string>();
                _MenuItem.Add("TP vers Joueur");
                _MenuItem.Add("TP le joueur");
                _MenuItem.Add("Kick");
                //CheckBoxes
                List<String> _Checkbox = new List<string>();
                _Checkbox.Add("GodMode");
                _Checkbox.Add("Invisible");

                API.shared.triggerClientEvent(player, "create_menu", "Menu Admin", "Menu Admin", _NomListe, _ConteneurListe, _MenuItem, _Checkbox);
            }
        }
        public static void TpToPlayer(Client sender, string socialClub)
        {
            Client target = EntityManager.GetClient(socialClub);
            sender.position = target.position;
        }
        public static void TpPlayer(Client sender, string socialClub)
        {
            Client target = EntityManager.GetClient(socialClub);
            target.position = sender.position;
        }
        public static void KickPlayer(Client sender, string socialClub)
        {
            Client target = EntityManager.GetClient(socialClub);
            API.shared.kickPlayer(target, "");
        }
        public void GiveMoney(string socialClub, int amount)
        {
            Client target = EntityManager.GetClient(socialClub);
            int money = target.getSyncedData("Money");
            money += amount;
            target.setSyncedData("Money", money);
            UpdatePlayerMoney(target);
        }

        public static int AdminRankCheck(Client sender)
        {
            int rank = sender.getSyncedData("adminRank");
            return rank;
        }

        public  void SetAdminRank(string socialClub, int rank)
        {
            var client = EntityManager.GetClient(socialClub);
            Mysql.DBPlayers.SetAdminRank(client, rank);
            client.setSyncedData("adminRank", rank);
        }
        public static void SetInvisible(Client player)
        {
            if (player.getData("invisible") == null || player.getData("invisible") == false)
            {
                API.shared.setEntityTransparency(player.handle, 0);
                player.setData("invisible", true);
            }
            else
            {
                API.shared.setEntityTransparency(player.handle, 255);
                player.setData("invisible", false);
            }
        }
        public static void SetInvincible(Client player)
        {
            if (player.getData("Invincible") == null || player.getData("Invicible") == false)
            {
                API.shared.setEntityInvincible(player.handle, true);
                player.setData("Invicible", true);
            }
            else
            {
                API.shared.setEntityInvincible(player.handle, false);
                player.setData("Invicible", false);
            }
        }
        public void getPosition(Client player)
        {
            API.consoleOutput("{0},{1},{2}", player.position.X, player.position.Y, player.position.Z);
        }
        public void vetmentCmd(Client player, int slot, int drawable, int texture)
        {
            API.setPlayerClothes(player, slot, drawable, texture);
        }
    }
    
}

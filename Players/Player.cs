using System;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using System.Data;
using static LSRP_VFR.Items.Items;
using GrandTheftMultiplayer.Server.Constant;
using static LSRP_VFR.Main;
using static LSRP_VFR.Mysql.DBPlayers;
namespace LSRP_VFR.Players
{
    public class Player : Script
    {
        private string nomprenom;

        public Player()
        {
            API.onPlayerConnected += OnPlayerConnected;
            API.onPlayerDisconnected += OnPlayerDisconnected;
            API.onPlayerFinishedDownload += OnPlayerFinishedDownloadHandler;
            API.onResourceStop += OnResourceStop;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        public void OnPlayerConnected(Client player)
        {
            if (player.socialClubName == null) { API.kickPlayer(player, "Social Club return null"); };
            player.freeze(true);
            //API.sendNativeToPlayer(player, Hash.DO_SCREEN_FADE_OUT, 0);
            API.sendNotificationToAll("~r~[SERVEUR] ~s~le joueur " + player.name + " vient de se connecter.");
        }

        private void OnPlayerFinishedDownloadHandler(Client player)
        {
            StartInitPlayer(player);
            EntityManager.Add(player);
        }

        public static bool IsPlayerLoggedIn(Client player)
        {
            if (API.shared.hasEntitySyncedData(player, "LOGGED_IN"))
            {
                return API.shared.getEntitySyncedData(player, "LOGGED_IN");
            }
            return false;
        }

        public void OnClientEventTrigger(Client sender, string eventName, params object[] args)
        {
            if (eventName == "AnimationSystem")
            {
                API.playPlayerAnimation(sender, (int)(AnimationFlags.OnlyAnimateUpperBody | AnimationFlags.AllowPlayerControl), args[0].ToString(), args[1].ToString());
            }
        }

        public void StartInitPlayer(Client player)
        {
            try
            {
                if (!ExistPlayerInfo(player))
                {
                    CharacterCreator.StartCreator(player);
                }
                else
                {
                    PlayerConnection(player);
                }
            }
            catch (Exception e)
            {
                API.consoleOutput("~r~[ERROR][PLAYER] : ~s~" + e.ToString());
            }
        }

        public void OnPlayerDisconnected(Client player, string reason)
        {
            UpdatePlayerInfo(player);
            API.sendNotificationToAll("~r~[SERVEUR] ~s~le joueur " + player.name + " vient de se déconnecter. " + reason);
            EntityManager.Remove(player);
            API.deleteEntity(player);
        }

        private void OnResourceStop()
        {
            var allPlayers = API.getAllPlayers();
            foreach (Client player in allPlayers)
            {
                UpdatePlayerInfo(player);
                API.kickPlayer(player);
            }
        }

        public static Client GetClientPlayerByName(String name)
        {
            var allPlayers = API.shared.getAllPlayers();
            foreach (Client player in allPlayers)
            {
                if (player.socialClubName == name)
                {
                    return player;
                }
            }
            return null;
        }

        private void PlayerConnection(Client player)
        {
            DataTable result = GetPlayerInfo(player);
            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    API.setEntitySyncedData(player.handle, "Money", Convert.ToInt32(row["money"]));
                    API.setEntitySyncedData(player.handle, "adminRank", Convert.ToInt32(row["adminRank"]));
                    API.setEntitySyncedData(player.handle, "LSPDrank", Convert.ToInt32(row["LSPDrank"]));
                    API.setEntitySyncedData(player.handle, "EMSrank", Convert.ToInt32(row["EMSrank"]));
                    API.setEntitySyncedData(player.handle, "BankMoney", Convert.ToInt32(row["bank"]));
                    API.setEntityData(player.handle, "Jailed", Convert.ToBoolean(row["jailed"]));
                    InventoryHolder ih = new InventoryHolder();
                    ih.Owner = player.handle;
                    API.setEntityData(player, "InventoryHolder", ih);
                    API.setEntityData(player.handle, "weight", 0);
                    API.setEntityData(player.handle, "weight_max", 25);
                    API.setEntitySyncedData(player.handle, "IS_DEATH", false);

                    if (!((row["inventory"]).Equals("[]")))
                    {
                        var inventairerow = Convert.ToString(row["inventory"]);
                        var inventaire = inventairerow.Split(new[] { "],[" }, StringSplitOptions.None);
                        foreach (var I in inventaire)
                        {
                            var I2 = I.ToString().Replace("[", "").Replace("]", "");
                            var I3 = I2.Split(new[] { "," }, StringSplitOptions.None);
                            Item item = ItemByID(Convert.ToInt16(I3[0]));
                            ih.AddItemToInventory(item, Convert.ToInt16(I3[1]));
                        }
                    }
                    if (!(row["characters"]).Equals("[]"))
                    {
                        var characters = Convert.ToString(row["characters"]);
                        var characters1 = characters.Split(new[] { "],[" }, StringSplitOptions.None);

                        var SexeChar = characters1[0].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");
                        var VisageMere = characters1[1].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");
                        var VisagePere = characters1[2].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");
                        var CheveuxChar = characters1[3].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");
                        var ColCheveux = characters1[4].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");
                        var ColYeux = characters1[5].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");
                        var VisageColor = characters1[6].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");
                        var EyeBrown = characters1[7].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");
                        var EyeBrownColor = characters1[8].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");

                        var Nom = characters1[9].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");
                        var Prenom = characters1[10].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");
                        var Age = characters1[11].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");

                        string Nom1 = Convert.ToString(Nom);
                        string Prenom1 = Convert.ToString(Prenom);
                        string Age1 = Convert.ToString(Age);
                        nomprenom = "" + Nom1 + " " + Prenom1;

                        API.setEntitySyncedData(player, "VisageMere1", Convert.ToInt32(VisageMere));
                        API.setEntitySyncedData(player, "VisagePere1", Convert.ToInt32(VisagePere));
                        API.setEntitySyncedData(player, "CheveuxChar1", Convert.ToInt32(CheveuxChar));
                        API.setEntitySyncedData(player, "ColCheveux1", Convert.ToInt32(ColCheveux));
                        API.setEntitySyncedData(player, "ColYeux1", Convert.ToInt32(ColYeux));
                        API.setEntitySyncedData(player, "VisageColor1", Convert.ToInt32(VisageColor));
                        API.setEntitySyncedData(player, "EyeBrown1", Convert.ToInt32(CheveuxChar));
                        API.setEntitySyncedData(player, "EyeBrownColor1", Convert.ToInt32(ColCheveux));

                        if (Convert.ToInt32(SexeChar) == 1885233650) // FreeModeMale01
                        {
                            player.setSyncedData("Sexe", "Homme");
                        }
                        else if (Convert.ToInt32(SexeChar) == -1667301416) // FreemodeFemale01
                        {
                            player.setSyncedData("Sexe", "Femme");
                        }

                        // Skin
                        API.setPlayerSkin(player, (PedHash)Convert.ToInt32(SexeChar));
                        API.triggerClientEvent(player, "setplayerskin", player.handle);


                        API.setEntitySyncedData(player.handle, "Nom", Nom1);
                        API.setEntitySyncedData(player.handle, "Prenom", Prenom1);
                        API.setEntitySyncedData(player.handle, "Nom_Prenom", Nom1 + " " + Prenom1);
                        API.setEntitySyncedData(player.handle, "Age", Age1);

                        var vetements1 = Convert.ToString(row["clothing"]).Split(new[] { "],[" }, StringSplitOptions.None);
                        player.setData("Pants", Convert.ToInt32(vetements1[0].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "")));
                        player.setData("Chemise", Convert.ToInt32(vetements1[1].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "")));
                        player.setData("Survet", Convert.ToInt32(vetements1[2].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "")));
                        player.setData("Chaussures", Convert.ToInt32(vetements1[3].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "")));
                        player.setData("Bras", Convert.ToInt32(vetements1[4].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "")));
                        API.setPlayerClothes(player, 4, player.getData("Pants"), 0);
                        API.setPlayerClothes(player, 8, player.getData("Chemise"), 0);
                        API.setPlayerClothes(player, 11, player.getData("Survet"), 0);
                        API.setPlayerClothes(player, 6, player.getData("Chaussures"), 0);
                        API.setPlayerClothes(player, 3, player.getData("Bras"), 0);
                        API.setEntityData(player.handle, "Pants", 0);
                    }

                    if (!(row["position"]).Equals("[]"))
                    {
                        var position1 = Convert.ToString(row["position"]).Split(new[] { "],[" }, StringSplitOptions.None);

                        float posX1 = Convert.ToSingle(position1[0].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", ""));
                        float posY1 = Convert.ToSingle(position1[1].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", ""));
                        float posZ1 = Convert.ToSingle(position1[2].ToString().Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", ""));
                        API.setEntityPosition(player, new Vector3(posX1, posY1, posZ1 + 1f));
                    }
                    else
                    {
                        API.setEntityPosition(player.handle, new Vector3(-1033.028, -2732.261, 14));
                        API.setEntityRotation(player.handle, new Vector3(0, 0, -37.83434));
                    }

                    API.setEntitySyncedData(player, "PLAYER_HUNGRY", Convert.ToInt32(row["hunger"]));
                    API.setEntitySyncedData(player, "PLAYER_THIRSTY", Convert.ToInt32(row["thirst"]));
                    API.setPlayerHealth(player, Convert.ToInt32(row["health"]));
                    API.setEntitySyncedData(player, "VOICE_RANGE", "Parler");
                    API.setPlayerNametagVisible(player, false);
                    API.setEntitySyncedData(player.handle, "Jobs", false);
                    API.setEntitySyncedData(player.handle, "InProgress", false);
                    API.setEntityData(player.handle, "weight_max", 25);
                    API.setEntityDimension(player, 0);
                    API.setEntitySyncedData(player, "Police", false);
                    API.setEntitySyncedData(player, "IsMedic", false);
                    API.setEntitySyncedData(player, "Arrested", false);
                    LoadLicence(player);
                    API.setEntitySyncedData(player, "IsOnComa", false);
                    API.triggerClientEvent(player, "Teamspeak_Connect", nomprenom);
                    API.triggerClientEvent(player, "StartStatus");
                    API.setEntitySyncedData(player, "LOGGED_IN", true);
                    player.freeze(false);
                    API.sendNativeToPlayer(player, Hash.DO_SCREEN_FADE_IN, 750);
                }
            }
        }

        public static void OnProgress(Client player, bool statut = true)
        {
            API.shared.setEntitySyncedData(player, "InProgress", statut);
        }

        public static bool IsOnProgress(Client player)
        {
            bool statut = false;
            statut = API.shared.getEntitySyncedData(player, "InProgress");
            return statut;
        }

        public static bool IsArrested(Client player)
        {
            bool statut = false;
            statut = API.shared.getEntitySyncedData(player, "Arrested");
            return statut;
        }

        private static void LoadLicence(Client player)
        {
            DataTable licences = GetPlayerLicences(player);
            if (licences.Rows.Count != 0)
            {
                foreach (DataRow row in licences.Rows)
                {
                    bool P_Voiture = Convert.ToBoolean(row["voiture"]);
                    bool P_Moto = Convert.ToBoolean(row["moto"]);
                    bool P_Camion = Convert.ToBoolean(row["poidslourd"]);
                    bool P_Taxi = Convert.ToBoolean(row["taxi"]);
                    bool P_Fourriere = Convert.ToBoolean(row["fourriere"]);
                    bool P_Pizza = Convert.ToBoolean(row["pizza"]);

                    API.shared.setEntityData(player, "P_Voiture", P_Voiture);
                    API.shared.setEntityData(player, "P_Moto", P_Moto);
                    API.shared.setEntityData(player, "P_Camion", P_Camion);
                    API.shared.setEntityData(player, "P_Taxi", P_Taxi);
                    API.shared.setEntityData(player, "P_Pizza", P_Pizza);
                    API.shared.setEntityData(player, "P_Fourriere", P_Fourriere);

                }
            }
        }
    }
}
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Data;
using System.Threading;
using static LSRP_VFR.Items.Items;
using static LSRP_VFR.Mysql.Database;
using static LSRP_VFR.Players.Player;

namespace LSRP_VFR.Mysql
{
    class DBPlayers : Script
    {
        public DBPlayers()
        {
            API.onPlayerConnected += OnPlayerConnected;
        }

        private void OnPlayerConnected(Client player)
        {
            try
            {
                API.delay(100, true, () => {
                    while (player != null)
                        {
                            SavePlayerLoop(player);
                            Thread.Sleep(120000);
                        }
                });
            }
            catch (Exception ex)
            {
                API.consoleOutput("[MySQL][ERROR] " + ex.ToString());
            }
        }

        public void SavePlayerLoop(Client player)
        {
            try
            {
                if (IsPlayerLoggedIn(player))
                {
                    if (player.vehicle != null)
                    {
                        DBVehicles.SaveVehicle(player.vehicle);
                    }
                    UpdatePlayerInfo(player);
                }

            }
            catch (Exception ex)
            {
                API.consoleOutput("[MySQL][ERROR] " + ex.ToString());
            }
        }

        public static void InsertPlayerInfo(Client player, string characters)
        {
            try
            {
                string query = String.Format("INSERT INTO user (name, ip, money, bank, rpname, inventory, position, characters, adminRank, LSPDrank) VALUES ('{0}','{1}','{2}','{3}','{4}','[]','[]','{5}','0','0'); INSERT INTO licences (owner) VALUES ('{0}')",
                        player.socialClubName,
                        player.address,
                        player.getSyncedData("Nom_Prenom").toString(),
                        "500",
                        "15000",
                        characters
                    );
                InsertQuery(query);
                /*
                API.shared.getSetting<string>("MoneyStart"),
                API.shared.getSetting<string>("BankMoneyStart"),
                */
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR] : ~s~" + e.ToString());
            }
        }

        public static void UpdateCloth(Client player, string cloth)
        {
            try
            {
                if (API.shared.getEntitySyncedData(player, "LOGGED_IN") == true)
                {
                    InsertQuery("UPDATE user SET clothing='" + cloth + "' WHERE name='" + player.socialClubName.ToString() + "'");
                }
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR] : ~s~" + e.ToString());
            }
        }

        public static void UpdatePlayerInfo(Client player)
        {
            try
            {
                if (Players.Player.IsPlayerLoggedIn(player))
                {
                    Vector3 PlayerPos = API.shared.getEntityPosition(player);

                    InventoryHolder inventory = API.shared.getEntityData(player, "InventoryHolder");
                    UpdatePlayerMoney(player);
                    string pos = "[[" + PlayerPos.X.ToString() + "],[" + PlayerPos.Y.ToString() + "],[" + PlayerPos.Z.ToString() + "]]";

                    var invs = "";
                    foreach (InventoryItem ii in inventory.Inventory)
                    {
                        invs += "[" + ii.Details.ID.ToString() + "," + ii.Quantity.ToString() + "],";
                    }
                    char[] car = { ',' };
                    var inventaires = "[" + invs.TrimEnd(car) + "]";

                    int hunger = API.shared.getEntitySyncedData(player, "PLAYER_HUNGRY");
                    int drink = API.shared.getEntitySyncedData(player, "PLAYER_THIRSTY");
                    int health = API.shared.getPlayerHealth(player);
                    string query = String.Format("UPDATE user SET position='{0}', inventory='{1}', hunger='{2}', thirst='{3}', health='{4}' WHERE name='{5}'", pos, inventaires, hunger, drink, health, player.socialClubName);
                    InsertQuery(query);
                }
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR] : ~s~" + e.ToString());
            }
        }

        public static void UpdatePlayerMoney(Client player)
        {
            try
            {
                if ((API.shared.getEntitySyncedData(player, "LOGGED_IN") == true))
                {
                    int money = API.shared.getEntitySyncedData(player.handle, "Money");
                    int bank = API.shared.getEntitySyncedData(player.handle, "BankMoney");
                    InsertQuery(String.Format("UPDATE user SET money='{0}', bank='{1}' WHERE name='{2}'", money.ToString(), bank.ToString(), player.socialClubName));
                }
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR] : ~s~" + e.ToString());
            }
        }

        public static Boolean ExistPlayerInfo(Client player)
        {
            if (player != null || (API.shared.getEntitySyncedData(player, "LOGGED_IN") == false))
            {
                DataTable result = GetQuery("SELECT * FROM user WHERE name='" + player.socialClubName + "'");
                if (result.Rows.Count != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public static void SetAdminRank(Client player, int rank)
        {
            try
            {
                InsertQuery("UPDATE user SET adminRank='" + rank.ToString() + "' WHERE name='" + player.socialClubName + "'");
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR] : ~s~" + e.ToString());
            }
        }

        public static DataTable GetPlayerInfo(Client player)
        {
            try
            {
                if (player != null)
                {
                    DataTable result = GetQuery("SELECT money, bank, position, jailed, inventory, characters, adminRank , LSPDrank, clothing, hunger, thirst, health, EMSrank FROM user WHERE name='" + player.socialClubName + "'");
                    return result;
                }
                return null;
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR] : ~s~" + e.ToString());
                return null;
            }
        }

        /***************************************************************/
        /*                          LICENCES                           */
        /***************************************************************/

        public static DataTable GetPlayerLicences(Client player)
        {
            try
            {
                DataTable result = GetQuery("SELECT * FROM licences WHERE owner='" + player.socialClubName + "'");
                return result;
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR] : ~s~" + e.ToString());
                return null;
            }
        }

        public static void UpdatePlayerLicences(Client player)
        {
            InsertQuery(String.Format("UPDATE licences SET voiture='{0}', moto='{1}', poidslourd='{2}', taxi='{3}', pizza='{4}', fourriere='{5}' WHERE owner='{6}'",
                BoolToInt(player, "P_Voiture"),
                BoolToInt(player, "P_Moto"),
                BoolToInt(player, "P_Camion"),
                BoolToInt(player, "P_Taxi"),
                BoolToInt(player, "P_Pizza"),
                BoolToInt(player, "P_Fourriere"),
                player.socialClubName
            ));
        }

        private static int BoolToInt(Client player, string licence)
        {
            if (API.shared.getEntityData(player, licence))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        

        public static void GetWhitelisted(Client player)
        {
            DataTable result = GetQuery("SELECT validated FROM whitelist WHERE socialclubName='" + player.socialClubName + "' LIMIT 1");
            if (result.Rows.Count != 0)
            {
                switch (Convert.ToInt16(result.Rows[0]["validated"]))
                {
                    case 0:
                        API.shared.kickPlayer(player, "[WHITELIST] Demande effectuée, mais non validée par le staff.");
                        break;
                    case 1:
                        API.shared.kickPlayer(player, "[WHITELIST] Demande validée, en attente d'un entretien oral.");
                        break;
                    case 2:
                        break;
                    case 3:
                        API.shared.kickPlayer(player, "[WHITELIST] Refusée.");
                        break;
                    default:
                        API.shared.kickPlayer(player, "[WHITELIST] Vous n'êtes pas whitelisté, + d'info sur adastragaming.fr");
                        break;
                }
            }
            else
            {
                API.shared.kickPlayer(player, "[WHITELIST] Vous n'êtes pas whitelisté, + d'info sur adastragaming.fr");
            }
        }
    }
}
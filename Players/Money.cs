using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Shared.Math;
using static LSRP_VFR.Mysql.DBPlayers;

namespace LSRP_VFR.Players
{
    public class Money : Script
    {
        public const int ATM_ROOT = 18;
        public const int ATM_TRANSFER = 19;

        public Money()
        {
            //PopulateCashpoints();
            API.onResourceStart += onStart;
            API.onClientEventTrigger += ScriptEvent;
        }

        public void ScriptEvent(Client sender, string eventName, object[] args)
        {
            if (eventName == "menu_handler_select_item")
            {
                var callbackId = (int)args[0];
                var index = (int)args[1];
                if (callbackId == ATM_ROOT)
                {
                    if (index == 0)
                    {
                        Balance(sender);
                    }
                    else if (index == 1)
                    {
                        var icallback = 3;
                        var showtext = "";
                        var maxlen = 10;
                        API.shared.triggerClientEvent(sender, "get_user_input", icallback, showtext, maxlen, null);
                    }
                    else if (index == 2)
                    {
                        var icallback = 4;
                        var showtext = "";
                        var maxlen = 10;
                        API.shared.triggerClientEvent(sender, "get_user_input", icallback, showtext, maxlen, null);
                    }
                    else if (index == 3)
                    {
                        var list = API.getAllPlayers();
                        API.setEntityData(sender, "list", list);
                        list.Remove(sender);
                        List<string> Actions = new List<string>();
                        foreach (Client player in list)
                        {
                            string name = API.getEntitySyncedData(player, "Nom_Prenom");
                            Actions.Add(name);
                        }

                        API.shared.triggerClientEvent(sender, "bettermenuManager", 190, "Transférer", "Selectionner le destinataire: ", false, Actions);
                    }
                }
                if (callbackId == ATM_TRANSFER)
                {
                    var icallback = 5;
                    var showtext = "";
                    var maxlen = 10;
                    API.shared.triggerClientEvent(sender, "get_user_input", icallback, showtext, maxlen, index);
                }
            }
            else if (eventName == "menu_handler_user_input")
            {
                if (args[1] != null)
                {
                    var icallback = (int)args[0];
                    var msg = (string)args[1];
                    if (icallback == 3)
                    {
                        int n;
                        bool isNumeric = int.TryParse(msg, out n);
                        if (isNumeric)
                        {
                            if (TakeBankMoney(sender, n))
                            {
                                GiveMoney(sender, n);
                                var bal = API.getEntitySyncedData(sender, "BankMoney");
                                UpdatePlayerMoney(sender);
                                //API.sendNotificationToPlayer(sender, "Nouveau solde : " + "$" + bal.ToString());
                                API.sendPictureNotificationToPlayer(sender, "           $" + bal.ToString(), "CHAR_BANK_MAZE", 0, 0, "~g~RETRAIT COMPLETE", "Nouveau solde :");
                                return;
                            }
                            API.sendPictureNotificationToPlayer(sender, "Fonds insuffisant", "CHAR_BANK_MAZE", 0, 0, "~r~REFUSEE", " ");
                            //API.sendNotificationToPlayer(sender, "Fonds insuffisant");
                            return;
                        }
                        API.sendPictureNotificationToPlayer(sender, "Operation invalide", "CHAR_BANK_MAZE", 0, 0, "~r~REFUSEE", " ");
                        //API.sendNotificationToPlayer(sender, "Operation invalide");
                    }
                    if (icallback == 4)
                    {
                        int n;
                        bool isNumeric = int.TryParse(msg, out n);
                        if (isNumeric)
                        {
                            if (TakeMoney(sender, n))
                            {
                                GiveBankMoney(sender, n);
                                var bal = API.getEntitySyncedData(sender, "BankMoney");
                                UpdatePlayerMoney(sender);
                                API.sendPictureNotificationToPlayer(sender, "           $" + bal.ToString(), "CHAR_BANK_MAZE", 0, 0, "~g~TRANSACTION TERMINEE", "Nouveau solde:");
                                return;
                            }
                            API.sendNotificationToPlayer(sender, "Tu n'as pas assez d'argent");
                            return;
                        }
                        API.sendPictureNotificationToPlayer(sender, "Fausse opération", "CHAR_BANK_MAZE", 0, 0, "~r~REFUSEE", "");
                    }
                    if (icallback == 5)
                    {
                        int n;
                        bool isNumeric = int.TryParse(msg, out n);
                        if (isNumeric)
                        {
                            if (TakeBankMoney(sender, n))
                            {
                                List<Client> list = API.getEntityData(sender, "list");
                                int index = (int)args[2];
                                API.sendPictureNotificationToPlayer(list[index],"           $" + msg, "CHAR_BANK_MAZE", 0, 0, "~g~TRANSFERT REUSSI", "From: " + sender.name);
                                GiveBankMoney(list[index], n);
                                UpdatePlayerMoney(sender);
                                return;
                            }
                            API.sendPictureNotificationToPlayer(sender, "Fonds insuffisant", "CHAR_BANK_MAZE", 0, 0, "~r~REFUSEE", " ");
                            return;
                        }
                        API.sendPictureNotificationToPlayer(sender, "Fausse opération", "CHAR_BANK_MAZE", 0, 0, "~r~REFUSEE", " ");
                    }
                }
            }
        }

        public void onStart()
        {
            /*  REWORK NEEDED!!!
            API.delay(1800000, false, () =>
            {
                foreach(Client player in API.getAllPlayers())
                {
                    var bankrmoney = API.shared.getEntitySyncedData(player, "BankMoney");
                    API.shared.setEntitySyncedData(player, "BankMoney", bankrmoney);
                    API.call("Database", "updatePlayerMoney", player);
                    API.sendPictureNotificationToPlayer(player, "From: Department of Social Security \n  $" + "- \n Balance: $" + bankrmoney.ToString(), "CHAR_BANK_MAZE", 1, 1, "Maze Bank Co.", "TRANSFER RECIEVED");
                }

            });
            */
        }

        public static bool TakeMoney(Client sender, int amount)
        {
            try
            {
                var playermoney = API.shared.getEntitySyncedData(sender, "Money");
                if ((playermoney >= amount) && (amount > 0))
                {
                    API.shared.setEntitySyncedData(sender, "Money", playermoney - amount);
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][MONEY] : ~s~" + e.ToString());
                return false;
            }
        }

        public static void GiveMoney(Client sender, int amount)
        {
            try
            {
                if (amount > 0)
                {
                    var playermoney = API.shared.getEntitySyncedData(sender, "Money");
                    API.shared.setEntitySyncedData(sender, "Money", playermoney + amount);
                    UpdatePlayerMoney(sender);
                }
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][MONEY] : ~s~" + e.ToString());
            }
        }

        public static void GiveMoneyToOther(Client sender, Client receiver, int amount)
        {
            try
            {
                if (amount > 0)
                {
                    var playermoney = API.shared.getEntitySyncedData(sender, "Money");
                    if ((playermoney - amount) >= 0)
                    {
                        var receivermoney = API.shared.getEntitySyncedData(receiver, "Money");
                        var sendermoney = API.shared.getEntitySyncedData(sender, "Money");
                        API.shared.setEntitySyncedData(receiver, "Money", receivermoney + amount);
                        API.shared.setEntitySyncedData(sender, "Money", sendermoney - amount);
                    }
                }
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][MONEY] : ~s~" + e.ToString());
            }

        }

        public static void GiveBankMoney(Client sender, int amount)
        {
            try
            {
                if (amount > 0)
                {
                    var bankrmoney = API.shared.getEntitySyncedData(sender, "BankMoney");
                    API.shared.setEntitySyncedData(sender, "BankMoney", bankrmoney + amount);
                }
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][MONEY] : ~s~" + e.ToString());
            }
        }

        public static bool TakeBankMoney(Client sender, int amount)
        {
            try
            {
                var bankrmoney = API.shared.getEntitySyncedData(sender, "BankMoney");
                if ((bankrmoney >= amount) && (amount >= 0))
                {
                    API.shared.setEntitySyncedData(sender, "BankMoney", bankrmoney - amount);
                    return true;
                }
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][MONEY] : ~s~" + e.ToString());
                return false;
            }
            return false;
        }

        public class ATM
        {
            public Vector3 Position { get; set; }

            public ATM(Vector3 position)
            {
                /*
                Position = position;
                ColShape col = API.shared.createSphereColShape(Position, 3);
                col.setData("ATM", true);
                var b = API.shared.createBlip(Position);
                API.shared.setBlipSprite(b, 108);
                b.shortRange = true;
                */

            }
        }

        public void Balance(Client sender)
        {
            try
            {
                var bal = API.getEntitySyncedData(sender, "BankMoney");
                //API.sendPictureNotificationToPlayer(sender, "           $" + bal.ToString(), "CHAR_BANK_MAZE", 0, 0, "SOLDE BANCAIRE:", "");
                API.sendNotificationToPlayer(sender, "SOLDE BANCAIRE: " + "$" + bal.ToString());
            }
            catch (Exception e)
            {
                API.consoleOutput("~r~[ERROR][MONEY] : ~s~" + e.ToString());
            }

        }
    }
}
/*
            try
            {

            }
            catch (Exception e)
            {
                API.consoleOutput("~r~[ERROR][MONEY] : ~s~" + e.ToString());
            }
            */
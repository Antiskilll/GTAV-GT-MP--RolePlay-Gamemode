using GrandTheftMultiplayer.Server.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Managers;
using static LSRP_VFR.Items.Items;

namespace LSRP_VFR.Players
{
    public class DeathManager : Script
    {
        public DeathManager()
        {
            
            API.onPlayerDeath += OnPlayerDeath;
            API.onClientEventTrigger += OnClientEventTrigger;
            API.onPlayerRespawn += OnPlayerRespawnHandler; 
        }

        private void OnPlayerRespawnHandler(Client player)
        {
            API.sendNativeToPlayer(player, Hash._DISABLE_AUTOMATIC_RESPAWN, true);

            InventoryHolder ih = new InventoryHolder();
            ih.Owner = player.handle;
            API.setEntityData(player, "InventoryHolder", ih);
            API.setEntitySyncedData(player, "Money", 0);
            API.sendNativeToPlayer(player, Hash.FREEZE_ENTITY_POSITION, player, false);
            API.setEntitySyncedData(player.handle, "PLAYER_HUNGRY", 100);
            API.setEntitySyncedData(player.handle, "PLAYER_THIRSTY", 100);

            var price = 100;
            var bankrmoney = API.shared.getEntitySyncedData(player, "BankMoney");
            API.shared.setEntitySyncedData(player, "BankMoney", bankrmoney - price);
            API.sendNotificationToPlayer(player, "~r~[Hôpital] ~s~Vous êtes dorénavant sur pied, vous nous avez payé la somme de: $~r~" + price.ToString());

        }

        private void OnClientEventTrigger(Client sender, string eventName, object[] arguments)
        {
            if (eventName == "menu_handler_select_item")
            {
                if ((int)arguments[0] == 452)
                {
                    InventoryHolder ih = API.getEntityData(sender, "InventoryHolder");
                    ih.RemoveItemFromInventory(ItemByID(93), 1);

                    var list = API.getEntityData(sender, "list");
                    int index = (int)arguments[1];   
                    Client recever = list[index];
                    API.resetEntityData(sender, "list");
                    if (Faction.EMS.Hospital.IsMedic(sender))
                    {
                        Unkillme(recever);
                    } else {
                        Random rnd = new Random();
                        int random = rnd.Next(0, 2);
                        if (random == 0)
                        {
                            Unkillme(recever);
                        }
                        else
                        {
                            Iamdead(recever);
                            API.sendNotificationToPlayer(sender, "Vous vous êtes pris pour un médecin, vous avez tué la victime.");
                        }
                    }
                }
            }
            else if (eventName == "suicideEvent")
            {
                Iamdead(sender);
            }
            else if (eventName == "callmedic")
            {
                string msg = "Une personne dans le coma a été signalée.";
                Faction.EMS.Hospital.CallMedic(sender, msg);
            }
        }

        private void OnPlayerDeath(Client player, NetHandle entityKiller, int weapon)
        {
            API.sendNativeToPlayer(player, Hash.SET_FADE_IN_AFTER_DEATH_ARREST, false);
            API.sendNativeToPlayer(player, Hash.SET_FADE_OUT_AFTER_DEATH, false);
            API.sendNativeToPlayer(player, Hash._DISABLE_AUTOMATIC_RESPAWN, true);
            API.setEntitySyncedData(player, "IsOnComa", true);
            API.triggerClientEvent(player, "MakeInComa");
        }

        public static bool IsOnComaPlayer(Client player)
        {
            return API.shared.getEntitySyncedData(player, "IsOnComa");
        }
        [Command]
        public static void Unkillme(Client player)
        {
            API.shared.sendNativeToPlayer(player, Hash.NETWORK_RESURRECT_LOCAL_PLAYER, player.position.X, player.position.Y, player.position.Z, player.rotation.Z, false, false);
            API.shared.sendNativeToPlayer(player, Hash.RESURRECT_PED, player);
            API.shared.sendNativeToPlayer(player, Hash._RESET_LOCALPLAYER_STATE, player);
            API.shared.sendNativeToPlayer(player, Hash.RESET_PLAYER_ARREST_STATE, player);
            API.shared.triggerClientEvent(player, "MakeOutComa");
            //API.playPlayerAnimation(player, (int)(AnimationFlags.Loop), "missmic2leadinmic_2_intleadout", "ko_on_floor_idle");
            API.shared.setPlayerHealth(player, 5);
            API.shared.setEntitySyncedData(player, "IsOnComa", false);
        }
        [Command]
        public static void Iamdead(Client player)
        {
            API.shared.sendNativeToPlayer(player, Hash.SET_FADE_IN_AFTER_DEATH_ARREST, true);
            API.shared.sendNativeToPlayer(player, Hash.SET_FADE_OUT_AFTER_DEATH, true);
            API.shared.sendNativeToPlayer(player, Hash._DISABLE_AUTOMATIC_RESPAWN, false);
            API.shared.triggerClientEvent(player, "MakeOutComa");
            API.shared.setEntitySyncedData(player, "IsOnComa", false);
        }
    }
}

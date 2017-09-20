using System;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using static LSRP_VFR.Mysql.DBPlayers;
namespace LSRP_VFR.Players
{
    class CharacterCreator : Script
    {

        public CharacterCreator()
        {
            API.onClientEventTrigger += ClientEvent;
        }

        private void ClientEvent(Client sender, string eventName, object[] args)
        {
            if (eventName == "CHARFINISHED")
            {
                try
                {
                    API.sendNativeToPlayer(sender, Hash.DO_SCREEN_FADE_OUT, 200);

                    string character = "[" + ""
                        + "[" + API.getEntityModel(sender).ToString() + "],"
                        + "[" + args[1].ToString() + "],"
                        + "[" + args[2].ToString() + "],"
                        + "[" + args[3].ToString() + "],"
                        + "[" + args[4].ToString() + "],"
                        + "[" + args[5].ToString() + "],"
                        + "[" + args[6].ToString() + "],"
                        + "[" + args[7].ToString() + "],"
                        + "[" + args[8].ToString() + "]"
                        + "]";

                    InsertPlayerInfo(sender, character);
                    API.delay(5000, true, () => {
                        API.call("Player", "StartInitPlayer", sender);
                    });
                }
                catch (Exception e)
                {
                    API.consoleOutput("~r~[ERROR][PLAYER] : ~s~" + e.ToString());
                }

            }
        }

        public static void StartCreator(Client player)
        {
            API.shared.sendNativeToPlayer(player, Hash.DO_SCREEN_FADE_IN, 750);
            API.shared.setEntityPosition(player, new Vector3(402.612, -996.3103, -99.00027));
            API.shared.setEntityRotation(player, new Vector3(0, 0, 177.1582));
            Random rnd = new Random();
            var dim = rnd.Next(1, 99999);
            player.setSkin(PedHash.FreemodeMale01);
            API.shared.setEntityDimension(player, dim);
            API.shared.triggerClientEvent(player, "StarCharacterCustomizationTrigger");
            player.freeze(true);
        }
    }
}

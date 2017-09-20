using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using static LSRP_VFR.Main;

namespace LSRP_VFR.Global
{
    class Teamspeak: Script
    {
        public Teamspeak()
        {
            API.onPlayerConnected += API_onPlayerConnected;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        private void API_onPlayerConnected(Client player)
        {
            try
            {
                API.delay(500, false, () => {
                    if (player != null && player.exists && Players.Player.IsPlayerLoggedIn(player))
                    {
                        //API.consoleOutput("test");
                        List<Client> players = API.getAllPlayers();
                        if (players.Count != 0)
                        {
                            List<string> playerNames = new List<string>();
                            Vector3 PlayerPos = API.getEntityPosition(player);
                            String result = "";
                            players.Remove(player);
                            foreach (Client streamedPlayers in players)
                            {
                                var streamedPlayerPos = API.getEntityPosition(streamedPlayers);
                                var distance = PlayerPos.DistanceTo(streamedPlayerPos);
                                var voiceRange = API.getEntitySyncedData(streamedPlayers, "VOICE_RANGE");
                                var range = 20;
                                if (voiceRange == "Crier") range = 30;
                                if (voiceRange == "Parler") range = 20;
                                if (voiceRange == "Chuchoter") range = 6;
                                if (distance < range) playerNames.Add(API.getEntitySyncedData(streamedPlayers, "Nom_Prenom") + "|" + distance.ToString() + "|" + range.ToString());
                            }

                            if (playerNames.Count != 0)
                            {
                                result = String.Join(";", playerNames.ToArray());
                                API.triggerClientEvent(player, "updateTeamspeak", result);
                            }
                            else
                            {
                                API.triggerClientEvent(player, "updateTeamspeak", "0");
                            }
                        }
                        else
                        {
                            API.triggerClientEvent(player, "updateTeamspeak", "0");
                        }
                    }
                });
            }
            catch (Exception e)
            {
                API.consoleOutput("~r~[ERROR][TEAMSPEAK] : ~s~" + e.ToString());
            }
        }

        private void OnClientEventTrigger(Client sender, string eventName, object[] arguments)
        {
            if (eventName == "Speak")
            {
                if ((int)arguments[0] == 0)
                {
                    API.stopPlayerAnimation(sender);
                }
                else if ((int)arguments[0] == 1)
                {
                    if (!Players.Player.IsArrested(sender))
                    {
                        API.playPlayerAnimation(sender, (int)(AnimationFlags.Loop | AnimationFlags.AllowPlayerControl), "mp_facial", "mic_chatter");
                    }
                }
            }
        }

    }
}

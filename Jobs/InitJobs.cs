using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using System.Threading;

namespace LSRP_VFR.Jobs
{
    class InitJobs : Script
    {
        private Ped pnjemploi;

        public InitJobs()
        { 
            API.consoleOutput("[Jobs][INFO] Initialisation des jobs!");
            //API.onResourceStart += OnResourceStart;
            //API.onResourceStop += OnResourceStop;
            API.onClientEventTrigger += OnClientEventTrigger;

        }

        private void OnResourceStop()
        {
            API.deleteEntity(pnjemploi);
        }

        private void OnResourceStart()
        {
            Vector3 pnjpos = new Vector3(-723.7092, -420.7821, 34.99681);
            pnjemploi = API.createPed((PedHash)(-321892375), pnjpos, 0);
            API.setEntityRotation(pnjemploi, new Vector3(0, 0, 86.3));
            API.setEntitySyncedData(pnjemploi, "Interaction", "Interim");

            Blip blip = API.shared.createBlip(pnjpos);
            API.setBlipName(blip, "Agence Interim");
            blip.shortRange = true;
            blip.sprite = 408;
        }

        private void OnClientEventTrigger(Client sender, string eventName, params object[] args)
        {
            if (eventName == "Interim")
            {
                List<String> Metiers = new List<string>();
                Metiers.Add("Livreur");
                Metiers.Add("Quitter le menu");
                API.triggerClientEvent(sender, "bettermenuManager",100, "Agence d'intérim", "Sélectionnez un métier",false, Metiers);
            }else if (eventName == "menu_handler_select_item")
            {
                if ((int)args[0] == 100)
                {
                    API.triggerClientEvent(sender, "markerblip", new Vector3(136.9859, -1069.336, 29.19238)- new Vector3(0, 0, 1f));
                    bool colision = false;
                    ColShape colShape = API.createCylinderColShape(new Vector3(136.9859, -1069.336, 29.19238), 1f, 1f);
                    colShape.onEntityEnterColShape += (shape, entity) =>
                    {
                        var players = API.getPlayerFromHandle(entity);

                        if (players == null)
                        {
                            return;
                        }
                        else
                        {
                            if (players.handle == sender.handle)
                            {
                                API.triggerClientEvent(sender, "removemarkerblip");
                                colision = true;
                            }
                        }

                    };
                    while (colision == false)
                    {
                        Thread.Sleep(3000);
                    }
                    API.deleteColShape(colShape);
                }
            }
        }
    }
}

using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSRP_VFR.Faction.LSPD
{
    public class LSPD_Service : Script
    {
        public LSPD_Service()
        {
            API.onResourceStart += OnResourceStart;
            API.onEntityEnterColShape += OnEntityEnterColShape;
            API.onEntityExitColShape += OnEntityExitColShape;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        private void OnResourceStart()
        {
            Vector3 position = new Vector3(456.1482, -988.5895, 30.68961); //Coords QG 1 LSPD
            Vector3 position2 = new Vector3(127.4131, -727.3068, 242.1519); // Coords QG 2 LSPD EAST
            CylinderColShape m_colShape = API.createCylinderColShape(position, 1.0f, 1.0f);
            CylinderColShape m_colShape2 = API.createCylinderColShape(position2, 1.0f, 1.0f);
            m_colShape.setData("Shape", "Service");
            m_colShape2.setData("Shape", "Service");
            var marker = API.createMarker(1, position - new Vector3(0, 0, 1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 100, 255, 255, 255);
            var marker2 = API.createMarker(1, position2 - new Vector3(0, 0, 1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 100, 255, 255, 255);
        }

        private void OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "menu_handler_select_item")
            {
                if ((int)arguments[0] == 201)
                {
                    API.triggerClientEvent(sender, "LSPD_ServiceCloth");
                    if (API.getEntitySyncedData(sender, "Sexe") == "Homme")
                    {
                        API.setPlayerClothes(sender, 11, 55, 0); //Survet
                        API.setPlayerClothes(sender, 3, 0, 0); // Bras
                        API.setPlayerClothes(sender, 8, 57, 0); // Chemise
                        API.setPlayerClothes(sender, 6, 25, 0); // Chaussure
                        API.setPlayerClothes(sender, 4, 31, 0); // Pantalon
                        API.setPlayerClothes(sender, 8, 58, 0);
                        API.setPlayerAccessory(sender, 0, 46, 0);
                        if (API.getEntitySyncedData(sender, "LSPDrank") == 4)
                        {
                            API.setPlayerClothes(sender, 9, 15, 0);
                        }
                        if (API.getEntitySyncedData(sender, "LSPDrank") == 5)
                        {
                            API.setPlayerClothes(sender, 9, 16, 0);
                        }
                    }
                    else if (API.getEntitySyncedData(sender, "Sexe") == "Femme")
                    {
                        API.setPlayerClothes(sender, 11, 48, 0); //Survet
                        API.setPlayerClothes(sender, 3, 0, 0); // Bras
                        API.setPlayerClothes(sender, 8, 0, 0); // Chemise
                        API.setPlayerClothes(sender, 8, 3, 0); // Chemise
                        API.setPlayerClothes(sender, 6, 25, 0); // Chaussure
                        API.setPlayerClothes(sender, 4, 30, 0); // Pantalon
                        API.setPlayerClothes(sender, 8, 58, 0);
                        if (API.getEntitySyncedData(sender, "LSPDrank") == 4)
                        {
                            API.setPlayerClothes(sender, 9, 17, 0);
                        }
                        if (API.getEntitySyncedData(sender, "LSPDrank") == 5)
                        {
                            API.setPlayerClothes(sender, 9, 18, 0);
                        }
                    }
                    API.setEntitySyncedData(sender, "Police", true);
                    GiveWeapon(sender, -1951375401);
                    GiveWeapon(sender, 1737195953);
                    GiveWeapon(sender, 911657153);
                    GiveWeapon(sender, 1593441988);
                    GiveWeapon(sender, -2084633992);
                    GiveWeapon(sender, 487013001);

                }
            }
        }

        private void OnEntityEnterColShape(ColShape colshape, NetHandle entity)
        {
            if (colshape.hasData("Shape"))
            {
                if (colshape.getData("Shape") == "Service")
                {
                    var players = API.getPlayerFromHandle(entity);
                    if (players != null)
                    {
                        if (players.getSyncedData("LSPDrank") > 0)
                        {
                            List<string> Actions = new List<string>();
                            Actions.Add("Prendre son service");
                            Actions.Add("Quitter le menu");
                            API.triggerClientEvent(players, "bettermenuManager", 201, "Vestiaire", "Prise de service: ", false, Actions);
                        }
                    }
                }
            }
        }

        private void OnEntityExitColShape(ColShape colshape, NetHandle entity)
        {
            if (colshape.hasData("Shape"))
            {
                if (colshape.getData("Shape") == "Service")
                {
                    var player = API.getPlayerFromHandle(entity);
                    if (player == null) return;
                    API.triggerClientEvent(player, "LSPD_QUITMENU");
                }
            }
        }

        public void GiveWeapon(Client player, int hash)
        {
            API.givePlayerWeapon(player, (WeaponHash)hash, 100, false, true);
        }

        public static bool IsCop (Client player)
        {
            return API.shared.getEntitySyncedData(player, "Police");
        }
    }
}
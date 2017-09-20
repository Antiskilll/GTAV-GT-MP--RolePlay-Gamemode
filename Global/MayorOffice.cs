using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Shared;

namespace LSRP_VFR.Global
{
    class MayorOffice : Script
    {
        private Vector3 Enter1mayor = new Vector3(-129.3403, -599.5688, 48.2477);
        private Vector3 Enter2mayor = new Vector3(-125.3833, -624.7169, 48.41582);
        private Vector3 MayorRoomEnter = new Vector3(-140.8429, -616.7669, 168.8205);
        private Vector3 MayorRoomExit = new Vector3(-141.2628, -614.3312, 168.8205);
        private Vector3 ExitMayer = new Vector3(-119.9218, -590.1616, 48.22081);

        public MayorOffice()
        {
            API.onResourceStart += API_onResourceStart;
            API.onClientEventTrigger += API_onClientEventTrigger;
        }

        private void API_onResourceStart()
        {
            var blip = API.createBlip(ExitMayer);
            API.setBlipSprite(blip, 78);
            API.setBlipShortRange(blip, true);
            API.setBlipName(blip, "Mairie");

            Marker EnterMayor1 = API.createMarker(1, Enter1mayor - new Vector3(0, 0, 1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 100, 255, 255, 255);
            Marker EnterMayor2 = API.createMarker(1, Enter2mayor - new Vector3(0, 0, 1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 100, 255, 255, 255);
            CylinderColShape EnterCol1 = API.createCylinderColShape(Enter1mayor, 1f, 1f);
            CylinderColShape EnterCol2 = API.createCylinderColShape(Enter2mayor, 1f, 1f);

            Marker ExitMayor = API.createMarker(1, MayorRoomExit - new Vector3(0, 0, 1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 100, 255, 255, 255);
            CylinderColShape ExitCol1 = API.createCylinderColShape(MayorRoomExit, 2f, 2f);

            EnterCol1.onEntityEnterColShape += Col1_onEntityEnterColShape;
            EnterCol2.onEntityEnterColShape += Col2_onEntityEnterColShape;
            ExitCol1.onEntityEnterColShape += ExitCol1_onEntityEnterColShape;

            EnterCol1.onEntityExitColShape += EnterCol1_onEntityExitColShape;
            EnterCol2.onEntityExitColShape += EnterCol2_onEntityExitColShape;
            ExitCol1.onEntityExitColShape += ExitCol1_onEntityExitColShape;
        }

        private void API_onClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "menu_handler_select_item")
            {
                if ((int)arguments[0] == 27)
                {
                    API.setEntityPosition(sender, MayorRoomEnter);
                }
                else if ((int)arguments[0] == 28)
                {
                    API.setEntityPosition(sender, ExitMayer);
                }
            }
        }



        private void ExitCol1_onEntityEnterColShape(ColShape shape, NetHandle entity)
        {
            if (entity == null) return;
            if (API.getEntityType(entity) != EntityType.Player) return;
            Client player = API.getPlayerFromHandle(entity);
            ExitMenuMayor(player);
        }

        private void Col1_onEntityEnterColShape(ColShape shape, NetHandle entity)
        {
            if (entity == null) return;
            if (API.getEntityType(entity) != EntityType.Player) return;
            Client player = API.getPlayerFromHandle(entity);
            EnterMenuMayor(player);
        }

        private void Col2_onEntityEnterColShape(ColShape shape, NetHandle entity)
        {
            if (entity == null) return;
            if (API.getEntityType(entity) != EntityType.Player) return;
            Client player = API.getPlayerFromHandle(entity);
            EnterMenuMayor(player);
        }

        private void EnterMenuMayor(Client player)
        {
            List<string> Actions = new List<string>();
            Actions.Add("Entrer");
            Actions.Add("Quitter le menu");
            API.triggerClientEvent(player, "bettermenuManager", 27, "Mairie", "", false, Actions);
        }

        private void ExitMenuMayor(Client player)
        {
            List<string> Actions = new List<string>();
            Actions.Add("Sortir");
            Actions.Add("Quitter le menu");
            API.triggerClientEvent(player, "bettermenuManager", 28, "Mairie", "", false, Actions);
        }

        private void ExitCol1_onEntityExitColShape(ColShape shape, NetHandle entity)
        {
            if (entity == null) return;
            if (API.getEntityType(entity) != EntityType.Player) return;
            Client player = API.getPlayerFromHandle(entity);
            API.triggerClientEvent(player, "menu_handler_close_menu");
        }

        private void EnterCol2_onEntityExitColShape(ColShape shape, NetHandle entity)
        {
            if (entity == null) return;
            if (API.getEntityType(entity) != EntityType.Player) return;
            Client player = API.getPlayerFromHandle(entity);
            API.triggerClientEvent(player, "menu_handler_close_menu");
        }

        private void EnterCol1_onEntityExitColShape(ColShape shape, NetHandle entity)
        {
            if (entity == null) return;
            if (API.getEntityType(entity) != EntityType.Player) return;
            Client player = API.getPlayerFromHandle(entity);
            API.triggerClientEvent(player, "menu_handler_close_menu");
        }
    }
}

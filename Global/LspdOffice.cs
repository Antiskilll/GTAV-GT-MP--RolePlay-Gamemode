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
    class LspdOffice : Script
    {
        private Vector3 Enter1lspd = new Vector3(-1094.284, -810.0017, 19.28703);
        //private Vector3 Enter2lspd = new Vector3(134.635, -765.831, 242.152);
        private Vector3 LSPDroomEnter = new Vector3(134.635, -765.831, 242.152);
        private Vector3 LSPDroomExit = new Vector3(136.1126, -761.4921, 242.152);
        private Vector3 ExitLSPD = new Vector3(-1061.069, -826.5229, 19.21117);


        public LspdOffice()
        {
            API.onResourceStart += API_onResourceStart;
            API.onClientEventTrigger += API_onClientEventTrigger;
        }

        private void API_onResourceStart()
        {
            var blip = API.createBlip(ExitLSPD);
            API.setBlipSprite(blip, 487);
            API.setBlipColor(blip, 63);
            API.setBlipShortRange(blip, true);
            API.setBlipName(blip, "LSPD QG");

            Marker Enter1Lspd = API.createMarker(1, Enter1lspd - new Vector3(0, 0, 1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 100, 255, 255, 255);
            //Marker Enter2Lspd = API.createMarker(1, Enter2lspd - new Vector3(0, 0, 1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 100, 255, 255, 255);
            CylinderColShape EnterCol3 = API.createCylinderColShape(Enter1lspd, 1f, 1f);
            //CylinderColShape EnterCol4 = API.createCylinderColShape(Enter2lspd, 1f, 1f);

            Marker LSPDRoomExit = API.createMarker(1, LSPDroomExit - new Vector3(0, 0, 1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 100, 255, 255, 255);            
            CylinderColShape ExitCol3 = API.createCylinderColShape(LSPDroomExit, 2f, 2f);

            EnterCol3.onEntityEnterColShape += Col3_onEntityEnterColShape;
            //EnterCol4.onEntityEnterColShape += ExitCol3_onEntityEnterColShape;
            ExitCol3.onEntityEnterColShape += ExitCol3_onEntityEnterColShape;

            EnterCol3.onEntityExitColShape += EnterCol3_onEntityExitColShape;
            //EnterCol4.onEntityEnterColShape += ExitCol3_onEntityEnterColShape;
            ExitCol3.onEntityExitColShape += ExitCol3_onEntityExitColShape;
           
        }

        private void API_onClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "menu_handler_select_item")
            {
                if ((int)arguments[0] == 16)
                {
                    API.setEntityPosition(sender, LSPDroomEnter);
                }
                else if ((int)arguments[0] == 15)
                {
                    API.setEntityPosition(sender, ExitLSPD);
                }
            }
        }



        private void ExitCol3_onEntityEnterColShape(ColShape shape, NetHandle entity)
        {
            if (entity == null) return;
            if (API.getEntityType(entity) != EntityType.Player) return;
            Client player = API.getPlayerFromHandle(entity);
            ExitMenuLSPD(player);
        }

        private void Col3_onEntityEnterColShape(ColShape shape, NetHandle entity)
        {
            if (entity == null) return;
            if (API.getEntityType(entity) != EntityType.Player) return;
            Client player = API.getPlayerFromHandle(entity);
            EnterMenuLSPD(player);
        }

        private void Col4_onEntityEnterColShape(ColShape shape, NetHandle entity)
        {
            if (entity == null) return;
            if (API.getEntityType(entity) != EntityType.Player) return;
            Client player = API.getPlayerFromHandle(entity);
            EnterMenuLSPD(player);
        }

        private void EnterMenuLSPD(Client player)
        {
            List<string> Actions = new List<string>();
            Actions.Add("Entrer");
            Actions.Add("Quitter le menu");
            API.triggerClientEvent(player, "bettermenuManager", 16, "LSPD QG", "", false, Actions);
        }

        private void ExitMenuLSPD(Client player)
        {
            List<string> Actions = new List<string>();
            Actions.Add("Sortir");
            Actions.Add("Quitter le menu");
            API.triggerClientEvent(player, "bettermenuManager", 15, "LSPD QG", "", false, Actions);
        }

        private void ExitCol3_onEntityExitColShape(ColShape shape, NetHandle entity)
        {
            if (entity == null) return;
            if (API.getEntityType(entity) != EntityType.Player) return;
            Client player = API.getPlayerFromHandle(entity);
            API.triggerClientEvent(player, "menu_handler_close_menu");
        }

        private void EnterCol4_onEntityExitColShape(ColShape shape, NetHandle entity)
        {
            if (entity == null) return;
            if (API.getEntityType(entity) != EntityType.Player) return;
            Client player = API.getPlayerFromHandle(entity);
            API.triggerClientEvent(player, "menu_handler_close_menu");
        }

        private void EnterCol3_onEntityExitColShape(ColShape shape, NetHandle entity)
        {
            if (entity == null) return;
            if (API.getEntityType(entity) != EntityType.Player) return;
            Client player = API.getPlayerFromHandle(entity);
            API.triggerClientEvent(player, "menu_handler_close_menu");
        }
    }
}

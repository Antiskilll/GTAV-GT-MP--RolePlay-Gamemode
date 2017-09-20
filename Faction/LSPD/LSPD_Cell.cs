using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;

namespace LSRP_VFR.Faction.LSPD
{
    class LSPD_Cell : Script
    {
        private Vector3 ceilmanagerpos = new Vector3(462.1221, -998.4777, 24.91488);
        private int C1;
        private int C2;
        private int C3;

        private Boolean C1State = false;
        private Boolean C2State = false;
        private Boolean C3State = false;

        public LSPD_Cell()
        {
            API.onResourceStart += API_onResourceStart;
            API.onClientEventTrigger += API_onClientEventTrigger;
        }

        private void API_onClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "CellManager")
            {
                switch (Convert.ToInt16(arguments[0]))
                {
                    case 1:
                        if (C1State) { ChangeDoorStat(C1, false); C1State = false; } else { ChangeDoorStat(C1, true); C1State = true; }
                        break;
                    case 2:
                        if (C2State) { ChangeDoorStat(C2, false); C2State = false; } else { ChangeDoorStat(C2, true); C2State = true; }
                        break;
                    case 3:
                        if (C3State) { ChangeDoorStat(C3, false); C3State = false; } else { ChangeDoorStat(C3, true); C3State = true; }
                        break;
                    default:
                        break;
                }
            }
        }

        private void API_onResourceStart()
        {
            C1 = API.exported.doormanager.registerDoor(631614199, new Vector3(461.8065, -994.4086, 25.06443));
            C2 = API.exported.doormanager.registerDoor(631614199, new Vector3(461.8065, -997.6583, 25.06443));
            C3 = API.exported.doormanager.registerDoor(631614199, new Vector3(461.8065, -1001.302, 25.06443));

            int[] doors = {
                C1,
                C2,
                C3
            };
            foreach (int door in doors)
            {
                API.exported.doormanager.setDoorState(door, false, 0);
            }
            
            CylinderColShape ceil_colShape = API.createCylinderColShape(ceilmanagerpos, 8.0f, 3.0f);
            ceil_colShape.onEntityEnterColShape += (shape, entity) =>
            {
                var player = API.getPlayerFromHandle(entity);
                if (player == null){return;}               
                if (LSPD_Service.IsCop(player))
                {
                    API.triggerClientEvent(player, "OpenCeilManager", C1State, C2State, C3State);
                }
            };
        }

        private void ChangeDoorStat(int door, bool state)
        {
            API.exported.doormanager.setDoorState(door, state, 0);            
        }
    }
}

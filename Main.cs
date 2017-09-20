using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Shared.Math;
using System;

namespace LSRP_VFR
{
    class Main : Script
    {
        private string serverName = "~r~[FR] ∑ ~y~Los Santos RolePlay FR~r~";
        private string changresourceName = "~r~lsrpvfr";

        public Main()
        {
            API.onResourceStart += OnResourceStart;
            API.setServerName(serverName);
            API.setGamemodeName(changresourceName);
        }

        private void OnResourceStart()
        {
            LoadIPL();
        }

        public void LoadIPL()
        {
            API.consoleOutput("Chargements des IPLS en cours ...");
            // Yacht
            API.requestIpl("hei_yacht_heist");
            API.requestIpl("hei_yacht_heist_bar");
            API.requestIpl("hei_yacht_heist_bar_lod");
            API.requestIpl("hei_yacht_heist_bedrm");
            API.requestIpl("hei_yacht_heist_bedrm_lod");
            API.requestIpl("hei_yacht_heist_bridge");
            API.requestIpl("hei_yacht_heist_bridge_lod");
            API.requestIpl("hei_yacht_heist_distantlights");
            API.requestIpl("hei_yacht_heist_enginrm");
            API.requestIpl("hei_yacht_heist_enginrm_lod");
            API.requestIpl("hei_yacht_heist_lod");
            API.requestIpl("hei_yacht_heist_lodlights");
            API.requestIpl("hei_yacht_heist_lounge");
            API.requestIpl("hei_yacht_heist_lounge_lod");
            API.requestIpl("hei_yacht_heist_slod");


            API.removeIpl("facelobby");


            API.removeIpl("v_carshowroom");
            API.removeIpl("shutter_open");
            API.removeIpl("shutter_closed");
            API.removeIpl("shr_int");
            API.removeIpl("csr_inMission");
            API.removeIpl("fakeint");
            API.requestIpl("shr_int");

            //Interrior office
            API.requestIpl("ex_dt1_02_office_01a"); // MAYOR
        
            API.consoleOutput("IPL Charger!");
        }

        public static Vector3 XYInFrontOfPoint(Vector3 pos, float angle, float distance)
        {
            Vector3 ret = pos.Copy();
            ret.X += (distance * (float)Math.Sin(-angle));
            ret.Y += (distance * (float)Math.Cos(-angle));
            return ret;
        }

        [Flags]
        public enum AnimationFlags
        {
            Loop = 1 << 0,
            StopOnLastFrame = 1 << 1,
            OnlyAnimateUpperBody = 1 << 4,
            AllowPlayerControl = 1 << 5,
            Cancellable = 1 << 7
        }
    }
}

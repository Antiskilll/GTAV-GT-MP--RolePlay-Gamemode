using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using System;
using System.Threading;

namespace LSRP_VFR.Global
{
    public class TimeManager : Script
    {
        private DateTime _lastminutesUpdate = DateTime.Now;
        private DateTime _lasthoursUpdate = DateTime.Now;
        public int hours = 8;
        public int minutes = 0;

        public TimeManager()
        {
            API.onUpdate += onUpdate;
            //foreach (var player in API.getAllPlayers()) API.freezePlayerTime(player, true);
            API.consoleOutput("[SERVER] Initialisation de la gestion du temps!");
        }   

        private void onUpdate()
        {
            if (DateTime.Now.Subtract(_lastminutesUpdate).Seconds < 8) return;
            _lastminutesUpdate = DateTime.Now;
            minutes = minutes + 1;
            if (minutes == 60)
            {
                minutes = 0;
                hours = hours + 1;
            };
            if (hours == 24)
            {
                hours = 0;
            };
            API.setTime(hours, minutes);
        }
    }
}
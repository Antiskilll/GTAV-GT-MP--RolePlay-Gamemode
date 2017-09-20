using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using System;
using System.Threading;

namespace LSRP_VFR.Global
{
    public class WeatherManager : Script
    {
        bool WinterMod = false;
        Thread weatherThread = null;
        public WeatherManager()
        {
            API.onResourceStart += WeatherInit;
            API.onPlayerFinishedDownload += OnPlayerFinishedDownload;
            API.onResourceStop += OnResourceStop;
        }

        private void OnResourceStop()
        {
            weatherThread.Abort();
        }

        private void OnPlayerFinishedDownload(GrandTheftMultiplayer.Server.Elements.Client player)
        {        
            if (WinterMod) {
                API.triggerClientEvent(player, "merrychristmas");
            }
        }

        private void WeatherInit()
        {
            weatherThread = new Thread(UpdateWeather);
            weatherThread.Start();
            API.consoleOutput("[SERVER] Initialisation de la météo!");
            WinterMod = API.getSetting<bool>("WinterMod");
            if (WinterMod)
            {
                API.consoleOutput("[Weather] WinterMod activated!");
            }
        }
        Random rnd = new Random();
        public void UpdateWeather()
        {
            while (true)
            {
                int timeweather = rnd.Next(1800000, 2700000); // 8 minutes || 45 minutes
                if (!WinterMod)
                {
                    int weather = rnd.Next(0, 7);
                    API.setWeather(weather);
                    Thread.Sleep(timeweather);
                } else {
                    int weather = rnd.Next(0, 12);
                    API.setWeather(weather);                  
                    Thread.Sleep(timeweather);
                }
            }
        }
    }
}
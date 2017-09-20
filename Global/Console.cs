using GrandTheftMultiplayer.Server.API;
using System;

namespace LSRP_VFR.Global
{
public class ConsoleScript : Script
{
    public ConsoleScript()
    {
        API.onResourceStart += OnResourceStartHandler;
    }

    public void OnResourceStartHandler()
    {
        while (true)
        {
            string cmd = Console.ReadLine();
            string param = "";

            int space = cmd.IndexOf(" ");
            if (space > 0)
            {
                param = cmd.Remove(0, space + 1);
                cmd = cmd.Remove(space, cmd.Length - space);
                switch (cmd)
                {
                    case "serverstop":
                        API.stopResource("AdAstraRP");
                        System.Environment.Exit(2);
                        break;
                    case "start":
                        API.startResource(param);
                        break;
                    case "stop":
                        API.stopResource(param);
                        break;
                    case "restart":
                        API.stopResource(param);
                        API.startResource(param);
                        break;

                }
            }
        }
    }
}
}

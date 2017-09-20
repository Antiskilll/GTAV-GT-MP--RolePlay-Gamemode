using System.IO;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;

namespace LSRP_VFR.Global
{
    public class Newcommands : Script
    {
        [Command("coords")]
        public void Coords(Client player, string coordName)
        {
            Vector3 playerPosGet = API.getEntityPosition(player);
            var pPosX = (playerPosGet.X.ToString().Replace(',', '.') + ", ");
            var pPosY = (playerPosGet.Y.ToString().Replace(',', '.') + ", ");
            var pPosZ = (playerPosGet.Z.ToString().Replace(',', '.'));
            Vector3 playerRotGet = API.getEntityRotation(player);
            var pRotX = (playerRotGet.X.ToString().Replace(',', '.') + ", ");
            var pRotY = (playerRotGet.Y.ToString().Replace(',', '.') + ", ");
            var pRotZ = (playerRotGet.Z.ToString().Replace(',', '.'));

            API.sendChatMessageToPlayer(player, "Your position is: ~y~" + playerPosGet, "~w~Your rotation is: ~y~" + playerRotGet);
            StreamWriter coordsFile;
            if (!File.Exists("SavedCoords.txt"))
            {
                coordsFile = new StreamWriter("SavedCoords.txt");
            }
            else
            {
                coordsFile = File.AppendText("SavedCoords.txt");
            }
            API.sendChatMessageToPlayer(player, "~r~Coordinates have been saved!");
            coordsFile.WriteLine("| " + coordName + " | " + "Saved Coordenates: " + pPosX + pPosY + pPosZ + " Saved Rotation: " + pRotX + pRotY + pRotZ);
            coordsFile.Close();
        }
        [Command("savedcoords")]
        public void SavedCoords(Client player)
        {
            API.sendChatMessageToPlayer(player, "~r~Current Saved Coordenates:");
            int counter = 0;
            string coordsLine;
            System.IO.StreamReader file = new System.IO.StreamReader("SavedCoords.txt");
            while ((coordsLine = file.ReadLine()) != null)
            {
                API.sendChatMessageToPlayer(player, coordsLine);
                counter++;
            }
            file.Close();
        }
    }
}
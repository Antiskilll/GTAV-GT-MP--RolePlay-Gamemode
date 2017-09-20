using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System.Collections.Generic;
using static LSRP_VFR.Items.Items;
using LSRP_VFR.Faction.EMS;
namespace LSRP_VFR.Items
{
    public class Defibrillator : Item
    {

        public Defibrillator(int id, string name, string description, int weight) : base(id, name, description, weight)
        {

        }
        public override void Use(Client c)
        {
            var players = API.shared.getPlayersInRadiusOfPlayer(5f, c);
            List<string> Actions = new List<string>();
            List<Client> ComaArray = new List<Client>();
            API.shared.setEntityData(c, "list", players);
            foreach (Client player in players)
            {
                if (Players.DeathManager.IsOnComaPlayer(player))
                {
                    Actions.Add(API.shared.getEntitySyncedData(player, "Nom_Prenom"));
                    ComaArray.Add(player);
                }
                
            }
            API.shared.setEntityData(c, "list", ComaArray);
            if (players.Count == 0) {
                API.shared.sendNotificationToPlayer(c, "Aucun blesse proche de vous.");
            } else {
                API.shared.triggerClientEvent(c, "bettermenuManager", 452, "Défibrillator", "", false, Actions);
            }

        }   
    }
}

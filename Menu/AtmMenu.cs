using GrandTheftMultiplayer.Server.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;

namespace LSRP_VFR.Menu
{
    class AtmMenu : Script
    {
        public AtmMenu()
        {
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        private void OnClientEventTrigger(Client sender, string eventName, object[] args)
        {
            if (eventName == "ATM")
            {
                List<string> Actions = new List<string>();
                Actions.Add("Solde Bancaire");
                Actions.Add("Retirer");
                Actions.Add("Dépôt");
                Actions.Add("Transfert d'argent");
                API.triggerClientEvent(sender, "bettermenuManager", 18, "ATM", "Sélectionnez une option:", false, Actions);
            }
        }
    }
}

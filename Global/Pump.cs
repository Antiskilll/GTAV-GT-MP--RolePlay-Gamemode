using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;

namespace LSRP_VFR.Global
{
    class Pump : Script
    {
        public Pump()
        {
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        private void OnClientEventTrigger(Client sender, string eventName, object[] arguments)
        {
            try
            {
                switch (eventName)
                {
                    case "USE_PUMP":
                        {
                            if (sender.vehicle != null) { API.sendNotificationToPlayer(sender, "~r~[ESSENCE]~s~Vous devez être à l'exterieur de votre véhicule");return; };
                            List <KeyValuePair<NetHandle, int>> vehs = new List<KeyValuePair<NetHandle, int>>();
                            List<string> Label = new List<string>();
                            List<String> Actions = new List<string>();
                            decimal price = API.getSetting<decimal>("EssencePrice");
                            foreach (var veh in API.getAllVehicles())
                            {
                                Vector3 vehPos = API.getEntityPosition(veh);
                                Vector3 pumpPos = (Vector3)arguments[0];
                                float distanceVehicleToPump = sender.position.DistanceTo(vehPos);
                                if (distanceVehicleToPump < 5f)
                                {
                                    decimal currentfuel = Convert.ToDecimal(API.getEntitySyncedData(veh, "VEHICLE_FUEL"));                   
                                    var calc = Decimal.ToInt32(currentfuel * price);
                                    int calculprice = ((API.getEntitySyncedData(veh, "VEHICLE_FUEL_MAX") - calc));
                                    vehs.Add(new KeyValuePair<NetHandle, int>(veh, calculprice));
                                    string plate = API.getEntitySyncedData(veh, "Plate");
                                    Actions.Add(plate);
                                    Label.Add("$ " + calculprice.ToString());

                                }
                            }
                            if (vehs.Count == 0) { API.sendNotificationToPlayer(sender, "~r~[ERREUR] ~s~Aucun véhicule n'est près de la pompe."); return; }
                            API.setEntityData(sender, "VehicleOfUsingShop", vehs);
                            API.triggerClientEvent(sender, "bettermenuManager", 30, "Pompe à essence", "Mettre le plein d'essence dans: ", false, Actions, Label);
                            break;
                        }
                    case "menu_handler_select_item":
                        if ((int)arguments[0] == 30)
                        {
                            List<KeyValuePair<NetHandle, int>> vehs = new List<KeyValuePair<NetHandle, int>>();
                            vehs = API.getEntityData(sender, "VehicleOfUsingShop");
                            API.resetEntityData(sender, "VehicleOfUsingShop");
                            KeyValuePair<NetHandle, int> veharray = vehs[(int)arguments[1]];
                            int price = veharray.Value;
                            if (Players.Money.TakeMoney(sender, price))
                            {
                                NetHandle veh = veharray.Key;
                                string plate = API.getEntitySyncedData(veh, "Plate");
                                API.setEntitySyncedData(veh, "VEHICLE_FUEL", 100);
                                API.sendNotificationToPlayer(sender, "~r~[ESSENCE]~s~ Vous avez fait le plein de votre véhicule ~r~" + plate + " ~s~pour la somme de $ ~r~" + price);
                            }
                            else
                            {
                                API.sendNotificationToPlayer(sender, "~r~[ESSENCE]~s~ Vous n'avez pas assez d'argent sur vous pour faire le plein de votre véhicule.");
                            }
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR] : ~s~" + e.ToString());
            }

        }
    }
}

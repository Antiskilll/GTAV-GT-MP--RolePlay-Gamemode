using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSRP_VFR.Shop
{
    class ScootRent: Script
    {
        private Vector3 PnjPos = new Vector3(-1009.382, -2683.575, 13.97357);
        private Vector3 PnjRot = new Vector3(0, 0, 147.56);
        private Vector3 SpawnPos = new Vector3(-1013.873,-2689.962,13.97614);
        private Vector3 SpawnRot = new Vector3(0, 0, 150.439);
        private Ped pnjrent;
        private Blip blip;
        private VehicleHash RentModel;
        private int Price;
        private List<Vehicle> vehicle = new List<Vehicle>();
        public ScootRent()
        {
            API.onResourceStart += OnResourceStart;
            API.onResourceStop += OnResourceStop;
            API.onPlayerDisconnected += API_onPlayerDisconnected;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        private void API_onPlayerDisconnected(Client player, string reason)
        {
            var vehiclescoot = vehicle.Find(x => x.getData("Owner") == player.socialClubName);
            if (vehiclescoot != null)
            {
                vehicle.Remove(vehiclescoot);
                API.deleteEntity(vehiclescoot);
            }
        }

        public static void OpenMenuScootRent(Client sender)
        {
            List<String> Actions = new List<string>();
            Actions.Add("Louer un scooter");
            API.shared.triggerClientEvent(sender, "bettermenuManager", 170, "Location de scooter", "Sélectionner votre véhicule: ", false, Actions);
        }

        private void OnClientEventTrigger(Client sender, string eventName, object[] arguments)
        {
            if (eventName == "menu_handler_select_item")
            {
                if ((int)arguments[0] == 170)
                {
                    List<String> Actions = new List<string>();
                    List<string> label = new List<string>();
                    Actions.Add("BMX:");
                    label.Add("100$");
                    Actions.Add("Faggio:");
                    label.Add("250$");
                    Actions.Add("Faggio2");
                    label.Add("350$");
                    Actions.Add("Faggio3");
                    label.Add("350$");
                    API.triggerClientEvent(sender, "bettermenuManager", 171, "Location de scooter", "Sélectionner votre véhicule: ", false, Actions, label);

                }
                else if ((int)arguments[0] == 171)
                {
                    switch ((int)arguments[1])
                    {
                        case 0:
                            RentModel = (VehicleHash)(int)1131912276; // BMX
                            Price = 100;
                            break;
                        case 1:
                            RentModel = (VehicleHash)(int)-1842748181; // FAGGIO
                            Price = 250;
                            break;
                        case 2:
                            RentModel = (VehicleHash)(int)55628203; // FAGGIO2
                            Price = 350;
                            break;
                        case 3:
                            RentModel = (VehicleHash)(int)-1289178744; // FAGGIO3
                            Price = 350;
                            break;
                    }
                    if (API.getEntitySyncedData(sender, "Money") <= Price)
                    {
                        API.triggerClientEvent(sender, "display_subtitle", "Vous n'avez pas assez d'argent sur vous!", 30000);
                    }else
                    {
                        API.setEntitySyncedData(sender, "Money", (API.getEntitySyncedData(sender, "Money") - Price));
                        Vehicle scooterRent = API.createVehicle(RentModel, SpawnPos, SpawnRot, 0, 0, 0);
                        vehicle.Add(scooterRent);
                        API.triggerClientEvent(sender, "display_subtitle", "Le magasin de location vous prête un véhicule\n ~s~pour une durée d'une heure.", 30000);

                        Items.Items.InventoryHolder ih = new Items.Items.InventoryHolder();
                        ih.Owner = scooterRent.handle;
                        API.shared.setEntityData(scooterRent, "InventoryHolder", ih);
                        API.setEntitySyncedData(scooterRent, "VEHICLE_FUEL", 100);
                        API.setEntitySyncedData(scooterRent, "VEHICLE_FUEL_MAX", 100);
                        API.setEntityData(scooterRent, "weight", 0);
                        API.setEntityData(scooterRent, "weight_max", 0);
                        API.delay(3600000, true, () => { DeleteThread(sender); });
                    }

                }
            }
        }

        private void DeleteThread(Client owner)
        {
            var vehiclescoot = vehicle.Find(x => x.getData("Owner") == owner.socialClubName);
            vehicle.Remove(vehiclescoot);
            API.deleteEntity(vehiclescoot);
            API.triggerClientEvent(owner, "display_subtitle", "Le magasin de location vous a reprit votre véhicule.", 30000);

        }


        private void OnResourceStart()
        {
            pnjrent = API.createPed((PedHash)(-984709238), PnjPos, 0f);
            API.setEntityRotation(pnjrent, PnjRot);
            API.setEntitySyncedData(pnjrent, "Interaction", "ScooterRent");
            blip = API.shared.createBlip(PnjPos);
            API.setBlipName(blip, "Location de scooter");
            blip.shortRange = true;
            blip.sprite = 512;
        }

        private void OnResourceStop()
        {
            API.deleteEntity(pnjrent);
            API.deleteEntity(blip);  
        }
    }
}

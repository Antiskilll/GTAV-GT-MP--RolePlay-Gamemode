using LSRP_VFR.Players;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using static LSRP_VFR.Mysql.DBPlayers;
using static LSRP_VFR.Mysql.DBVehicles;

namespace LSRP_VFR.Shop
{
    class VehicleShop : Script
    {
        private Blip blip;
        private Ped p;
        private XElement xelement = XElement.Load("paints.xml");

        public VehicleShop()
        {
            API.onResourceStart += OnResourceStart;
            API.onClientEventTrigger += OnClientEventTrigger;
            API.onResourceStop += OnResourceStop;
        }

        private void OnResourceStart()
        {
            //      NAME || VEHICLE TYPE || VECTOR3 X Y Z || HASH PNJ || ROTATION PNJ || VECTOR3 SPAWN VEHICLE
            var concess = new object[8, 11] {
                { "Compact Cars", 0, 187.5617, -1252.634, 29.19846, -261389155, 171, 192.0181, -1256.243, 29.20 , "Voiture"},
                { "Coupes", 3, -31.44708, -1106.933, 26.42236, -1022961931, -15, -52.80899, -1110.959, 26.43581 , "Voiture"},
                { "SUV", 2, -83.63953, 80.13905, 71.54692, -1022961931, 144, -97.80535, 92.29456, 71.84579 , "Voiture"},
                { "Muscle Cars", 4, -1134.78, -1984.87, 13.16, -261389155, 171, -1134.78, -1974.87, 13.16 , "Voiture"},
                { "Sports Cars", 6, -803.05, -226, 37.21, -1022961931, 171, -802, -232.4, 36.69 , "Voiture"},
                { "Supercars", 7, -806.43, -223, 37.21, -1022961931, 171, -811, -223.6, 37.13 , "Voiture"},
                { "Moto", 8, -69.62, -1829.72, 26.94, -261389155, 171, -64.46, -1832.45, 26.87 , "Moto"},
                { "Vans", 12, 460.4, -1987.7, 22.96, -261389155, 171, 453.4, -1984, 23.2, "Voiture"},
                //{ "Bateau", 14, -855.75, -1350.95, 1.6, -261389155, 171, -866.14, -1355.24, 0 , "Voiture"},
                //{ "Hélicoptère", 15, -729.1, -1434.3, 5, -1022961931, 171, -724.59, -1443.7, 6 , "Avion"},
                //{ "Avion", 16, -1121.1, -2454.1, 13.94, 416176080, 171, -1127.1, -2426.87, 13.94 , "Avion"},
                //{ "Camion", 20, 167.5898, 2773.553, 45.70288, -261389155, 171, 164.8515, 2754.782, 42.72427 , "Voiture"},
            };

            int[] doors = {
                API.exported.doormanager.registerDoor(1417577297, new Vector3(-37.33113, -1108.873, 26.7198)),
                API.exported.doormanager.registerDoor(2059227086, new Vector3(-39.13366, -1108.218, 26.7198)),
                API.exported.doormanager.registerDoor(1417577297, new Vector3(-60.54582, -1094.749, 26.88872)),
                API.exported.doormanager.registerDoor(2059227086, new Vector3(-59.89302, -1092.952, 26.88362)),
                API.exported.doormanager.registerDoor(-2051651622, new Vector3(-33.80989, -1107.579, 26.57225)),
                API.exported.doormanager.registerDoor(-2051651622, new Vector3(-31.72353, -1101.847, 26.57225)),
            };
            foreach (int door in doors)
            {
                API.exported.doormanager.setDoorState(door, false, 0);
            }

            for (int i = 0; i < concess.GetLength(0); i++)
            {
                string name = (string)concess[i, 0];
                int vclass = (int)concess[i, 1];
                double x = Convert.ToDouble(concess[i, 2]);
                double y = Convert.ToDouble(concess[i, 3]);
                double z = Convert.ToDouble(concess[i, 4]);
                int modelhash = (int)concess[i, 5];
                float pedheading = (float)(int)concess[i, 6];
                double cx = Convert.ToDouble(concess[i, 7]);
                double cy = Convert.ToDouble(concess[i, 8]);
                double cz = Convert.ToDouble(concess[i, 9]);
                string licence = Convert.ToString(concess[i, 10]);

                p = API.createPed((PedHash)modelhash, new Vector3(x, y, z), pedheading, 0);
                API.setEntityData(p, "cX", cx);
                API.setEntityData(p, "cY", cy);
                API.setEntityData(p, "cZ", cz);

                API.setEntitySyncedData(p, "Interaction", "Concess");
                API.setEntityData(p, "Concess", name);
                API.setEntityData(p, "Licence", licence);
                blip = API.createBlip(new Vector3(x, y, z));
                blip.shortRange = true;
                API.setBlipSprite(blip, 225);
                API.setBlipName(blip, name);
                switch (vclass)
                {
                    case 2:
                        API.setBlipSprite(blip, 67);
                        break;
                    case 8:
                        API.setBlipSprite(blip, 226);
                        break;
                    case 15:
                        API.setBlipSprite(blip, 43);
                        break;
                    case 16:
                        API.setBlipSprite(blip, 251);
                        break;
                    case 20:
                        API.setBlipSprite(blip, 198);
                        break;
                    case 18:
                        API.setBlipTransparency(blip, 255);
                        break;
                    default:
                        break;
                }
            }
        }

        public static void OpenMenuConcess(Client sender, NetHandle pnj)
        {
            if (!CheckLicenceShop(sender, (string)API.shared.getEntityData(pnj, "Licence")))
            {
                API.shared.sendNotificationToPlayer(sender, "~r~[SHOP]~s~ Vous n'avez pas la licence requise.");
                return;
            }
            if (File.Exists("Vehicles.xml"))
            {
                var licence = API.shared.getEntityData(sender, "pnj");
                List<string> Actions = new List<string>();
                List<string> label = new List<string>();
                List<KeyValuePair<VehicleHash, int>> Products = new List<KeyValuePair<VehicleHash, int>>();
                string vclass = API.shared.getEntityData(pnj, "Concess");
                API.shared.setEntityData(sender, "pnj", pnj);

                DataTable result = Mysql.DBVehicles.GetConcessVehicles(vclass);
                if (result.Rows.Count != 0)
                {
                    foreach (DataRow row in result.Rows)
                    {
                        string name2 = Convert.ToString(row["name"]);
                        VehicleHash hash = (VehicleHash)Convert.ToInt32(row["vehiclehash"]);
                        string name = API.shared.getVehicleDisplayName(hash);
                        int price = Convert.ToInt32((row["price"]));
                        Products.Add(new KeyValuePair<VehicleHash, int>(hash, price));
                        Actions.Add(name);
                        label.Add("Prix: " + price.ToString() + "$");
                    }
                }

                API.shared.setEntityData(sender, "ProductsOfUsingShop", Products);
                API.shared.triggerClientEvent(sender, "bettermenuManager", 190, "Concessionnaire", "Sélectionner une voiture:", false, Actions, label);

            }
        }

        private void OnClientEventTrigger(Client sender, string eventName, object[] arguments)
        {
            if (eventName == "menu_handler_select_item")
            {
                if ((int)arguments[0] == 190)
                {
                    API.consoleOutput(arguments[1].ToString());
                    API.setEntityData(sender, "VehicleOfUsingShop", (int)arguments[1]);
                    List<string> Actions = new List<string>();
                    if (File.Exists("paints.xml"))
                    {     
                        IEnumerable<XElement> Paints = xelement.Elements();
                        foreach (var p in Paints)
                        {
                            Actions.Add(p.Attribute("Name").Value.ToString());
                        }
                    }
                    API.triggerClientEvent(sender, "bettermenuManager", 191, "Concessionnaire", "Sélectionner une peinture:", false, Actions);
                }
                else if ((int)arguments[0] == 191)
                {
                    var pnj = API.getEntityData(sender, "pnj");
                    double cx = API.getEntityData(pnj, "cX");
                    double cy = API.getEntityData(pnj, "cY");
                    double cz = API.getEntityData(pnj, "cZ");

                    List<KeyValuePair<VehicleHash, int>> Products = new List<KeyValuePair<VehicleHash, int>>();
                    Products = API.getEntityData(sender, "ProductsOfUsingShop");
                    int VehicleChoise = API.getEntityData(sender, "VehicleOfUsingShop");
                    API.resetEntityData(sender, "ProductsOfUsingShop");
                    API.resetEntityData(sender, "VehicleOfUsingShop");
                    KeyValuePair<VehicleHash, int> veharray = Products[(int)VehicleChoise];
                    VehicleHash model = veharray.Key;
                    int price = veharray.Value;
                    API.consoleOutput(model.ToString());
                    if ((int)model == 0) {API.sendNotificationToPlayer(sender, "~r~[ERREUR] ~s~Le véhicule que vous demandez n'est pas disponible."); return;  }
                    if (Money.TakeMoney(sender, price))
                    {      
                        Vehicle car2 = API.createVehicle(model, new Vector3(cx, cy, cz), new Vector3(0, 0, 20), 111, 111);
                        string plate = Vehicles.Vehicle.RandomPlate();
                        API.setVehicleNumberPlate(car2, plate);
                        API.setEntitySyncedData(car2, "Owner", sender.socialClubName);
                        API.setEntitySyncedData(car2, "OwnerName", sender.getSyncedData("Nom_Prenom"));
                        API.setEntitySyncedData(car2, "Plate", plate.ToString());
                        Items.Items.InventoryHolder ih = new Items.Items.InventoryHolder();
                        ih.Owner = car2.handle;
                        API.setEntityData(car2, "InventoryHolder", ih);
                        API.setEntitySyncedData(car2, "VEHICLE_FUEL", 100);
                        API.setEntitySyncedData(car2, "VEHICLE_FUEL_MAX", 100);
                        API.setEntityData(car2, "weight", 0);
                        API.setEntityData(car2, "weight_max", Vehicles.Vehicle.GetVehicleWeight(model));
                        API.setVehicleLocked(car2, true);
                        API.setEntitySyncedData(car2, "Locked", true);
                        string paint = xelement.Descendants("Vehicle").ElementAt((int)arguments[1]).Attribute("Color").Value;
                        int color = Convert.ToInt32(paint);
                        API.setVehiclePrimaryColor(car2, color);
                        InsertVehicle(sender, model, plate, new Vector3(cx, cy, cz), color);
                        UpdatePlayerMoney(sender);
                        API.setVehicleEngineStatus(car2, false);
                        API.triggerClientEvent(sender, "display_subtitle", "Vendu! Voici les clefs de votre véhicule.", 3000);
                    }
                    else
                    {
                        API.triggerClientEvent(sender, "display_subtitle", "Désolé, vous n'avez pas assez d'argent sur vous!", 3000);
                    }
                }
            }
        }

        public static bool CheckLicenceShop(Client player,  string licence)
        {
            switch (licence)
            {
                case "Voiture":
                    return (bool)API.shared.getEntityData(player, "P_Voiture");
                case "Camion":
                    return (bool)API.shared.getEntityData(player, "P_Camion");
                case "Moto":
                    return(bool)API.shared.getEntityData(player, "P_Moto");
                case "Avion":
                    return (bool)API.shared.getEntityData(player, "P_Voiture");
                default:
                    API.shared.sendNotificationToPlayer(player, "ERREUR");
                    return false;
            }
        }

        private void OnResourceStop()
        {
            API.deleteEntity(p);
            API.deleteEntity(blip);
        }
    }
}

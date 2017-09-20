using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Server.Elements;
using static AdAstraRP.Mysql.DBPlayers;
using static AdAstraRP.Mysql.DBHousing;
using System.Data;
using static AdAstraRP.Items.Items;
using AdAstraRP.Players;

namespace AdAstraRP.Housing
{
    class InitHousing : Script
    {
        private static List<ColShape> OutcolshapeList = new List<ColShape>();
        private static List<ColShape> IntcolshapeList = new List<ColShape>();
        private static List<ColShape> BoxcolshapeList = new List<ColShape>();
        private static List<Client> HouseOwnerList = new List<Client>();
        private static List<NetHandle> HandleList = new List<NetHandle>();

        public InitHousing()
        {
            API.onResourceStart += OnResourceStart;
            API.onEntityEnterColShape += OnEntityEnterColShape;
            API.onEntityExitColShape += OnEntityExitColShape;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        private void OnResourceStart()
        {
            API.consoleOutput("[HOUSING] Créations des maisons.");
            // ID     || This is ID, okay?         || Integrer
            // OutPos || Marker position exterior  || Vector3
            // IntPos || Marker position intern    || Vector3
            // BoxPos || Inventory position        || Vector3
            // Place  || Number Housing place      || Integrer
            // Price  || Price you need a drawing? || Integrer
            var housing = new object[20, 6] {
                // TODO : Mettre les coordonnees du 3 eme vecteur
                {1, new Vector3(-974.1464, -1433.113, 7.679172), new Vector3(346.5235, -1002.901, -99.1962), new Vector3(351.1463, -998.3478, -99.19627), 10, 150}, /* ? */
                {2, new Vector3(-1439.59, -550.6906, 34.7418), new Vector3(-1460.366, -522.0636, 56.929), new Vector3(-1460.366, -522.0636, 56.929), 10, 150}, /* ? */
                {3, new Vector3(-773.6732, 313.0458, 85.69814), new Vector3(-780.152, 340.443, 207.621), new Vector3(-1460.366, -522.0636, 56.929), 10, 150}, /* ? */
                {4, new Vector3(1665.579, 4776.712, 41.93869), new Vector3(265.3285, -1002.704, -99.0085), new Vector3(259.428, -1003.992, -99.00858), 10, 150},
                {5, new Vector3(-688.8965, 598.6945, 143.5084), new Vector3(-680.1067, 590.6495, 145.393), new Vector3(-680.0543, 588.8796, 137.7697), 10, 150},
                {6, new Vector3(-751.1387, 621.1008, 142.2527), new Vector3(-761.0836, 617.9774, 144.1539), new Vector3(-762.1716, 618.7916, 136.5306), 10, 150},
                {7, new Vector3(-853.2899, 698.7006, 148.7756), new Vector3(-859.5645, 688.7182, 152.8571), new Vector3(-858.3636, 697.5562, 145.253), 10, 150},
                {8, new Vector3(-1294.228, 456.4709, 97.0794), new Vector3(-1289.639, 446.7739, 97.8989), new Vector3(-1287.93, 455.9638, 90.29469), 10, 150},
                {9, new Vector3(-558.0556, 666.2042, 145.1311), new Vector3(-572.4428, 658.958, 145.8364), new Vector3(-568.6632, 667.1772, 138.2321), 10, 150},
                {10, new Vector3(349.893, 442.8174, 147.3472), new Vector3(340.6531, 436.7456, 149.394), new Vector3(337.8813, 436.6778, 141.7708), 10, 150},
                {11, new Vector3(371.9392, 430.4312, 145.1107), new Vector3(373.2864, 420.6612, 145.9045), new Vector3(376.511, 429.0471, 138.3001), 10, 150},
                {12, new Vector3(-305.5824, 6330.911, 32.48935), new Vector3(346.5235, -1002.901, -99.1962), new Vector3(351.3212, -993.597, -99.19617), 10, 150},
                {13, new Vector3(-933.4771, -383.6144, 38.9613), new Vector3(-913.1502, -384.5727, 85.4804), new Vector3(-915.6313, -375.6594, 85.48913), 10, 150},
                {14, new Vector3(-617.9388, 35.7848, 43.5558), new Vector3(-598.9042, 41.8059, 93.6261), new Vector3(-596.8677, 50.22554, 93.63481), 10, 150},
                {15, new Vector3(-914.3189, -455.2902, 39.5998), new Vector3(-900.6082, -431.0182, 121.607), new Vector3(-898.0314, -440.2217, 121.6154), 10, 150},
                {16, new Vector3(118.8673, 567.283, 183.1295), new Vector3(117.5057, 557.3167,184.3022), new Vector3(118.3505, 566.1226, 176.6971), 10, 150},
                {17, new Vector3(-177.3793, 503.8313, 136.8531), new Vector3(-173.286, 495.0179, 137.667), new Vector3(-174.5556, 493.8842, 130.0435), 10, 150},
                {18, new Vector3(1901.745, 3783.513, 32.79797), new Vector3(265.3285, -1002.704, -99.0085), new Vector3(259.4277, -1003.99, -99.00865), 10, 150},
                {19, new Vector3(-258.1236, -969.0657, 31.2199), new Vector3(-281.0908, -943.2817, 92.5108), new Vector3(-273.4648, -947.9965, 92.51958), 10, 150},
                {20, new Vector3(-49.3243, -583.1716, 37.0333), new Vector3(-21.0966, -580.4884, 90.1148), new Vector3(-25.66122, -588.0934, 90.12351), 10, 150}
            };
            int h = housing.GetLength(0);
            for (int i = 0; i < h; i++)
            {
                int ID = (int)housing[i, 0];
                Vector3 OutPos = (Vector3)housing[i, 1];
                Vector3 IntPos = (Vector3)housing[i, 2];
                Vector3 BoxPos = (Vector3)housing[i, 3];
                int Place = (int)housing[i, 4];
                Blip blip = API.createBlip(OutPos);
                //API.setBlipColor(blip, blipColor);
                API.setBlipShortRange(blip, true);
                API.setBlipSprite(blip, 40);
                API.setBlipName(blip, ID.ToString());
                API.createMarker(1, OutPos - new Vector3(0, 0, 1f), new Vector3(), new Vector3(), new Vector3(1, 1, 1), 255, 255, 255, 255, 0);
                CylinderColShape Outcolshape = API.createCylinderColShape(OutPos, 1f, 1f);

                API.createMarker(1, IntPos - new Vector3(0, 0, 1f), new Vector3(), new Vector3(), new Vector3(1, 1, 1), 255, 255, 255, 255, 0);
                CylinderColShape Intcolshape = API.createCylinderColShape(IntPos, 1f, 1f);

                API.createMarker(1, BoxPos - new Vector3(0, 0, 1f), new Vector3(), new Vector3(), new Vector3(1, 1, 1), 255, 255, 255, 255, 0);
                CylinderColShape Boxcolshape = API.createCylinderColShape(BoxPos, 1f, 1f);

                int Price = (int)housing[i, 5];
                Outcolshape.setData("Housing_ID", ID);
                Outcolshape.setData("Housing_Place", Place);
                Outcolshape.setData("Housing_Price", Price);
                Outcolshape.setData("Housing_OutPos", OutPos);
                Outcolshape.setData("Housing_IntPos", IntPos);
                OutcolshapeList.Add(Outcolshape);

                Intcolshape.setData("Housing_ID", ID);
                Intcolshape.setData("Housing_OutPos", OutPos);
                IntcolshapeList.Add(Intcolshape);

                Boxcolshape.setData("Housing_ID", ID);
                BoxcolshapeList.Add(Boxcolshape);
            }
        }


        private static void OnEntityEnterColShape(ColShape colshape, NetHandle entity)
        {
            if (entity == null) return;
            Client player = API.shared.getPlayerFromHandle(entity);
            if (player == null) return;
            if (colshape.hasData("Housing_ID") && OutcolshapeList.Contains(colshape))
            {
                int ID    = colshape.getData("Housing_ID");
                int Place = colshape.getData("Housing_Place");
                int Price = colshape.getData("Housing_Price");
                player.setData("Housing_ID", ID);
                List<string> Actions = new List<string>();
                List<string> label = new List<string>();
                Actions.Add("Acheter");
                label.Add("$"+Price.ToString());
                Actions.Add("Vendre");
                label.Add("$" + CalculeSellHousePrice(Price).ToString());
                DataTable HouseOwner = GetAllHousePlayerInID(ID);
                if (HouseOwner.Rows.Count != 0)
                {
                    foreach (DataRow row in HouseOwner.Rows)
                    {
                        string ownerDB = row["Owner"].ToString();
                        Client owner = EntityManager.GetClient(ownerDB);

                        if (EntityManager.GetClientList().Contains(owner))
                        {
                            HouseOwnerList.Add(owner);
                            if (owner == player)
                            {
                                Actions.Add("Rentrer chez vous");
                            }
                            else
                            {
                                Actions.Add(owner.getSyncedData("Nom_Prenom"));
                            }

                            label.Add("");
                        }

                    }
                }
                API.shared.triggerClientEvent(player, "bettermenuManager", 50, "Immobilier", "Menu Immobilier: ", false, Actions, label);

            }
            else if (colshape.hasData("Housing_ID") && IntcolshapeList.Contains(colshape))
            {
                int ID = colshape.getData("Housing_ID");
                player.setData("Housing_ID", ID);
                List<string> Actions = new List<string>();
                Actions.Add("Sortir");
                API.shared.triggerClientEvent(player, "bettermenuManager", 52, "Immobilier", "Menu Immobilier: ", false, Actions);

            }
            else if (colshape.hasData("Housing_ID") && BoxcolshapeList.Contains(colshape))
            {
                //int ID = colshape.getData("Housing_ID");
                //player.setData("Housing_ID", ID);
                List<string> Actions = new List<string>();
                Actions.Add("Ouvrir l'inventaire");
                API.shared.triggerClientEvent(player, "bettermenuManager", 53, "Immobilier", "Menu Immobilier: ", false, Actions);

            }
        }

        private void OnClientEventTrigger(Client sender, string eventName, object[] arguments)
        {
            if (eventName == "menu_handler_select_item")
            {
                if ((int)arguments[0] == 50) {
                    int ID = sender.getData("Housing_ID");
                    //sender.resetData("Housing_ID"); // A reset seulement quand la personne quitte l'appart
                    var HousingColshape = OutcolshapeList.Find(x => x.getData("Housing_ID") == ID);
                    int Place = HousingColshape.getData("Housing_Place");
                    int Price = HousingColshape.getData("Housing_Price");

                    /****************************************************************************/
                    /*                                  ACHETER                                 */
                    /****************************************************************************/

                    if ((int)arguments[1] == 0)
                    {
                        if (CheckNumberRentHousing(ID, Place))
                        {
                            if (GetHousePlayer(ID, sender.socialClubName).Rows.Count != 0)
                            {
                                API.sendNotificationToPlayer(sender, "Vous avez déjà une maison / appartement ici.");
                            }
                            else
                            {
                                if (Money.TakeBankMoney(sender, Price))
                                {
                                    AddHouseOwner(sender, ID, RandomHouseDimension());
                                    API.sendNotificationToPlayer(sender, "Vous avez acheté une maison / appartement pour la somme de $" + Price.ToString());
                                } else
                                {
                                    API.sendNotificationToPlayer(sender, "Vous n'avez pas assez d'argent sur votre compte en banque.");
                                }
                            }
                        }
                        else
                        {
                            API.sendChatMessageToPlayer(sender, "Vous n'avez pas de maison / appartement de disponible ici.");
                        }
                    }
                    /****************************************************************************/
                    /*                                  VENDRE                                  */
                    /****************************************************************************/
                    else if ((int)arguments[1] == 1)
                    {
                        int SellPrice = CalculeSellHousePrice(Price);
                        if (GetHousePlayer(ID, sender.socialClubName).Rows.Count != 0)
                        {
                            SellHouseOwner(ID, sender.socialClubName);
                            Money.GiveMoney(sender, SellPrice);
                            API.sendNotificationToPlayer(sender, "Vous avez vendu une maison / appartement pour la somme de $" + SellPrice.ToString());
                        }
                        else
                        {
                            API.sendNotificationToPlayer(sender, "Vous n'avez pas de maison / appartement à vendre ici.");
                        }
                    }
                    else
                    {
                        /****************************************************************************/
                        /*                                  DEMANDE                                 */
                        /****************************************************************************/

                        Client HouseOwner = HouseOwnerList[(int)arguments[1] - 2];
                        if (HouseOwner == sender)
                        {
                            PutInMyHome(sender);
                        } else {
                            if (HouseOwner.hasData("InHouse") && HouseOwner.getData("InHouse") == ID)
                            {
                                HouseOwner.setData("Housing_Wait_Enter", sender.getSyncedData("Nom_Prenom"));
                                List<string> Actions = new List<string>();
                                Actions.Add("Ouvrir la porte");
                                Actions.Add("Refuser d'ouvrir");
                                API.triggerClientEvent(HouseOwner, "bettermenuManager", 51, "Immobilier", sender.getSyncedData("Nom_Prenom") + " Sonne à la porte", false, Actions);
                            } else
                            {
                                API.sendNotificationToPlayer(sender, HouseOwner.getSyncedData("Nom_Prenom") + " n'est pas chez lui.");
                            }
                        }
                    }
                }
                /****************************************************************************/
                /*                                  RENTRER                                 */
                /****************************************************************************/
                else if ((int)arguments[0] == 51)
                {
                    string GetEnterClientName = sender.getData("Housing_Wait_Enter");
                    Client GetEnterClient = EntityManager.GetClientName(GetEnterClientName);
                    int ID = sender.getData("Housing_ID");
                    var HousingColshape = OutcolshapeList.Find(x => x.getData("Housing_ID") == ID);
                    

                    if ((int)arguments[1] == 0)
                    {
                        Vector3 IntPos = (Vector3)HousingColshape.getData("Housing_IntPos");
                        DataTable houseplayerinfotable = GetHousePlayer(ID, sender.socialClubName);
                        if (houseplayerinfotable.Rows.Count != 0)
                        {
                            foreach (DataRow row in houseplayerinfotable.Rows)
                            {
                                GetEnterClient.dimension = Convert.ToInt32(row["Dimension"]);
                                GetEnterClient.setData("Housing_Owner", sender.socialClubName);
                            }
                        }

                        GetEnterClient.position = IntPos;
                        GetEnterClient.setData("Housing_ID", ID);
                    }
                    else if ((int)arguments[1] == 1)
                    {
                        API.sendNotificationToPlayer(GetEnterClient, "Personne n'ouvre la porte.");
                    }
                }
                /****************************************************************************/
                /*                                  SORTIR                                  */
                /****************************************************************************/
                else if ((int)arguments[0] == 52)
                {
                    OnExitHouse(sender);
                }
                /****************************************************************************/
                /*                                  INVENTAIRE                              */
                /****************************************************************************/
                // OUVERTURE DU MENU
                else if ((int)arguments[0] == 53)
                {
                    OpenHouseInventory(sender);
                }
                // CHOIX DE LA QUANTITE
                else if ((int)arguments[0] == 54)
                {
                    InventoryHolder ih = API.getEntityData(sender, "Home_InventoryHolder");
                    var item = ih.Inventory[(int)arguments[1]];
                    API.setEntityData(sender, "LastSelectedItem", item);
                    API.shared.triggerClientEvent(sender, "get_user_input", 55, "", 3, null);
                }
            }
            else if (eventName == "menu_handler_user_input")
            { 
                // COMFIRMATION
                if ((int)arguments[0] == 55)
                {
                    InventoryItem item = (InventoryItem)API.getEntityData(sender, "LastSelectedItem");
                    int qty = Convert.ToInt32(arguments[1]);
                    Items.Items.InventoryHolder invHome = API.getEntityData(sender, "Home_InventoryHolder");
                    Items.Items.InventoryHolder invreciever = API.getEntityData(sender, "InventoryHolder");
                    API.consoleOutput(qty.ToString());
                    int itemplayerqty = item.Quantity;
                    if (qty <= itemplayerqty)
                    {
                        if (invreciever.CheckWeightInventory(item.Details, qty))
                        {
                            invHome.RemoveItemFromInventory(item.Details, qty);
                            invreciever.AddItemToInventory(item.Details, qty);
                            API.sendNotificationToPlayer(sender, "Vous avez récupéré " + qty.ToString() + " " + item.Details.Name + " de votre coffre.");
                            UpdatePlayerInfo(sender);

                            string OwnerHouseName = sender.getData("Housing_Owner"); // check à qui appartient l'appartement
                            int ID = sender.getData("Housing_ID"); // check de l'id de l'appartement
                            SaveHouseInventory(sender, ID, OwnerHouseName);

                            API.resetEntityData(sender, "LastSelectedItem");
                            API.resetEntityData(sender, "NearbyList");
                            API.resetEntityData(sender, "Home_InventoryHolder");
                        }
                        else
                        {

                            API.sendNotificationToPlayer(sender, "Vous n'avez pas la place dans votre inventaire!");
                        }
                    }
                    else
                    {
                        API.sendNotificationToPlayer(sender, "Vous en avez pas autant sur vous");
                    }
                }

            }
        }

        private void OnEntityExitColShape(ColShape colshape, NetHandle entity)
        {
            var player = API.getPlayerFromHandle(entity);
            if (player == null) return;
            API.triggerClientEvent(player, "menu_handler_close_menu");
        }

        private static int CalculeSellHousePrice(int Price)
        {
            decimal Demultiplicateur = (decimal)1.5;
            return (int)Math.Round((double)(Price / Demultiplicateur),2);
        }

        private static int RandomHouseDimension()
        {
            try
            {
                Random rnd = new Random();
                int resultat = rnd.Next(99999);
                if (resultat.Equals(GetAllHouseDimension(resultat)))
                {
                    RandomHouseDimension();
                }
                return resultat;
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][HOUSE] : ~s~" + e.ToString());
                return 0;
            }
        }

        private void PutInMyHome(Client player)
        {
            int ID = player.getData("Housing_ID");
            var HousingColshape = OutcolshapeList.Find(x => x.getData("Housing_ID") == ID);
            Vector3 IntPos = (Vector3)HousingColshape.getData("Housing_IntPos");
            DataTable houseplayerinfotable = GetHousePlayer(ID, player.socialClubName);
            if (houseplayerinfotable.Rows.Count != 0)
            {
                foreach (DataRow row in houseplayerinfotable.Rows)
                {
                    player.dimension = Convert.ToInt32(row["Dimension"]);
                }
            }
            player.setData("Housing_Owner", player.socialClubName);
            player.position = IntPos;
            player.setData("InHouse", ID);
            
        }

        private void OpenHouseInventory(Client sender)
        {
            NetHandle House = API.createObject((int)-53650680, new Vector3(0, 0, 0), new Vector3(0, 0, 0), 10);
            HandleList.Add(House);
            InventoryHolder HouseStock = new InventoryHolder();
            HouseStock.Owner = House;
            API.setEntityData(House, "weight_max", 100);
            API.setEntityData(House, "weight", 0);
            API.setEntityData(sender, "HouseObject", House);
            string OwnerHouseName = sender.getData("Housing_Owner"); // check à qui appartient l'appartement
            int ID = sender.getData("Housing_ID"); // check de l'id de l'appartement
 
            //Client OwnerHouseClient = EntityManager.GetClientName(OwnerHouseName);
            var result = GetHouseInventory(ID, OwnerHouseName);
            List<String> Actions = new List<string>();
            if (result.Rows.Count != 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    if (!((row["Inventory"]).Equals("[]")))
                    {
                        var inventairerow = Convert.ToString(row["inventory"]);
                        var inventaire = inventairerow.Split(new[] { "],[" }, StringSplitOptions.None);
                        foreach (var I in inventaire)
                        {
                            var I2 = I.ToString().Replace("[", "").Replace("]", "");
                            var I3 = I2.Split(new[] { "," }, StringSplitOptions.None);
                            Item item = ItemByID(Convert.ToInt16(I3[0]));
                            int ItemQty = Convert.ToInt16(I3[1]);
                            Actions.Add(item.Name + " :  " + ItemQty);
                            HouseStock.AddItemToInventory(item, ItemQty);
                        }
                    }
                }
                API.triggerClientEvent(sender, "bettermenuManager", 54, "Inventaire Maison", "Selectionner l'item : ", false, Actions);

            }
            API.setEntityData(sender, "Home_InventoryHolder", HouseStock);
        }

        private static void OnExitHouse(Client sender)
        {
            if (sender.hasData("HouseObject"))
            {
                var obj = API.shared.getEntityData(sender, "HouseObject");
                NetHandle HouseObject = HandleList.Find(x => x.Equals(obj));
                API.shared.deleteEntity(HouseObject);
                API.shared.resetEntityData(sender, "HouseObject");
            }


            int ID = sender.getData("Housing_ID");
            var HousingColshape = IntcolshapeList.Find(x => x.getData("Housing_ID") == ID);
            Vector3 Outpos = (Vector3)HousingColshape.getData("Housing_OutPos");
            if (sender.hasData("InHouse")) sender.resetData("InHouse");
            if (sender.hasData("Housing_Wait_Enter")) sender.resetData("Housing_Wait_Enter");
            if (sender.hasData("Housing_Owner")) sender.resetData("Housing_Owner");
            sender.position = Outpos;
            sender.dimension = 0;
        }
    }
}

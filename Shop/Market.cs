using LSRP_VFR.Players;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Constant;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using static LSRP_VFR.Mysql.DBPlayers;
using static LSRP_VFR.Items.Items;

namespace LSRP_VFR.Shop
{
    class Market : Script
    {
        private Ped pnj;
        private Blip blip;
        private static List<KeyValuePair<int, int>> products = new List<KeyValuePair<int, int>>();
        public Market()
        {
            API.onResourceStart += OnResourceStart;
            API.onResourceStop += OnResourceStop;
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        private void OnResourceStart()
        {
            products.Add(new KeyValuePair<int, int>(ITEM_ID_SPRUNK, 100));
            products.Add(new KeyValuePair<int, int>(ITEM_ID_ECOLA, 100));
            products.Add(new KeyValuePair<int, int>(ITEM_ID_EWATER, 75));
            products.Add(new KeyValuePair<int, int>(ITEM_ID_EATCHICKEN, 150));

            List<KeyValuePair<Vector3, Vector3 >> marketpos = new List<KeyValuePair<Vector3, Vector3>>();
            marketpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(-46.313, -1757.504, 29.42), new Vector3(0, 0, 46)));
            marketpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(24.376, -1345.558, 29.421), new Vector3(0, 0, 267)));
            marketpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(1134.182, -982.477, 46.416), new Vector3(0, 0, 275)));
            marketpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(373.015, 328.332, 103.566), new Vector3(0, 0, 257)));
            marketpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(2676.389, 3280.362, 55.241), new Vector3(0, 0, 332)));
            marketpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(1958.960, 3741.979, 32.344), new Vector3(0, 0, 303)));
            marketpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(-2966.391, 391.324, 15.043), new Vector3(0, 0, 88)));
            marketpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(-1698.542, 4922.583, 42.064), new Vector3(0, 0, 324))); 
            marketpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(1164.565, -322.121, 69.205), new Vector3(0, 0, 100)));
            marketpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(-1486.530, -377.768, 40.163), new Vector3(0, 0, 147)));
            marketpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(-1221.568, -908.121, 12.326), new Vector3(0, 0, 31)));
            marketpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(-706.153, -913.464, 19.216), new Vector3(0, 0, 82)));
            marketpos.Add(new KeyValuePair<Vector3, Vector3>(new Vector3(1728.543, 6416.813, 35.03722), new Vector3(0, 0, -137)));

            foreach (KeyValuePair<Vector3, Vector3> keyValue in marketpos)
            {
                Vector3 pos = keyValue.Key;
                Vector3 rot = keyValue.Value;

                pnj = API.createPed((PedHash)416176080, pos, 1, 0);
                API.setEntityRotation(pnj, rot);
                API.setEntitySyncedData(pnj, "Interaction", "Market");
                API.playPedScenario(pnj, "PROP_HUMAN_BUM_SHOPPING_CART"); 
                blip = API.shared.createBlip(pos);
                blip.shortRange = true;
                API.shared.setBlipSprite(blip, 52);
                API.setBlipName(blip, "Market");
            }
        }

        public static void OpenMenuMarket(Client sender)
        {
            try
            {
                List<string> Actions = new List<string>();
                List<string> label = new List<string>();
                foreach (KeyValuePair<int, int> entry in products)
                {
                    Item item = ItemByID(entry.Key);
                    Actions.Add(item.Name);
                    label.Add("Prix: $" + entry.Value);
                }
                API.shared.triggerClientEvent(sender, "bettermenuManager", 182, "Supérette", "Sélectionner un item:", false, Actions, label);
                API.shared.setEntityData(sender, "ProductsOfUsingShop", products);
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][OpenMenuMarket] : ~s~" + e.ToString());
            }
        }

        private void OnClientEventTrigger(Client sender, string eventName, object[] arguments)
        {
            try
            {

                if (eventName == "menu_handler_select_item")
                {
                    if ((int)arguments[0] == 182)
                    {
                        if (arguments[1] == null) return;
                        var Products = API.getEntityData(sender, "ProductsOfUsingShop");
                        var item = ItemByID(Products[(int)arguments[1]].Key);
                        var price = Products[(int)arguments[1]].Value;
                        API.resetEntityData(sender, "ProductsOfUsingShop");
                        InventoryHolder ih = API.getEntityData(sender, "InventoryHolder");
                        if (ih.CheckWeightInventory(item, 1))
                        {
                            if (Money.TakeMoney(sender, price))
                            {
                                ih.AddItemToInventory(item, 1);
                                UpdatePlayerMoney(sender);
                                API.triggerClientEvent(sender, "display_subtitle", "Item ajouté à votre inventaire", 3000);
                            }
                            else
                            {
                                API.triggerClientEvent(sender, "display_subtitle", "Désolé, vous n'avez pas assez d'argent", 3000);
                            }
                        }
                        else
                        {
                            API.triggerClientEvent(sender, "display_subtitle", "Désolé, vous n'avez pas assez de place dans votre inventaire.", 3000);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR][OnClientEventTrigger] : ~s~" + e.ToString());
            }
        }

        private void OnResourceStop()
        {
            API.deleteEntity(pnj);
            API.deleteEntity(blip);
        }
    }
}

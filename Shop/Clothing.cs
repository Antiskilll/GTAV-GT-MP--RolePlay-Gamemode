using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared;
using GrandTheftMultiplayer.Shared.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSRP_VFR.Shop
{
    public class Clothing : Script
    {
        public Clothing()
        {
            ClothingShop();
            API.onClientEventTrigger += OnClientEventTrigger;
        }

        private void OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if(eventName == "changecloth")
            {
                /*
                 * Outils de test
                API.consoleOutput("****Base****");
                API.consoleOutput(sender.getData("Pants").ToString() + " " + sender.getData("Chemise").ToString() + " " + sender.getData("Survet").ToString() + " " + sender.getData("Chaussures").ToString() + " " + sender.getData("Bras").ToString());
                API.consoleOutput("****Selection****");
                API.consoleOutput(Convert.ToInt32(arguments[0]) + " " + Convert.ToInt32(arguments[1]) + " " + Convert.ToInt32(arguments[2]) + " " + Convert.ToInt32(arguments[3]) + " " + Convert.ToInt32(arguments[4]));
                */
                sender.setData("Pants",((int)Convert.ToInt32(arguments[0]) != -1) ? Convert.ToInt32(arguments[0]) : sender.getData("Pants"));
                sender.setData("Chemise",((int)Convert.ToInt32(arguments[1]) != -1) ? Convert.ToInt32(arguments[1]) : sender.getData("Chemise"));
                sender.setData("Survet",((int)Convert.ToInt32(arguments[2]) != -1) ? Convert.ToInt32(arguments[2]) : sender.getData("Survet"));
                sender.setData("Chaussures",((int)Convert.ToInt32(arguments[3]) != -1) ? Convert.ToInt32(arguments[3]) : sender.getData("Chaussures"));
                sender.setData("Bras",((int)Convert.ToInt32(arguments[4]) != -1) ? Convert.ToInt32(arguments[4]) : sender.getData("Bras"));
                API.setPlayerClothes(sender, 4, sender.getData("Pants"), 0);

                if ((sender.getData("Survet")) == 55 && sender.getSyncedData("Sexe") == "Homme") { sender.setData("Survet", 0); } // Retrait des tenue flics
                if ((sender.getData("Survet")) == 48 && sender.getSyncedData("Sexe") == "Femme") { sender.setData("Survet", 0); }

                API.setPlayerClothes(sender, 8, sender.getData("Chemise"), 0);
                API.setPlayerClothes(sender, 11, sender.getData("Survet"), 0);
                API.setPlayerClothes(sender, 6, sender.getData("Chaussures"), 0);
                API.setPlayerClothes(sender, 3, sender.getData("Bras"), 0);
                /*
                 * Outil de test
                API.consoleOutput("****Resultat****");
                API.consoleOutput(sender.getData("Pants").ToString()+" "+ sender.getData("Chemise").ToString()+" "+ sender.getData("Survet").ToString()+" " +sender.getData("Chaussures").ToString() +" "+ sender.getData("Bras").ToString());
                */
                string clothing = "["+"["+ sender.getData("Pants").ToString()+"],"+ "["+ sender.getData("Chemise").ToString() + "]," + "[" + sender.getData("Survet").ToString() + "]," + "[" + sender.getData("Chaussures").ToString()+ "]," + "[" + sender.getData("Bras").ToString() + "]" + "]";
                Mysql.DBPlayers.UpdateCloth(sender, clothing);
            }
            else if(eventName == "resetcloth")
            {
                API.setPlayerClothes(sender, 4, sender.getData("Pants"), 0);
                API.setPlayerClothes(sender, 8, sender.getData("Chemise"), 0);
                API.setPlayerClothes(sender, 11, sender.getData("Survet"), 0);
                API.setPlayerClothes(sender, 6, sender.getData("Chaussures"), 0);
            }
        }

        public void ClothingShop()
        {
            Vector3 position = new Vector3(427.0037, -800.3123, 29.49114);
            ColShape repair = API.createCylinderColShape(position, 3f, 2f);
            var myBlip = API.createBlip(position);
            API.setBlipShortRange(myBlip, true);
            API.setBlipSprite(myBlip, 73);
            API.setBlipColor(myBlip, 3);
            API.setBlipName(myBlip, "Magasin de vetement");
            API.createMarker(1, position - new Vector3(0f, 0f, 1f), new Vector3(), new Vector3(), new Vector3(1f, 1f, 1f), 255, 0, 255, 0);
            repair.onEntityEnterColShape += (shape, entity) =>
            {
                var players = API.getPlayerFromHandle(entity);
                if (players == null)
                {
                    return;
                }
                else
                {
                    List<string> _Pantalon = new List<string>();
                    for(int i = 0; i < 85; i++)
                    {
                        _Pantalon.Add(i.ToString());
                    }
                    List<string> _Chemise = new List<string>();
                    for (int i = 0; i < 95; i++)
                    {
                        _Chemise.Add(i.ToString());
                    }
                    List<string> _Survetement = new List<string>();
                    for (int i = 0; i < 205; i++)
                    {
                        _Survetement.Add(i.ToString());
                    }
                    List<string> _Chaussure = new List<string>();
                    for (int i = 0; i < 205; i++)
                    {
                        _Chaussure.Add(i.ToString());
                    }
                    List<string> _Bras= new List<string>();
                    for (int i = 0; i < 100; i++)
                    {
                        _Bras.Add(i.ToString());
                    }
                    List<String>[] _ConteneurListe = new List<String>[5];
                    _ConteneurListe[0] = _Pantalon;
                    _ConteneurListe[1] = _Chemise;
                    _ConteneurListe[2] = _Survetement;
                    _ConteneurListe[3] = _Chaussure;
                    _ConteneurListe[4] = _Bras;
                    List<string> _NomListe = new List<string>();
                    _NomListe.Add("Pantalon");
                    _NomListe.Add("Chemise");
                    _NomListe.Add("Survetement");
                    _NomListe.Add("Chaussure");
                    _NomListe.Add("Bras");
                    List<String> Actions = new List<string>();
                    Actions.Add("Valider");
                    Actions.Add("Annuler");
                    API.triggerClientEvent(players, "clothing", 300, "Menu Vetement", "", _NomListe, _ConteneurListe,Actions);
                }

            };
        }
    }
}

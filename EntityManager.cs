using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Shared;
using System;
using System.Collections.Generic;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;

namespace LSRP_VFR
{
    public class EntityManager
    {
        private static List<Client> _Client = new List<Client>();
        private static List<Vehicle> _Vehicule = new List<Vehicle>();
        private static List<ColShape> _ColShape = new List<ColShape>();

        //Recuperer la liste joueur
        public static List<Client> GetClientList()
        {
            return _Client;
        }
        //Ajouter un joueur a la liste
        internal static void Add(Client player)
        {
            _Client.Add(player);
        }
        //Supprimer un joueur de la liste
        internal static void Remove(Client player)
        {
            _Client.Remove(player);
        }
        //Recup un jouer via son socialClub
        public static Client GetClient(string SocialClub)
        {
            return _Client.Find(x => x.socialClubName == SocialClub);
        }
        public static Client GetClientName(string name)
        {
            foreach (Client client in _Client)
            {
                if (client.getSyncedData("Nom_Prenom") == name)
                {
                    return client;
                }
            }
            return null;
        }

        //Vehicule
        public static void Add(Vehicle vehicle)
        {
            _Vehicule.Add(vehicle);
        }
        public static void Remove(Vehicle vehicle)
        {
            _Vehicule.Remove(vehicle);
        }
        public static Vehicle GetVehicle(NetHandle vehicle)
        {
            return _Vehicule.Find(x=>x.handle == vehicle);
        }

        // Colshape

        //Ajouter un Colshape a la liste
        internal static void AddColshape(ColShape colshape)
        {
            _ColShape.Add(colshape);
        }

    }
}

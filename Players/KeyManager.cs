using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrandTheftMultiplayer.Shared;
using static LSRP_VFR.Players.Player;
using LSRP_VFR.Faction.EMS;

namespace LSRP_VFR.Players
{
    class KeyManager: Script
    {

        public KeyManager()
        {
            API.onClientEventTrigger += API_onClientEventTrigger;
        }

        private void API_onClientEventTrigger(GrandTheftMultiplayer.Server.Elements.Client sender, string eventName, params object[] arguments)
        {
            if (eventName == "onKeyDown")
            {
                if (!IsPlayerLoggedIn(sender) && DeathManager.IsOnComaPlayer(sender) && IsArrested(sender) && IsOnProgress(sender)) { return; }
                // Menu Admin "F1"
                if ((int)arguments[0] == 0)
                {
                    Admin.AdminMenu.AdminCommand(sender);
                }
                // Menu Telephone "O"
                else if ((int)arguments[0] == 1)
                {
                    Menu.MenuTelephone.OpenMenuTelephone(sender);
                }
                // Menu Joueur "Y"
                else if ((int)arguments[0] == 2)
                {
                    Menu.MainInventory.OpenInventory(sender);
                }
                // Change Voice Range "W"
                else if ((int)arguments[0] == 3)
                {
                    //GtaLifeTs.ChangeVoiceSystem(sender);
                }
                // UnLock & Delock "U"
                else if ((int)arguments[0] == 4)
                {
                    Vehicles.Vehicle.LockUnlockVehOwner(sender);
                }
                // Vehicle Menu "I"
                else if ((int)arguments[0] == 5)
                {
                    Menu.MenuVehicle.OpenMenuVehicle(sender);
                }
                // Interaction Key "E"
                else if ((int)arguments[0] == 6)
                {
                    if (API.getEntitySyncedData(sender, "InProgress") == false) { 
                        if (API.hasEntityData(sender, "OnFarmZone") && API.getEntityData(sender, "OnFarmZone") != null)
                        {                   
                            Farm.InitFarm.StartFarming(sender);
                        }
                        else
                        {
                            GetInteraction(sender);
                        }
                    }
                }
                else if ((int)arguments[0] == 7)
                {
                    Jobs.Fourriere.TowVehicle(sender);
                }
            }
        }

        private void GetInteraction(Client player)
        {
            List<NetHandle>peds = API.getAllPeds();
            foreach (NetHandle ped in peds)
            {
                if (player.position.DistanceToSquared(API.getEntityPosition(ped)) < 4)
                {
                    if (API.hasEntitySyncedData(ped, "Interaction"))
                    {
                        string interaction = API.getEntitySyncedData(ped, "Interaction");
                        switch (interaction)
                        {
                            case "Hospital":
                                Hospital.OpenMenuHospital(player);
                                break;
                            case "ScooterRent":
                                Shop.ScootRent.OpenMenuScootRent(player);
                                break;
                            case "Market":
                                Shop.Market.OpenMenuMarket(player);
                                break;
                            case "Concess":
                                Shop.VehicleShop.OpenMenuConcess(player, ped);
                                break;
                            case "Ammunation":
                                Shop.Ammunation.OpenMenuAmmunation(player);
                                break;
                            case "Cops":
                                Faction.LSPD_Main.OnOpenCopsMenu(player);
                                break;
                            case "Traitement":
                                Farm.InitFarm.StartTraitement(player, ped);
                                break;
                            case "Trader":
                                Farm.InitFarm.StartTraid(player, ped);
                                break;
                            case "Prefecture":
                                Licence.OpenMenuLicence(player);
                                break;
                            case "EMS":
                                Hospital.OpenMenuServiceEMS(player);
                                break;
                            default:
                                API.sendNotificationToPlayer(player, "ERREUR!");
                                break;
                        }
                        return;
                    }   
                }
            }
        }
    }
}

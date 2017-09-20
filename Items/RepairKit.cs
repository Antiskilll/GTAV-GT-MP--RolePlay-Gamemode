using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using static LSRP_VFR.Items.Items;

namespace LSRP_VFR.Items
{
    public class RepairKit : Item
    {
        public RepairKit(int id, string name, string description, int weight) : base(id, name, description, weight)
        {

        }

        public override void Use(Client c)
        {
            if (c.vehicle == null)
            {
                API.shared.sendNotificationToPlayer(c, "Vous devez être dans un véhicule pour le réparer");
                return;
            }
            API.shared.setVehicleHealth(c.vehicle, 2000);
            InventoryHolder ih = c.getData("InventoryHolder");
            ih.RemoveItemFromInventory(this, 1);
            API.shared.sendNotificationToPlayer(c, "~g~ Véhicule Réparé");
        }
    }
}

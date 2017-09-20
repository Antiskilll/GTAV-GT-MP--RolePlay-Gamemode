using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using static LSRP_VFR.Items.Items;

namespace LSRP_VFR.Items
{
    public class Tire : Item
    {
        public Tire(int id, string name, string description, int weight) : base(id, name, description, weight)
        {

        }

        public override void Use(Client c)
        {
            if (!API.shared.isPlayerInAnyVehicle(c))
            {
                API.shared.triggerClientEvent(c, "display_subtitle", "Vous devez être dans un véhicule pour réparer ses pneus.");
            }
            else
            {
                for (var i = 0; i < 7; i++)
                {
                    API.shared.popVehicleTyre(c.vehicle, i, false);
                }
                InventoryHolder ih = API.shared.getEntityData(c, "InventoryHolder");
                ih.RemoveItemFromInventory(this, 1);
            }

        }
    }

}

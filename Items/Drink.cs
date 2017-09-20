using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using static LSRP_VFR.Items.Items;

namespace LSRP_VFR.Items
{
    public class Drink : Item
    {
        public Drink(int id, string name, string description, int weight, int drink) : base(id, name, description, weight)
        {
            Drink1 = drink;
        }

        public int Drink1 { get; private set; }

        public override void Use(Client c)
        {
            int Thirst = API.shared.getEntitySyncedData(c.handle, "PLAYER_THIRSTY");
            int newThirst = Thirst + Drink1;
            if (newThirst > 100) { newThirst = 100; }
            API.shared.setEntitySyncedData(c.handle, "PLAYER_THIRSTY", newThirst);
            API.shared.triggerClientEvent(c, "UpdateSurvival");
            InventoryHolder ih = API.shared.getEntityData(c, "InventoryHolder");
            ih.RemoveItemFromInventory(this, 1);
        }
    }
}

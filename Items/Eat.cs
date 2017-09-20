using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using static LSRP_VFR.Items.Items;

namespace LSRP_VFR.Items
{
    public class Eat : Item
    {
        public Eat(int id, string name, string description, int weight, int food) : base(id, name, description, weight)
        {
            Food = food;
        }

        public int Food { get; private set; }

        public override void Use(Client c)
        {
            int Thirst = API.shared.getEntitySyncedData(c, "PLAYER_HUNGRY");
            int newThirst = Thirst + Food;
            if (newThirst > 100) { newThirst = 100; }
            API.shared.setEntitySyncedData(c, "PLAYER_HUNGRY", newThirst);
            API.shared.triggerClientEvent(c, "UpdateSurvival");
            InventoryHolder ih = API.shared.getEntityData(c, "InventoryHolder");
            ih.RemoveItemFromInventory(this, 1);
        }
    }
}

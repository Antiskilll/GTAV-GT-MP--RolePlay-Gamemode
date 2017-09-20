using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using static LSRP_VFR.Items.Items;

namespace LSRP_VFR.Items
{
    public class Health : Item
    {
        public int AmountToHeal { get; set; }


        public Health(int id, string name, string description, int weight, int amountToHeal) : base(id, name, description, weight)
        {
            AmountToHeal = amountToHeal;
        }
        public override void Use(Client c)
        {
            int actualHealth = API.shared.getPlayerHealth(c);
            int NewHeath = actualHealth + AmountToHeal;
            if (NewHeath > 100) NewHeath = 100;
            API.shared.setPlayerHealth(c, NewHeath);
            InventoryHolder ih = API.shared.getEntityData(c, "InventoryHolder");
            ih.RemoveItemFromInventory(this, 1);
        }

    }
}

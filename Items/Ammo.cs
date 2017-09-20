using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Shared;
using static LSRP_VFR.Items.Items;

namespace LSRP_VFR.Items
{
    public class Ammo : Item
    {
        public int Qty { get; set; }
        public uint WHash { get; set; }

        public Ammo(int id, string name, string description, int weight, uint hash, int quantity) : base(id, name, description, weight)
        {
            Qty = quantity;
            WHash = hash;
        }
        public override void Use(Client c)
        {
            //API.shared.sendNativeToPlayer(c, Hash.ADD_AMMO_TO_PED, args);
            API.shared.setPlayerWeaponAmmo(c, (WeaponHash)WHash, 20);
            InventoryHolder ih = API.shared.getEntityData(c, "InventoryHolder");
            ih.RemoveItemFromInventory(this, 1);
        }
    }
}

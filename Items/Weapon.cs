using GrandTheftMultiplayer.Server;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using static LSRP_VFR.Items.Items;

namespace LSRP_VFR.Items
{
    public class Weapon : Item
    {
        public string InternalName { get; set; }

        public Weapon(int id, string name, string description, int weight, string internalname) : base(id, name, description, weight)
        {
            InternalName = internalname;
        }
        public override void Use(Client c)
        {
            API.shared.givePlayerWeapon(c, API.shared.weaponNameToModel(InternalName), 500, true, false);
        }
    }
}

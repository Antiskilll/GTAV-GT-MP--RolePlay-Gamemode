using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using static LSRP_VFR.Items.Items;

namespace LSRP_VFR.Items
{
    public class ItemNonTraiter : Item
    {
        public ItemNonTraiter(int id, string name, string description, int weight) : base(id, name, description, weight)
        {

        }

        public override void Use(Client c)
        {
            API.shared.triggerClientEvent(c, "display_subtitle", "Cette item est inutilisable!");
        }
    }
}

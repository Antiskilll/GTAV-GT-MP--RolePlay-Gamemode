using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using static LSRP_VFR.Items.Items;

namespace LSRP_VFR.Items
{
    public class Narcotic : Item
    {
        public int AmountToHeal { get; set; }
        public int AmountToDamage { get; set; }

        public Narcotic(int id, string name, string description, int weight, int amountToHeal, int amountToDamage) : base(id, name, description, weight)
        {
            AmountToHeal = amountToHeal;
            AmountToDamage = amountToDamage;
        }
        public override void Use(Client c)
        {
            API.shared.playPlayerScenario(c, "WORLD_HUMAN_SMOKING_POT");
        }

    }
}

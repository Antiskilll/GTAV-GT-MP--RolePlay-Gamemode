using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using static LSRP_VFR.Items.Items;

namespace LSRP_VFR.Items
{
    public class ItemWeed : Item
    {
        public ItemWeed(int id, string name, string description, int weight) : base(id, name, description, weight)
        {

        }

        public override void Use(Client c)
        {
            int timeeffect = 32000; //32 sc

            API.shared.triggerClientEvent(c, "display_subtitle", "Un bon petit joint!", 3000);
            API.shared.playPlayerScenario(c, "world_human_drug_dealer");
            

/*
            object[] test = new object[4];

            test[0] = c;
            test[1] = "WORLD_HUMAN_DRUG_DEALER";
            test[2] = 0;
            test[3] = true;

            API.shared.sendNativeToAllPlayers(Hash.TASK_START_SCENARIO_IN_PLACE, test);
            */
            API.shared.triggerClientEvent(c, "smokeweedeveryday", timeeffect);
            API.shared.sleep(timeeffect);
            API.shared.stopPlayerAnimation(c);
            InventoryHolder ih = API.shared.getEntityData(c, "InventoryHolder");
            ih.RemoveItemFromInventory(this, 1);
        }
    }
}

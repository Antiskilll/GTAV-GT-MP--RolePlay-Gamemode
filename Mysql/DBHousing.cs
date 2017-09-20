using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LSRP_VFR.Items.Items;
using static LSRP_VFR.Mysql.Database;

namespace LSRP_VFR.Mysql
{
    class DBHousing : Script
    {
        public DBHousing()
        {

        }

        public static DataTable GetAllHousePlayerInID(int ID)
        {
            DataTable result;
            return result = GetQuery("SELECT Owner FROM housing WHERE ID='" + ID + "'");
        }

        public static DataTable GetHousePlayer(int ID, string SocialClub)
        {
            DataTable result;
            return result = GetQuery("SELECT Owner, Inventory, Dimension FROM housing WHERE ID='" + ID + "' AND Owner='" + SocialClub + "'");
        }

        public static DataTable GetHouseInventory(int ID, string SocialClub)
        {
            DataTable result;
            return result = GetQuery("SELECT Inventory FROM housing WHERE ID='" + ID.ToString() + "' AND Owner='" + SocialClub + "'");
        }

        public static void SaveHouseInventory(Client player, int ID, string SocialClub)
        {
            InventoryHolder inventory = API.shared.getEntityData(player, "Home_InventoryHolder");
 
            var invs = "";
            foreach (InventoryItem ii in inventory.Inventory)
            {
                invs += "[" + ii.Details.ID.ToString() + "," + ii.Quantity.ToString() + "],";
            }
            char[] car = { ',' };
            var inventaires = "[" + invs.TrimEnd(car) + "]";


            InsertQuery("UPDATE housing SET Inventory='" + inventaires + "' WHERE ID='" + ID.ToString() + "' AND Owner='" + SocialClub + "'");

        }

        public static bool CheckNumberRentHousing(int ID, int MaxPlace)
        {

            DataTable result = GetQuery("SELECT COUNT(*) FROM housing WHERE ID='" + ID + "'");
            if (result.Rows.Count != MaxPlace)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static void AddHouseOwner(Client player, int ID, int Dimension)
        {
            try
            {
                string query = String.Format("INSERT INTO housing (ID, Owner, Dimension, Inventory) VALUES ('{0}','{1}','{2}','[]')",
                    ID.ToString(),
                    player.socialClubName,
                    Dimension.ToString()
                );
                InsertQuery(query);

            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR] : ~s~" + e.ToString());
            }
        }

        public static void SellHouseOwner(int ID, string SocialClub)
        {
            try
            {
                DataTable result = GetQuery("DELETE FROM housing WHERE ID='" + ID + "' AND Owner='" + SocialClub + "'");

            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR] : ~s~" + e.ToString());
            }
        }

        public static Boolean GetAllHouseDimension(int dimension)
        {
            try
            {
                if (dimension != 0)
                {
                    DataTable result = GetQuery("SELECT * FROM housing WHERE Dimension='" + dimension + "'");
                    if (result.Rows.Count != 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                API.shared.consoleOutput("~r~[ERROR] : ~s~" + e.ToString());
                return false;
            }
        }
    }
}

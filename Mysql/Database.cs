using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Threading;
using System.Security.Cryptography;
using System.Text;
using static LSRP_VFR.Mysql.DBPlayers;

namespace LSRP_VFR.Mysql
{
    class Database : Script
    {
        private static string connectionStr;

        public Database()
        {      
            API.onResourceStart += OnResourceStart;
        }

        private void OnResourceStart()
        {
            try
            {
                connectionStr = String.Format("server={0};user={1};password={2};database={3};port={4}",
                        API.getSetting<string>("host"),
                        API.getSetting<string>("user"),
                        API.getSetting<string>("password"),
                        API.getSetting<string>("database"),
                        API.getSetting<string>("port")
                    );
                using (MySqlConnection conn = new MySqlConnection(connectionStr))
                {
                    API.consoleOutput("[MySQL][INFO] Attempting connecting to MySQL");
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        API.consoleOutput("[MySQL][INFO] Connected to MySQL");
                        DBVehicles.GetCarSpawn();
                    }
                }
            }
            catch (Exception ex)
            {
                API.consoleOutput("[MySQL][ERROR] " + ex.ToString());
                API.delay(5000, true, () => { Environment.Exit(1); });
            }
        }




        public static void InsertQuery(string sql)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionStr))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    API.shared.consoleOutput("[MySQL][ERROR] " + ex.ToString());
                }
            }
        }

        public static DataTable GetQuery(string sql)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionStr))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    conn.Open();
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    DataTable results = new DataTable();
                    results.Load(rdr);
                    rdr.Close();
                    return results;

                }
                catch (Exception ex)
                {
                    API.shared.consoleOutput("[MySQL][ERROR] " + ex.ToString());
                    return null;
                }
            }
        }

        public static int CheckPlayerPassword(Client player, String givenPassword)
        {
            DataTable result = GetQuery("SELECT password FROM whitelist WHERE socialclubName='" + player.socialClubName + "' LIMIT 1");
            if (result.Rows.Count != 0)
            {
                HashAlgorithm algorithm = SHA1.Create();
                StringBuilder sb = new StringBuilder();
                byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(givenPassword));

                foreach (byte b in hash)
                    sb.Append(b.ToString("X2"));

                string givenPasswordHash = sb.ToString();

                if (givenPasswordHash != Convert.ToString(result.Rows[0]["password"]))
                {
                    return 1;
                } else {
                    return 0;
                }
            } else {
                return 2;
            }
        }
    }
}

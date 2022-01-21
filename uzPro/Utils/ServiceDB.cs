using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uzPro.Utils
{
    public static class ServiceDB
    {
        public static string BackUp(string dir){
            
            
            SqlConnection con = new SqlConnection();
            SqlCommand sqlcmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            var sqlConStrBuilder = new SqlConnectionStringBuilder(Properties.Settings.Default.connString);

            string fileName = string.Format("uzi_{0}", DateTime.Now.ToString("yyyy_MM_dd_h_mm_tt"));
            string backupFileName = String.Format("{0}\\{1}.bak", dir, fileName);

            try
            {
                using (var connection = new SqlConnection(sqlConStrBuilder.ConnectionString))
                {
                    var query = String.Format("BACKUP DATABASE {0} TO DISK='{1}'",
                        sqlConStrBuilder.InitialCatalog, backupFileName);

                    using (var command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                return "Backup Completed!";
                
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            
        }
    }
}

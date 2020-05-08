using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentRegistrationApplication
{

    class DbHandler
    {

        // --- mysql Connection ---
        static string connString = "datasource=127.0.0.1;port=3306;username=root;password=;database=studentrecords;";
        MySqlConnection conn = new MySqlConnection(connString);


        // --- Open connection method ---
        public void openConnection() {
            if (conn.State == System.Data.ConnectionState.Closed) {
                conn.Open();
                //MessageBox.Show("Con open");
            }
            else if (conn.State == System.Data.ConnectionState.Open)
            {
                conn.Close();
                //MessageBox.Show("Con closed");
            }
        }

        // --- Return connection method
        public MySqlConnection getConnection() {
            return conn;
        }

        
    }
}

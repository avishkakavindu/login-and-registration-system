using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentRegistrationApplication
{
    public partial class ViewRec : Form
    {
        public ViewRec()
        {
            InitializeComponent();

        }
        static string selected;

        public ViewRec(string select)
        {
            InitializeComponent();
            selected = select;
            
        }
        // --- Create object for HashSalt and pass num of iterations 
        HashSalt hashSalt = new HashSalt(100);  // 100 iterations 
        DbHandler db = new DbHandler();

        
        
        private void ViewRec_Load(object sender, EventArgs e)
        {
            db.openConnection();    // open connection
            
            string sQuery = "SELECT indexno,firstname,lastname,address,gender,dob,email,faculty,mobile,image FROM student WHERE indexno ='" + selected + "'";
            MySqlCommand cmd = new MySqlCommand(sQuery, db.getConnection());
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            
            txtindex.Text = table.Rows[0][0].ToString();
            txtfname.Text = table.Rows[0][1].ToString();
            txtlname.Text = table.Rows[0][2].ToString();
            txtaddress.Text = table.Rows[0][3].ToString();
            if (table.Rows[0][4].ToString() == "M")
            {
                radiomale.Checked = true;
            }
            else
            {
                radiofemale.Checked = true;
            }
            txtdob.Text = table.Rows[0][5].ToString();
            txtemail.Text = table.Rows[0][6].ToString();
            txtfaculty.Text = table.Rows[0][7].ToString();
            txtmobile.Text = table.Rows[0][8].ToString();

            /*
            *
            *
            *    Invalid Argument exception Due to Empty image filled- EXCEPTION HANDLED
            *
            */
            try
            {
                byte[] data = new byte[0];
                data = (byte[])(table.Rows[0][9]);

                MemoryStream mem = new MemoryStream(data);
                picImg.Image = Image.FromStream(mem);
            }
            catch (Exception)
            {
                string message = "No Image file found on Database!";
                string title = "Image File Not Found";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Information);
            }
            db.openConnection();    // close connection
        }
    }
}

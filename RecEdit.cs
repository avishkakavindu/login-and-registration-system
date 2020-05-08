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
    public partial class RecEdit : Form
    {
        static string selected = "";
        DbHandler db = new DbHandler();
        Records openRecs = (Records)Application.OpenForms["Records"];   // obj for open record form manipulation

        public RecEdit()
        {
            InitializeComponent();
        }

        public RecEdit(string str)
        {
            selected = str;
            InitializeComponent();
        }

        private void RecEdit_Load(object sender, EventArgs e)
        {
            db.openConnection();    // open connection
            // --- populate faculty cmb ---
            MySqlCommand cmdCmb = new MySqlCommand("SELECT facultyname FROM faculty;", db.getConnection());
            

            using (var reader = cmdCmb.ExecuteReader())
            {
                while (reader.Read())
                {
                    cmbfaculty.Items.Add(reader.GetString("facultyname"));
                }
            }
            
            string sQuery = "SELECT indexno,firstname,lastname,address,gender,dob,email,faculty,mobile,image FROM student WHERE indexno ='"+selected+"'";
            MySqlCommand cmd = new MySqlCommand(sQuery, db.getConnection());
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            MessageBox.Show(sQuery);
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
            dateDob.Value = Convert.ToDateTime(table.Rows[0][5]);
            txtemail.Text = table.Rows[0][6].ToString();
            cmbfaculty.SelectedIndex = cmbfaculty.FindStringExact(table.Rows[0][7].ToString());
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

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "png files(*.png)|*.png|jpg files(*.jpg)|*jpg|All files(*.*)|*.*";

            if (file.ShowDialog() == DialogResult.OK)
            {
                if (new FileInfo(file.FileName).Length > (150 * 1024))
                {
                    MessageBox.Show("File size is too large!", "File Size Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning); //handle invalid file size here
                }
                else
                {
                    picImg.Image = Image.FromFile(file.FileName);    // preview img
                }
            }
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            // --- confirmation with message box ---
            string message = "Do you want to Save the changes?";
            string title = "Please Confirm!";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                db.openConnection();    // open connection
                MySqlCommand cmd = new MySqlCommand("UPDATE `student` SET indexno=@index,firstname=@firstname,lastname=@lastname,address=@address,gender=@gender,dob=@dob,email=@email,faculty=@faculty,mobile=@mobile,image=@image WHERE indexno ='" + selected + "'", db.getConnection());

                // --- image ---
                MemoryStream ms = new MemoryStream();
                picImg.Image.Save(ms, picImg.Image.RawFormat);
                byte[] img = ms.ToArray();
                // ----
                AddEdit addEdit = new AddEdit();
                
                cmd.Parameters.Add("@index", MySqlDbType.VarChar).Value = txtindex.Text;
                cmd.Parameters.Add("@firstname", MySqlDbType.VarChar).Value = txtfname.Text;
                cmd.Parameters.Add("@lastname", MySqlDbType.VarChar).Value = txtlname.Text;
                cmd.Parameters.Add("@address", MySqlDbType.VarChar).Value = txtaddress.Text;
                cmd.Parameters.Add("@gender", MySqlDbType.VarChar).Value = addEdit.getGender();
                cmd.Parameters.Add("@dob", MySqlDbType.Date).Value = dateDob.Value.Date;
                cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = txtemail.Text;
                cmd.Parameters.Add("@faculty", MySqlDbType.VarChar).Value = cmbfaculty.GetItemText(cmbfaculty.SelectedItem);
                cmd.Parameters.Add("@mobile", MySqlDbType.VarChar).Value = txtmobile.Text;
                cmd.Parameters.Add("@image", MySqlDbType.Blob).Value = img;
                
                // query executed?
                if (cmd.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Record added!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    openRecs.searchData("");    // refreshing by reloading data from db
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed!, please retry", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                db.openConnection();    // close connection

            }
        }
    }
}

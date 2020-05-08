using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace StudentRegistrationApplication
{
    public partial class Login : Form
    {

        public Login()
        {
            InitializeComponent();
        }

        // --- Check what radio button selected (gender)
        private string getGender()
        {
            string gender;
            if (radiofemale.Checked)
            {
                gender = "F";
            }
            else
            {
                gender = "M";
            }

            return gender;
        }

        // --- Create object for HashSalt and pass num of iterations 
        HashSalt hashSalt = new HashSalt(100);  // 100 iterations 
        

        private void Login_Load(object sender, EventArgs e)
        {
            DbHandler db = new DbHandler();
            MySqlCommand cmd = new MySqlCommand("SELECT facultyname FROM faculty;", db.getConnection());
            db.openConnection();    // open connection

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    cmbfaculty.Items.Add(reader.GetString("facultyname"));
                }
            }
        }

        /*
         *  --- SignIn Handling ---
         */
        private void btnSignin_Click(object sender, EventArgs e)
        {
            string enteredPass = txtLoginPassword.Text;

            DbHandler db = new DbHandler();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable table = new DataTable();
            MySqlCommand cmd = new MySqlCommand("SELECT password, salt FROM student WHERE indexno=@index;", db.getConnection());
            db.openConnection();    // open connection
            
                cmd.Parameters.Add("@index", MySqlDbType.VarChar).Value = txtLoginusername.Text;

                adapter.SelectCommand = cmd;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    string pass = table.Rows[0][0].ToString();
                    string salt = table.Rows[0][1].ToString();

                    string newPass = hashSalt.generateHash(enteredPass, hashSalt.saltToByte(salt));

                    if (hashSalt.authenticateUser(enteredPass, pass, salt))
                    {
                        // MessageBox.Show("correct ='" + pass + "'\nEntered pass ='" + hashSalt.authenticateUser(enteredPass, pass, salt) + "'");
                        // --- form Records obj
                        Records gotoRecords = new Records();
                        gotoRecords.Show(); // goto Records
                        this.Hide();

                }
                    else
                    {
                        string message = "User name & Password did not Match!?";
                        string title = "Attention!";
                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
                    }
                
                }
                else
                {
                    string message = "User name NotFound!?";
                    string title = "Attention!";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
                }
            //this.Close();
            db.openConnection();    // close connection




        }

        // --- login password ---
        private void txtLoginPassword_Enter(object sender, EventArgs e)
        {
            txtLoginPassword.UseSystemPasswordChar = true;
        }
        /*
         *  --- ^ SignIn Handling ^ ---
         */


        /*
         *  --- SignUp Handling ---
         */
        private void btnSignup_Click(object sender, EventArgs e)
        {
            // --- confirmation with message box ---
            string message = "Do you want to Save the changes?";
            string title = "Please Confirm!";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                if (txtpassword.Text == txtconfirmpassword.Text)
                {

                    DbHandler db = new DbHandler();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO `student`(`indexno`,`firstname`,`lastname`,`address`,`gender`,`dob`,`email`,`faculty`,`mobile`,`password`,`salt`,`image`)VALUES(@index, @firstname, @lastname, @address, @gender, @dob, @email, @faculty, @mobile, @password, @salt, @image);", db.getConnection());

                    // --- image ---
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                    byte[] img = ms.ToArray();
                    // -------------

                    var salt = hashSalt.generateSalt();  // generates random salt type string
                    var byteSalt = hashSalt.saltToByte(salt);   // gets byte[] from salt string

                    cmd.Parameters.Add("@index", MySqlDbType.VarChar).Value = txtindex.Text;
                    cmd.Parameters.Add("@firstname", MySqlDbType.VarChar).Value = txtfname.Text;
                    cmd.Parameters.Add("@lastname", MySqlDbType.VarChar).Value = txtlname.Text;
                    cmd.Parameters.Add("@address", MySqlDbType.VarChar).Value = txtaddress.Text;
                    cmd.Parameters.Add("@gender", MySqlDbType.VarChar).Value = getGender();
                    cmd.Parameters.Add("@dob", MySqlDbType.Date).Value = dateDob.Value.Date;
                    cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = txtemail.Text;
                    cmd.Parameters.Add("@faculty", MySqlDbType.VarChar).Value = cmbfaculty.GetItemText(cmbfaculty.SelectedItem);
                    cmd.Parameters.Add("@mobile", MySqlDbType.VarChar).Value = txtmobile.Text;
                    cmd.Parameters.Add("@password", MySqlDbType.VarChar).Value = hashSalt.generateHash(txtpassword.Text, byteSalt); // get (password+salt) hashed from db 
                    cmd.Parameters.Add("@salt", MySqlDbType.VarChar).Value = salt;
                    cmd.Parameters.Add("@image", MySqlDbType.Blob).Value = img;

                    db.openConnection();    // open connection
                    // execute query
                    if (cmd.ExecuteNonQuery() == 1)
                    {
                        MessageBox.Show("Record added!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed!, please retry", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    db.openConnection();    // close connection

                }
                else
                {
                    // --- password confirmation failed msg ---
                    DialogResult r = MessageBox.Show("Password Confirmation failed?", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtpassword.Focus();
                }

            }
            else
            {
                // Do something  
            }
        }
        // --- password ---
        private void txtpassword_Enter(object sender, EventArgs e)
        {
            txtpassword.UseSystemPasswordChar = true;
        }
        // ---confirm password ---
        private void txtconfirmpassword_Enter(object sender, EventArgs e)
        {
            txtconfirmpassword.UseSystemPasswordChar = true;
        }

        // --- image upload ---
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
                    pictureBox1.Image = Image.FromFile(file.FileName);    // preview img
                }
            }
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}

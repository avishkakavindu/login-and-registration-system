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
    public partial class AddEdit : Form
    {
        public AddEdit()
        {
            InitializeComponent();
        }

       
       // --- Check what radio button selected (gender)
        public string getGender()
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
        DbHandler db = new DbHandler();
        Records openRecs = (Records)Application.OpenForms["Records"];   // obj for open record form manipulation

        // --- Form Onload ---
        private void AddEdit_Load(object sender, EventArgs e)
        {
            db.openConnection();    // open connection
            MySqlCommand cmd = new MySqlCommand("SELECT facultyname FROM faculty;", db.getConnection());
            
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    cmbfaculty.Items.Add(reader.GetString("facultyname"));
                }
            }
            db.openConnection();    // close connection
        }

        // --- On button click save ---
        private void btnsave_Click(object sender, EventArgs e)
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

                    db.openConnection();    // open connection     
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO `student`(`indexno`,`firstname`,`lastname`,`address`,`gender`,`dob`,`email`,`faculty`,`mobile`,`password`,`salt`,`image`)VALUES(@index, @firstname, @lastname, @address, @gender, @dob, @email, @faculty, @mobile, @password, @salt, @image);", db.getConnection());

                    // --- image ---
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                    byte[] img = ms.ToArray();
                    // ----

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
                    cmd.Parameters.Add("@salt", MySqlDbType.VarBinary).Value = salt;
                    cmd.Parameters.Add("@image", MySqlDbType.Blob).Value = img;

                    
                    // execute query
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
                else
                {
                    // --- password confirmation failed msg ---
                    DialogResult r = MessageBox.Show("Password confirmation failed?", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtpassword.Focus();
                }

            }
           else
           {
                // Do something  
           }
            
        }




        private void textBox2_Enter(object sender, EventArgs e)
        {
            txtpassword.UseSystemPasswordChar = true;
        }

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
    }
}

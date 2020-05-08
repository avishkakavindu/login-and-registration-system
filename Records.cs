using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentRegistrationApplication
{
    public partial class Records : Form
    {
        string selectedRow = "";
        
        DbHandler db = new DbHandler();

        public Records()
        {
            InitializeComponent();
        }

        private void Records_FormClosing(object sender, FormClosingEventArgs e)
        {
            Login login = new Login();
            login.Show();
            
        }

        public string getConstraints()
        {
            ArrayList selected = new ArrayList();
            for (int i = 0; i < constraintsList.Items.Count; i++)
            {
                if (constraintsList.GetItemChecked(i))
                {
                    selected.Add("`" + (string)constraintsList.Items[i] + "`") ;    // get selected items in to an array                   
                }
                
            }
            if (selected.Count == 0)
            {
                return "`indexno`, `firstname`, `lastname`, `address`, `gender`, `dob`, `email`, `faculty`, `mobile`";
            }
            // if array isn't empty
            string str = string.Join(", ", selected.ToArray());
            return str;
        }

        // --- Search query ---
        public void searchData(string searchString)
        {
            db.openConnection();    // open connection
            string sQuery = "SELECT indexno,firstname,lastname,address,gender,dob,email,faculty,mobile FROM student WHERE CONCAT("+getConstraints().ToString()+" ) LIKE '%" + searchString + "%'";
            MySqlCommand cmd = new MySqlCommand(sQuery, db.getConnection());
            
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            gridviewtable.DataSource = table;
            //gridviewtable.Columns.Remove("View");
            

            db.openConnection();    // open connection
        }


        // ---n form load ---
        private void Records_Load(object sender, EventArgs e)
        {
            string sQuery = "SELECT indexno,firstname,lastname,address,gender,dob,email,faculty,mobile FROM student WHERE CONCAT(" + getConstraints().ToString() + " ) LIKE '%%'";
            MySqlCommand cmd = new MySqlCommand(sQuery, db.getConnection());

            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            gridviewtable.DataSource = table;

            // --- view buttons ---
            DataGridViewButtonColumn viewBtn = new DataGridViewButtonColumn();

            // - view btn -
            viewBtn.HeaderText = "View";
            viewBtn.Name = "View";
            viewBtn.Text = "View";
            viewBtn.FlatStyle = FlatStyle.Flat;
            viewBtn.DefaultCellStyle.BackColor = Color.FromArgb(46, 169, 76);
            viewBtn.UseColumnTextForButtonValue = true;
            viewBtn.Width = 50;
            //// - del btn-
            //delBtn.HeaderText = "Delete";
            //delBtn.Name = "Delete";
            //delBtn.Text = "Delete";
            //delBtn.DefaultCellStyle.BackColor = Color.Tomato; 
            //delBtn.UseColumnTextForButtonValue = true;
            //delBtn.Width = 50;

            gridviewtable.Columns.Insert(9, viewBtn);
            //gridviewtable.Columns.Add(delBtn);


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string searchValue = txtsearch.Text.ToString();
            searchData(searchValue);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedRow == "")
            {
                // --- In no rec selected ---
                string message = "Please select your record from the table";
                string title = "No Record Selected!";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning);
            }
            else
            {
                RecEdit editRec = new RecEdit();
                new RecEdit(selectedRow);
                editRec.Show();
            }

        }
        // --- if row selected get index---
        private void gridviewtable_Click(object sender, EventArgs e)
        {
            selectedRow = gridviewtable.CurrentRow.Cells[0].Value.ToString();
            //MessageBox.Show(selectedRow);
            if (selectedRow == "View")
            {
                //MessageBox.Show("Selected data: " + selectedRow);
                selectedRow = gridviewtable.CurrentRow.Cells[1].Value.ToString();
                //MessageBox.Show("Selected data: " + selectedRow);

            }

        }

        // ---upon click on button view ---
        private void gridviewtable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ViewRec viewRec = new ViewRec();
            if (e.ColumnIndex == gridviewtable.Columns["View"].Index)
            {
                if (gridviewtable.CurrentRow.Cells[0].Value.ToString() == "View")
                {
                    //MessageBox.Show("if"+gridviewtable.CurrentRow.Cells[1].Value.ToString());
                    new ViewRec(gridviewtable.CurrentRow.Cells[1].Value.ToString());
                    viewRec.Show();
                }
                else
                {
                    //MessageBox.Show("else"+gridviewtable.CurrentRow.Cells[0].Value.ToString());
                    new ViewRec(gridviewtable.CurrentRow.Cells[0].Value.ToString());
                    viewRec.Show();
                }
                
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddEdit addNew = new AddEdit();

            addNew.Show();
            constraintsList.Visible = false;
        }

        

        private void comboBox1_MouseClick_1(object sender, MouseEventArgs e)
        {
            constraintsList.Visible = !constraintsList.Visible;
        }

        private void txtsearch_MouseClick(object sender, MouseEventArgs e)
        {
            constraintsList.Visible = false;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            constraintsList.Visible = false;
        }

        private void gridviewtable_MouseClick(object sender, MouseEventArgs e)
        {
            constraintsList.Visible = false;
        }

        private void constraintsList_MouseLeave(object sender, EventArgs e)
        {
            constraintsList.Visible = false;
        }

        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            constraintsList.Visible = false;
        }

       
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net;
using System.Data.Entity.Migrations;
using System.Data.Entity;

namespace StudentsFormsApp
{
    public partial class Form1 : Form
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=DESKTOP-EP4UIND\SQLEXPRESS;Initial Catalog=MyDatabase;Integrated Security=True");
        StudentModelEntities db = new StudentModelEntities();
        int StudentID = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'myDatabaseDataSet.Students' table. You can move, or remove it, as needed.
            //this.studentsTableAdapter.Fill(this.myDatabaseDataSet.Students);

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            //this.studentsTableAdapter.Fill(this.myDatabaseDataSet.Students);
            load_grid();
        }

        private void load_grid()
        {
            List<Student> datas = new List<Student>();
            if (txtSearch.Text == "")
            {
                datas = db.Students.ToList();
            }

            dataGridView1.DataSource = datas;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            List<Student> datas = new List<Student>();
            if (txtSearch.Text == "")
            {
                datas = db.Students.ToList();
            }

            else
            {
                datas = db.Students.Where(a =>
                  a.FirstName.Contains(txtSearch.Text.Trim())
                || a.LastName.Contains(txtSearch.Text.Trim())
                || a.RollNumber.Contains(txtSearch.Text.Trim())
                || a.Class.Contains(txtSearch.Text.Trim())
                || a.FatherName.Contains(txtSearch.Text.Trim())
                || a.MotherName.Contains(txtSearch.Text.Trim())
                || a.Address.Contains(txtSearch.Text.Trim())
                || a.Section.Contains(txtSearch.Text.Trim())).ToList();
            }

            dataGridView1.DataSource = datas;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //SP 
                if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand("StudentAddOrDelete", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@mode", "Add");
                sqlCmd.Parameters.AddWithValue("@StudentID", 0);
                sqlCmd.Parameters.AddWithValue("@FirstName", txtFName.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@LastName", txtLName.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Class", txtClass.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Section", txtSection.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@FatherName", txtFather.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@MotherName", txtMother.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@RollNumber", txtRoll.Text.Trim());
                sqlCmd.ExecuteNonQuery();
                MessageBox.Show("Data Saved Successfully");
                load_grid();

                //EF
                //datas = db.Students.Where(a =>
                //      a.FirstName.Equals(txtFName.Text.Trim())
                //   && a.LastName.Equals(txtLName.Text.Trim())
                //   && a.RollNumber.Equals(txtRoll.Text.Trim())
                //   && a.Class.Equals(txtClass.Text.Trim())
                //   && a.FatherName.Equals(txtFather.Text.Trim())
                //   && a.MotherName.Equals(txtMother.Text.Trim())
                //   && a.Address.Equals(txtAddress.Text.Trim())
                //   && a.Section.Equals(txtSection.Text.Trim())).ToList();

                //Student datas = new Student();
                //datas.FirstName = txtFName.Text.Trim();
                //datas.LastName = txtLName.Text.Trim();
                //datas.Address = txtAddress.Text.Trim();
                //datas.Class = txtClass.Text.Trim();
                //datas.Section = txtSection.Text.Trim();
                //datas.FatherName = txtFather.Text.Trim();
                //datas.MotherName = txtMother.Text.Trim();
                //datas.RollNumber = txtRoll.Text.Trim();

                //db.Students.Add(datas);
                //db.SaveChanges();
                //MessageBox.Show("Data Saved Successfully");

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something Went Wrong");
            }

            finally
            {
                sqlCon.Close();
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index != -1)
            {
                StudentID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value.ToString());
                txtFName.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                txtLName.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
                txtAddress.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                txtClass.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                txtSection.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
                txtFather.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
                txtMother.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString();
                txtRoll.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
                btnClear.Enabled = true;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFName.Text = "";
            txtLName.Text = "";
            txtAddress.Text = "";
            txtClass.Text = "";
            txtSection.Text = "";
            txtRoll.Text = "";
            txtFather.Text = "";
            txtMother.Text = "";
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //Student datas = new Student();
            //StudentID = datas.StudentID;
            try
            {
                if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand("StudentAddOrDelete", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@mode", "Edit");
                sqlCmd.Parameters.AddWithValue("@StudentID", StudentID);
                sqlCmd.Parameters.AddWithValue("@FirstName", txtFName.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@LastName", txtLName.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Class", txtClass.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Section", txtSection.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@FatherName", txtFather.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@MotherName", txtMother.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@RollNumber", txtRoll.Text.Trim());
                sqlCmd.ExecuteNonQuery();
                MessageBox.Show("Data Updated Successfully");
                load_grid();

                //EF
                //Student datas = new Student();
                //if (datas.StudentID == StudentID)
                //{
                //    datas.FirstName = txtFName.Text.Trim();
                //    datas.LastName = txtLName.Text.Trim();
                //    datas.Address = txtAddress.Text.Trim();
                //    datas.Class = txtClass.Text.Trim();
                //    datas.Section = txtSection.Text.Trim();
                //    datas.FatherName = txtFather.Text.Trim();
                //    datas.MotherName = txtMother.Text.Trim();
                //    datas.RollNumber = txtRoll.Text.Trim();

                //    //db.Students.Add(datas);
                //    //db.Entry(datas).State = EntityState.Modified;
                //    db.SaveChanges();
                //    MessageBox.Show("Data Updated Successfully");
                //}

                //else MessageBox.Show("Error");

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something Went Wrong");
            }


        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand("StudentAddOrDelete", sqlCon);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@mode", "Delete");
                sqlCmd.Parameters.AddWithValue("@StudentID", StudentID);
                sqlCmd.Parameters.AddWithValue("@FirstName", txtFName.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@LastName", txtLName.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Class", txtClass.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@Section", txtSection.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@FatherName", txtFather.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@MotherName", txtMother.Text.Trim());
                sqlCmd.Parameters.AddWithValue("@RollNumber", txtRoll.Text.Trim());
                sqlCmd.ExecuteNonQuery();
                MessageBox.Show("Data Deleted Successfully");
                load_grid();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Something Went Wrong");

            }


        }

        private void btnUrl_Click(object sender, EventArgs e)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(@"http://www.google.com");
            WebResponse response = request.GetResponse();
            if(response == null) MessageBox.Show("Something Went Wrong");
            else if(response != null) MessageBox.Show("Got It");
            response.Close();
        }
    }
}

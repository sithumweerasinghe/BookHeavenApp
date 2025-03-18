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

namespace BookHeavenApp
{
    public partial class CustomersForm : Form
    {
        private Database db = new Database();

        public CustomersForm()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;

            InitializeComponent();
            LoadCustomers();

        }
        private void LoadCustomers()
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Customers";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvCustomers.DataSource = dt;
            }
        }
        private void dgvCustomers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            {
                if (e.RowIndex >= 0)
                {
                    txtFirstName.Text = dgvCustomers.SelectedRows[0].Cells[1].Value.ToString();
                    txtLastName.Text = dgvCustomers.SelectedRows[0].Cells[2].Value.ToString();
                    txtContactInfo.Text = dgvCustomers.SelectedRows[0].Cells[3].Value.ToString();
                    txtAddress.Text = dgvCustomers.SelectedRows[0].Cells[4].Value.ToString();
                }
            }
        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void title_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtContactInfo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO Customers (FirstName, LastName, ContactInfo, Address) VALUES (@FirstName, @LastName, @ContactInfo, @Address)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                cmd.Parameters.AddWithValue("@ContactInfo", txtContactInfo.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Customer Added Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCustomers(); // Refresh data
                ClearFields();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count > 0)
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE Customers SET FirstName=@FirstName, LastName=@LastName, ContactInfo=@ContactInfo, Address=@Address WHERE CustomerID=@CustomerID";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CustomerID", dgvCustomers.SelectedRows[0].Cells[0].Value);
                    cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@LastName", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@ContactInfo", txtContactInfo.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Customer Updated Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadCustomers(); // Refresh data
                    ClearFields();
                }
            }
            else
            {
                MessageBox.Show("Please select a customer to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Customers WHERE FirstName LIKE @Search OR LastName LIKE @Search OR ContactInfo LIKE @Search";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Search", "%" + txtFirstName.Text + "%");
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvCustomers.DataSource = dt;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete this customer?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    using (MySqlConnection conn = db.GetConnection())
                    {
                        conn.Open();
                        string query = "DELETE FROM Customers WHERE CustomerID=@CustomerID";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@CustomerID", dgvCustomers.SelectedRows[0].Cells[0].Value);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Customer Deleted Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCustomers(); // Refresh data
                        ClearFields();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a customer to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            {
                ClearFields();
            }
        }
        private void ClearFields()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtContactInfo.Text = "";
            txtAddress.Text = "";
        }

        private void CustomersForm_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}

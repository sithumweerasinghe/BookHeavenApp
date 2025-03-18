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
    public partial class SalesForm : Form
    {
        private Database db = new Database();

        public SalesForm()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;

            InitializeComponent();
            LoadCustomers();
            LoadBooks();
            LoadSales();
        }

        private void LoadCustomers()
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT CustomerID, CONCAT(FirstName, ' ', LastName) AS Name FROM Customers";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cmbCustomer.Items.Add(new ComboBoxItem(reader["Name"].ToString(), reader["CustomerID"]));
                }
                reader.Close();
            }
        }

        private void LoadBooks()
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT BookID, Title, Price FROM Books";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cmbBook.Items.Add(new ComboBoxItem(reader["Title"].ToString(), reader["BookID"]));
                }
                reader.Close();
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbBook_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblTotalPrice_Click(object sender, EventArgs e)
        {

        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedItem == null || cmbBook.SelectedItem == null || string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Please select a customer, book, and enter quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO Sales (CustomerID, BookID, Quantity, TotalPrice) VALUES (@CustomerID, @BookID, @Quantity, @TotalPrice)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", ((ComboBoxItem)cmbCustomer.SelectedItem).Value);
                cmd.Parameters.AddWithValue("@BookID", ((ComboBoxItem)cmbBook.SelectedItem).Value);
                cmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtQuantity.Text));
                cmd.Parameters.AddWithValue("@TotalPrice", Convert.ToDecimal(lblTotalPrice.Text.Trim('$')));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Sale Completed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSales();
            }
        }

        private void LoadSales()
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT s.SaleID, c.FirstName, c.LastName, b.Title, s.Quantity, s.TotalPrice, s.SaleDate " +
                               "FROM Sales s " +
                               "JOIN Customers c ON s.CustomerID = c.CustomerID " +
                               "JOIN Books b ON s.BookID = b.BookID";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvSales.DataSource = dt;
            }
        }
        private void dgvSales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle cell click events if needed
        }
        


        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            if (cmbBook.SelectedItem != null && int.TryParse(txtQuantity.Text, out int quantity))
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT Price FROM Books WHERE BookID = @BookID";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@BookID", ((ComboBoxItem)cmbBook.SelectedItem).Value);
                    decimal price = Convert.ToDecimal(cmd.ExecuteScalar());

                    lblTotalPrice.Text = "$" + (price * quantity).ToString("0.00");
                }
            }
        }

        public class ComboBoxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public ComboBoxItem(string text, object value)
            {
                Text = text;
                Value = value;
            }

            public override string ToString()
            {
                return Text;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void SalesForm_Load(object sender, EventArgs e)
        {

        }

        private void btnSell_Click_1(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnSell_Click_2(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedItem == null || cmbBook.SelectedItem == null || string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Please select a customer, book, and enter quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO Sales (CustomerID, BookID, Quantity, TotalPrice) VALUES (@CustomerID, @BookID, @Quantity, @TotalPrice)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", ((ComboBoxItem)cmbCustomer.SelectedItem).Value);
                cmd.Parameters.AddWithValue("@BookID", ((ComboBoxItem)cmbBook.SelectedItem).Value);
                cmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtQuantity.Text));
                cmd.Parameters.AddWithValue("@TotalPrice", Convert.ToDecimal(lblTotalPrice.Text.Trim('$')));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Sale Completed!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSales();
            }
        }
    }
    }

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
    public partial class OrdersForm : Form
    {
        private Database db = new Database();

        public OrdersForm()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;

            InitializeComponent();
            LoadCustomers();
            LoadBooks();
            LoadOrders();
            LoadOrderStatuses(); 

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
            }
        }

        private void LoadBooks()
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT BookID, Title FROM Books";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    cmbBook.Items.Add(new ComboBoxItem(reader["Title"].ToString(), reader["BookID"]));
                }
            }
        }
    
    private void OrdersForm_Load(object sender, EventArgs e)
        {

        }

        private void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            if (cmbCustomer.SelectedItem == null || cmbBook.SelectedItem == null || string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Please select a customer, book, and enter quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO Orders (CustomerID, BookID, Quantity, Status) VALUES (@CustomerID, @BookID, @Quantity, 'Pending')";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CustomerID", ((ComboBoxItem)cmbCustomer.SelectedItem).Value);
                cmd.Parameters.AddWithValue("@BookID", ((ComboBoxItem)cmbBook.SelectedItem).Value);
                cmd.Parameters.AddWithValue("@Quantity", Convert.ToInt32(txtQuantity.Text));

                cmd.ExecuteNonQuery();
                MessageBox.Show("Order Placed Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadOrders();
            }
        }
        private void LoadOrders()
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT o.OrderID, c.FirstName, c.LastName, b.Title, o.Quantity, o.OrderDate, o.Status " +
                               "FROM Orders o " +
                               "JOIN Customers c ON o.CustomerID = c.CustomerID " +
                               "JOIN Books b ON o.BookID = b.BookID";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvOrders.DataSource = dt;
            }
        }
        private void LoadOrderStatuses()
        {
            cmbOrderStatus.Items.Add("Pending");
            cmbOrderStatus.Items.Add("Shipped");
            cmbOrderStatus.Items.Add("Delivered");
            cmbOrderStatus.Items.Add("Cancelled");
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
                return Text; // This ensures the ComboBox displays the text value
            }
        }

        private void btnUpdateStatus_Click_1(object sender, EventArgs e)
        {
            if (dgvOrders.SelectedRows.Count == 0 || cmbOrderStatus.SelectedItem == null)
            {
                MessageBox.Show("Select an order and a new status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "UPDATE Orders SET Status=@Status WHERE OrderID=@OrderID";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@OrderID", dgvOrders.SelectedRows[0].Cells[0].Value);
                cmd.Parameters.AddWithValue("@Status", cmbOrderStatus.SelectedItem.ToString());

                cmd.ExecuteNonQuery();
                MessageBox.Show("Order Status Updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadOrders();
            }
        }

        private void lblCustomer_Click(object sender, EventArgs e)
        {

        }
    }

}

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
using System.Windows.Forms.DataVisualization.Charting;

namespace BookHeavenApp
{
    public partial class ReportsForm : Form
    {
        private Database db = new Database();

        public ReportsForm()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;

            InitializeComponent();
            GenerateReport();

        }
        private void GenerateReport()
        {
            LoadTotalSales();
            LoadBestSellingBooks();
            LoadTopCustomers();
            LoadSalesChart();
        }

        private void LoadTotalSales()
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT SUM(TotalPrice) AS TotalSales FROM Sales";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                object result = cmd.ExecuteScalar();
                lblTotalSales.Text = "Total Sales: $" + (result != DBNull.Value ? Convert.ToDecimal(result).ToString("0.00") : "0.00");
            }
        }

        private void LoadBestSellingBooks()
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT b.Title, SUM(s.Quantity) AS TotalSold " +
                               "FROM Sales s JOIN Books b ON s.BookID = b.BookID " +
                               "GROUP BY b.Title ORDER BY TotalSold DESC LIMIT 5";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvBestSellers.DataSource = dt;
            }
        }
        private void LoadTopCustomers()
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT c.FirstName, c.LastName, SUM(s.TotalPrice) AS TotalSpent " +
                               "FROM Sales s JOIN Customers c ON s.CustomerID = c.CustomerID " +
                               "GROUP BY c.CustomerID ORDER BY TotalSpent DESC LIMIT 5";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvTopCustomers.DataSource = dt;
            }
        }
        private void LoadSalesChart()
        {
            // Clear previous chart data
            chartSales.Series.Clear();

            // Add a new series for monthly sales
            Series series = chartSales.Series.Add("Monthly Sales");
            series.ChartType = SeriesChartType.Line;

            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT DATE_FORMAT(SaleDate, '%Y-%m') AS Month, SUM(TotalPrice) AS Sales " +
                               "FROM Sales GROUP BY Month ORDER BY Month";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                bool dataFound = false;

                // Check if we have any data
                while (reader.Read())
                {
                    string month = reader["Month"].ToString();
                    object sales = reader["Sales"];

                    // Check if sales is not DBNull
                    if (sales != DBNull.Value)
                    {
                        series.Points.AddXY(month, Convert.ToDouble(sales)); // Add data point
                        dataFound = true;
                    }
                    else
                    {
                        Console.WriteLine("No sales data for month: " + month);
                    }
                }

                // If no data was found, display a message
                if (!dataFound)
                {
                    MessageBox.Show("No sales data available for the selected period.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // You can optionally log or debug if the data is being added
                    Console.WriteLine("Sales data loaded for chart.");
                }
            }
        }


        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void ReportsForm_Load(object sender, EventArgs e)
        {

        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }

        private void btnGenerateReport_Click_1(object sender, EventArgs e)
        {
            GenerateReport();
        }

        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            // Create a SaveFileDialog to specify the export location and filename
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            saveFileDialog.Title = "Export to CSV";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Create a DataTable from DataGridView (replace dgvBestSellers with your actual DataGridView name)
                DataTable dt = (DataTable)dgvBestSellers.DataSource;

                StringBuilder sb = new StringBuilder();

                // Add column headers to the CSV file
                foreach (DataColumn column in dt.Columns)
                {
                    sb.Append(column.ColumnName + ",");
                }

                sb.AppendLine();  // New line after headers

                // Add rows data to the CSV file
                foreach (DataRow row in dt.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        sb.Append(item.ToString() + ",");
                    }
                    sb.AppendLine();  // New line after each row
                }

                // Write the data to the CSV file
                File.WriteAllText(saveFileDialog.FileName, sb.ToString());

                MessageBox.Show("Exported to CSV successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnExporttCSV_Click(object sender, EventArgs e)
        {

        }

        private void btnExportToCSV_Click(object sender, EventArgs e)
        {
            // Create a SaveFileDialog to specify the export location and filename
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv";
            saveFileDialog.Title = "Export Customers to CSV";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Create a DataTable from the DataGridView (replace dgvCustomers with your actual DataGridView name)
                DataTable dt = (DataTable)dgvTopCustomers.DataSource;  // Update dgvCustomerData with your DataGridView name

                StringBuilder sb = new StringBuilder();

                // Add column headers to the CSV file
                foreach (DataColumn column in dt.Columns)
                {
                    sb.Append(column.ColumnName + ",");
                }

                sb.AppendLine();  // New line after headers

                // Add rows data to the CSV file
                foreach (DataRow row in dt.Rows)
                {
                    foreach (var item in row.ItemArray)
                    {
                        sb.Append(item.ToString() + ",");
                    }
                    sb.AppendLine();  // New line after each row
                }

                // Write the data to the CSV file
                File.WriteAllText(saveFileDialog.FileName, sb.ToString());

                MessageBox.Show("Customers Exported to CSV successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

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

namespace BookHeavenApp
{
    public partial class SuppliersForm : Form
    {
        private Database db = new Database();
        public SuppliersForm()
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;

            InitializeComponent();
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Suppliers";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgvSuppliers.DataSource = dt;
            }
        }
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void SuppliersForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO Suppliers (SupplierName, ContactPerson, ContactNumber, Email, Address) VALUES (@SupplierName, @ContactPerson, @ContactNumber, @Email, @Address)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SupplierName", txtSupplierName.Text);
                cmd.Parameters.AddWithValue("@ContactPerson", txtContactPerson.Text);
                cmd.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Supplier Added Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadSuppliers();
            }
        }

        private void btnUpdateSupplier_Click(object sender, EventArgs e)
        {
            if (dgvSuppliers.SelectedRows.Count > 0)
            {
                using (MySqlConnection conn = db.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE Suppliers SET SupplierName=@SupplierName, ContactPerson=@ContactPerson, ContactNumber=@ContactNumber, Email=@Email, Address=@Address WHERE SupplierID=@SupplierID";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@SupplierID", dgvSuppliers.SelectedRows[0].Cells[0].Value);
                    cmd.Parameters.AddWithValue("@SupplierName", txtSupplierName.Text);
                    cmd.Parameters.AddWithValue("@ContactPerson", txtContactPerson.Text);
                    cmd.Parameters.AddWithValue("@ContactNumber", txtContactNumber.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Supplier Updated Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadSuppliers();
                }
            }
            else
            {
                MessageBox.Show("Please select a supplier to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDeleteSupplier_Click(object sender, EventArgs e)
        {
            if (dgvSuppliers.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want to delete this supplier?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    using (MySqlConnection conn = db.GetConnection())
                    {
                        conn.Open();
                        string query = "DELETE FROM Suppliers WHERE SupplierID=@SupplierID";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@SupplierID", dgvSuppliers.SelectedRows[0].Cells[0].Value);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Supplier Deleted Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadSuppliers();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a supplier to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            if (dgvSuppliers.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV Files (*.csv)|*.csv";
                sfd.FileName = "Suppliers.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName))
                    {
                        // Write column headers
                        for (int i = 0; i < dgvSuppliers.Columns.Count; i++)
                        {
                            sw.Write(dgvSuppliers.Columns[i].HeaderText);
                            if (i < dgvSuppliers.Columns.Count - 1)
                                sw.Write(",");
                        }
                        sw.WriteLine();

                        // Write data rows
                        foreach (DataGridViewRow row in dgvSuppliers.Rows)
                        {
                            for (int i = 0; i < dgvSuppliers.Columns.Count; i++)
                            {
                                sw.Write(row.Cells[i].Value);
                                if (i < dgvSuppliers.Columns.Count - 1)
                                    sw.Write(",");
                            }
                            sw.WriteLine();
                        }
                    }
                    MessageBox.Show("Exported Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("No records found to export.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

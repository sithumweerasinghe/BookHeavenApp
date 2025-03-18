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
    public partial class ClerkDashboard : Form
    {
        public ClerkDashboard()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            BooksForm reportsForm = new BooksForm();
            reportsForm.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SalesForm reportsForm = new SalesForm();
            reportsForm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BooksForm reportsForm = new BooksForm();
            reportsForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OrdersForm reportsForm = new OrdersForm();
            reportsForm.ShowDialog();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            OrdersForm reportsForm = new OrdersForm();
            reportsForm.ShowDialog();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            SalesForm reportsForm = new SalesForm();
            reportsForm.ShowDialog();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // Prompt user to confirm logout
            var confirmResult = MessageBox.Show("Are you sure you want to logout?",
                                                 "Logout Confirmation",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);

            // If the user confirms, log out and show the login screen
            if (confirmResult == DialogResult.Yes)
            {
                // Clear any session data if necessary
                // For example, clearing username, role, etc.
                // SessionManager.ClearUserSession(); // You can create a session management class

                // Hide the current form (Main Form)
                this.Hide();

                // Create and show the login form
                LoginForm loginForm = new LoginForm();
                loginForm.Show();

                // Optionally, close the current form (if you don't want it to remain open)
                // this.Close();
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace REIC_POMS
{
    public partial class LoginScreen : Form
    {
        private MySQLDatabaseDriver sql;

        public LoginScreen()
        {
            InitializeComponent();
            sql = new MySQLDatabaseDriver();
            //sql.Restore();
            ConnectionStringManager.SetMySQLConString("localhost", "root", "", "reicpoms");
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                sql.Backup();
                this.Close(); //Exit the program
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //JUST TO TRANSITION TO THE MAIN SCREEN
            //if ((txtUsername.Text == "wennie") && (txtPassword.Text == "1234"))
            if ((txtUsername.Text == "Username") && (txtPassword.Text == "Password")) //Temporarily changed it, kasi kakatamad to input wennie and 1234 all the time
            {
                Item_MainScreen ims = new Item_MainScreen();
                this.Hide(); //Actually closes the Log-in Form instead of leaving it open in the background.
                ims.ShowDialog();
                this.Close(); //Close the Log-in Screen
            }
            else
                MessageBox.Show("The username or password you entered is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            //Insert validation for Admin account
            DatabaseSettingsForm settings = new DatabaseSettingsForm();
            settings.ShowDialog();
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using MySql.Data.MySqlClient;

namespace REIC_POMS
{
    public partial class Item_AddForm : Form
    {
        private MySQLDatabaseDriver sql;
        private ArrayList supplierList;
        private ArrayList supplierDropdownList;
        private bool cancel;
        private bool filledOut;
        private int dateResult;
        private int supplierIDFK;

        public Item_AddForm()
        {
            InitializeComponent();
            sql = new MySQLDatabaseDriver();

            supplierList = new ArrayList();
            sql.SelectAllSuppliers(supplierList);

            supplierDropdownList = new ArrayList();
            sql.SelectAllSupplierNames(supplierDropdownList);
            supplierDropdownList.Sort(); //Sort list alphabetically
            supplierDropdownList.Insert(0, "Select Supplier");
            cbbSupplierName.DataSource = supplierDropdownList; //Populate the dropdown

            /*
            try
            {
                FileStream fs = new FileStream(@"supplier.txt", FileMode.Open);
                StreamReader readin = new StreamReader(fs);
                while (!readin.EndOfStream)
                {
                    string[] text = readin.ReadLine().Split('|');
                    supplierList.Add(new Supplier(int.Parse(text[0]), text[1], text[2], text[3], text[4], text[5])); //Recreate the Supplier
                    supplierDropdownList.Add(text[1]); //Just the Supplier Names
                }
                readin.Close();
                fs.Close();
                supplierDropdownList.Sort(); //Sort list alphabetically
                supplierDropdownList.Insert(0, "Select Supplier");
                cbbSupplierName.DataSource = supplierDropdownList; //Populate the dropdown w/ all Supplier Names
            }
            catch (Exception e) { }*/

        }

        private void Item_AddForm_Load(object sender, EventArgs e)
        { }

        public void SetMyCustomFormat()
        {
            // Set the Format type and the CustomFormat string.
            dtpFromDate.Format = DateTimePickerFormat.Custom;
            dtpToDate.Format = DateTimePickerFormat.Custom;
            dtpFromDate.CustomFormat = "MM/dd/yyyy";
            dtpToDate.CustomFormat = "MM/dd/yyyy";
        }

        private void cbbUOM_SelectedIndexChanged(object sender, EventArgs e)
        {
           // cbbUOM.SelectedIndex = 0;
        }

        /*
        we use DateTime, para we could compare two dates if FromDate is earlier than ToDate through DateTime.Compare 
        pero if we could think of ways pa how to compare this as a String, we could just put it as "dtpFromDate.Value.ToString()"
        */
        public string PartNumber { get { return txtPartNumber.Text; } }
        public string ItemName { get { return txtItemName.Text; } }
        public string SupplierUnitPrice { get { return nudSuppPrice.Value.ToString(); } }
        public string Markup { get { return cbbMarkup.Text; } }
        public string ReicUnitPrice { get { return txtREICPrice.Text; } }
        public string Moq { get { return nudMOQ.Value.ToString(); } }
        public string Uom { get { return cbbUOM.Text; } }
        public DateTime FromDate { get { return dtpFromDate.Value; } }
        public DateTime ToDate { get { return dtpToDate.Value; } }
        public string ItemDescription { get { return txtItemDesc.Text; } }
        public string SupplierName { get { return cbbSupplierName.Text; } }
        /*public string SupplierPerson { get { return txtSupplierPerson.Text; } }
        public string SupplierNumber { get { return txtSupplierNumber.Text; } }
        public string SupplierEmail { get { return txtSupplierEmail.Text; } }
        public string SupplierAddress { get { return txtSupplierAddress.Text; } }*/
        public string SupplierPerson
        {
            set { txtSupplierPerson.Text = value; }
            get { return txtSupplierPerson.Text; }
        }

        public string SupplierNumber
        {
            set { txtSupplierNumber.Text = value; }
            get { return txtSupplierNumber.Text; }
        }

        public string SupplierEmail
        {
            set { txtSupplierEmail.Text = value; }
            get { return txtSupplierEmail.Text; }
        }

        public string SupplierAddress
        {
            set { txtSupplierAddress.Text = value; }
            get { return txtSupplierAddress.Text; }
        }

        public int SupplierIDFK
        {
            set { supplierIDFK = value; }
            get { return supplierIDFK; }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cancel = true;
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            do
            {
                dateResult = DateTime.Compare(FromDate, ToDate);
                if (
                    (PartNumber.Length == 0) ||
                    (ItemName.Length == 0) ||
                    (SupplierUnitPrice.Length == 0) ||
                    (Markup.Length == 0) ||
                    (ReicUnitPrice.Length == 0) ||
                    (Moq.Length == 0) ||
                    (Uom.Length == 0) ||
                    (ItemDescription.Length == 0) ||
                    (SupplierName.Length == 0) ||
                    (SupplierPerson.Length == 0) ||
                    (SupplierNumber.Length == 0) ||
                    (SupplierEmail.Length == 0) ||
                    (SupplierAddress.Length == 0)
                   )
                {
                    
                    DialogResult result = MessageBox.Show("All fields are required to be filled out.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (result == DialogResult.OK)
                    {
                        return;
                    }
                }
                else if (dateResult > 0)
                {
                    DialogResult result = MessageBox.Show("dont be stupid, fromDate later than toDate, ano yan joke!.", "Bawal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (result == DialogResult.OK)
                    {
                        return;
                    }
                }
                else if (dateResult == 0)
                {
                    DialogResult result = MessageBox.Show("dont be stupid, fromDate later equal toDate, ano yan joke!.", "Bawal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (result == DialogResult.OK)
                    {
                        return;
                    }
                }

                else { filledOut = true; }

            } while ((filledOut == false)||(dateResult > 0)||(dateResult ==0));

            SupplierIDFK = sql.SelectSupplierID(SupplierName);
            cancel = false;
            this.Close();
        }

        public bool Cancel
        {
            get { return cancel; }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabItemForm.SelectedTab = tabSupplierDetails;
        }

        private void nudSuppPrice_ValueChanged(object sender, EventArgs e)
        {
            CalculateAndUpdateUI();
        }

        private void cbbMarkup_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateAndUpdateUI();
        }

        void CalculateAndUpdateUI()
        {
           
            if ((SupplierUnitPrice.Length != 0) || (Markup.Length != 0))
            {

                double suppUnitPrice, markup, reicPrice;
                suppUnitPrice = 0;
                reicPrice = 0;
                markup = 0;

                double.TryParse(SupplierUnitPrice, out suppUnitPrice);
                double.TryParse(Markup, out markup);

                reicPrice = suppUnitPrice + (suppUnitPrice * markup/100);
                
                txtREICPrice.Text = reicPrice.ToString("0.00");
            }
        }

       private void txtREICPrice_TextChanged(object sender, EventArgs e)
        {
            CalculateAndUpdateUI();
        }

        private void cbbSupplierName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbSupplierName.Text == "Select Supplier")
            {
                SupplierPerson = null;
                SupplierNumber = null;
                SupplierEmail = null;
                SupplierAddress = null;
                return;
            }

            for (int i = 0; i < supplierList.Count; i++)
            {
                Supplier s = (Supplier)supplierList[i]; 
                if (s.SupplierName == cbbSupplierName.Text)
                {
                    SupplierPerson = s.SupplierPerson;
                    SupplierNumber = s.SupplierNumber;
                    SupplierEmail = s.SupplierEmail;
                    SupplierAddress = s.SupplierAddress;
                    break; 
                }
            }
        }
    }
}

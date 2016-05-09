﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data; //Enables use of DataTable
using MySql.Data.MySqlClient; //Enables connection to MySQL
using System.Windows.Forms;
using System.Collections; //Enables use of ArrayList

namespace REIC_POMS
{
    class MySQLDatabaseDriver
    {
        private MySqlConnection connection;
        private MySqlCommand command;
        private MySqlCommand countCommand; //Added (used in Select methods)
        private MySqlDataReader myReader; //Added
        private MySqlDataReader countReader; //Added (used in Select methods)

        //----------------
        //  CODING NOTES |
        //----------------
        /*  ExecuteNonQuery
                - Used for operations where there is nothing returned from the SQL Query or Stored Procedure.
                - Preferred use will be for INSERT, UPDATE and DELETE Operations.
            ExecuteScalar
                - When you just need one Cell value i.e. one column and one row.
                * For SelectCustomerID and SelectSupplierID
        */

        //------------------------
        //  CONNECT & DISCONNECT |
        //------------------------
        public void ConnectToSQL() //Activate the connection between VS and MySQL
        {
            connection = new MySqlConnection("server=localhost; database=reicpoms; user=root; password=; convert zero datetime=true; allow zero datetime=true;");
            connection.Open();

            //DEBUG MESSAGES
            if (connection.State == System.Data.ConnectionState.Open)
            { MessageBox.Show("Connection to SQL successful!"); }
            else
            { MessageBox.Show("Connection to SQL failed!"); }
        }

        public void DisconnectFromSQL()
        {
            connection.Close();
        }

        //---------------------
        //  SELECT STATEMENTS |
        //---------------------
        public int GetRowCount(string tableName, string year, string month)
        { //Purpose: Aid in autogeneration of RFQ, PQ, and PO numbers
            ConnectToSQL();
            string yearMonth = year + month;
            MessageBox.Show("Yearmonth:" + yearMonth);
            switch (tableName)
            {
                case "rfq_t":
                    countCommand = new MySqlCommand(string.Format("SELECT COUNT(*) FROM rfq_t WHERE rfq_no LIKE '{0}%'" + ";", yearMonth), connection);
                    break;
                case "pq_t":
                    countCommand = new MySqlCommand(string.Format("SELECT COUNT(*) FROM pq_t WHERE pq_no LIKE '{0}%'" + ";", yearMonth), connection);
                    break;
                case "po_t":
                    countCommand = new MySqlCommand(string.Format("SELECT COUNT(*) FROM po_t WHERE po_no LIKE '{0}%'" + ";", yearMonth), connection);
                    break;
            }
            object countReader = countCommand.ExecuteScalar();
            int rowCount = int.Parse(countReader.ToString());
            MessageBox.Show(rowCount.ToString()); //Debug purposes
            DisconnectFromSQL();
            return rowCount;
        }

        public void SelectAllItems(ArrayList result)
        { //Purpose: Streamreader data from database into ArrayList
            ConnectToSQL();
            command = new MySqlCommand("SELECT * FROM item_t;", connection);
            myReader = command.ExecuteReader();
            while (myReader.Read())
            {
                result.Add(new Item(myReader["part_number"].ToString(),
                                    myReader["item_name"].ToString(),
                                    myReader["item_description"].ToString(),
                                    double.Parse(myReader["supplier_unit_price"].ToString()),
                                    int.Parse(myReader["mark_up_percentage"].ToString()),
                                    double.Parse(myReader["reic_unit_price"].ToString()),
                                    int.Parse(myReader["minimum_order_quantity"].ToString()),
                                    myReader["unit_of_measurement"].ToString(),
                                    myReader["from_date"].ToString(),
                                    myReader["to_date"].ToString(),
                                    int.Parse(myReader["supplier_id"].ToString())));
            }
            DisconnectFromSQL();
        }

        public void SelectAllCustomers(ArrayList result)
        {
            ConnectToSQL();
            command = new MySqlCommand("SELECT * FROM customer_t;", connection);
            myReader = command.ExecuteReader();
            while (myReader.Read())
            {
                result.Add(new Customer(int.Parse(myReader["customer_id"].ToString()),
                                        myReader["business_name_style"].ToString(),
                                        myReader["tin_number"].ToString(),
                                        myReader["company_name"].ToString(),
                                        myReader["contact_person"].ToString(),
                                        myReader["contact_number"].ToString(),
                                        myReader["account_number"].ToString(),
                                        myReader["email_address"].ToString(),
                                        myReader["address"].ToString()));
            }
            DisconnectFromSQL();
        }

        public void SelectAllCustomerNames(ArrayList result)
        {
            ConnectToSQL();
            command = new MySqlCommand("SELECT company_name FROM customer_t;", connection);
            myReader = command.ExecuteReader();
            while (myReader.Read())
            {
                result.Add(myReader[0].ToString());
            }
            DisconnectFromSQL();
        }

        public int SelectCustomerID(string customerName)
        {
            ConnectToSQL();
            int idResult;
            command = new MySqlCommand(string.Format("SELECT customer_id FROM customer_t WHERE company_name='{0}';", customerName), connection);
            object queryResult = command.ExecuteScalar();
            MessageBox.Show("Customer ID: " + queryResult.ToString()); //Debug purposes
            idResult = int.Parse(queryResult.ToString());
            DisconnectFromSQL();
            return idResult;
        }

        public void SelectAllSuppliers(ArrayList result)
        {
            ConnectToSQL();
            command = new MySqlCommand("SELECT * FROM supplier_t;", connection);
            myReader = command.ExecuteReader();
            while (myReader.Read())
            {
                result.Add(new Supplier(int.Parse(myReader["supplier_id"].ToString()),
                                        myReader["supplier_name"].ToString(),
                                        myReader["contact_person"].ToString(),
                                        myReader["contact_number"].ToString(),
                                        myReader["email_address"].ToString(),
                                        myReader["address"].ToString()));
            }
            DisconnectFromSQL();
        }

        public void SelectAllSupplierNames(ArrayList result)
        {
            ConnectToSQL();
            command = new MySqlCommand("SELECT supplier_name FROM supplier_t;", connection);
            myReader = command.ExecuteReader();
            while (myReader.Read())
            {
                result.Add(myReader[0].ToString());
            }
            DisconnectFromSQL();
        }

        public int SelectSupplierID(string supplierName)
        {
            ConnectToSQL();
            int idResult;
            command = new MySqlCommand(string.Format("SELECT supplier_id FROM supplier_t WHERE supplier_name='{0}';", supplierName), connection);
            object queryResult = command.ExecuteScalar();
            MessageBox.Show("Supplier ID: " + queryResult.ToString()); //Debug purposes
            idResult = int.Parse(queryResult.ToString());
            DisconnectFromSQL();
            return idResult;
        }

        public Supplier SelectSupplierDetails(int id)
        { //Retrieves a specific Supplier's row | Used in View Forms of RFQ and PO
            ConnectToSQL();
            Supplier s;
            command = new MySqlCommand(string.Format("SELECT * FROM supplier_t WHERE supplier_id={0};", id), connection);
            myReader = command.ExecuteReader();
            myReader.Read();
            s = new Supplier(id,
                             myReader["supplier_name"].ToString(),
                             myReader["contact_person"].ToString(),
                             myReader["contact_number"].ToString(),
                             myReader["email_address"].ToString(),
                             myReader["address"].ToString());
            DisconnectFromSQL();
            return s;
        }

        public void SelectAllRFQ(ArrayList result)
        { //For the ArrayList containing ALL RFQ | Used to set values of View Form's labels
            ConnectToSQL();
            command = new MySqlCommand("SELECT * FROM rfq_t;", connection);
            myReader = command.ExecuteReader();
            while (myReader.Read())
            {
                result.Add(new RFQ(myReader["rfq_no"].ToString(),
                                   myReader["date_of_request"].ToString(),
                                   myReader["payment_terms"].ToString(),
                                   myReader["delivery_terms"].ToString(),
                                   int.Parse(myReader["customer_id"].ToString()),
                                   int.Parse(myReader["supplier_id"].ToString())));
            }
            DisconnectFromSQL();
        }

        public void SelectAllRFQDGV(DataGridView dgvName)
        { //To populate the RFQ Main Screen DGV (Plus the actual Customer & Supplier names, not their IDs)
            ConnectToSQL();
            command = new MySqlCommand("SELECT rfq_no, DATE_FORMAT(date_of_request, '%m/%d/%Y'), supplier_name, company_name " +
                                       "FROM rfq_t, customer_t, supplier_t " +
                                       "WHERE rfq_t.customer_id = customer_t.customer_id " +
                                       "AND rfq_t.supplier_id = supplier_t.supplier_id;", connection);
            myReader = command.ExecuteReader();
            while (myReader.Read())
            {
                dgvName.Rows.Add(myReader["rfq_no"].ToString(),
                                 myReader["DATE_FORMAT(date_of_request, '%m/%d/%Y')"].ToString(),
                                 myReader["supplier_name"].ToString(),
                                 myReader["company_name"].ToString());
            }
            DisconnectFromSQL();
        }

        public RFQ SelectRFQDetails(string rfqNo) //NEWLY ADDED
        { //Retrieves a specific RFQ's row | Used in View Forms of RFQ
            ConnectToSQL();
            RFQ r;
            command = new MySqlCommand(string.Format("SELECT rfq_no, DATE_FORMAT(date_of_request, '%m/%d/%Y'), payment_terms, delivery_terms, customer_id, supplier_id FROM rfq_t WHERE rfq_no={0};", rfqNo), connection);
            //SELECT Statement is the same as "SELECT * FROM rfq_t WHERE rfq_no={0};" Had to mention all, since will format the date
            myReader = command.ExecuteReader();
            myReader.Read();
            r = new RFQ(rfqNo,
                        myReader["date_of_request"].ToString(),
                        myReader["payment_terms"].ToString(),
                        myReader["delivery_terms"].ToString(),
                        int.Parse(myReader["customer_id"].ToString()),
                        int.Parse(myReader["supplier_id"].ToString()));
            DisconnectFromSQL();
            return r;
        }

        public void SelectSpecificRFQOrderLine(string rfqNo, DataGridView dgvName)
        { //Retrieves all OrderLines of a specific RFQ | Used in View Forms
            ConnectToSQL();
            command = new MySqlCommand(string.Format("SELECT item_name, item_description, unit_of_measurement, quantity " +
                                                     "FROM rfq_order_line_t, item_t " +
                                                     "WHERE rfq_no='{0}' " +
                                                     "AND rfq_order_line_t.part_number = item_t.part_number; ", rfqNo), connection);
            myReader = command.ExecuteReader();
            while (myReader.Read())
            {
                dgvName.Rows.Add(myReader["item_name"].ToString(),
                                 myReader["item_description"].ToString(),
                                 myReader["unit_of_measurement"].ToString(),
                                 int.Parse(myReader["quantity"].ToString()));
                MessageBox.Show("Added");
            }
            DisconnectFromSQL();
        }

        public void SelectAllPQ(ArrayList result)
        { //For the ArrayList containing ALL PQ

        }

        public void SelectAllPQDGV(DataGridView dgvName)
        { //To populate the RFQ Main Screen DGV (Plus the actual Customer names, not their IDs)

        }

        public void SelectAllPO(ArrayList result)
        { //For the ArrayList containing ALL PO

        }

        public void SelectAllPODGV(DataGridView dgvName)
        { //To populate the PO Main Screen DGV (Plus the actual Customer and Supplier names, not their IDs)

        }

        public void SelectAllSIDR(ArrayList result)
        { //For the ArrayList containing ALL SIDR

        }

        public void SelectAllSIDRDGV(DataGridView dgvName)
        { //To populate the SIDR Main Screen DGV (Plus the actual Customer names, not their IDs)

        }

        //---------------------
        //  INSERT STATEMENTS |
        //---------------------
        public void Insert(string insertStatement)
        {
            try
            {
                ConnectToSQL();
                command = new MySqlCommand(insertStatement, connection);
                command.ExecuteNonQuery();
                MessageBox.Show("Save successful.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            DisconnectFromSQL();
        }

        public void InsertItem(Item i)
        {
            //Dates must be inserted as SQL's format: YYYY-MM-DD
            Insert(string.Format("INSERT INTO item_t " +
                "(part_number, item_name, item_description, supplier_unit_price, mark_up_percentage, reic_unit_price, minimum_order_quantity, unit_of_measurement, from_date, to_date, supplier_id) " +
                "VALUES ('{0}', '{1}', '{2}', {3}, {4}, {5}, {6}, '{7}', '{8}', '{9}', {10});", i.PartNumber,
                                                                                               i.ItemName,
                                                                                               i.ItemDescription,
                                                                                               i.SupplierUnitPrice,
                                                                                               i.Markup,
                                                                                               i.ReicUnitPrice,
                                                                                               i.Moq,
                                                                                               i.Uom,
                                                                                               i.FromDateNoTime,
                                                                                               i.ToDateNoTime,
                                                                                               i.SupplierID));
        }
    
        public void InsertSupplier(Supplier s)
        {
            Insert(string.Format("INSERT INTO supplier_t " +
                "(supplier_id, supplier_name, contact_person, contact_number, email_address, address) " + 
                "VALUES ({0}, '{1}', '{2}', '{3}', '{4}', '{5}');", s.SupplierID,
                                                                    s.SupplierName,
                                                                    s.SupplierPerson,
                                                                    s.SupplierNumber,
                                                                    s.SupplierEmail,
                                                                    s.SupplierAddress));
        }

        public void InsertCustomer(Customer c)
        {
            Insert(string.Format("INSERT INTO customer_t " +
                   "(customer_id, business_name_style, tin_number, company_name, contact_person, contact_number, account_number, email_address, address) " +
                   "VALUES ({0}, '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}');", c.CustomerID,
                                                                                            c.BusinessName,
                                                                                            c.TinNumber,
                                                                                            c.CustomerName,
                                                                                            c.CustomerPerson,
                                                                                            c.CustomerNumber,
                                                                                            c.AccountNumber,
                                                                                            c.CustomerEmail,
                                                                                            c.CustomerAddress));
        }

        public void InsertRFQ(RFQ r)
        {
            //Have to convert any date from REIC's format to SQL's format (or else, what'll be saved into database is '0000-00-00')
            string[] dateParts = r.RequestDate.Split('/'); //REIC's format: 05/06/2016 (MM/DD/YYYY)
            string convertedDate = dateParts[2] + "-" + dateParts[0] + "-" + dateParts[1]; //SQL's format: 2016-05-06 (YYYY-MM-DD)
            Insert(string.Format("INSERT INTO rfq_t " +
                "(rfq_no, date_of_request, payment_terms, delivery_terms, customer_id, supplier_id) " + 
                "VALUES ('{0}', '{1}', '{2}', '{3}', {4}, {5});", r.RFQNo,
                                                                  convertedDate,
                                                                  r.PaymentTerms,
                                                                  r.DeliveryTerms,
                                                                  r.CustomerID,
                                                                  r.SupplierID));
        }

        public void InsertRFQOrderLine(RFQ_OrderLine rol)
        {
            Insert(string.Format("INSERT INTO rfq_order_line_t " +
                "(rfq_no, part_number, quantity) " +
                "VALUES ('{0}', '{1}', {2});", rol.RFQNo, rol.PartNumber, rol.Quantity));
        }

        public void InsertPQ(PQ p)    
        {
            
        }

        /*
        public void InsertPQOrderLine(PQ_OrderLine pol)
        {

        }

        public void InsertPO(PO p)
        {
            //If will add Account Number, place it after Payment Terms
            //If will add Supplier ID, place it after Order Total
        }

        public void InsertPOOrderLine(PO_OrderLine pol)
        {

        }

        public void InsertSIDR(SIDR s)
        {

        }

        public void InsertSIDROrderLine(SIDR_OrderLine sol)
        {

        }
        */

        //---------------------
        //  UPDATE STATEMENTS |
        //---------------------
        public void Update(string updateStatement)
        {
            try
            {
                ConnectToSQL();
                command = new MySqlCommand(updateStatement, connection);
                command.ExecuteNonQuery();
                MessageBox.Show("Update successful.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            DisconnectFromSQL();
        }

        public void UpdateItem(Item i)
        {

        }

        public void UpdateCustomer(Customer c)
        {
            Update(string.Format("UPDATE customer_t " +
               "SET business_name_style='{0}', tin_number='{1}', company_name='{2}', contact_person='{3}', contact_number='{4}', account_number='{5}', email_address='{6}', address='{7}' " +
               "WHERE customer_id={8};", c.BusinessName,
                                         c.TinNumber,
                                         c.CustomerName,
                                         c.CustomerPerson,
                                         c.CustomerNumber,
                                         c.AccountNumber,
                                         c.CustomerEmail,
                                         c.CustomerAddress,
                                         c.CustomerID));
        }

        public void UpdateSupplier(Supplier s)
        {
            Update(string.Format("UPDATE supplier_t " +
               "SET supplier_name='{0}', contact_person='{1}', contact_number='{2}', email_address='{3}', address='{4}' " +
               "WHERE supplier_id={5};", s.SupplierID,
                                         s.SupplierName,
                                         s.SupplierPerson,
                                         s.SupplierNumber,
                                         s.SupplierEmail,
                                         s.SupplierAddress));
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace REIC_POMS
{
    class RFQ
    {
        //CONSTRUCTOR
        public RFQ(string rfqNo,
                   string requestDate, //In Data Dictionary, this is a DATE. What did Ken use in the Item class? HELP.
                   string paymentTerms,
                   string accountNumber, //DOUBLE-CHECK
                   string deliveryTerms,
                   int customerID, //Foreign Key
                   int supplierID) //Foreign Key
                   /* Old (Before I realized we should base it from the Data Dictionary)
                   //string inFavorOf, No longer neededed
                   string customerName, //ENIGMA
                   string supplierName, //ENIGMA (plus its other fields)
                   string supplierPerson,
                   string supplierNumber,
                   string supplierEmail,
                   string supplierAddress,
                   ArrayList rfqItems) */  

        {
            RFQNo = rfqNo;
            RequestDate = requestDate;
            PaymentTerms = paymentTerms;
            AccountNumber = accountNumber;
            DeliveryTerms = deliveryTerms;
            CustomerID = customerID;
            SupplierID = supplierID;
            /*InFavorOf = inFavorOf;
            CustomerName = customerName;
            SupplierName = supplierName;
            SupplierPerson = supplierPerson;
            SupplierNumber = supplierNumber;
            SupplierEmail = supplierEmail;
            SupplierAddress = supplierAddress;
            RFQItems = rfqItems;*/
        }

        //PROPERTIES
        public string RFQNo { get; set; }
        public string RequestDate { get; set; } //In Data Dictionary, this is a DATE.
        public string PaymentTerms { get; set; }
        public string AccountNumber { get; set; }
        public string DeliveryTerms { get; set; }
        public int CustomerID { get; set; }
        public int SupplierID { get; set; }
        /*public string InFavorOf { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string SupplierPerson { get; set; }
        public string SupplierNumber { get; set; }
        public string SupplierEmail { get; set; }
        public string SupplierAddress { get; set; }
        public ArrayList RFQItems { get; set; }*/
    }
}
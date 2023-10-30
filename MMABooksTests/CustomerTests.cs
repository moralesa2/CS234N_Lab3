using NUnit.Framework;

using MMABooksBusiness;
using MMABooksProps;
using MMABooksDB;

using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;

using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace MMABooksTests
{
    [TestFixture]
    internal class CustomerTests
    {
        [SetUp]
        public void TestResetDatabase()
        {
            CustomerDB db = new CustomerDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestNewCustomerConstructor()
        {
            Customer c = new Customer();
            Assert.AreEqual(string.Empty, c.Address);
            Assert.AreEqual(string.Empty, c.Name);
            Assert.IsTrue(c.IsNew);
            Assert.IsFalse(c.IsValid);
        }

        
        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Customer c = new Customer(1);
            Assert.IsTrue(c.Name.Length > 0);
            Assert.AreEqual("Molunguri, A", c.Name);
            Assert.AreEqual("1108 Johanna Bay Drive", c.Address);
            Assert.IsFalse(c.IsNew);
            Assert.IsTrue(c.IsValid);
        }

        [Test]
        public void TestSaveToDataStore()
        {
            Customer c = new Customer();
            c.Name = "Tobe Deleted";
            c.Address = "101 Test Delete";
            c.City = "Deletecity";
            c.State = "OR";
            c.Zipcode = "10001";
            c.Save();
            Customer c2 = new Customer(c.CustomerID);
            Assert.AreEqual(c2.Address, c.Address);
            Assert.AreEqual(c2.Name, c.Name);
        }

        [Test]
        public void TestUpdate()
        {
            Customer c = new Customer(1);
            c.Name = "Edited Name";
            c.Address = "Edited Address";
            c.Save();

            Customer c2 = new Customer(1);
            Assert.AreEqual(c2.Name, c.Name);
            Assert.AreEqual(c2.Address, c.Address);
        }

        [Test]
        public void TestDelete()
        {
            Customer c = new Customer();
            c.Name = "Tobe Deleted";
            c.Address = "101 Test Delete";
            c.City = "Deletecity";
            c.State = "OR";
            c.Zipcode = "10001";

            c.Delete();
            c.Save();
            Assert.Throws<Exception>(() => new Customer(c.CustomerID));
        }


        [Test]
        public void TestGetList()
        {
            Customer c = new Customer();
            List<Customer> customers = (List<Customer>)c.GetList();
            Assert.AreEqual(696, customers.Count);
            Assert.AreEqual("Abeyatunge, Derek", customers[0].Name);
            Assert.AreEqual(157, customers[0].CustomerID);
        }


        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Customer c = new Customer();
            Assert.Throws<Exception>(() => c.Save());
        }


        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Customer c = new Customer();
            Assert.Throws<Exception>(() => c.Save());
            c.Name = "Test Name";
            Assert.Throws<Exception>(() => c.Save());
        }

        [Test]
        public void TestInvalidPropertySet()
        {
            Customer c = new Customer();
            Assert.Throws<ArgumentOutOfRangeException>(() => c.Name = "01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890");
            Assert.Throws<ArgumentOutOfRangeException>(() => c.Address = "012345678901234567890123456789012345678901234567890");
            Assert.Throws<ArgumentOutOfRangeException>(() => c.City = "012345678901234567890");
            Assert.Throws<ArgumentOutOfRangeException>(() => c.State = "ORE");
            Assert.Throws<ArgumentOutOfRangeException>(() => c.Zipcode = "0123456789012345");

        }

        [Test]
        public void TestConcurrencyIssue()
        {
            Customer c1 = new Customer(1);
            Customer c2 = new Customer(1);

            c1.Name = "Updated first";
            c1.Save();

            c2.Name = "Updated second";
            Assert.Throws<Exception>(() => c2.Save());
        }
    }
}

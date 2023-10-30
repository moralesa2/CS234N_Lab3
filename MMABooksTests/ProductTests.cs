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
    internal class ProductTests
    {
        [SetUp]
        public void TestResetDatabase()
        {
            ProductDB db = new ProductDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetProductData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestNewProductConstructor()
        {
            Product p = new Product();
            Assert.AreEqual(string.Empty, p.ProductCode);
            Assert.AreEqual(string.Empty, p.Description);
            Assert.IsTrue(p.IsNew);
            Assert.IsFalse(p.IsValid);
        }


        [Test]
        public void TestRetrieveFromDataStoreContructor()
        {
            // retrieves from Data Store
            Product p = new Product(1);
            Assert.IsTrue(p.ProductCode.Length > 0);
            Assert.AreEqual("A4CS", p.ProductCode);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", p.Description);
            Assert.IsFalse(p.IsNew);
            Assert.IsTrue(p.IsValid);
        }

        [Test]
        public void TestSaveToDataStore()
        {
            Product p = new Product();
            p.ProductCode = "T3ST";
            p.Description = "This is a Test Product";
            p.UnitPrice = 100.01m;
            p.OnHandQuantity = 1;
            p.Save();
            Product p2 = new Product(p.ProductID);
            Assert.AreEqual(p2.ProductCode, p.ProductCode);
            Assert.AreEqual(p2.Description, p.Description);
        }


        [Test]
        public void TestUpdate()
        {
            Product p = new Product(1);
            p.ProductCode = "Test Code";
            p.Description = "Edited Description";
            p.Save();

            Product p2 = new Product(1);
            Assert.AreEqual(p2.ProductCode, p.ProductCode);
            Assert.AreEqual(p2.Description, p.Description);
        }

        [Test]
        public void TestDelete()
        {
            Product p = new Product(1);
            p.Delete();
            p.Save();
            Assert.Throws<Exception>(() => new Product(1));
        }

        [Test]
        public void TestGetList()
        {
            Product p = new Product();
            List<Product> Products = (List<Product>)p.GetList();
            Assert.AreEqual(16, Products.Count);
            Assert.AreEqual("A4CS", Products[0].ProductCode);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", Products[0].Description);
        }

        [Test]
        public void TestNoRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Product p = new Product();
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestSomeRequiredPropertiesNotSet()
        {
            // not in Data Store - abbreviation and name must be provided
            Product p = new Product();
            Assert.Throws<Exception>(() => p.Save());
            p.ProductCode = "??";
            Assert.Throws<Exception>(() => p.Save());
        }

        [Test]
        public void TestInvalidPropertySet()
        {
            Product p = new Product();
            Assert.Throws<ArgumentOutOfRangeException>(() => p.ProductCode = "01234567890");
            Assert.Throws<ArgumentOutOfRangeException>(() => p.Description = "012345678901234567890123456789012345678901234567890");
            Assert.Throws<ArgumentOutOfRangeException>(() => p.UnitPrice = -1);
            Assert.Throws<ArgumentOutOfRangeException>(() => p.OnHandQuantity = -1);
        }

        [Test]
        public void TestConcurrencyIssue()
        {
            Product p1 = new Product(1);
            Product p2 = new Product(1);

            p1.Description = "Updated first";
            p1.Save();

            p2.Description = "Updated second";
            Assert.Throws<Exception>(() => p2.Save());
        }
    }
}

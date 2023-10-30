using NUnit.Framework;

using MMABooksProps;
using MMABooksDB;

using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;

using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;
using System.Runtime.Intrinsics.Arm;

namespace MMABooksTests
{
    [TestFixture()]
    internal class ProductDBTests
    {
        ProductDB db;

        [SetUp]
        public void ResetData()
        {
            db = new ProductDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetProductData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieve()
        {
            ProductProps p = (ProductProps)db.Retrieve(1);
            Assert.AreEqual("A4CS", p.ProductCode);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", p.Description);
        }

        [Test]
        public void TestRetrieveAll()
        {
            List<ProductProps> list = (List<ProductProps>)db.RetrieveAll();
            Assert.AreEqual(16, list.Count);
        }

        [Test]
        public void TestCreate()
        {
            ProductProps p = new ProductProps();
            p.ProductID = 1;
            p.ProductCode = "T3ST";
            p.Description = "This is a Test Product";
            p.UnitPrice = 100.0000m;
            p.OnHandQuantity = 1;
            db.Create(p);
            ProductProps p2 = (ProductProps)db.Retrieve(p.ProductID);
            Assert.AreEqual(p.GetState(), p2.GetState());

            //Cleanup
            db.Delete(p);
        }

        [Test]
        public void TestDelete()
        {
            ProductProps p = new ProductProps();
            p.ProductCode = "T3ST";
            p.Description = "This is a Test Product";
            p.UnitPrice = 100.0000m;
            p.OnHandQuantity = 1;
            db.Create(p);

            Assert.True(db.Delete(p));
            Assert.Throws<Exception>(() => db.Retrieve(p.ProductID));
        }

        [Test]
        public void TestUpdate()
        {
            ProductProps p = (ProductProps)db.Retrieve(1);
            p.ProductCode = "T4ST";
            p.Description = "Test description";
            Assert.True(db.Update(p));
            p = (ProductProps)db.Retrieve(1);
            Assert.AreEqual("T4ST", p.ProductCode);
            Assert.AreEqual("Test description", p.Description);
        }
    }
}

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
    internal class CustomerDBTests
    {
        CustomerDB db;

        [SetUp]
        public void ResetData()
        {
            db = new CustomerDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieve()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(1);
            Assert.AreEqual("Birmingham", p.City);
            Assert.AreEqual("Molunguri, A", p.Name);
        }

        [Test]
        public void TestRetrieveAll()
        {
            List<CustomerProps> list = (List<CustomerProps>)db.RetrieveAll();
            Assert.AreEqual(696, list.Count);
        }

        [Test]
        public void TestCreate()
        {
            CustomerProps p = new CustomerProps();
            p.Name = "Test Name";
            p.Address = "101 Test Address";
            p.City = "Testcity";
            p.State = "OR";
            //Remember to use valid (existing) statecode in testing 
            p.Zipcode = "10001";
            db.Create(p);
            CustomerProps p2 = (CustomerProps)db.Retrieve(p.CustomerID);
            Assert.AreEqual(p.GetState(), p2.GetState());

            //Cleanup
            db.Delete(p);
        }

        [Test]
        public void TestDelete()
        {
            CustomerProps p = new CustomerProps();
            p.Name = "Tobe Deleted";
            p.Address = "101 Test Delete";
            p.City = "Deletecity";
            p.State = "OR";
            p.Zipcode = "10001";
            db.Create(p);

            Assert.True(db.Delete(p));
            Assert.Throws<Exception>(() => db.Retrieve(p.CustomerID));
        }

        [Test]
        public void TestUpdate()
        {
            CustomerProps p = (CustomerProps)db.Retrieve(1);
            p.Name = "Test name";
            p.Address = "Test address";
            Assert.True(db.Update(p));
            p = (CustomerProps)db.Retrieve(1);
            Assert.AreEqual("Test name", p.Name);
            Assert.AreEqual("Test address", p.Address);
        }
    }
}   

using MMABooksProps;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMABooksTests
{
    internal class ProductPropsTests
    {
        ProductProps props;

        [SetUp]
        public void Setup()
        {
            props = new ProductProps();
            props.ProductID = 1;
            props.ProductCode = "T3ST";
            props.Description = "This is a Test Product";
            props.UnitPrice = 100.01m;
            props.OnHandQuantity = 1;
        }

        [Test]
        public void TestGetState()
        {
            string jsonString = props.GetState();
            Console.WriteLine(jsonString);
            Assert.IsTrue(jsonString.Contains(props.ProductCode));
            Assert.IsTrue(jsonString.Contains(props.Description));
        }

        [Test]
        public void TestClone()
        {
            ProductProps newProps = (ProductProps)props.Clone();
            Assert.AreEqual(props.ProductID, newProps.ProductID);
            Assert.AreEqual(props.Description, newProps.Description);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }

        [Test]
        public void TestSetState()
        {
            string jsonString = props.GetState();
            ProductProps newProps = new ProductProps();
            newProps.SetState(jsonString);
            Assert.AreEqual(props.ProductID, newProps.ProductID);
            Assert.AreEqual(props.Description, newProps.Description);
            Assert.AreEqual(props.ConcurrencyID, newProps.ConcurrencyID);
        }
    }
}

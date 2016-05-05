using System;
using NameliDotNet;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NameliDotNet.Tests
{
    [TestClass]
    public class NameliTest
    {
        public Nameli Nameli { get; set; }

        public NameliTest()
        {
            Nameli = new Nameli(NameliLocale.UnitedStates);
        }

        [TestMethod]
        public void FirstName()
        {
            string maleFirst = Nameli.FirstName(NameliGender.Male);
            if (string.IsNullOrWhiteSpace(maleFirst)) Assert.Fail();

            string femaleFirst = Nameli.FirstName(NameliGender.Female);
            if (string.IsNullOrWhiteSpace(femaleFirst)) Assert.Fail();

            string other = Nameli.FirstName(NameliGender.Other);
            if (string.IsNullOrWhiteSpace(other)) Assert.Fail();

            List<string> bulk = new List<string>();
            for (int n = 0; n < 300; ++n)
            {
                bulk.Add(Nameli.FirstName(NameliGender.Female));
            }
            Assert.AreEqual(bulk.Count, 300);
        }

        [TestMethod]
        public void LastName()
        {
            string lastname = Nameli.LastName();
            if (string.IsNullOrWhiteSpace(lastname)) Assert.Fail();

            List<string> bulk = new List<string>();
            for (int n = 0; n < 300; ++n)
            {
                bulk.Add(Nameli.LastName());
            }
            Assert.AreEqual(bulk.Count, 300);
        }

        [TestMethod]
        public void FirstAndLast()
        {
            string firstAndLast = Nameli.FirstAndLast(NameliGender.Male);
            if (string.IsNullOrWhiteSpace(firstAndLast)) Assert.Fail();
            if (!firstAndLast.Contains(" ")) Assert.Fail();
        }

        [TestMethod]
        public void LastAndFirst()
        {
            string lastAndFirst = Nameli.LastAndFirst(NameliGender.Female);
            if (string.IsNullOrWhiteSpace(lastAndFirst)) Assert.Fail();
            if (!lastAndFirst.Contains(" ")) Assert.Fail();
        }

        [TestMethod]
        public void StateAbbrev()
        {
            string abbrev = Nameli.StateAbbr();
            if (string.IsNullOrWhiteSpace(abbrev)) Assert.Fail();

            //List<string> abbrevs = new List<string>();
            //for (int n = 0; n < 300; ++n)
            //{
            //    abbrevs.Add(Nameli.StateAbbr());
            //}
            //int count = abbrevs.Count;
        }

        [TestMethod]
        public void State()
        {
            string state = Nameli.State();
            if (string.IsNullOrWhiteSpace(state)) Assert.Fail();

            //List<string> states = new List<string>();
            //for (int n = 0; n < 300; ++n)
            //{
            //    states.Add(Nameli.State());
            //}
            //int count = states.Count;
        }

        [TestMethod]
        public void PhoneNumber()
        {
            string phone = Nameli.Phone();
            if (string.IsNullOrWhiteSpace(phone)) Assert.Fail();

            phone = Nameli.Phone(true);
            if (string.IsNullOrWhiteSpace(phone)) Assert.Fail();
        }

        [TestMethod]
        public void Email()
        {
            string email = Nameli.Email();
            if (string.IsNullOrWhiteSpace(email)) Assert.Fail();

            List<string> bulk = new List<string>();
            for (int n = 0; n < 300; ++n)
            {
                bulk.Add(Nameli.Email());
            }
            Assert.AreEqual(bulk.Count, 300);
        }

        [TestMethod]
        public void Addresses()
        {
            string addressLineOne = Nameli.AddressLineOne();
            if (string.IsNullOrWhiteSpace(addressLineOne)) Assert.Fail();

            string addressLineTwo = Nameli.AddressLineTwo();
            if (string.IsNullOrWhiteSpace(addressLineTwo)) Assert.Fail();

            string shipping = Nameli.ShippingAddress();
            if (string.IsNullOrWhiteSpace(shipping)) Assert.Fail();

            string shippingCountry = Nameli.ShippingAddress(true);
            if (string.IsNullOrWhiteSpace(shipping)) Assert.Fail();
            // Sweet! "Ion Street W Lane, MA 23922"

            List<string> bulk = new List<string>();
            for (int n = 0; n < 300; ++n)
            {
                bulk.Add(Nameli.AddressLineOne());
            }
            Assert.AreEqual(bulk.Count, 300);
        }

        [TestMethod]
        public void City()
        {
            string city = Nameli.City();
            if (string.IsNullOrWhiteSpace(city)) Assert.Fail();
        }

        [TestMethod]
        public void SocialSecurityNumber()
        {
            string ssn = Nameli.SocialSecurityNumber();
            Assert.AreNotEqual(ssn.Length, 0);
            Assert.AreEqual(ssn.Length, 11);
        }

        [TestMethod]
        public void BirthDate()
        {
            DateTime birthDate = Nameli.BirthDay();
            Assert.AreNotEqual(birthDate, DateTime.Now);

            birthDate = Nameli.BirthDay(-200);
            Assert.AreNotEqual(birthDate, DateTime.Now);

            birthDate = Nameli.BirthDay(200);
            Assert.AreNotEqual(birthDate, DateTime.Now);
        }

        [TestMethod]
        public void CompanyName()
        {
            string name = Nameli.CompanyName();
            if (string.IsNullOrWhiteSpace(name)) Assert.Fail();
        }
    }
}

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

            List<string> names = new List<string>();
            for (int n = 0; n < 300; ++n)
            {
                names.Add(Nameli.FirstName(NameliGender.Female));
            }
            int count = names.Count;
        }

        [TestMethod]
        public void LastName()
        {
            string lastname = Nameli.LastName();
            if (string.IsNullOrWhiteSpace(lastname)) Assert.Fail();

            //List<string> names = new List<string>();
            //for (int n = 0; n < 300; ++n)
            //{
            //    names.Add(Nameli.LastName());
            //}
            //int count = names.Count;
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
        public void AddressLineOne()
        {
            string addressLineOne = Nameli.AddressLineOne();

            //List<string> one = new List<string>();
            //for (int n = 0; n < 300; ++n)
            //{
            //    one.Add(Nameli.AddressLineOne());
            //}
            //int count = one.Count;
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
    }
}

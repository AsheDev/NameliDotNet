using System;
using NameliDotNet;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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

            //List<string> names = new List<string>();
            //for(int n = 0; n < 300; ++n)
            //{
            //    names.Add(Nameli.FirstName(NameliGender.Female));
            //}
            //int count = names.Count;
        }

        [TestMethod]
        public void SocialSecurityNumber()
        {
            string ssn = Nameli.SocialSecurityNumber();
            Assert.AreNotEqual(ssn.Length, 0);
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

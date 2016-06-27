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
        }

        [TestMethod]
        public void State()
        {
            string state = Nameli.State();
            if (string.IsNullOrWhiteSpace(state)) Assert.Fail();
        }

        [TestMethod]
        public void PhoneNumber()
        {
            string phone = Nameli.Phone();
            if (string.IsNullOrWhiteSpace(phone)) Assert.Fail();

            phone = Nameli.Phone(true);
            if (string.IsNullOrWhiteSpace(phone)) Assert.Fail();

            List<string> bulk = new List<string>();
            for (int n = 0; n < 300; ++n)
            {
                bulk.Add(Nameli.Phone());
            }
            Assert.AreEqual(bulk.Count, 300);
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

            List<string> bulk = new List<string>();
            for (int n = 0; n < 300; ++n)
            {
                bulk.Add(Nameli.City());
            }
            Assert.AreEqual(bulk.Count, 300);
        }

        [TestMethod]
        public void SocialSecurityNumber()
        {
            string ssn = Nameli.SocialSecurityNumber();
            Assert.AreNotEqual(ssn.Length, 0);
            Assert.AreEqual(ssn.Length, 11);

            List<string> bulk = new List<string>();
            for (int n = 0; n < 300; ++n)
            {
                bulk.Add(Nameli.SocialSecurityNumber());
            }
            Assert.AreEqual(bulk.Count, 300);
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

            List<string> bulk = new List<string>();
            for (int n = 0; n < 300; ++n)
            {
                bulk.Add(Nameli.CompanyName());
            }
            Assert.AreEqual(bulk.Count, 300);
        }

        [TestMethod]
        public void StringGenerator()
        {
            Test test = new Test();
            string text = test.Generate(25);
        }

        [TestMethod]
        public void MarkovPhrase()
        {
            string _sampleText = "Sometimes fate is like a small sandstorm that keeps changing directions. You change direction but the sandstorm chases you. You turn again, but the storm adjusts. Over and over you play this out, like some ominous dance with death just before dawn. Why? Because this storm isn’t something that blew in from far away, something that has nothing to do with you. This storm is you. Something inside of you. So all you can do is give in to it, step right inside the storm, closing your eyes and plugging up your ears so the sand doesn’t get in, and walk through it, step by step. There’s no sun there, no moon, no direction, no sense of time. Just fine white sand swirling up into the sky like pulverized bones. That’s the kind of sandstorm you need to imagine. " +
                        "And you really will have to make it through that violent, metaphysical, symbolic storm. No matter how metaphysical or symbolic it might be, make no mistake about it: it will cut through flesh like a thousand razor blades. People will bleed there, and you will bleed too. Hot, red blood. You’ll catch that blood in your hands, your own blood and the blood of others. " +
                        "And once the storm is over you won’t remember how you made it through, how you managed to survive. You won’t even be sure, in fact, whether the storm is really over. But one thing is certain. When you come out of the storm you won’t be the same person who walked in. That’s what this storm’s all about.";
            IStrategy strategy = new PhraseStrategy(_sampleText, 2);
            MarkovStrategies two = new MarkovStrategies(strategy);
            // 2, 1, 1 means 1 word has 50% the next 25% and the other 25%
            string text = two.GenerateText();
        }

        [TestMethod]
        public void MarkovWord()
        {
            IList<string> testing = new Warehouse().GetUSMaleNames();
            // grab a list of names from the warehouse
            IStrategy names = new NameStrategy(testing, 3); // 1 is *really* bad
            MarkovStrategies generator = new MarkovStrategies(names);

            IList<string> allTheNames = new List<string>();
            for(int n = 0; n < 300; ++n)
            {
                allTheNames.Add(generator.GenerateText());
            }

            List<string> bulk = new List<string>();
            for (int n = 0; n < 300; ++n)
            {
                bulk.Add(Nameli.FirstName(NameliGender.Male));
            }
        }
    }
}

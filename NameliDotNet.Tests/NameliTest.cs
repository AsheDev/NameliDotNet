using System;
using NameliDotNet;
using NameliDotNet.Interfaces;
using NameliDotNet.Strategies;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NameliDotNet.Tests
{
    [TestClass]
    public class NameliTest
    {
        private Nameli _nameli { get; set; }
        private Warehouse _warehouse { get; set; }

        public NameliTest()
        {
            /* Some of the options that are available to choose from by default */

            //IStrategy original = new OriginalWordStrategy();
            //IStrategy name = new NameStrategy();
            //IStrategy phrase = new PhraseStrategy();
            _nameli = new Nameli();
            //_nameli = new Nameli(original); // alternatively
            //_nameli = new Nameli(original, NameliLocale.UnitedStates); // alternatively
        }

        [TestMethod]
        public void FirstName()
        {
            string maleFirst = _nameli.FirstName(NameliGender.Male);
            Assert.IsFalse(string.IsNullOrWhiteSpace(maleFirst));

            string secondMaleFirst = _nameli.FirstName(NameliGender.Male); // this tests the male name list being reused
            Assert.IsFalse(string.IsNullOrWhiteSpace(secondMaleFirst));
            
            string femaleFirst = _nameli.FirstName(NameliGender.Female);
            Assert.IsFalse(string.IsNullOrWhiteSpace(femaleFirst));

            string other = _nameli.FirstName(NameliGender.Other);
            Assert.IsFalse(string.IsNullOrWhiteSpace(other));

            List<string> bulk = new List<string>();
            for (int n = 0; n < 300; ++n)
            {
                bulk.Add(_nameli.FirstName(NameliGender.Female));
            }
            Assert.AreEqual(bulk.Count, 300);
        }

        [TestMethod]
        public void LastName()
        {
            string lastname = _nameli.LastName();
            Assert.IsFalse(string.IsNullOrWhiteSpace(lastname));

            List<string> bulk = new List<string>();
            for (int n = 0; n < 300; ++n)
            {
                bulk.Add(_nameli.LastName());
            }
            Assert.AreEqual(bulk.Count, 300);
        }

        [TestMethod]
        public void FirstAndLast()
        {
            string firstAndLast = _nameli.FirstAndLast(NameliGender.Male);
            Assert.IsFalse(string.IsNullOrWhiteSpace(firstAndLast));
            Assert.IsTrue(firstAndLast.Contains(" "));
        }

        [TestMethod]
        public void LastAndFirst()
        {
            string lastAndFirst = _nameli.LastAndFirst(NameliGender.Female);
            Assert.IsFalse(string.IsNullOrWhiteSpace(lastAndFirst));
            Assert.IsTrue(lastAndFirst.Contains(", "));
        }

        [TestMethod]
        public void StateAbbrev()
        {
            string abbrev = _nameli.StateAbbr();
            Assert.IsFalse(string.IsNullOrWhiteSpace(abbrev));
        }

        [TestMethod]
        public void State()
        {
            string state = _nameli.State();
            Assert.IsFalse(string.IsNullOrWhiteSpace(state));
        }

        [TestMethod]
        public void PhoneNumber()
        {
            string phone = _nameli.Phone();
            Assert.IsFalse(string.IsNullOrWhiteSpace(phone));

            phone = _nameli.Phone(true);
            Assert.IsFalse(string.IsNullOrWhiteSpace(phone));

            List<string> bulk = new List<string>();
            for (int n = 0; n < 300; ++n)
            {
                bulk.Add(_nameli.Phone());
            }
            Assert.AreEqual(bulk.Count, 300);
        }

        [TestMethod]
        public void Email()
        {
            string email = _nameli.Email();
            Assert.IsFalse(string.IsNullOrWhiteSpace(email));

            List<string> bulk = new List<string>();
            for (int n = 0; n < 300; ++n)
            {
                bulk.Add(_nameli.Email());
            }
            Assert.AreEqual(bulk.Count, 300);
        }

        [TestMethod]
        public void Addresses()
        {
            string addressLineOne = _nameli.AddressLineOne();
            Assert.IsFalse(string.IsNullOrWhiteSpace(addressLineOne));

            string addressLineTwo = _nameli.AddressLineTwo();
            Assert.IsFalse(string.IsNullOrWhiteSpace(addressLineTwo));

            string shipping = _nameli.ShippingAddress();
            Assert.IsFalse(string.IsNullOrWhiteSpace(shipping));

            string shippingCountry = _nameli.ShippingAddress(true);
            Assert.IsFalse(string.IsNullOrWhiteSpace(shippingCountry));
            // Sweet! "Ion Street W Lane, MA 23922"

            List<string> bulk = new List<string>();
            for (int n = 0; n < 300; ++n)
            {
                bulk.Add(_nameli.AddressLineOne());
            }
            Assert.AreEqual(bulk.Count, 300);
        }

        [TestMethod]
        public void City()
        {
            string city = _nameli.City();
            Assert.IsFalse(string.IsNullOrWhiteSpace(city));

            List<string> bulk = new List<string>();
            for (int n = 0; n < 300; ++n)
            {
                bulk.Add(_nameli.City());
            }
            Assert.AreEqual(bulk.Count, 300);
        }

        [TestMethod]
        public void SocialSecurityNumber()
        {
            string ssn = _nameli.SocialSecurityNumber();
            Assert.IsTrue(ssn.Length > 0);
            Assert.IsTrue(ssn.Length == 11);

            List<string> bulk = new List<string>();
            for (int n = 0; n < 300; ++n)
            {
                bulk.Add(_nameli.SocialSecurityNumber());
            }
            Assert.AreEqual(bulk.Count, 300);
        }

        [TestMethod]
        public void BirthDate()
        {
            DateTime birthDate = _nameli.BirthDay();
            Assert.AreNotEqual(birthDate, DateTime.Now);

            birthDate = _nameli.BirthDay(-200);
            Assert.AreNotEqual(birthDate, DateTime.Now);

            birthDate = _nameli.BirthDay(200);
            Assert.AreNotEqual(birthDate, DateTime.Now);
        }

        [TestMethod]
        public void CompanyName()
        {
            string name = _nameli.CompanyName();
            Assert.IsFalse(string.IsNullOrWhiteSpace(name));

            List<string> bulk = new List<string>();
            for (int n = 0; n < 300; ++n)
            {
                bulk.Add(_nameli.CompanyName());
            }
            Assert.AreEqual(bulk.Count, 300);
        }

        [TestMethod]
        public void Phrase()
        {
            //string sampleText = "Sometimes fate is like a small sandstorm that keeps changing directions. You change direction but the sandstorm chases you. You turn again, but the storm adjusts. Over and over you play this out, like some ominous dance with death just before dawn. Why? Because this storm isn’t something that blew in from far away, something that has nothing to do with you. This storm is you. Something inside of you. So all you can do is give in to it, step right inside the storm, closing your eyes and plugging up your ears so the sand doesn’t get in, and walk through it, step by step. There’s no sun there, no moon, no direction, no sense of time. Just fine white sand swirling up into the sky like pulverized bones. That’s the kind of sandstorm you need to imagine. " +
            //            "And you really will have to make it through that violent, metaphysical, symbolic storm. No matter how metaphysical or symbolic it might be, make no mistake about it: it will cut through flesh like a thousand razor blades. People will bleed there, and you will bleed too. Hot, red blood. You’ll catch that blood in your hands, your own blood and the blood of others. " +
            //            "And once the storm is over you won’t remember how you made it through, how you managed to survive. You won’t even be sure, in fact, whether the storm is really over. But one thing is certain. When you come out of the storm you won’t be the same person who walked in. That’s what this storm’s all about.";
            //IStrategy phraseStrategy = new PhraseStrategy(2);
            //_nameli.ChangeStrategy(phraseStrategy);
            // 2, 1, 1 means 1 word has 50% the next 25% and the other 25%

            // TODO
            // I need to think about this one
            // I can't expect people to KNOW to change the strategy here
            // maybe give it a default strategy but allow it to be overriden
            string text = _nameli.Phrase();
            Assert.IsFalse(string.IsNullOrWhiteSpace(text));
        }

        //[TestMethod]
        public void WordVariations()
        {
            /* I was using this to test the different strategies against themselves.
             * */

            //// grab a list of names from the warehouse
            ////IList<string> maleNamesUS = new Warehouse().GetUSMaleNames();
            
            //IStrategy nameStrategy = new NameStrategy(3); // 1 is *really* bad
            //Nameli nameli = new Nameli(nameStrategy, NameliLocale.UnitedStates);

            //IList<string> allTheNames = new List<string>();
            ////for (int n = 0; n < 300; ++n) allTheNames.Add(nameli.);

            //// use my original Markov code
            //List<string> bulk = new List<string>();
            //for (int n = 0; n < 300; ++n) bulk.Add(_nameli.FirstName(NameliGender.Male));

            //// OR
            //nameStrategy = new OriginalWordStrategy();
            //nameStrategy.SetSourceText(_warehouse.GetUSMaleNames()); // HMMM WHAT TO DO HERE
            //nameli.ChangeStrategy(new OriginalWordStrategy());
            
            //List<string> bulk2 = new List<string>();
            //for (int n = 0; n < 300; ++n) bulk2.Add(nameli.FirstName(NameliGender.Male));
        }
    }
}

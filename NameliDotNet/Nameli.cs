using System;
using System.Linq;
using System.Text;
using NameliDotNet.Interfaces;
using NameliDotNet.Strategies;
using System.Collections.Generic;

namespace NameliDotNet
{
    public class Nameli
    {
        protected NameliLocale _locale;
        protected Random _random;
        internal IStrategy _strategy;
        internal Warehouse _warehouse;

        private IList<Matches> _firstNames;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="strategy"></param>
        public Nameli(IStrategy strategy = null, NameliLocale locale = NameliLocale.UnitedStates)
        {
            _strategy = (!ReferenceEquals(strategy, null)) ? strategy : new OriginalWordStrategy(); 
            _locale = NameliLocale.UnitedStates; // This is forced! First iteration focuses on one region
            _random = new Random();
            _warehouse = new Warehouse();
        }

        public void ChangeStrategy(IStrategy strategy)
        {
            if (ReferenceEquals(strategy, null)) throw new ArgumentNullException("Error encountered while changing strategies. Strategy may not be null.");
            _strategy = strategy;
        }

        /// <summary>
        /// This does nothing currently.
        /// </summary>
        /// <param name="locale"></param>
        public void ChangeLocale(NameliLocale locale)
        {
            // 1st iteration focuses on one region
            // so it's being overwritten for the time being
            _locale = NameliLocale.UnitedStates;
        }

        /// <summary>
        /// Generate a person's first name
        /// </summary>
        /// <param name="gender">The gender associated with the first name</param>
        /// <returns></returns>
        public virtual string FirstName(NameliGender gender = 0)
        {
            if (gender == NameliGender.Random) gender = (NameliGender)_random.Next(1, 4);
            IList<string> names = DeterminGenderOfNames(gender);
            if (!_strategy.CompareLists(names)) _strategy.SetSourceText(DeterminGenderOfNames(gender));
            return _strategy.GenerateText();
        }

        /// <summary>
        /// Generate a person's last name
        /// </summary>
        /// <returns></returns>
        public virtual string LastName()
        {
            IList<string> seed;
            switch(_locale)
            {
                default: // USA
                    seed = _warehouse.GetUSSurnames();
                    break;
            }
            if (!_strategy.CompareLists(seed)) _strategy.SetSourceText(seed);
            return _strategy.GenerateText();
        }

        /// <summary>
        /// Generate the first and last name of a person
        /// </summary>
        /// <param name="gender">The gender associated with the first name</param>
        /// <returns></returns>
        public virtual string FirstAndLast(NameliGender gender = 0)
        {
            if (gender == NameliGender.Random) gender = (NameliGender)_random.Next(1, 4);
            return FirstName(gender) + " " + LastName(); // could be different for SouthKorea
        }

        /// <summary>
        /// Generate the first and last name of a person
        /// </summary>
        /// <param name="gender">The gender associated with the first name</param>
        /// <returns></returns>
        public virtual string LastAndFirst(NameliGender gender = 0)
        {
            if (gender == NameliGender.Random) gender = (NameliGender)_random.Next(1, 4);
            return LastName() + ", " + FirstName(gender); // could be different for SouthKorea
        }

        /// <summary>
        /// Generate a birthdate.
        /// </summary>
        /// <param name="age">The age of the person. Can be left blank.</param>
        /// <returns></returns>
        public virtual DateTime BirthDay(int age = 0)
        {
            //if (age < 0) throw new ArgumentException("Age may not be less than zero.");
            if (age < 0 || age == 0 || age > 120) age = _random.Next(27, 89);
            return DateTime.Now.AddYears(-age).AddMonths(-_random.Next(0, 12)).AddDays(-_random.Next(0, 32));
        }

        /// <summary>
        /// Generates a basic street address.
        /// </summary>
        /// <returns></returns>
        public virtual string AddressLineOne()
        {
            // TODO: I don't believe I've setup streets to be country based
            IList<string> streets = new List<string>();
            switch (_locale)
            {
                default:
                    streets = _warehouse.GetStreetNames();
                    break;
            }
            if (!_strategy.CompareLists(streets)) _strategy.SetSourceText(streets);
            return _strategy.GenerateText();
        }

        /// <summary>
        /// Generates the city, state, and zip of a 
        /// street address.
        /// </summary>
        /// <remarks>Note, this doesn't yet account for 
        /// foreign countries and how they may handle 
        /// this differently than the US.</remarks>
        /// <returns></returns>
        public virtual string AddressLineTwo()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(City() + ", ");
            builder.Append(StateAbbr() + " ");
            builder.Append(Zip());
            return builder.ToString();
        }

        /// <summary>
        /// Generates a full shipping address.
        /// </summary>
        /// <returns></returns>
        public virtual string ShippingAddress(bool includeCountry = false)
        {
            string address = AddressLineOne() + " " + AddressLineTwo();
            if(includeCountry)
            {
                address += (Convert.ToBoolean(_random.Next(0, 2))) ? " " + Country() : " " + CountryAbbreviated();
            }
            return address;
        }

        /// <summary>
        /// Generates a city name
        /// </summary>
        /// <returns></returns>
        public virtual string City()
        {
            IList<string> seed = new List<string>();
            switch (_locale)
            {
                default: // defaults to USA currently
                    seed = _warehouse.GetUSCityNames();
                    break;
            }
            if (!_strategy.CompareLists(seed)) _strategy.SetSourceText(seed);
            return _strategy.GenerateText();
        }

        /// <summary>
        /// Retrieves a list of States including territories 
        /// and commonwealths
        /// </summary>
        /// <returns></returns>
        public virtual string State()
        {
            IList<string> seed = new List<string>();
            switch (_locale)
            {
                default: // defaults to USA currently
                    seed = _warehouse.GetUSStates();
                    break;
            }
            return seed[_random.Next(0, seed.Count)];
        }

        /// <summary>
        /// Retrieve the abbreviation associated with a state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual string GetStateAbbreviation(string state)
        {
            throw new NotImplementedException("I haven't gotten here yet! Sorry.");
            //if (string.IsNullOrWhiteSpace(state)) throw new ArgumentNullException("No value was passed to Nameli.GetStateAbbreviation method");
            //IList<string> abbrevs = _warehouse.GetUSStateAbbrevs();
            //abbrev = _warehouse.GetUSStateAbbrevs().FirstOrDefault(a => a == abbrev.ToUpper());
        }

        public virtual string County()
        {
            return "Cork"; // heh
        }

        /// <summary>
        /// Retrieve the name of the country for the currently 
        /// defined locale.
        /// </summary>
        /// <returns></returns>
        public virtual string Country()
        {
            return _warehouse.GetCountriesFull()[(int)_locale];
        }

        /// <summary>
        /// Retrieve the abbreviated form of the country for the 
        /// currently defined locale.
        /// </summary>
        /// <returns></returns>
        public virtual string CountryAbbreviated()
        {
            return _warehouse.GetCountriesAbbreviated()[(int)_locale];
        }

        /// <summary>
        /// Retrieves a list of State abbreviations including 
        /// territories and commonwealths
        /// </summary>
        /// <returns></returns>
        public virtual string StateAbbr()
        {
            IList<string> seed;
            switch(_locale)
            {
                default: // defaults to USA
                    seed = _warehouse.GetUSStateAbbrevs();
                    break;
            }
            return seed[_random.Next(0, seed.Count)];
        }

        public virtual string GetAbbreviationState(string abbrev)
        {
            throw new NotImplementedException("In process. Sorry!");
        }

        /// <summary>
        /// Generate a zip code.
        /// </summary>
        /// <param name="includePlusFour">Indicate whether to include the "+4 code" appended to the zip.</param>
        /// <returns></returns>
        public virtual string Zip(bool includePlusFour = false)
        {
            StringBuilder build = new StringBuilder();
            string zip = _random.Next(501, 99959).ToString();
            build.Append(ZeroPad(5, zip));

            if (includePlusFour)
            {
                build.Append("-");
                string plusFour = _random.Next(1, 9999).ToString();
                build.Append(ZeroPad(4, plusFour));
            }

            return build.ToString();
        }

        /// <summary>
        /// Generate a user's email.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual string Email(string name = null)
        {
            // TODO: this needs locale support!
            bool includeNumber = Convert.ToBoolean(_random.Next(0, 2));
            if (string.IsNullOrWhiteSpace(name)) name = (Convert.ToBoolean(_random.Next(0, 2))) ? FirstName() : LastAndFirst();

            name = name.Replace(", ", "").Replace("-", "");
            if (includeNumber) name += _random.Next(76, 2732).ToString();

            IList<string> domains;
            switch (_locale)
            {
                default: // defaults to USA
                    domains = _warehouse.GetUSEmailDomains();
                    break;
            }

            return name = (name + "@" + domains[_random.Next(0, domains.Count)]).ToLower();
        }

        /// <summary>
        /// Generates a phone number
        /// </summary>
        /// <param name="includeAreaCode"></param>
        /// <returns></returns>
        public virtual string Phone(bool includeAreaCode = false)
        {
            List<string> separators = new List<string> { ".", "-" };

            StringBuilder builder = new StringBuilder();
            builder.Append(_random.Next(234, 987).ToString());
            string separator = separators[_random.Next(0, separators.Count)];
            builder.Append(separator);
            builder.Append(_random.Next(2361, 7982).ToString());

            if (includeAreaCode)
            {
                bool braces = Convert.ToBoolean(_random.Next(0, 2));
                if (braces)
                {
                    builder.Insert(0, ") ");
                }
                else
                {
                    builder.Insert(0, separator);
                }

                builder.Insert(0, _random.Next(201, 771).ToString());
                if (braces) builder.Insert(0, "(");
            }

            return builder.ToString();
        }

        /// <summary>
        /// Generate a Social Security Number
        /// </summary>
        /// <returns></returns>
        public virtual string SocialSecurityNumber()
        {
            StringBuilder builder = new StringBuilder();
            string first = _random.Next(1, 999).ToString();
            builder.Append(ZeroPad(3, first) + "-");

            string second = _random.Next(1, 99).ToString();
            builder.Append(ZeroPad(2, second) + "-");

            string third = _random.Next(1, 9999).ToString();
            builder.Append(ZeroPad(4, third));

            return builder.ToString();
        }

        // this guy needs some work (rethinking)
        // break this out into a specialized class?
        public virtual string CompanyName()
        {
            // Do I want to localize this? Probably in the future...
            IList<string> seed = _warehouse.GetCompanyNames();
            if (!_strategy.CompareLists(seed)) _strategy.SetSourceText(seed);
            return _strategy.GenerateText();
        }

        /// <summary>
        /// Generate a number of sentences using Markov chains. Defaults to 4. Not 
        /// currently settable :(
        /// </summary>
        /// <param name="strategy">The strategy to be used to generate phrases. The included 
        /// PhraseStrategy class is purposefully built for this operation.</param>
        /// <returns></returns>
        public virtual string Phrase(IStrategy strategy = null)
        {
            if (ReferenceEquals(strategy, null)) _strategy = new PhraseStrategy(2);
            int sentenceCount = 4; // TODO: I need to support the sentenceCount

            IList<string> seed = _warehouse.BasicEnglishText();
            if (!_strategy.CompareLists(seed)) _strategy.SetSourceText(seed);
            return _strategy.GenerateText();
        }

        /// <summary>
        /// Determines which list of names to be using based on locale 
        /// and gender
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        private IList<string> DeterminGenderOfNames(NameliGender gender)
        {
            IList<string> nameSeed = new List<string>();
            switch (_locale)
            {
                default: // USA. Eventually this needs to be based on the country
                    switch (gender)
                    {
                        case NameliGender.Male:
                            nameSeed = _warehouse.GetUSMaleNames();
                            break;
                        case NameliGender.Female:
                            nameSeed = _warehouse.GetUSFemaleNames();
                            break;
                        default: // NameliGender.Other
                            nameSeed = _warehouse.GetUSNeutralNames();
                            break;
                    }
                    break;
            }
            return nameSeed;
        }

        /// <summary>
        /// Pad leading zeros on a string.
        /// </summary>
        /// <param name="maxLength">The max length, including leading zeros, the final string must be.</param>
        /// <param name="toPad">The string without padded zeros.</param>
        /// <returns></returns>
        private string ZeroPad(int maxLength, string toPad)
        {
            StringBuilder pad = new StringBuilder();
            for (int n = 0; n < (maxLength - toPad.Length); ++n) pad.Append("0");
            pad.Append(toPad);
            return pad.ToString();
        }
    }
}

using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace NameliDotNet
{
    public class Nameli
    {
        protected NameliLocale _locale;
        protected Random _random;
        internal NameGenerator _nameGen;
        internal Warehouse _warehouse;

        public Nameli(NameliLocale locale)
        {
            //_locale = locale;
            _locale = NameliLocale.UnitedStates; // 1st iteration focuses on one region
            _random = new Random();
            _nameGen = new NameGenerator(_random);
            _warehouse = new Warehouse();
        }

        /// <summary>
        /// Generate a person's first name
        /// </summary>
        /// <param name="gender">The gender associated with the first name</param>
        /// <returns></returns>
        public virtual string FirstName(NameliGender gender = 0)
        {
            if (gender == NameliGender.Random) gender = (NameliGender)_random.Next(1, 4);
            return _nameGen.CreateName(DetermineNames(gender));
        }

        /// <summary>
        /// Generate a person's last name
        /// </summary>
        /// <returns></returns>
        public virtual string LastName()
        {
            return _nameGen.CreateName(_warehouse.GetUSSurnames());
        }

        /// <summary>
        /// Generate the first and last name of a person
        /// </summary>
        /// <param name="gender">The gender associated with the first name</param>
        /// <returns></returns>
        public virtual string FirstAndLast(NameliGender gender = 0)
        {
            if (gender == NameliGender.Random) gender = (NameliGender)_random.Next(1, 4);
            return FirstName(gender) + " " + LastName();
        }

        /// <summary>
        /// Generate the first and last name of a person
        /// </summary>
        /// <param name="gender">The gender associated with the first name</param>
        /// <returns></returns>
        public virtual string LastAndFirst(NameliGender gender = 0)
        {
            if (gender == NameliGender.Random) gender = (NameliGender)_random.Next(1, 4);
            return LastName() + ", " + FirstName(gender);
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

        public virtual string AddressLineOne()
        {
            return "";
        }

        public virtual string AddressLineTwo()
        {
            return "";
        }

        public virtual string StreetAddress()
        {
            return AddressLineOne() + " " + AddressLineTwo();
        }

        /// <summary>
        /// Retrieves a list of States including territories 
        /// and commonwealths
        /// </summary>
        /// <returns></returns>
        public virtual string State()
        {
            IList<string> states = _warehouse.GetUSStates();
            return states[_random.Next(0, states.Count)];
        }

        /// <summary>
        /// Retrieve the abbreviation associated with a state
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual string GetStateAbbreviation(string state)
        {
            throw new NotImplementedException();
            //if (string.IsNullOrWhiteSpace(state)) throw new ArgumentNullException("No value was passed to Nameli.GetStateAbbreviation method");
            //IList<string> abbrevs = _warehouse.GetUSStateAbbrevs();
            //abbrev = _warehouse.GetUSStateAbbrevs().FirstOrDefault(a => a == abbrev.ToUpper());
        }

        public virtual string County()
        {
            return "Cork";
        }

        /// <summary>
        /// Retrieves a list of State abbreviations including 
        /// territories and commonwealths
        /// </summary>
        /// <returns></returns>
        public virtual string StateAbbr()
        {
            IList<string> abbrevs = _warehouse.GetUSStateAbbrevs();
            return abbrevs[_random.Next(0, abbrevs.Count)];
        }

        /// <summary>
        /// Retrieve the state associated with an abbreviation
        /// </summary>
        /// <param name="abbrev"></param>
        /// <returns></returns>
        public virtual string GetAbbreviationState(string abbrev)
        {
            throw new NotImplementedException();
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

        public virtual string Email(string name = null)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                // make up a name
            }
            else
            {
                // use the name and maybe randomize it a bit
            }

            // create a list of domains for the emails
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="includeAreaCode"></param>
        /// <returns></returns>
        public virtual string Phone(bool includeAreaCode = false)
        {
            // TODO: needs locale support!
            List<string> separators = new List<string> { ".", "-" };

            StringBuilder builder = new StringBuilder();
            builder.Append(_random.Next(234, 987).ToString());
            string separator = separators[_random.Next(0, separators.Count)];
            builder.Append(separator);
            builder.Append(_random.Next(2361, 7982).ToString());

            if(includeAreaCode)
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

        // break this out into a specialized class?
        public virtual string CompanyName()
        {
            return "";
        }



        /// <summary>
        /// Determines which list of names to be using based on locale 
        /// and gender
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="gender"></param>
        /// <returns></returns>
        private IList<string> DetermineNames(NameliGender gender)
        {
            IList<string> nameSeed = new List<string>();
            switch (_locale)
            {
                case NameliLocale.UnitedStates:
                    switch (gender)
                    {
                        case NameliGender.Male:
                            nameSeed = _warehouse.GetUSMaleNames();
                            break;
                        case NameliGender.Female:
                            nameSeed = _warehouse.GetUSFemaleNames();
                            break;
                        default: // NameliGender.Other
                            throw new NotImplementedException("Haven't gotten here yet!");
                            break;
                    }
                    break;
                default: // Great Britain
                    switch (gender)
                    {
                        case NameliGender.Male:
                            throw new NotImplementedException("Haven't gotten here yet!");
                            break;
                        case NameliGender.Female:
                            throw new NotImplementedException("Haven't gotten here yet!");
                            break;
                        default: // NameliGender.Other
                            throw new NotImplementedException("Haven't gotten here yet!");
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

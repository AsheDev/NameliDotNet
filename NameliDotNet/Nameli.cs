using System;
using System.Text;
using System.Collections.Generic;

namespace NameliDotNet
{
    public class Nameli
    {
        private NameliLocale _locale;
        private Random _random;
        private NameGenerator _nameGen;

        public Nameli(NameliLocale locale)
        {
            _locale = locale;
            _random = new Random();
            _nameGen = new NameGenerator(_random);
        }

        /// <summary>
        /// 
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
                    switch(gender)
                    {
                        case NameliGender.Male:
                            nameSeed = GetUSMaleNames();
                            break;
                        case NameliGender.Female:
                            nameSeed = GetUSFemaleNames();
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
        /// Generate a person's first name
        /// </summary>
        /// <param name="gender">The gender associated with the first name</param>
        /// <returns></returns>
        public virtual string FirstName(NameliGender gender = 0)
        {
            if (gender == NameliGender.Random) gender = (NameliGender)_random.Next(1, 4);
            return _nameGen.CreateFirstName(DetermineNames(gender));
        }

        public virtual string LastName(NameliGender gender = 0)
        {
            return "";
        }

        public virtual string FirstAndLast(NameliGender gender = 0)
        {
            return FirstName() + " " + LastName();
        }

        public virtual string LastAndFirst(NameliGender gender = 0)
        {
            return LastName() + ", " + FirstName();
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

        public virtual string State()
        {
            return "North Carolina";
        }

        public virtual string County()
        {
            return "Cork";
        }

        public virtual string StateAbbr()
        {
            return "NC";
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

        public virtual string Email()
        {
            return "";
        }

        public virtual string Phone()
        {
            return "";
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

        //private IList<string> MaleNames { get; set; }

        private IList<string> GetUSMaleNames()
        {
            IList<string> names = new List<string>
            {
                "jack",
                "oliver",
                "james",
                "lewis",
                "alexander",
                "charlie",
                "lucas",
                "logan",
                "harris",
                "daniel",
                "finlay",
                "jacob",
                "leo",
                "mason",
                "noah",
                "harry",
                "alfie",
                "max",
                "callum",
                "aaron",
                "adam",
                "thomas",
                "ethan",
                "rory",
                "cameron",
                "archie",
                "oscar",
                "matthew",
                "nathan",
                "joshua",
                "brodie",
                "william",
                "liam",
                "ryan",
                "jamie",
                "harrison",
                "joseph",
                "dylan",
                "samuel",
                "riley",
                "david",
                "ollie",
                "andrew",
                "connor",
                "luke",
                "muhammad",
                "jaxon",
                "kyle",
                "benjamin",
                "michael",
                "caleb",
                "jackson",
                "george",
                "finn",
                "leon",
                "fraser",
                "murray",
                "jake",
                "john",
                "arran",
                "angus",
                "cole",
                "robert",
                "cooper",
                "isaac",
                "jayden",
                "aiden",
                "kai",
                "theo",
                "jude",
                "ben",
                "tyler",
                "ruaridh",
                "owen",
                "blake",
                "freddie",
                "euan",
                "josh",
                "blair",
                "robbie",
                "hamish",
                "kian",
                "sam",
                "aidan",
                "jay",
                "christopher",
                "reuben",
                "cody",
                "luca",
                "lachlan",
                "elliot",
                "evan",
                "sonny",
                "calum",
                "henry",
                "rhys",
                "carson",
                "harvey",
                "calvin",
                "callan",
                "ross",
                "zac",
                "kayden",
                "lyle",
                "scott",
                "corey",
                "hunter",
                "olly",
                "zachary",
                "alex",
                "anthony",
                "innes",
                "lochlan",
                "marcus",
                "jax",
                "sebastian",
                "gabriel",
                "jordan",
                "sean",
                "austin",
                "mohammed",
                "patrick",
                "brody",
                "carter",
                "harley",
                "parker",
                "charles",
                "kaiden",
                "shay",
                "elliott",
                "fergus",
                "louis",
                "mohammad",
                "roman",
                "zack",
                "joey",
                "kieran",
                "jason",
                "jaxson",
                "zach",
                "alan",
                "dominic",
                "conor",
                "declan",
                "mark",
                "ewan",
                "nicholas",
                "reece",
                "bradley",
                "edward",
                "arthur",
                "ellis",
                "steven",
                "ashton",
                "danny",
                "eli",
                "flynn",
                "hugo",
                "kaleb",
                "zak",
                "kacper",
                "keir",
                "layton",
                "struan",
                "theodore",
                "tom",
                "bailey",
                "craig",
                "elijah",
                "frankie",
                "ruairidh",
                "gregor",
                "hayden",
                "joel",
                "stephen",
                "finley",
                "louie",
                "robin",
                "filip",
                "myles",
                "rocco",
                "caelan",
                "cian",
                "findlay",
                "magnus",
                "ciaran",
                "junior",
                "oskar",
                "peter",
                "alistair",
                "jakub",
                "jenson",
                "mitchell",
                "antoni",
                "arlo",
                "paul",
                "xander",
                "alasdair",
                "leighton",
                "brandon",
                "dexter",
                "duncan",
                "ruben",
                "tommy",
                "zander",
                "grayson",
                "kevin",
                "levi",
                "maxwell",
                "travis",
                "eric",
                "toby",
                "ali",
                "jonathan",
                "lukas",
                "rowan",
                "seth",
                "spencer",
                "taylor",
                "felix",
                "quinn",
                "ronan",
                "shaun",
                "albert",
                "billy",
                "cillian",
                "dean",
                "jonah",
                "nathaniel",
                "stuart",
                "adrian",
                "bryce",
                "frederick",
                "ibrahim",
                "joe",
                "marc",
                "rogan",
                "teddy",
                "jace",
                "jan",
                "jasper",
                "kerr",
                "lee",
                "marcel",
                "martin",
                "niall",
                "stanley",
                "abdullah",
                "aleksander",
                "caiden",
                "cayden",
                "colton",
                "lennon",
                "stewart",
                "casey",
                "christian",
                "hudson",
                "lauchlan",
                "lawson",
                "maximilian",
                "nico",
                "saul",
                "szymon",
                "arron",
                "aston",
                "brian",
                "campbell",
                "ezra",
                "julian",
                "marco",
                "muhammed",
                "zain",
                "ajay",
                "alfred",
                "bobby",
                "cohen",
                "colin",
                "donald",
                "douglas",
                "kenzie",
                "miller",
                "oran",
                "rio",
                "rudi",
                "shane",
                "wiktor",
                "alastair",
                "bruce",
                "clark",
                "ian",
                "kody",
                "lennox",
                "leyton"
            };
            return names;
        }

        private IList<string> GetUSFemaleNames()
        {
            IList<string> names = new List<string>
            {
                "emily",
                "sophie",
                "olivia",
                "isla",
                "jessica",
                "ava",
                "amelia",
                "ella",
                "lucy",
                "lily",
                "grace",
                "chloe",
                "freya",
                "ellie",
                "millie",
                "emma",
                "anna",
                "eva",
                "sophia",
                "mia",
                "charlotte",
                "eilidh",
                "ruby",
                "hannah",
                "aria",
                "evie",
                "georgia",
                "poppy",
                "erin",
                "katie",
                "holly",
                "orla",
                "layla",
                "skye",
                "rosie",
                "harper",
                "maisie",
                "leah",
                "zoe",
                "daisy",
                "amber",
                "amy",
                "hollie",
                "isabella",
                "niamh",
                "molly",
                "robyn",
                "alice",
                "sofia",
                "lilly",
                "maya",
                "lacey",
                "rebecca",
                "scarlett",
                "lexi",
                "willow",
                "abigail",
                "brooke",
                "esme",
                "lola",
                "paige",
                "gracie",
                "emilia",
                "mila",
                "zara",
                "megan",
                "abbie",
                "kayla",
                "sienna",
                "ivy",
                "summer",
                "cara",
                "thea",
                "imogen",
                "sarah",
                "rose",
                "ayla",
                "bella",
                "mya",
                "elizabeth",
                "rachel",
                "iona",
                "julia",
                "elsie",
                "amelie",
                "darcy",
                "lauren",
                "mollie",
                "arianna",
                "eve",
                "matilda",
                "caitlin",
                "beth",
                "maria",
                "phoebe",
                "heidi",
                "hope",
                "ariana",
                "georgie",
                "sadie",
                "aimee",
                "arya",
                "carly",
                "nina",
                "penelope",
                "aila",
                "alexis",
                "emilie",
                "eden",
                "lena",
                "martha",
                "rowan",
                "annabelle",
                "darcie",
                "elise",
                "orlaith",
                "sara",
                "victoria",
                "madison",
                "ailsa",
                "annie",
                "darcey",
                "quinn",
                "violet",
                "bonnie",
                "faith",
                "hanna",
                "lyla",
                "lara",
                "lucie",
                "aoife",
                "myla",
                "taylor",
                "alexandra",
                "casey",
                "clara",
                "lois",
                "penny",
                "cora",
                "maja",
                "naomi",
                "zuzanna",
                "keira",
                "nieve",
                "isobel",
                "lana",
                "callie",
                "florence",
                "halle",
                "macie",
                "bethany",
                "evelyn",
                "flora",
                "laura",
                "lucia",
                "alba",
                "caoimhe",
                "hayley",
                "jennifer",
                "alyssa",
                "ellis",
                "lillie",
                "maci",
                "maddison",
                "natalia",
                "piper",
                "aaliyah",
                "aurora",
                "eliza",
                "hallie",
                "harley",
                "tilly",
                "eleanor",
                "harriet",
                "jasmine",
                "kara",
                "anya",
                "luna",
                "macy",
                "mirren",
                "savannah",
                "alexa",
                "april",
                "catherine",
                "edith",
                "lexie",
                "libby",
                "louisa",
                "marnie",
                "rosa",
                "connie",
                "ella-rose",
                "faye",
                "gabriella",
                "georgina",
                "indie",
                "liliana",
                "lottie",
                "neve",
                "peyton",
                "ada",
                "ciara",
                "elena",
                "esmee",
                "iris",
                "isabelle",
                "laila",
                "maryam",
                "zoey",
                "leila",
                "melissa",
                "pippa",
                "arabella",
                "charlie",
                "eloise",
                "fatima",
                "heather",
                "khloe",
                "louise",
                "orlagh",
                "aisha",
                "alicia",
                "alisha",
                "autumn",
                "belle",
                "kirsty",
                "caitlyn",
                "isabel",
                "jorgie",
                "kacey",
                "katherine",
                "lydia",
                "maia",
                "saoirse",
                "skylar",
                "abby",
                "annabel",
                "carmen",
                "cerys",
                "erica",
                "fearne",
                "frankie",
                "jenna",
                "kate",
                "macey",
                "marley",
                "morgan",
                "nicole",
                "payton",
                "seren",
                "zainab",
                "alicja",
                "aliza",
                "ana",
                "elodie",
                "emelia",
                "harlow",
                "hazel",
                "helena",
                "kaci",
                "kaitlyn",
                "kendall",
                "madeleine",
                "maggie",
                "nadia",
                "alex",
                "amelia-rose",
                "ariella",
                "brooklyn",
                "carys",
                "charley",
                "claudia",
                "darci",
                "emmie",
                "felicity",
                "katy",
                "kiera",
                "stella",
                "talia",
                "vanessa",
                "abbey",
                "alana",
                "aliyah",
                "anaya",
                "ayda",
                "beau",
                "billie",
                "demi",
                "edie",
                "esther",
                "francesca",
                "gabriela",
                "jessie"
            };
            return names;
        }
    }
}

# NameliDotNet
A .Net library for generating fake data. Not super impressed with a library I recently found I decided to roll my own and release it here and allow contributions. I am aiming to make it fully extensible and a bit smarter than your average fake data implementation with a markov chain generator for fake names and addresses and whatnot.

## Classes
**Nameli.cs** is the primary, and really the only, way you'll be interacting with the library (though it's set up to be extended!). I would like to support multiple regions in the future so it accepts a "locale", which is currently locked to the United States, which will help the data being generated match the desired region or country.

Once instantiated Nameli has a number of methods to help generate data:
* FirstName
* LastName
* FirstAndLast
* LastAndFirst
* Birthday
* AddressLineOne
* AddressLineTwo
* StreetAddress
* State
* StateAbbreviation
* County
* Zip
* Country
* Email
* Phone
* SocialSecurityNumber

## Going Forward
I would like to add a few more features as this goes along but what's listed, I believe, is a decent aim for version 1.

## Contributing
Feel free! Jump in, give a hand, and let's get something great going!

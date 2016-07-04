# NameliDotNet
A .NET library for generating fake data. Not super impressed with a library I recently found I decided to roll my own,  release it here and allow contributions. I am aiming to make it fully extensible and a bit smarter than your average fake data implementation with a markov chain generator for fake names and addresses and whatnot.

## Classes
**Nameli.cs** is the primary, and really the only, way you'll be interacting with the library. Nameli is built in a way to be extendable insofar as its methods are all marked as virtual along with emloying the Strategy pattern to allow for modification of the text generation algorithm. Additionally, I would like to support multiple regions in the future so it accepts a "locale", which is currently locked to the United States, which will help the data being generated match the desired region or country.

Once instantiated Nameli has a number of methods to help generate data:
* `FirstName()` generates a realistic first name.
* `LastName()` will generate a realistic last name.
* `FirstAndLast()` will create a name similar to "Marc Smith".
* `LastAndFirst()` creates a name such as "Wendell, Alice".
* `BirthDay()` generates a fake birthdate.
* `AddressLineOne()` creates a fake address.
* `AddressLineTwo()` creates additional data for a fake address.
* `ShippingAddress()` will create a complete shipping address.
* `City()` generates a fake city name.
* `State()` generates a state based on localization.
* `County()` generates a fake county name.
* `Country()` uses an enum to generate a country. Only the United States is currently supported. Sorry!
* `StateAbbreviation()` retrieves a random state, territory, or commonwealth from the currently selected locale.
* `Zip()` generates a random ZIP code.
* `Email()` creates an email address based on a number of factors that are toggleable.
* `Phone()` will generate a realistic phone number in various fashions.
* `SocialSecurityNumber()` will create a fake social security number.
* `CompanyName()` creates a fake company name.
* `Phrase()` uses Markov chains to generate fake text. To work best this should be used with the `ChangeStrategy()` method to ensure the `PhraseStrategy` is used to get the best possible text generation.

Additionally, there are a couple utility methods to assist with text generation:
* `ChangeStrategy()` which will change the text generation algorithm without the need to create a new instance.
* `ChangeLocale()` allows the locale to be changed without needing to instantiate a new object.
* `GetStateAbbreviation()` grabs the abbreviation for a state.
* `CountryAbbreviated()` retrieves the abbreviated form of the currently selected locale.

### Instantiation Examples
Nameli can be instantiated in a few different ways:
* `Nameli nameli = new Nameli();` is the standard initialization.
* `Nameli nameli = new Nameli(new NameStrategy());` forces a specific type of text generation algorithm.
* `Nameli nameli = new Nameli(new NameStrategy(), NameliLocale.UnitedStates);` will initialize a new instance with a specific strategy along with the specified localization for the subsequent text generation.

### Method Examples
A few examples of what you may get from the methods:
* `nameli.FirstAndLast()`: "Aidan Aves"
* `nameli.ShippingAddress(true)`: "Ion Street W Lane, MA 2392 United States"
* `nameli.Email()`: "holmesviolet551@snet.net", "larsonpayton1562@comcast.net"

## Strategies
I chose to implement the Strategy pattern so that the alogrithms for generating useful text could easily be swapped out. The Nameli library has three of them available: my original Markov chain implementation `OriginalWordStrategy`, my subsequent work on a different Markov pattern `NameStrategy`, and finally `PhraseStrategy`. `OriginalWordStrategy` and `NameStrategy` focus on single words for generating text while `PhraseStrategy` works in conjunction with the `Phrase()` method to generate chunks of text that resemble English. The default implementation used by Nameli is `OriginalWordStrategy` and can easily be swapped out via Nameli's constructor or the `ChangeStrategy()` method.

## Going Forward
I would like to add a few more features as this goes along but what's listed, I believe, is a decent aim for version 1. Also, the algorithm for the markov chaining isn't perfect and will likely see revisions in the future, but currently it's a solid first-pass.

## Contributing
Feel free! Jump in, give a hand, and let's get something great going!

## API2Card
API2Card is intended to take an open5e API url and produce data that can be used by rpg-cards utility.

Currently this supports Spells, Monster, Weapons, and Magic Items. 

It should work on any platform supporting dotnet (e.g. Windows, macOS, and Linux)

## How to use

dotnet API2Card.dll CARD_SIZE SEARCH_TYPE SEARCH_ARGS

Example: 
> dotnet API2Card.dll S35x50 Monster Name Goblin > goblin.json

run "dotnet API2Card.dll" without any arguments to get a full list of supported arguments

From here, navigate to https://crobi.github.io/rpg-cards/generator/generate.html (or your own copy) and click "Load from file". Select the file generated.


## Known Issues
Some cards will not generate properly and will look funny. For example "Animate Objects" has an ascii table which you may need to manually format. Not much that I can do now untill rpg-cards supports tables.
The generation scheme is only partially smart, it may push parts of a card to a Part 2 or Part 3 when some finagling would get it to fit on just 1 or 2 cards, such as on Banishment. I have tweaked the settings to return good results 98% of the time and you may need to manually modify the remaining 2%.


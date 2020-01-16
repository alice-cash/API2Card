## API2Card
API2Card is intended to take an open5e API url and produce data that can be used by rpg-cards utility.

Currently this only supports the spells adata. 

It should work on any platform supporting dotnet (e.g. Windows, macOS, and Linux)

## How to use

First, determine the API search query to use. https://open5e.com/api-docs has more information on this. For example the following returns all level 2 spells for the Cleric class:
https://api.open5e.com/spells/?dnd_class__icontains=Cleric&level_int__iexact=2

Next determine the card size you wish to use. The current rpg-cards supports 3.5"x5.0", "7.5"x5.0", 2.5"x3.5", and 2.25"x3.5". These are conviently named as "S35x50, S75x50, S25x35, and S225x35" for the arguments.

From the command line run it as follows:      
> dotnet API2Card.dll S35x50 "https://api.open5e.com/spells/?dnd_class__icontains=Cleric&level_int__iexact=2" "Cleric_lvl2.json"

From here, navigate to https://crobi.github.io/rpg-cards/generator/generate.html (or your own copy) and click "Load from file". Select the file generated.


## Known Issues
Some cards will not generate properly and will look funny. For example "Animate Objects" has an ascii table which you may need to manually format. Not much that I can do now.
The generation scheme is only partially smart, it may push parts of a card to a Part 2 or Part 3 when some finagling would get it to fit on just 1 or 2 cards, such as on Banishment. I have tweaked the settings to return good results 98% of the time and you may need to manually modify the remaining 2%.


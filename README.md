# Sell That!

This mod enables configuration for the traders (Haldor) available wares, as well as what he will buy.

See the [Valheim wiki](https://github.com/Valheim-Modding/Wiki/wiki/ObjectDB-Table) to get a list of item names which can be used.

All changes are done at run-time. Any changes to trader will go back to default if the mod is uninstalled, or the configurations disabled.

# Features

- Remove all existing items
- Add any new item, with custom price and amount.
- List of existing items, for easy modification
- Order items how you want them.
- Set items that Haldor will buy, at the value of your choice.
- Server-side configs

# Manual Installation:

1. Install the [BepInExPack Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/)
2. Download the latest zip
3. Extract it in the \<GameDirectory\>\Bepinex\plugins\ folder.

# Client / Server

Sell That! needs to be installed on all clients to work.

From v1.2.0 clients will request the configurations currently loaded by the server, and use those without affecting the clients config files.
This means you should be able to have server-specific configurations, and the client can have its own setup for singleplayer.
For this to work, the mod needs to be installed on the server, and have configs set up properly there. When players join with Sell That 1.2.0, their mod will use the servers configs.

# How does it work?

Sell That! works by modifying the Trader when he gets "created". 
It loads up his list of items and then clears (optional) and/or inserts item by looking up item names in the games internal item database.

For the buying, Sell That! does not actually modify any item data, but instead injects itself into the store code that checks for what items can be sold, and when sold, it supplies its own value calculation for the item.
By default, it will output the actual value of the item, unless it finds a matching configuration, in which case it supplies that. This is also why the tooltip isn't giving the price. I will get on that at some point.

# Configuration

## General

'sell_that.cfg'

``` INI
[General]

## Kill switch for disabling all mod features.
# Setting type: Boolean
EnableMod = true

## When enabled, all existing items for sales gets removed before adding configured. 
## The easiest way to change any of the existing items, is by setting this to true, and adding all the defaults to the configuration in 'sell_that.selling.cfg'
# Setting type: Boolean
ClearAllExisting = true

[Debug]

## Enable debug logging.
# Setting type: Boolean
EnableDebug = false

## When Trader starts, his items will be logged in a file on disk in the plugin folder.
# Setting type: Boolean
DumpDefaultTraderItemsToFile = false
```

## Trader Wares  

'sell_that.selling.cfg'

Trader items are configured by creating a section as follows:

``` INI
[<Some-Unique-Section-Name>]
ItemName = <ItemPrefabName>
Order = <int> //Tries to insert based in traders list based on this. If -1 (or other negative number), it will be added somewhere at the end.
Price = <int> //Price of the item
StackSize = <int> //Number of items to get pr buy
Enabled = <bool> //Can be disabled without deletion.
```

The file comes pre-defined with all existing trader wares.

Multiple entries of the same item can be done by simply choosing a different section name

## Example

``` INI
[Iron Scrap One]
ItemName = IronScrap
Order = 0
Price = 100
StackSize = 1
Enabled = true

[Iron Scrap Two]
ItemName = IronScrap
Order = 1
Price = 500
StackSize = 5
Enabled = true

```

## Trader Buying

'sell_that.buying.cfg'

Trader will buy items that are configured by creating a section as follows:

``` INI
[<Some-Unique-Section-Name>]
ItemName = <ItemPrefabName>
Price = <int>

```

## Example

``` INI
[Wood One]
ItemName = Stone
Price = 20

[Stone Selling]
ItemName = Wood
Price = 10
```

# Changelog

- v1.2.0
	- Server-to-client config synchronization added.
- v1.1.0
	- Haldor now has a buying list
- v1.0.0
	- Initial release

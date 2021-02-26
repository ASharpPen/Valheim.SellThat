# Sell That!

This mod enables configuration for the traders (Haldor) available wares.

See the [Valheim wiki](https://github.com/Valheim-Modding/Wiki/wiki/ObjectDB-Table) to get a list of item names which can be used.

All changes are done at run-time. Any changes to trader will go back to default if the mod is uninstalled, or the configurations disabled.

This mod does not add support for configuring the selling of items (any item that has value can be sold, modifying the value in item data would allow this).

# Features

- Remove all existing items
- Add any new item, with custom price and amount.
- List of existing items, for easy modification
- Order items how you want them.

# Manual Installation:

1. Install the [BepInExPack Valheim](https://valheim.thunderstore.io/package/denikson/BepInExPack_Valheim/)
2. Download the latest zip
3. Extract it in the \<GameDirectory\>\Bepinex\plugins\ folder.

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

## When Trader starts, his items will be logged in a file on disk next to the mod dll.
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

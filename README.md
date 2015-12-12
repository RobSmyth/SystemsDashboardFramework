# TeamDashboard 

A Windows application (C# WPF) wall team/systems dashboard. This project is something I'm using but is not mature.

![Screen shot](./Images/Screenshot01.png "Screen shot")
# Features ##

- Tiles:
 - TeamCity last/current build configuration monitor
 - TeamCity list of available (last successful) builds
 - Days remaining countdown
 - Today's date
 - Message
 - Webbrowser
 - Image file watcher
- Drag and drop tile insertion
- Drag and drop tile positioning
- Quick configuration in UI
- Tiles can span multiple rows and columns

# Quick start #

- Install & run
- Press INSERT to add a new tile and drag and drop into position
- Double click on a tile to configure it

# Tiles #

## Message

The message tile displays a text message. Useful of adding labels to the dashboard such as your team's name.

![Message tile screen shot](./Images/Screenshot_MessageTile.png "Message tile")

To add a message tile:

1. Press INSERT, the insert pane will appear.
2. Drag and drop the message tile entry to where you want the tile.
3. To change the message double click on the tile.

![Message tile configuration screen shot](./Images/Screenshot_MessageTile_Config.png "Message tile configuration")

## Today's date tile

The today's date tile displays the current date.

![Today's date tile screen shot](./Images/Screenshot_TodaysDateTile.png "Today's date tile")

To add a date tile:

1. Press INSERT, the insert pane will appear.
2. Drag and drop the today's date tile entry to where you want the tile.


## TeamCity tiles

### Build configuration

The TeamCity build configuration tile displays the status of a current build, or if not build is running, the configuration's last build.

After a successful build:

![TeamCity configuration (SUCCESS) tile screen shot](./Images/Screenshot_TC_ConfigTile_Succeeded.png "TeamCity configuration (SUCCESS) tile")

After a failed build:

![TeamCity configuration (FAILED) tile screen shot](./Images/Screenshot_TC_ConfigTile_Failed.png "TeamCity configuration (FAILED) tile")

While building, no failure:

![TeamCity configuration (BUILDING) tile screen shot](./Images/Screenshot_TC_ConfigTile_Building.png "TeamCity configuration (BUILDING) tile")

To add a build tile:

1. Press INSERT, the insert pane will appear.
2. Drag and drop the build tile entry to where you want the tile.
3. Double click on the tile to configure it.

### Build agent

The TeamCity build agent tile displays an build agents state.

THIS TILE IS A WORK IN PROGRESS

![TeamCity agent tile screen shot](./Images/Screenshot_TC_AgentTile_Error.png "TeamCity agent tile")

To add a build agent tile:

1. Press INSERT, the insert pane will appear.
2. Drag and drop the today's date tile entry to where you want the tile.
3. Double click on the tile to configure it.






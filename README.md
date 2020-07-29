# RocketModToOpenMod
Converts RocketMod data files to OpenMod data files.

NOTE: This is not mandatory for the RocketMod link plugin. The point of this is to be a flawless way of having concrete, converted files instead of using the built in PermissionLink (which is more prone to bugs). PermissionLink currently only alters the permissions used at runtime. 

## Instructions

1. Grab the latest release from the releases page

2. Shove the exe file inside your Rocket Folder. For example, C:\Program Files (x86)\Steam\steamapps\common\Unturned\your_server\Rocket

3. Run the exe

4. A new folder named OpenMod will be generated

To save to XML or JSON, use cmd and run the exe file with the args json or xml.


## Usability

Supports:

- [x] Permissions

- [x] Some parts of user data

- [x] Core Translations

- [x] Adding jobs via external assemblies

File Types:

* XML - Read/Write

* YAML - Write

* JSON - Write

## Extending usability

To add a job, build this as a library then reference it in your project. Use the ExternalJob attribute and implement Job. The Job abstract class exposes multiple shared methods such as saving and loading data. Make sure to only have the WriteFileType in your constructor, for name, call it what you want to be the name of your generated file.


# <img src="./.github/icon.png" width="24"/> Libjector

[![](https://img.shields.io/badge/Powered%20By-.NET-blue?logo=microsoft&style=flat-square)](https://dotnet.microsoft.com)
[![](https://img.shields.io/badge/Made%20With-Visual%20Studio-blue?logo=visual-studio&style=flat-square)](https://visualstudio.microsoft.com)

A simple and easy-to-use DLL injector!

This injector is powered by [Bleak](https://github.com/Akaion/Bleak) which is by now deprecated and is no longer being maintained by the [original owner](https://github.com/Akaion), even so, it still works properly as intended. However, not all solutions come without [caveats](https://github.com/Akaion/Bleak#caveats).

## Features

* [X] Supports WoW64 and x64 injection
* [X] Supports multiple injection methods
  * [X] Create Thread: Creates a new thread in the process and uses it to load a DLL
  * [X] Hijack Thread: Hijacks an existing thread in the process and forces it to load a DLL
  * [X] Manual Map: Manually emulates part of the Windows loader to map the DLL into the process
* [X] Supports useful injection flags (for preventing detection by parent process)
  * [X] Hide DLL from [Process Environment Block](https://wikipedia.org/wiki/Process_Environment_Block)
  * [X] Randomize DLL headers
  * [X] Randomize DLL name
* [X] Simplistic user interface for easy use
* [X] Able to determine the architecture of a process or a DLL

## Screenshots

![](./.github/screenshots/0.png)
![](./.github/screenshots/1.png)
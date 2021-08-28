# <img src="./.github/icon.png" width="32"/> DentoInjector

[![](https://img.shields.io/badge/Powered%20By-.NET-blue?logo=microsoft&style=flat-square)](https://dotnet.microsoft.com)
[![](https://img.shields.io/badge/Made%20With-Visual%20Studio-blue?logo=visual-studio&style=flat-square)](https://visualstudio.microsoft.com)

## About This Project

This program injects DLLs into any processes with three special methods and special flags. This program is useful for hackers, developers and cheaters alike. This injector is powered by [Bleak](https://github.com/Akaion/Bleak), which is by now deprecated but it still works without any flaws!

### Features

* [X] Supports WoW64 and x64 injection
* [X] Supports ejection from process
* [X] Has a variety of injection methods
* [X] Has a variety of injection flags
* [X] Has a metro interface and it is well-designed (based on my opinion)
* [X] Can see if process or DLL is 64-bit or 32-bit

## Injection Methods

* CreateThread
* HijackThread
* ManualMap

## Injection Flags

* HideDllFromPeb
* RandomizeDllHeaders (No Ejection Support)
* RandomizeDllName

## Screenshots

![](./.github/screenshots/0.png)
![](./.github/screenshots/1.png)
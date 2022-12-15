# <img src=".github/icon.png" width="48px"/> Libjector

A simple and easy-to-use DLL injector!

## ⚙️ Features

- [X] Supports WoW64 and x64 injection
- [X] Supports multiple injection methods from [Bleak](https://github.com/Akaion/Bleak) and [Lunar](https://github.com/Dewera/Lunar)
  - [X] Create Thread: Creates a new thread in the process and uses it to load a DLL
  - [X] Hijack Thread: Hijacks an existing thread in the process and forces it to load a DLL
  - [X] Manual Map: Manually emulates part of the Windows loader to map the DLL into the process
  - [X] Map Library (Lunar): An alternative injection method; maps DLL directly into memory
- [X] Supports injection and mapping flags
  - [X] Hide DLL from [Process Environment Block](https://wikipedia.org/wiki/Process_Environment_Block)
  - [X] Randomize DLL headers
  - [X] Randomize DLL name
  - [X] Discard headers (Lunar)
  - [X] Skip initialization routines (Lunar)
- [X] Simplistic user interface for easy use (with [Adonis UI](https://github.com/benruehl/adonis-ui))
- [X] Able to determine the architecture of a process or a DLL

## 📸 Screenshots

![](.github/assets/0.gif)
![](.github/assets/1.png)
![](.github/assets/2.png)

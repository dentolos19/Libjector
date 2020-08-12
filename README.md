# WxInjector

This program injects DLLs into any processes with three special methods and special flags. This program is useful for hackers, developers and cheaters alike. This injector is powered by [Bleak](https://github.com/Akaion/Bleak), which is by now deprecated but it still works without any flaws!

ONE THING THAT YOU SHOULD KNOW: The program uses a lot of memory and requires the [.NET Core 3.1+ Runtime](https://dotnet.microsoft.com/download/dotnet-core) to be installed!

**Features**

* Supports WoW64 and x64 injection
* Supports ejection from process
* Has a variety of injection methods
* Has a variety of injection flags
* Has a metro interface and it is well-designed (based on my opinion)
* Can see if process or DLL is 64-bit or 32-bit
* Auto-reports crashes and analytics directly to me
* Created entirely in .NET

**Injection Methods**

* CreateThread
* HijackThread
* ManualMap

**Injection Flags**

* HideDllFromPeb
* RandomizeDllHeaders (No Ejection Support)
* RandomizeDllName

![Preview](https://dentolos19.github.io/previews/wxinjector.png)

### Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

### License

This project is licensed with the [MIT License](https://choosealicense.com/licenses/mit).

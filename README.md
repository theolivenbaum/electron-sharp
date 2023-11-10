<img src="https://github.com/theolivenbaum/electron-sharp/blob/main/assets/electron-sharp-logo.svg" width="100" height="100"/>

# ElectronSharp

Build cross platform desktop apps with .NET 7 and ASP.NET Core (Razor Pages, MVC), Blazor or [h5](https://h5.rocks). 

ElectronSharp is a __wrapper__ around a "normal" Electron application with an embedded ASP.NET Core application. It uses a socket-based IPC bridge we can invoke Electron APIs from .NET.

The CLI extensions hosts the toolset to build and start ElectronSharp-based applications.

ElectronSharp is a hard fork from the original [Electron.NET](https://github.com/ElectronSharp/Electron.NET) project, mantained by [me](https://github.com/theolivenbaum) and used to build the [Curiosity](https://curiosity.ai) app.


## 📦 NuGet:

* API [![NuGet](https://img.shields.io/nuget/v/ElectronSharp.API.svg?style=flat-square)](https://www.nuget.org/packages/ElectronSharp.API/) 
* CLI [![NuGet](https://img.shields.io/nuget/v/ElectronSharp.CLI.svg?style=flat-square)](https://www.nuget.org/packages/ElectronSharp.CLI/)

## 🛠 Requirements to run:

The current ElectronSharp CLI builds Windows/macOS/Linux binaries. The API uses .NET 7, so our minimum base OS is the same as [.NET 7](https://github.com/dotnet/core/blob/main/release-notes/7.0/supported-os.md).

Also you should have installed:

* npm [contained in nodejs](https://nodejs.org)

## 👩‍🏫 Usage

To activate and communicate with the Electron API, include the [ElectronSharp.API NuGet package](https://www.nuget.org/packages/ElectronSharp.API/) in your ASP.NET Core app. Check out the Electron.SampleApp for an example of how to use ElectronSharp.

````
PM> Install-Package ElectronSharp.API
````
### Program.cs

You start ElectronSharp up with an `UseElectron` WebHostBuilder-Extension. 

```csharp
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseElectron(args);
            webBuilder.UseStartup<Startup>();
        });
```

### Startup.cs

Open the Electron Window in the Startup.cs file: 

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    ...

    // Open the Electron-Window here
    Task.Run(async () => await Electron.WindowManager.CreateWindowAsync());
}
```

## 🚀 Start the Application

To start the application make sure you have installed the "[ElectronSharp.CLI](https://www.nuget.org/packages/ElectronSharp.CLI/)" packages as global tool:

```
dotnet tool install ElectronSharp.CLI -g
```

At the first time, you need an ElectronSharp project initialization. Type the following command in your app folder:

```
electron-sharp init
```

* Now a electronnet.manifest.json should appear in your ASP.NET Core project
* Now run the following:

```
electron-sharp start
```
### Hint

> If invoking any of those commands gives you strange errors (like .NET 5 not installed, but your project is .NET 6), it means you've typed electronize instead of electron-sharp.

### Note
> Only the first electronize start is slow. The next will go on faster.

## 🔭 Develop ElectronSharp apps using a file watcher

The file watcher is included with version 8.31.1 of ElectronSharp. For example, a file change can trigger compilation, test execution, or deployment. The ElectronSharp window will automatically refresh and new code changes will be visible more quickly. The following ElectronSharp CLI command is required:

```
electron-sharp start /watch
```

### Note
> Only the first electronize start is slow. The next will go on faster.

## 🐞 Debug

Start your ElectronSharp application with the ElectronSharp CLI command. In Visual Studio attach to your running application instance. Go in the __Debug__ Menu and click on __Attach to Process...__. Sort by your projectname on the right and select it on the list.


## 📔 Usage of the Electron-API

A complete documentation will follow. Until then take a look in the source code of the sample application:  
[ElectronSharp Sample App](https://github.com/theolivenbaum/electron-sharp/tree/main/ElectronSharp.SampleApp)  

  
## ⛏ Build

Here you need the ElectronSharp CLI as well. Type the following command in your ASP.NET Core folder:

```
electronize build /target win
```

There are additional platforms available:

```
electron-sharp build /target win
electron-sharp build /target osx
electron-sharp build /target osx-arm64
electron-sharp build /target linux
```

Those four "default" targets will produce packages for those platforms. Note that the `osx-arm64` is for Apple Silicon Macs.

For certain NuGet packages or certain scenarios you may want to build a pure x86 application. To support those things you can define the desired [.NET runtime](https://docs.microsoft.com/en-us/dotnet/core/rid-catalog), the [electron platform](https://github.com/electron-userland/electron-packager/blob/master/docs/api.md#platform) and [electron architecture](https://github.com/electron-userland/electron-packager/blob/master/docs/api.md#arch) like this:

```
electron-sharp build /target custom "win7-x86;win32" /electron-arch ia32 
```

The end result should be an electron app under your __/bin/desktop__ folder.

### Note
> macOS builds can't be created on Windows machines because they require symlinks that aren't supported on Windows (per [this Electron issue](https://github.com/electron-userland/electron-packager/issues/71)). macOS builds can be produced on either Linux or macOS machines.

## 👨‍💻 Original ([Electron.NET](https://github.com/ElectronSharp/Electron.NET)) Authors 

* **Gregor Biswanger** - (Microsoft MVP, Intel Black Belt and Intel Software Innovator) is a freelance lecturer, consultant, trainer, author and speaker. He is a consultant for large and medium-sized companies, organizations and agencies for software architecture, web- and cross-platform development. You can find Gregor often on the road attending or speaking at international conferences. - [Cross-Platform-Blog](http://www.cross-platform-blog.com) - Twitter [@BFreakout](https://www.twitter.com/BFreakout)  
* **Robert Muehsig** - Software Developer - from Dresden, Germany, now living & working in Switzerland. Microsoft MVP & Web Geek. - [codeinside Blog](https://blog.codeinside.eu) - Twitter [@robert0muehsig](https://twitter.com/robert0muehsig)  
  
See also the list of [contributors](https://github.com/ElectronSharp/Electron.NET/graphs/contributors) who participated in the original project.
  
  
## 🙋‍♀️🙋‍♂ Contributing
Feel free to submit a pull request if you find any bugs (to see a list of active issues, visit the [Issues section](https://github.com/theolivenbaum/electron-sharp/issues).
Please make sure all commits are properly documented.

## 🎉 License
MIT-licensed

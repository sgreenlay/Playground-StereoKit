# StereoKit Playground

This is a project I'm using to play around with [StereoKit](https://github.com/maluoi/StereoKit) on Meta Quest. This project is built with the dotnet CLI and can build/deploy on macOS without Visual Studio.

To build the project:
1. Install [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
2. Install [.NET MAUI](https://dotnet.microsoft.com/en-us/apps/maui) 

    `dotnet workload install android maui`

3. Build and run the app on a Meta Quest

    `dotnet build -t:Run -f net6.0-android`

Note that the first time you build you might see the error:

    `warning XA4301: APK already contains the item lib/arm64-v8a/libStereoKitC.so; ignoring.`

If you do you need to delete `~/.nuget/packages/stereokit/0.3.5/runtimes/linux-*`, clean the project and rebuild.
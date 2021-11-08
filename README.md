# Galaxy
Galaxy is a C# Http server library ((Tcp coming soon!)[https://github.com/jdadonut/galaxy/tree/Tcp]) that works as an alternative to ASP.NET. It is built with .NET Core 5 (should work on .NC6) and is currently in early beta.

## To-Do
- Tcp Support
- Dynamic Uri Parameters
- HTTPS Support 
- Proper Documentation
- WebSocket Support
- Some minor changes to the existing GalaxyHttpServer API (a la {}.UseIndex("index.html"))
- and more!

## Contributing
Make sure to view the discussions and projects before contributing, PRs with breaking changes to any set-in-stone/in-development APIs will be closed.

By contributing, you agree that the code you contribute will become applicable to the license of the repository, no matter what it is at any given time. You will be referenced as a Contributor but you will not have the ability to request your code be removed from the repository or project.

## Installing
Galaxy and Galaxy.Tcp are currently not on the NuGet repositories. You will have to install manually by cloning the repo and referencing the .csproj, as shown in `GalaxyExampleServer/GalaxyExampleServer.csproj`
The library will be added to NuGet repos once it is production-ready, which includes `Proper Documentation`, `HTTPS Support`, `WebSocket Support`, and the minor changes to existing APIs
### orleans ###

[Orleans](http://dotnet.github.io/orleans/Documentation/tutorials_and_samples/overview_helloworld.html) builds on the developer productivity of .NET and brings it to the world of distributed applications, such as cloud services.</br>

### kiwi.orleans ###
kiwi.orleans built on the basis of Mircosoft.Orleans and Mircosoft.CNTK to provides map services, deep learning, deep reinforcement learning and other acpabilities of distributed applications.

### dependencies ###
> [Orleans.Sagas](https://github.com/OrleansContrib/Orleans.Sagas) 

> [Orleans.MultiClient](https://github.com/OrleansContrib/Orleans.MultiClient)


### Add Orleans NuGet Pacakges ###
```
Project	Nuget Package
Silo	Microsoft.Orleans.Server
Silo	Microsoft.Extensions.Logging.Console
//provider
Silo Orleans.Providers.MongoDB

Client	Microsoft.Extensions.Logging.Console
Client	Microsoft.Orleans.Client
Grain Interfaces	Microsoft.Orleans.Core.Abstractions
Grain Interfaces	Microsoft.Orleans.CodeGenerator.MSBuild
Grains	Microsoft.Orleans.CodeGenerator.MSBuild
Grains	Microsoft.Orleans.Core.Abstractions
Grains	Microsoft.Extensions.Logging.Abstractions
```

### Configue MongoDB ###
```
mongod --dbpath <DB_PATH>
```

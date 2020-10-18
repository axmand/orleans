### orleans ###

[Orleans](http://dotnet.github.io/orleans/Documentation/tutorials_and_samples/overview_helloworld.html) builds on the developer productivity of .NET and brings it to the world of distributed applications, such as cloud services.</br>

### kiwi.orleans ###
kiwi.orleans built on the basis of Mircosoft.Orleans and Mircosoft.CNTK to provides map services, deep learning, deep reinforcement learning and other acpabilities of distributed applications.

### version limit ###
>use allowedVersions limit lib version
```
  <package id="ServiceStack" version="3.9.71" allowedVersions="[3.9.71]" targetFramework="net462" />
  <package id="ServiceStack.Common" version="3.9.71" allowedVersions="[3.9.71]" targetFramework="net462" />
  <package id="ServiceStack.OrmLite.SqlServer" version="3.9.71" allowedVersions="[3.9.71]" targetFramework="net462" />
  <package id="ServiceStack.Redis" version="3.9.71" allowedVersions="[3.9.71]" targetFramework="net462" />
  <package id="ServiceStack.Text" version="3.9.71" allowedVersions="[3.9.71]" targetFramework="net462" />
```
### dependencies ###
> [Orleans.Sagas](https://github.com/OrleansContrib/Orleans.Sagas) 

> [Orleans.MultiClient](https://github.com/OrleansContrib/Orleans.MultiClient) 

>[Orleans.Providers.MongoDB](https://github.com/OrleansContrib/Orleans.Providers.MongoDB)

### add orleans nuget pacakges ###
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

### configue mongodb ###
```
mongod --dbpath <DB_PATH>
```
### core function dependencies ###
>Engine.GIS:self-encapsulating libary.
```
PM> Install-Package Engine.GIS -Version 0.0.0.1
```
>Engine.ML.self-encapsulating libary provides CNN,DQN,SVM,RF alogorithms.
```
PM> Install-Package Engine.ML -Version 0.0.0.1
```

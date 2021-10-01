The Link below should help you  get started on your own wep-api

- https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio-code 

- to use this after clonning add the following in your command line:
1. dotnet add package Microsoft.EntityFrameworkCore.InMemory
2. dotnet dev-certs https --trust


Note to Developer:

1. The Launch Url is changed  to : "api/VmApi" and it is changed in the launchSettings.json if it needs to be change din the future.


Model class:
A model is a set of classes that represent the data that the app manages.

Below are commands to create the connected database:

- dotnet tool install --global dotnet-ef
- dotnet ef database drop
- dotnet ef migrations add InitialCreate
NOTE: every command runned inside of vscode to link to the dotnet has to start with "dotnet ef"

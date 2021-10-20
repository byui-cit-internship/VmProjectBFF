The Link below should help you  get started on your own wep-api

- https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio-code 

- to use this after clonning add the following in your command line:
1. dotnet add package Microsoft.EntityFrameworkCore.InMemory
2. dotnet dev-certs https --trust

To do an actual miration to create a database from exiting models:
1. dotnet ef migrations add InitialCreate
2. dotnet ef database update


update datbase by adding a column to a table:
1. dotnet ef migrations add usercolumn
2. dotnet ef database update

Note to Developer:

1. The Launch Url is changed  to : "api/VmApi" and it is changed in the launchSettings.json if it needs to be changed in the future.


Model class:
A model is a set of classes that represent the data that the app manages.

Below are commands to create the connected database:

# To use SQL Server for Mac try this: https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver15&pivots=cs1-bash

   
"DefaultConnection": "Server=host.docker.internal;Database=VmDB;Trusted_Connection=True;"
 "DefaultConnection": "Server=host.docker.internal,1433;Database=VmDB;Trusted_Connection=True;"


 "Server=DESKTOP-VV1SUF4;Database=VmDB;User Id=leonarine; Password=password;"

Server=host.docker.internal,1433;Database=VmDB;User Id=leonarine; Password=password
 dotnet add package FluentEmail.Smtp --version 3.0.0
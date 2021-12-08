# VM project
# Project Requirement:
1. Student have the ability to access/ request a VM through a user interface for a specific class.
2. Professors are able to provision a VM for the class that they are assigned to.
3. Student can only see what classes they are enrolled in.
4. Professor can only see the Class that they are enrolled in.
5. Professor can only change Vm status for Students that are in their own class.


# User Stories (Professors):
1. Professors signs In to application (Login page)
2. Professors sees a list of their classes 
3. Professors is able to click one of their classes and see the Students in that class and the Vm status for that student.
4. Professors is able to add a class for specific section and also add a Canvas token to that class on that section.
5. Professor is able to click a button to change the status of the Vm on a specifc student.
6. Professor is able to change the VM status of all the students in a class for a specific section.
7. Professor is able to create and delete a Vm for their class.

# User Stories(Student):
1. Student signs into the application (Login Page)
2. Student sees a list of the class that they are enrolled in.
3. Student clicks on a class and then sees their Vm and the Status of that Vm for that class
4. Student can send a request to Professor to change the Status of the VM via email.

# SET-UP
The Link below should help you  get started on your own wep-api:
- https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio-code 

TO DO after cloning the Repository:
1. Make sure that you have C # EXtension installed in your VS Code.
2. The following Link can be used to get all the Enviornment set up:https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio-code 

# NOTES about the Key Folders
NOTE: The project is divided into a few key parts that you will spend most of your time:
1. Controllers: This is where all your endpoints are created. Theses are the enpoints that
   the front end will call. We have a few key Controllers that is used
   - CourseController: this list all the End points that the Teacher will need to grab data from the database. If you want to edit and add more endpoints for the teacher this is where you will go.
   - StudentCourseController: This list all the endpoints for the student. Any changes and addition to endpoints that students will use should be added here.
   - EnrollmentController: this has endpoints that will create enrollement for Both student and teachers.
   - TokenController: This as of now Validates a given token, compares the payload with What is in the database and creates a user. NOTE: Will be replacing this soon.
   - VmTableController: this is Reponsible to create all the VM templates.
2. DAL Folder: This folder as of now has two files:
   - DbInitializer: This file prepopulates the Database with Data if the Database does not have any data. If you want to change any of the data and make it appilicable to the database, yiu must first drop the Database. RUN "dotnet ef database drop" to drop the current database and then make your changes. Then RUN "dotnet ef database update" then "dotnet run". NOTE: this will delete current data in the database and replaces it with the data in this file.
   - VmContext: This helps to map our Models into our connected database. If you add any models into the Models folder, and want that to be represented inside your Database, you will have to intialize it here. Follow the patten already outlined.
3. Models folder: This is maps identically to what we want our tables to look like in the Database.
   - Every variable declared with a getter and setter are going to be columns in your tabales.
   - Some of these variables have "[key]" or "[Required]" or "[StringLength(50)]" these are database annotations that can be added to be constrains on the databes. You can look more into it with: https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=net-5.0
4. Appsettings.json, appsetting.Development etc. These are files that helps with the config for the different environment that you will want to run your application in. To change which appsettings to use. Go into the properties folder and then the launchSettings.json and change " "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }" change developement to match which env you want to run in. Each of the appseting files contains a Connection string to a different DATABASE.

    - Connection string: this tells you app which database to connect to. This configuration is set up in the Startup.cs file under the ConfigureServices class.

5. Startup file: this files holds all of our configuration. From the config for Database to authentication config. This files gets called in the Program.cs file to config the App during start up.
6. Migration Folder: this contains all our mapping and all our model to database creation. This folder is generated during what is called a "dotnet migration" Read more about it at : https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli
   - We run migrations when we want to generate an database. Migration helps us to generate a database through what is called Code first database. The migration takes our models goes and connect to our sql server and genrates all the column and tables. This eliminates the need to go and create the Database first.
   - We also run mirgation when we make changes to our Models.
   - To run your first migration, RUN "dotnet ef migrations add [nameofyourMigration- make sure that there are no space], then RUN "dotnet ef database update" to apply the changes.
7. Handlers Folder: This folder contains a file that secures our Endpoints. It helps to add authentication to our endpoints by requiring our user to place a valid token into the Auithorization Headerwhen sending request to any backend api with the [Autorize] annotations.
8. Service folder: This folder contains a file called BackgroundService that runs two canvas api call every 1hour to update the database to enroll new students to the course that the teacher created. it runs through a list of Professor enrollment, grab their courses, from a course it grab the course_id, runs the first api and grabs a list of student. From that list of student it grabs a student and their Id and call another Api which gives their. It then takes that Info, creates a user and then enroll them into that class.


# To scafold a new controller:(creates a new controller for you without you having to type everything):
1. dotnet aspnet-codegenerator controller -name [Nameofmodel]Controller -async -api -m [nameofModel] -dc VmContext -outDir Controllers

# To make changes to database, Migrations
To Drop a database:
1. dotnet ef database drop

To run migration:
1. dotnet ef migrations add InitialCreate
2. dotnet ef database update

update database by adding a column to a table:
1. dotnet ef migrations add usercolumn
2. dotnet ef database update


# NOTES
Below are commands to create the connected database:
To use SQL Server for Mac try this: https://docs.microsoft.com/en-us/sql/linux/quickstart-install-connect-docker?view=sql-server-ver15&pivots=cs1-bash

 TO POINT TO DIFFERENT ENV:
 Note: If your enviornment is pointing to different DB, Make sure that you created that
 Database before
 1. Create your enviornment varible in the terminal with : 
 $env:ASPNETCORE_ENVIRONMENT = 'Production' or any other enviornment that you want to run
 2. Run $env:ASPNETCORE_ENVIRONMENT to make sure that the environment was created

 3. Make sure that you have already created the intitial create to create the migration:
 dotnet ef migrations add <name of your mirgation>

 4. Now apply that miragtion to the database with:
 dotnet ef database update

 5. Check that you tabels are in that DB



Note:
Connection string to SQL on docker
"DefaultConnection": "Server=host.docker.internal;Database=VmDB;Trusted_Connection=True;"
"DefaultConnection": "Server=host.docker.internal,1433;Database=VmDB;Trusted_Connection=True;"


"Server=DESKTOP-VV1SUF4;Database=VmDB;User Id=leonarine; Password=password;"
Server=host.docker.internal,1433;Database=VmDB;User Id=leonarine; Password=password

jenkins: Password: password, username:leonarine
hello
    


README - Developer Evaluation Application

Instructions
All updated code is available in the develop branch.

Steps for Configuration
1. Configure the project for execution:
   - After downloading the code, set the API project (Ambev.DeveloperEvaluation.WebApi) as the startup project.

2. Create the local database:
   - Create the local database in PostgreSQL using the following connection string in the appsettings.json file:
     "DefaultConnection": "Host=localhost;Database=DeveloperEvaluation;Username=postgres;Password=12345678"

3. Execute the Migrations:
   - In the Package Manager Console or via CMD, run the following commands:
     - To add a new migration:
       dotnet ef migrations add [MigrationName] --context DefaultContext --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi
     - To apply the migrations to the database:
       dotnet ef database update


Technical Considerations

1. Repositories
- The native IRepository interface pattern was used to simplify repository creation for persistence logic access.
- A generic repository was implemented, reducing the need to create specific interfaces for each entity and promoting code reuse for new repositories.

2. FluentValidation
- Validations were configured using dependency injection, ensuring greater flexibility and scalability in the application.

3. Unit and Integration Tests
- A test project was created using XUnit.
- The integration tests reuse the same application dependencies via Service Locator, eliminating the need for extensive mocks and allowing faster and more realistic testing.
- An integrated test for Handlers was implemented, which can serve as a reference for creating JSON validations for API endpoints. The test code can be translated into valid JSON for this purpose.


Important Observations

Issues with Migrations
During development, issues were encountered while running migration commands locally. To resolve these issues, a specific process was followed as described below:

Process for Executing Migrations
1. Drop the existing database (if necessary):
   dotnet ef database drop --force

2. Update the database:
   dotnet ef database update

3. List existing migrations:
   dotnet ef migrations list

4. Remove migrations (one at a time, if necessary):
   dotnet ef migrations remove

5. Create migrations again:
   dotnet ef migrations add InitialCreate

Command to Update the Database
- Navigate to the project's root directory in CMD:
  cd C:\Users\[REPLACE_WITH_LOCAL_USER]\source\repos\StenioDinartAmbevDeveloperEvaluation

- Run EF Core commands using the following syntax:
  - To create migrations:
    dotnet ef migrations add UpdateSaleFields --context DefaultContext --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi

  - To update the database:
    dotnet ef database update --context DefaultContext --project src/Ambev.DeveloperEvaluation.ORM --startup-project src/Ambev.DeveloperEvaluation.WebApi


Key Points
- The project follows the existing patterns in the initial template, with adjustments and improvements.
- The use of patterns such as generic repositories, FluentValidation, and integration tests ensures scalability and code reuse.
- The integration tests provide a starting point for complete endpoint validations.

If you have any questions or issues during execution, follow the processes described above or contact the responsible developer.

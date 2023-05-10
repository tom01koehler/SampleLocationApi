Sample REST Api written in .NET 6, EF Core6, and testing using MSTest with Moq.  Application will run immediately on pull with no additional setup; in memory database and cache.

# Requirements

- Write a restful api that takes in a name and address. 
- Persist those inputs.
- Name field should be considered as a unique set.
- Return proper messages for validation, duplicates, and errors.
- Allow updates to those inputs.
- Allow deletes to those inputs.
- Return all inputs persisted.
 
# Technical Requirements
- A .NET Core restful api.
- Use of Dependency Injection.
- Make use SOLID principals.
- Incorporate swagger to test endpoints.
- The persistence can be an in-memory database or a simple no sql db.
- Use a caching layer to retrieve persisted data and keep cache current for all updates.
- Write unit tests making use of mocks.
 
# Tooling Requirements
- Use Visual Studio, latest version if possible.
- Keep all projects in one solution.
- The solution should compile with no errors.
- The application should run cleanly.
- Easy local setup to ensure solution can be executed.
- Use .NET Core 6.
- Caching layer can be In Memory 
- Unit tests can be written using MSTests, NUnit, or Xunit.
- Use Mocks when unit testing persisted data.
- All tests should be rerunnable.



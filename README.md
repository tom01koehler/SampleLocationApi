Sample REST Api written in .NET 6, EF Core6, and testing using MSTest with Moq.

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
- If nosql db used, preference is RavenDb .
- Use .NET Core 6.
- Caching layer can be In Memory 
- Unit tests can be written using MSTests, NUnit, or Xunit.
- Use Mocks when unit testing persisted data.
- All tests should be rerunnable.

# Assumptions

- Use address as string instead of model.
- Unique "name" would be required, implemented with check for existence rather than catching DB error, alternate implementations may require the check.
- Assume "future" feature add of test application, use abstraction and conserder alternate use cases that would potentially reuse interfaces and implementations.

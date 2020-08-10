# People Web API
.Net Core 3.1 Web API running on Linux docker with repository pattern on mssql database, jwt login, in-memory unit tests and docker integration tests.

##JWT
 UserName and password for JWT token is set to "test" for purpose of simplicity in this example.

## Docker
Run by <code>docker-compose -f "docker-compose.yml" up -d --build</code> command to build and run the ASP.NET Core Web API in container with SQL Server running separately in another container with set volume.

## IISExpres
For running it locally on IISExpress change DefaultConnection in appsettings.json file to localDb.

## Integration testing with docker-compose
Run test only by from script, not as Unit tests in Visual Studio, then they will fail.

 ### The `run-integration-tests.bat` file
 This script will launch the `docker-compose` command to build and run the ASP.NET Core and the integration test projects in a container. As described by the `docker-compose-integrationtests.yml` file, here's the hierearchy of containers:
 * The integration test project
   * depends upon the webapi container
     * which depends upon a Sql Server for Linux container


 Integration tests will run as soon as the two containers have been created and a green message will appear in the output.

 Containers are stopped and the end and recreated at each execution of the script, so that test results can be deterministic.

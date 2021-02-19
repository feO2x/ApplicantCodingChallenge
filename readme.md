# Applicant Coding Challenge

This coding challenge consists of an ASP.NET Core web app and an Aurelia web client.

### How to Run the Web Service

To run the web service, please do one of the following:

- either open Visual Studio and press F5 to start debugging
- or open a command line and execute `dotnet run`

You can configure the web service by creating a custom appsettings.Development.json file which is ignored by git by default. You can overwrite the default settings of appsettings.json in this file.

The web service runs on http://localhost:5000 and https://localhost:5001 by default. Use https://localhost:5001/swagger to explore the Web API.

### How to Run The Web Client

To run the web client, you must first install the missing node_modules once via `yarn install` or `npm install` (one of these tools must be configured as a global tool). lock files are explicitely excluded in git so that you can use the package manager of your choice for the web client.

Afterwards, you can start the app via `yarn start` or `npm run start`. The web client runs on http://localhost:8080 by default and communicates with the web service on the aforementioned default ports. Please adjust "config/environment.json" if you let the service run on a different port.
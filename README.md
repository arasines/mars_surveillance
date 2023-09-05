# Welcome to Mars Surveillance Robot Simulator!
The Olympic Channel has recently developed a **Mars Surveillance Robot** with the intent of exploring the surface on Mars and explore suitability for future Olympic Sports on low gravity over there.

### Features

- .NET 7.0
- Support for REST API endpoints
- Clean Architecture on CQRS


### Tools required to run project

- .[NET SDK](https://dotnet.microsoft.com/en-us/download "NET SDK") - includes the .NET runtime and command line tools
- [Visual Studio Code](https://code.visualstudio.com/ "Visual Studio Code") - code editor that runs on Windows, Mac and Linux
- [C# extension for Visual Studio Code](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp "C# extension for Visual Studio Code") - adds support to VS Code for developing .NET applications

###  Run the Mars Surveillance Robot 
This solution contains a console application and a REST API service for processing robot JSON files.
1. Download or clone the project code from https://github.com/arasines/mars_surveillance

2. Open a terminal and navigate to the root directory of the solution.
3. Start the simulator by executing the BAT file **obs_test.bat** on the solution root folder  (where the mars_surveillance.sln file is located) :

	`obs_test c:\mypath\jsonInput.json c:\output\jsonOutput.json`
	to run the console program with input and output.

	`obs_test `
	without parameters to run the REST API

### Using the obs_test Script
To run the application on Windows on console mode, open a terminal and navigate to the root directory of the solution. Use the following command: 

`obs_test <inputPath>  <outputPath>`

To run the application on Windows on API mode, open a terminal and navigate to the root directory of the solution. Use the following command: 

`obs_test `

 You should see the message Now listening on: https://localhost:7198 and http://localhost:5007.  You also should be not able to access the [Swagger](https://localhost:7198/swagger/index.html "Swagger") endpoint to test the API.

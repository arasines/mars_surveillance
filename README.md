# Welcome to Mars Surveillance Robot Simulator!

The Olympic Channel has recently developed a **Mars Surveillance Robot** with the intent of exploring the surface on Mars and explore suitability for future Olympic Sports on low gravity over there.

## w to Run
This solution contains a console application and a REST API service for processing robot JSON files.
We also provide a obs_test BAT file to automatically launch the console or the API:

- obs_test c:\mypath\jsonInput.json c:\output\jsonOutput.json to run the console program with input and output.
- obs_test without parameters to run the REST API

### Using the obs_test Script
To run the application on Windows, open a terminal and navigate to the root directory of the solution. Use the following command: 

`obs_test <inputPath>  <outputPath>`

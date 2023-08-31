#!/bin/bash

# Check the number of arguments
if [ $# -eq 0 ]; then
    echo "No arguments provided. Running the REST API."
    # Run REST API
    dotnet run --project obs_test_api
elif [ $# -eq 2 ]; then
    echo "Running console application with input and output paths."
    # Run console application
    dotnet run --project obs_test_console $1 $2
else
    echo "Invalid number of arguments."
fi
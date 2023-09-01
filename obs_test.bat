@echo off
cls
REM This is obs_test.bat

ECHO             _____  ______    _       ______        ______     _    
ECHO            / ___ \(____  \  ^| ^|     ^|  ___ \   /\ (_____ \   ^| ^|   
ECHO           ^| ^|   ^| ^|____)  )  \ \    ^| ^| _ ^| ^| /  \ _____) )   \ \  
ECHO           ^| ^|   ^| ^|  __  (    \ \   ^| ^|^| ^|^| ^|/ /\ (_____ (     \ \ 
ECHO           ^| ^|___^| ^| ^|__)  )____) )  ^| ^|^| ^|^| ^| ^|__^| ^|    ^| ^|_____) )
ECHO            \_____/^|______(______/   ^|_^|^|_^|^|_^|______^|    ^|_(______/                                                     
ECHO                    ______   _____  ______   _____  _______ 
ECHO                   (_____ \ / ___ \(____  \ / ___ \(_______)
ECHO                    _____) ) ^|   ^| ^|____)  ) ^|   ^| ^|_       
ECHO                   (_____ (^| ^|   ^| ^|  __  (^| ^|   ^| ^| ^|      
ECHO                         ^| ^| ^|___^| ^| ^|__)  ) ^|___^| ^| ^|_____ 
ECHO                         ^|_^|\_____/^|______/ \_____/ \______)
echo.
echo.
REM Check for the -help option
IF "%~1"=="-help" (
	echo.
    echo Usage:
    echo.
    echo Run the REST API:
    echo   obs_test
    echo.
    echo Run the console application:
    echo   obs_test inputPath outputPath
    echo.
    goto :EOF
)
REM Check for the number of command-line arguments
IF "%~1"=="" (
    REM No arguments provided, run the REST API
    dotnet run --project obs_test_api
) ELSE IF "%~3"=="" (
    REM Two arguments provided, run the console application
    dotnet run --project obs_test_console %1 %2
) ELSE (
    REM Invalid number of arguments provided
    echo Invalid number of arguments.
)
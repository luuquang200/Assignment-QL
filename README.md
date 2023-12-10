# Running the Application

This application filters offers based on certain criteria. Here are the steps to set up the environment and run the application:

## Prerequisites

- .NET 7.0
- Newtonsoft.Json package

## Setup

1. **Install .NET Core SDK**: First, you need to install the .NET Core SDK on your computer. You can download it from the official Microsoft website or from [here](https://dotnet.microsoft.com/en-us/download/visual-studio-sdks).

2. **Clone the repository**: Clone the source code to your local machine.

3. **Install the Newtonsoft.Json package**: Navigate to the project directory and run the following command to add the Newtonsoft.Json package to the project:
    ```
    cd Assignment-QL\HomeProject
    ```
    ```
    dotnet add package Newtonsoft.Json
    ```

## Running the Application

1. **Prepare the input file**: Place the `input.json` file in the project directory.

2. **Run the application**: Open a command prompt, navigate to the project directory, and run the following command:
    ```
    dotnet run
    ```
    The application will prompt you to enter a date in the format `YYYY-MM-DD`. Enter the date and press `Enter`.

3. **View the results**: The filtered offers will be saved in the `output.json` file in the project directory. The console will also display the number of offers filtered and their titles.

## Notes

- The application will continue to prompt for dates until you enter `q` to quit.
- If the `input.json` file is not found, the application will display a message and prompt for the date again.
- If the entered date is not in the correct format, the application will display a message and prompt for the date again.

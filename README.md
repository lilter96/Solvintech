# Solvintech
API service for token-based authentication with daily renewal, integrated with the Bank of Russia exchange rates, built on ASP.NET Core and React.

## Running the Application Locally

This guide provides instructions on how to run the application locally for both development and production environments.

### Development Environment

1. **Prepare the Client Application:**
   - Navigate to the `clientapp` directory:
   - Install Node.js dependencies:
     ```
     npm install
     ```
   - Start the client application in development mode:
     ```
     npm run start
     ```

2. **Start the ASP.NET Core Application:**
   - For Visual Studio users, select the IIS Express profile and start the application as you would in development.
   - For CLI users, navigate to the ASP.NET Core project directory and execute (ensure you are in the directory containing the Solvintech.Presentation.csproj file):
     ```
     dotnet run --launch-profile "IIS Express"
     ```
   - You can manually navigate to the port indicated in your terminal (https) to view the application

### Production Environment

1. **Build the Client Application:**
   - In the `clientapp` folder:
     ```
     npm install
     npm run build
     ```

2. **Start the ASP.NET Core Application with Production Profile:**
   - For Visual Studio users, select the IIS Express Prod profile and start the application as you would in development.
   - For CLI users, navigate to the ASP.NET Core project directory and execute (ensure you are in the directory containing the Solvintech.Presentation.csproj file):
     ```
     dotnet run --launch-profile "IIS Express Prod"
     ```
   - You can manually navigate to the port indicated in your terminal (https) to view the application

# Main page
![image](https://github.com/lilter96/Solvintech/assets/72393399/ae73a44c-df46-481b-8538-eba7e1cabdb3)

# After login
![image](https://github.com/lilter96/Solvintech/assets/72393399/46ee8988-bebb-49e6-afd1-8ddb23b0570c)

# After registration
![image](https://github.com/lilter96/Solvintech/assets/72393399/828b38f7-094d-45b6-b19f-4d88fe407c37)

# Quotation endpoint
![image](https://github.com/lilter96/Solvintech/assets/72393399/25e345db-0d4c-4baf-a9ec-ab948b02efdc)

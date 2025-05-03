# NBATyPlay

## Table of Contents
- [Project Description](#project-description)
- [Tech Stack](#tech-stack)
- [Getting Started Locally](#getting-started-locally)
- [Available Scripts](#available-scripts)
- [Project Scope](#project-scope)
- [Project Status](#project-status)
- [License](#license)

## Project Description
NBATyPlay is a web application that enables users to create and participate in private NBA league betting. Administrators can create leagues with unique access codes and add NBA matches. Users register, join leagues using the access code, and place bets on match outcomes. The platform handles user registration/login, password hash using SHA256, match scheduling, automatic bet locking after match start, point calculation for correct bets, and ranking based on points.

## Tech Stack
- **Backend:** .NET 8 with Razor Pages
- **ORM & Database:** Entity Framework Core with MSSQL
- **Frontend:** Minimal JavaScript with responsive design supported by Bootstrap 5
- **CI/CD:** GitHub Actions
- **Hosting:** Options include Azure or Heroku (to be determined)

## Getting Started Locally
1. **Prerequisites:**
   - [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
   - MSSQL Server instance (or SQL Express)
2. **Clone the repository:**  
   `git clone https://github.com/tomaszprasolek/NBA_Betting.git`

3. **Navigate to the project directory:**  
      `cd NBA_Betting`
4. **Restore dependencies:**  
   `dotnet restore`
5. **Build the project:**  
   `dotnet build`
6. **Run the project:**  
   `dotnet run`
7. **Open your browser and navigate to:** http://localhost:5000
   
## Project Scope
- **User Management:** Registration, login, password hashing with SHA256, unique username validations.
- **Role and Permissions:** Management for SuperAdministrators, Administrators, and Users.
- **League Management:** Create leagues with unique access codes, join leagues.
- **Match Management:** Adding NBA matches with pre-defined teams, scheduling, input validation, and duplicate prevention.
- **Betting System:** Users place bets on match outcomes and score points based on correct predictions.
- **Ranking and Statistics:** Display user rankings within each league and show basic league statistics.
- **Administration Features:** Reset user passwords, delete accounts without losing historical data, and manage system statistics.

## Project Status
The project is currently in the MVP stage, focusing on the core functionalities required for private NBA league betting and basic administration.

## License
This project is licensed under the MIT License. (If a different license is required, please update accordingly.)
# ATMApp — Console Banking Simulation

A simple C# console banking simulation.

## Features

-   Login with card number and PIN
-   Deposit and withdraw funds
-   Account locks after 10 failed login attempts
-   PINs are hashed for security
-   User input is validated via validator methods

## Architecture Overview

-   `Domain/Entities/Account.cs` — Account data model
-   `Domain/Entities/MockData.cs` — Loads sample account data from JSON
-   `Domain/Services/AccountService.cs` — Transaction logic
-   `Domain/Services/UserLoginService.cs` — User authentication
-   `Domain/Services/ValidatorService.cs` — Business rules validation
-   `Domain/Utilities/Security.cs` — PIN hashing and verification
-   `UI/AppScreen.cs` — Console interface and user prompts
-   `UI/Validator.cs` — Raw user input validation
-   `App/ATMAppController.cs` — Application flow control
-   `App/Program.cs` — Entry point
-   `MockData/accounts.json` — Sample account data for testing

## Testing

Unit tests use NUnit. To run tests:

```bash
dotnet test
```

## Test Credentials (Mock Data)

| Full name      | Card number | PIN    |
| -------------- | ----------- | ------ |
| Ethan Carter   | 3636363636  | 123456 |
| Sophia Bennett | 9696969696  | 246810 |
| Liam Anderson  | 8686868686  | 357911 |
| Olivia Morgan  | 7878787878  | 111111 |
| James Walker   | 4848484848  | 222222 |

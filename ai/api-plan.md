# REST API Plan

This plan defines a RESTful API for NBATyPlay. It maps directly to the database schema, product requirements (PRD), and our .NET 8/Razor Pages tech stack.

---

## 1. Resources

- **Users** (maps to AspNetUsers)  
  Core user/identity data used for registration, login, and account management.

- **Leagues**  
  Represents private leagues. Each league has a unique LeagueCode, Name, Description, Owner, and links to matches and memberships.

- **Teams**  
  Static list of NBA teams used to select home/away teams when creating matches.

- **Matches**  
  Represents NBA matches within a league. Includes team selections, scheduled date/time, and results, with validation to ensure teams differ.

- **Bets**  
  Contains user bets on matches. Tracks bet choice and points awarded based on the match outcome.

- **LeagueMemberships**  
  Maps users to leagues with the role they play in the league (Admin, User, etc.).

---

## 2. Endpoints

### 2.1. Authentication & User Management

- **Registration**  
  - **Method:** `POST`  
  - **URL:** `/api/auth/register`  
  - **Description:** Register a new user. Requires password to be entered twice to confirm and prevent typing mistakes.
  - **Request Payload:**
    ```
    {
        "username": "string",
        "password": "string",
        "confirmPassword": "string"
    }
    ```
  - **Response:**  
    - `201 Created` on success.  
    - Error codes:
      - `400 Bad Request` for duplicate username or invalid data
      - `400 Bad Request` with message "Passwords do not match" when password confirmation fails

- **Login**  
  - **Method:** `POST`  
  - **URL:** `/api/auth/login`  
  - **Description:** Authenticate an existing user.  
  - **Request Payload:**
    ```
    {
      "username": "string",
      "password": "string"
    }
    ```
      - **Response:**  
    - `200 OK` with authentication token/session info.  
    - `401 Unauthorized` for incorrect credentials.

- **Get Current User Profile**  
  - **Method:** `GET`  
  - **URL:** `/api/users/me`  
  - **Description:** Retrieve the currently logged-in user’s profile.  
  - **Response:**  
    ```
    {
      "userId": "string",
      "username": "string",
      "roles": ["string"],
      ...
    }
    ```

- **Update User Profile**  
  - **Method:** `PUT`  
  - **URL:** `/api/users/me`  
  - **Description:** Update user profile information.  
  - **Request Payload:**  
    ```
    {
      "username": "string",
      "password": "string"  // New password if required; will be hashed with SHA256.
    }

    ```
  - **Response:**  
    - `200 OK` on success.

---

### 2.2. League Operations

- **List Leagues (User's Memberships)**  
  - **Method:** `GET`  
  - **URL:** `/api/leagues`  
  - **Description:** Retrieve leagues to which the user belongs.  
  - **Query Parameters:**  
    - `page` (optional), `pageSize` (optional), `sortBy` (optional).  
  - **Response:**  
    ```
    [
      {
        "leagueId": 1,
        "leagueCode": "ABC123",
        "leagueName": "My League",
        "description": "Description",
        "ownerId": "string",
        "createdDate": "2025-05-01T12:00:00Z"
      },
      ...
    ]

    ```

- **Create League**  
  - **Method:** `POST`  
  - **URL:** `/api/leagues`  
  - **Description:** Create a new league. The system will generate a unique LeagueCode and automatically assign the creator as the owner.  
  - **Request Payload:**
    ```
    {
      "leagueName": "string",
      "description": "string"
    }

    ```
  - **Response:**  
    - `201 Created` with league details.  
    - `400 Bad Request` when league name is not unique.

- **Get League Details**  
  - **Method:** `GET`  
  - **URL:** `/api/leagues/{leagueId}`  
  - **Description:** Get detailed information about a specific league.  
  - **Response:** Same as the create league payload with additional membership info.

- **Join League**  
  - **Method:** `POST`  
  - **URL:** `/api/leagues/join`  
  - **Description:** Join an existing league using a provided LeagueCode. A user cannot join the same league more than once.  
  - **Request Payload:**
    ```
    {
      "leagueCode": "string"
    }
    ```
  - **Response:**  
    - `200 OK` on success.  
    - `404 Not Found` or `400 Bad Request` if the LeagueCode is invalid.

- **Update/Delete League**  
  - **Method:** `PUT`/`DELETE`  
  - **URL:** `/api/leagues/{leagueId}`  
  - **Description:** Update league details or delete a league (restricted to league owners or SuperAdministrators).  
  - **Request Payload (PUT):**
    ```
    {
      "leagueName": "string",
      "description": "string"
    }
    ```
  - **Response:**  
    - `200 OK` on success or `403 Forbidden` if not authorized.

---

### 2.3. Match Management

- **List Matches**  
  - **Method:** `GET`  
  - **URL:** `/api/leagues/{leagueId}/matches`  
  - **Description:** Retrieve matches for a specific league with optional pagination/sorting by `matchDateTime`.  
  - **Query Parameters:**  
    - `page`, `pageSize`, `sortBy`, `filter`.
  - **Response:**  
    ```
    [
      {
        "matchId": 1,
        "leagueId": 1,
        "homeTeamId": 2,
        "awayTeamId": 3,
        "matchDateTime": "2025-06-01T19:00:00Z",
        "result": 1,
        "createdDate": "2025-05-01T12:00:00Z"
      },
      ...
    ]

    ```

- **Create Match**  
  - **Method:** `POST`  
  - **URL:** `/api/leagues/{leagueId}/matches`  
  - **Description:** Create a match within a league. Validate that the home and away teams are different and the match is scheduled in the future.  
  - **Request Payload:**
    ```
    {
      "homeTeamId": 2,
      "awayTeamId": 3,
      "matchDateTime": "2025-06-01T19:00:00Z"
    }
    ```
  - **Response:**  
    - `201 Created` with match details.  
    - `400 Bad Request` if validations fail.

- **Update/Delete Match**  
  - **Method:** `PUT`/`DELETE`  
  - **URL:** `/api/leagues/{leagueId}/matches/{matchId}`  
  - **Description:** Update or remove a match only if it has not started yet.  
  - **Request Payload (PUT):**
    ```
    {
      "homeTeamId": 2,
      "awayTeamId": 3,
      "matchDateTime": "2025-06-01T19:00:00Z"
    }
    ```
  - **Response:**  
    - `200 OK` on success.  
    - `403 Forbidden` if attempting to edit a started match.

- **Enter Match Result**  
  - **Method:** `POST`  
  - **URL:** `/api/leagues/{leagueId}/matches/{matchId}/result`  
  - **Description:** Submit the final result of a match. Once a result is entered, the system evaluates all bets.  
  - **Request Payload:**
    ```
    {
      "result": 1  // 1 = Home win, 2 = Away win, 3 = Overtime
    }
    ```
  - **Response:**  
    - `200 OK` after processing bets.  
    - `400 Bad Request` if result is invalid.

---

### 2.4. Betting Operations

- **Place/Update Bet**  
  - **Method:** `POST` or `PUT`  
  - **URL:** `/api/matches/{matchId}/bets`  
  - **Description:** Place a new bet or update an existing bet for a match. Only allowed if the match has not started.  
  - **Request Payload:**
    ```
    {
      "betChoice": 1  // 1 = Home, 2 = Away, 3 = Overtime
    }
    ```
  - **Response:**  
    - `201 Created` (POST) or `200 OK` (PUT) on success.  
    - `403 Forbidden` if the match has already started.

- **List Bets for a Match**  
  - **Method:** `GET`  
  - **URL:** `/api/matches/{matchId}/bets`  
  - **Description:** For administrators, view all bets on a specific match.  
  - **Response:**  
    ```
    [
      {
        "betId": 12,
        "userId": "string",
        "matchId": 1,
        "betChoice": 2,
        "pointsAwarded": 1,
        "createdDate": "2025-05-05T15:00:00Z"
      },
      ...
    ]
    ```

---

### 2.5. Rankings

- **Get League Rankings**  
  - **Method:** `GET`  
  - **URL:** `/api/leagues/{leagueId}/rankings`  
  - **Description:** Retrieve the ranking of users in a league, sorted by points (and alphabetically by username in ties).  
  - **Response:**  
    ```
    [
      {
        "userId": "string",
        "username": "string",
        "points": 10
      },
      ...
    ]
    ```

---

### 2.6. Administration (SuperAdministrator Endpoints)

- **List Users**  
  - **Method:** `GET`  
  - **URL:** `/api/admin/users`  
  - **Description:** Retrieve a list of all users for management purposes.  
  - **Response:** List of users with basic details.

- **Reset User Password**  
  - **Method:** `PUT`  
  - **URL:** `/api/admin/users/{userId}/reset-password`  
  - **Description:** Reset the password for a selected user. A new temporary password is generated.  
  - **Response:**  
    - `200 OK` on success.

- **Delete User Account**  
  - **Method:** `DELETE`  
  - **URL:** `/api/admin/users/{userId}`  
  - **Description:** Delete a user account. Historical league/bet data remain intact.  
  - **Response:**  
    - `200 OK` on a successful deletion.

---

## 3. Authentication and Authorization

- **Mechanism:**  
  Use ASP.NET Core Identity with cookie-based authentication.  
  - **Registration/Login:** Endpoints handle password hashing using SHA256 as specified in the PRD.  
  - **Role-Based Access:**  
    Use the built-in role management to authorize endpoints. For example, endpoints to manage leagues and matches enforce that only league owners or administrators can perform updates or enter match results.
- **Security:**  
  - Require HTTPS  
  - Validate that users can only interact with resources for which they have permissions  
  - Apply request throttling and other security best practices in production.

---

## 4. Validation and Business Logic

- **Validation Rules:**  
  - **Users:**  
    Validate uniqueness of usernames during registration.
  - **Leagues:**  
    Ensure `LeagueName` is unique and generate a unique `LeagueCode`.
  - **Matches:**  
    Validate that `HomeTeamId` and `AwayTeamId` are different and that `MatchDateTime` represents a future scheduling.
  - **Bets:**  
    Allow bet placement only before a match starts and enforce that each user can bet only once per match.
- **Business Logic:**  
  - **Match Result Processing:**  
    On entering a match result, the API evaluates all associated bets and awards points (1 point for correct predictions and 0 for overtimes as per PRD guidelines).
  - **Ranking Calculation:**  
    Rankings are computed dynamically based on points awarded and sorted in descending order. Users with equal points are sorted alphabetically by username.
  - **League Administration:**  
    League creation automatically assigns the creator while ensuring code uniqueness. Joining a league is handled by validating the provided code and preventing duplicate memberships.

---

*Assumptions:*  
- The API will be consumed by a web client integrated with Razor Pages.  
- Pagination, filtering, and sorting are implemented in list endpoints for scalability.  
- Error handling follows standard HTTP status codes with clear messaging for invalid operations.

This comprehensive plan addresses all key aspects from the database schema, PRD, and our tech stack, ensuring that the API is secure, maintainable, and aligned with the product requirements.

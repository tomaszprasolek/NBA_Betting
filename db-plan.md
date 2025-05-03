# Database Schema Plan

## Overview
The database schema for the MVP version of the application includes the following main entities:
- **Users** (based on ASP.NET Core Identity, extended with additional attributes)
- **Leagues**
- **Teams**
- **Matches**
- **Bets**
- **LeagueMemberships**

## Table Schemas

### Users
Based on ASP.NET Core Identity, extended with additional attributes:

### Leagues

### Teams

### Matches

### Bets

### LeagueMemberships

## Key Design Decisions
1. **Identity Tables Extension**: The standard ASP.NET Core Identity tables are extended with additional attributes specific to the application.
2. **Many-to-Many Relationship**: A many-to-many relationship between Users and Leagues is implemented using the `LeagueMemberships` table, which also tracks the user's role in the league (`RoleInLeague` as `TINYINT`).
3. **Unique Constraints**: Unique constraints are enforced for:
   - User logins
   - League codes
   - Team names
4. **Enum Mapping**:
   - The `BetChoice` column in the `Bets` table is of type `TINYINT` and maps to an enum in the code (e.g., Home, Away, Overtime).
   - The `RoleInLeague` column in the `LeagueMemberships` table is also of type `TINYINT` and maps to an enum in the code (e.g., Admin, User).
5. **Audit Columns**: All main tables include the following audit columns:
   - `CreatedDate`
   - `ModifiedDate`
6. **Simplified MVP Implementation**:
   - Cascading delete mechanisms are disabled.
   - Advanced Row-Level Security (RLS) is not implemented.

## Unresolved Issues
1. **Indexing Strategy**: The exact indexing strategy for frequently used columns is yet to be determined.
2. **Future Audit Requirements**: Additional audit and security requirements may need to be addressed in future iterations.

## Next Steps
- Finalize indexing strategies based on usage patterns.
- Revisit audit and security requirements post-MVP.

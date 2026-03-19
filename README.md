# Community Library Desk

ASP.NET Core MVC application for tracking Books, Members, and Loans in a small community library.

**Assessment #1 — Modern Programming Principles and Practice (Semester 2)**

---

## Tech Stack

- ASP.NET Core 8 MVC
- Entity Framework Core 8 with SQLite
- ASP.NET Core Identity (roles + users)
- Bogus (fake seed data)
- xUnit (unit tests)
- GitHub Actions CI

---

## Project Structure

```
CommunityLibrary.sln
├── CommunityLibrary.Domain/        # Entity classes (Book, Member, Loan)
├── CommunityLibrary.MVC/           # ASP.NET Core MVC web app
│   ├── Controllers/
│   ├── Data/                       # DbContext + seeder
│   ├── Models/                     # ViewModels
│   └── Views/
└── CommunityLibrary.Tests/         # xUnit tests
```

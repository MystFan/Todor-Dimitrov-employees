PairOfEmployees
================

Overview
--------
PairOfEmployees is a small Razor Pages web application (ASP.NET Core, .NET 10) that implements a solution for identifying pairs of employees who have worked together on the same project. The app accepts or reads employee project assignment data (employee id, project id, date from, date to) and calculates which pair(s) of employees have spent the longest time working together on common projects.

Key features
------------
- Razor Pages frontend for submitting or uploading input data
- Core processing logic that computes overlapping work periods per project and sums total collaboration time between employee pairs
- Console / file input support for CSV-formatted records (depending on included sample and utilities)

Tech stack
----------
- .NET 10
- ASP.NET Core Razor Pages

Getting started
---------------
Prerequisites:
- .NET 10 SDK (install from https://dotnet.microsoft.com)

Run locally:
1. Open a terminal in the solution folder: PairOfEmployees
2. Change directory to the web project if needed, for example:
   cd PairOfEmployees
3. Restore and build:
   dotnet restore
   dotnet build
4. Run the application:
   dotnet run

Project layout
--------------
- PairOfEmployees/        - Razor Pages web project (UI + app logic)
- README.md               - This document

Contributing
------------
Contributions, bug reports or improvements are welcome. Please open an issue or a pull request on the repository.

License
-------
Check the repository for a LICENSE file. If none exists, contact the project owner for licensing details.


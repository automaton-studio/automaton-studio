# Automaton

Automaton is an RPA like solution that makes it easier to automate software tasks.

## Updates

15/10/2023 
The project is still in early stage, but it's getting close to the alpha version. A demo website will be deployed to showcase the core functionality.

Until then, here are are few of the intended capabilities:
- create and debug flows
- deploy runner desktop applications on Windows machines to execute scheduled flows (intention is to add capability for Linux)
- create custom steps written in Python (big thing because it makes the application extendable)
- execute flows anywhere you have a working runner
- comes with basic logic steps like programming conditionals, logic loops, HTTP requests, sending email (more will be added on the fly)

The application can be installed on favourite web server that supports .NET 7/Blazor. Persistency happens in MySQL databases but will hopefully support MSSQL and others as well.

## Screenshots

![Screenshot 02](/screenshots/Screenshot 2023-10-16 001341.png?raw=true "Automaton Studio Flows")

![Screenshot 01](/screenshots/Screenshot-2022-05-27-103012.png?raw=true "Automaton Studio Designer")

## Technical Details

The solution consists of:
- Desktop/Web application used to design, debug, test and orchestrate flows
- Runner application to execute flows on other machines

Automaton Studio is written in C# / Blazor, and it relays on a other opensource projects.

## Licence

Apache License 2.0

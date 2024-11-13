# Automaton

Automaton is an RPA application which helps automate software tasks.

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

![Screenshot 01](/screenshots/Screenshot-2024-01-22-231011.png?raw=true "Studio Flow")

![Screenshot 02](/screenshots/Screenshot-2024-01-22-230540.png?raw=true "Studio Activity")

![Screenshot 03](/screenshots/Screenshot-2024-01-22-231417.png?raw=true "Studio Step")

![Screenshot 04](/screenshots/Screenshot-2024-01-22-231131.png?raw=true "Studio Python")

## Licence

Apache License 2.0

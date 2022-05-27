# Automaton

Automaton is an RPA experiement that tries to replicate (as open source) the famous Microsoft Flow Desktop application.

## Screenshots
![Screenshot 01](/screenshots/Screenshot-2022-05-27-103012.png?raw=true "Automaton Studio Designer")

![Screenshot 02](/screenshots/Screenshot-2022-05-27-103112.png?raw=true "Automaton Studio Flows")

## Details
The project is still in a very early stage, but the core infrastructure is getting there.

## Technical Details

The solution consists of:
- A Desktop Studio to design and execute flows locally
- A Web Studio based on the same core as desktop version to execute flows anywhere
- A Runner to execute flows on other computers

Studio is written in C# and Blazor and it relays on a few other opensource projects. Its core consists of a custom workflow solution made from scratch.

## Licence
MIT License
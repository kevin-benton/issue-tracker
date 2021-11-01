# Issue Tracker

This repository holds a simplified implementation of an issue tracker. I use this repo to learn 
new tech, new design patterns, and other ideas I wish to try. This README will contain info about 
running the system and any other documentation that doesn't belong in one of the lifecycle docs 
described below.

## Docs

I've created a series of docs to better plan and implement what I want to accomplish in this project. 
Here is a list and links to the documents and a short description.

1. [SRS](docs/srs.md) - Software requirements for the system. The what needs to be built and definition of done.
2. [SAD](docs/sad.md) - Software architecture for the system. How the system gets built.

## Running Locally

I'm assuming you already have [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=v3%2Cmacos%2Ccsharp%2Cportal%2Cbash%2Ckeda) installed. 
Additionally, you will need docker and docker compose installed as well. This project makes use of 
azurite and the Azure Cosmos Emulator.

I've also left the necessary local.settings.json in the repository since these keys are static for 
all local development.

To run locally, follow these steps:

1. Run the `scripts/start-test-env.sh` script. Currently this only works for Mac. All it does is wrap 
   running `docker compose` for the `docker-compose.test.yml` file and managing a certificate that is 
   needed. You will have to input your admin password.
2. Navigate to the [explorer](https://localhost:8081/_explorer/index.html) and create a database named 
   `issuetracker-db`. Then create the 3 following collections - `commands-col`, `events-col`, and 
   `issues-col`.
3. Once you have your database set up, run `dotnet build` in the root directory.
4. Navingate to `src/IssueTracker.Api/bin/Debug/netcoreapp3.1` and run `func host start` to start the 
   API.

After you've run, be sure to clean your local dev environment by running `scripts/destroy-test-env.sh`.

# Software Architecture

To support the issue tracker, an API must be created to provide a way to manipulate and store 
data to any potential clients. This document will define that API, the storage requirements, and 
any other (non-) functional details needed.

## Issue Structure

An issue will be defined with the following JSON structure.

```json
{
  "id": 0,
  "title": "",
  "description": "",
  "state": "",
  "priority": 0,
  "created": "timestamp",
  "updated": "timestamp",
  "deleted": "timestamp",
  "history": []
}
```

## API Endpoints

The endpoints will follow a basic REST/CRUD structure for creating and manipulating issues.

1. Get All Issues - `GET /api/issues`
2. Get Issue - `GET /api/issues/{id}`
3. Create Issue - `POST /api/issues`
4. Update Issue - `PUT /api/issues/{id}`
5. Delete Issue - `DELETE /api/issues/{id}`

## Validation

* Getting, updating, or deleting a specific issue should return a 404 if the ID is invalid.
* When creating an issue, both title and description are required fields. Priority is optional. State 
is set automatically.

## Database Design

While this design easily sets up for a relational database, this project will use Azure's Cosmos 
database. This NoSQL store is going to be used for 2 separate reasons. First, I do not know what the 
final shape of each issue will be. Therefore it will be easier to design a schemaless document and add 
to it where appropriate. Second, I will be taking advantage of the Cosmos change feed to implement an 
event sourcing architecture that will be defined below.

Because of the nature of the NoSQL DB. The only changes from the issue schema defined in the `Issue 
Structure` section above and the database schema below is the `id` field. In Cosmos, I will be using a 
`GUID` for the `id` field and adding an extra `issueId` field for the autoincrementing, easy to read ID.

## API Design

![API Design](imgs/api-design.png)

The above design shows an event sourcing architecture with CQRS for this issue tracker. The uses of 
event sourcing and CQRS provide 2 main needs for the system.

1. Event Sourcing will provide a history of all changes to an issue and can provide that via API.
2. CQRS will allow for multiple users to make changes to an issue without conflict since changes happen 
via command processing instead of direct database manipulation.

This also shows the how the decision for Cosmos was made. The use of the change feed provides a way to 
manage asynchronous messaging without having to create more infrastructure in a messaging bus nor 
implement transactional outboxes, etc.

Azure Functions are being chosen for 3 reasons.

1. I work with Azure Functions daily so it is my go to choice for development currently.
2. If I do ever want to deploy this to Azure, I can use a consumption plan and keep the price down very 
low.
3. Azure Functions provides bindings for Cosmos to make this be possible in the least amount of code.

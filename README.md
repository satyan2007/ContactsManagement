# Contact Management

## Overview
### Domain
This will contain all entities, exceptions, interfaces, types and logic specific to the domain layer.

### Application
This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure
This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### API
This layer is a Web API based on ASP.NET Core 5. This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only *Startup.cs* should reference Infrastructure.


## Technologies
urls ->https://localhost:7084/
.Net 8 

Angular 18

url
https://localhost:4200/

### Database Configuration

No Database requires it is using json file as asked.

--Resore dependency for both client and server

--Go to "contactsmanagement.client" 
1. Install Node.js and npm
2. Install Angular CLI 
npm install -g @angular/cli@18
Install dependencies:
npm install

# Tandem Code Challenge
 
 # Table Of Contents
 
 - 1: [Getting Started](#Getting-Started)
   - [Server-Side Resources](#Server-Side-Resources)
   - [Client-Side Resources](#Client-Side-Resources)
   - [Piecing it all together](#Piecing-it-all-together)
   - [What's happening under the hood](#whats-happening-under-the-hood)
 - 2: [Client-Side Infrastructure](#Client-Side-Infrastructure)
   - [Components](#Components)
   - [Interceptors](#Interceptors)
   - [APIs](#APIs)
 - 3: [Server-Side Infrastructure](#server-side-infrastructure)
   - [DI/IoC Setup](#diioc-setup)
   - [Request Pipeline hooks](#Request-Pipeline-hooks)
   - [Model Types/Relationships](#model-typesrelationships)
   - [Controllers](#Controllers)
   - [Adapters](#Adapters)
   - [Facades](#Facades)
   - [Repositories](#Repositories)
   - [In-House NuGets](#In-House-NuGets)
     - [Data Proxy](#Data-Proxy)
     - [Status Responses](#Status-Responses)
 - 4: [App Features](#App-Features)
   - [Account Management](#Account-Management)
   - [Token Management](#Token-Management)
   - [Game History](#Game-History)
   - [Relational Data Structures](#Relational-Data-Structures)
   - [Data Sources](#Data-Sources)
  
 # Getting Started
 This is a .NetCore server hosted Angular single-page application, or "SPA" for short. To run this application you'll need a couple of things.
 
 Download short-list
- [.NetCore SDK v3.1.403 and Runtime v3.1.9](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- [Visual Studio 2019 Community](https://visualstudio.microsoft.com/downloads/)
  - During install select the following workload modules
    - ASP.NET and web development
    - Universal Windows Platform Development
    - .NET Core cross-platform development
- [Node.js](https://nodejs.org/en)

Jump down to [Piecing it all together](#Piecing-it-all-together) from here

 ### Server-Side Resources
 First, the server that serves you the website is run via the .NetCore framework. So you'll need the SDK and the Runtime for that framework.
 
 Both can be downloaded from [here](https://dotnet.microsoft.com/download/dotnet-core/3.1)
 
 Pick the download that is appropriate for your operating system, make sure to download both the SDK and the Runtime.
 The latest version of each is recommended, that'd be SDK v3.1.403 and Runtime v3.1.9
 
 This will enable you to run the server-side code on your machine. To actually run the server-side code, please do so out of Visual Studio 2019. The community edition is free to download, you can find it [here](https://visualstudio.microsoft.com/downloads/)
 
 When installing Visual Studio 2019 you will be prompted to pick and choose what workload modules you would like to install based on what development you plan to do via Visual Studio 2019.
 
 Please select the following: 
 - 1: ASP.NET and web development
 - 2: Universal Windows Platform Development
 - 3: .NET Core cross-platform development
 
 There is one key reason I ask you to run this codebase out of VS2019. It implements a server-side code hosting application called, IIS Express, inherently, on your behalf.
 
 IIS, or, Internet Information Services, is a application for hosting compiled code and making it accessible on your local network. IIS Express performs this functionality on-the-fly for you when you run the application.
 
 Additionally, the application depends on server-side routing rules to enforce a specific application flow that prohibits navigating to pages without authorization explicitly from within the application itself.
 
 ### Client-Side Resources
 To run the client-side, please install, Node.js v14.15.0 or greater, you can find it [here](https://nodejs.org/en). The client-side code depends on Node.js for gathering the dependencies that it uses to run in the browser when served from the server.
 
 ### Piecing it all together
 Now that you have downloaded everything (thank you for your patience) there's one more download you need to do to get everything running. You need to download the resources the client-side code needs to run.
 
 To do this navigate to wherever you pulled this repository down to, and open a command prompt against the following folder.
 
 Source\WebService\ClientApp
 
 From there, assuming you've installed Node.js, run the command, "npm install" without the quotation marks. This will download the client-side resources required for the client-side code.
 
 After that, in Visual Studio 2019, expand the folder named, "ServiceImpl", right-click the list-item titled, "Tandem.WebService". From the right-click menu, select the option, "Set as Startup Project"
 
 Now, click the green play button at the top of the window titled, "IIS Express" to run the application.
 
  ### What's happening under the hood

 What this is doing behind the scenes, is taking all the client side resources bundling them up and storing them in your computer's random access memory at a local hosted address, specifically, http://localhost:4200. Then, it is taking all the server-side code, and bundling it up and hosting it on a local address, specifically http://localhost:49736 from your harddrive.
 
 http://localhost:49736 is the address that you will want to access. Visual Studo 2019 will open a browser tab, or new window and navigate to this address for you when you hit the green play button.
 
 The browser at this point is accessing the hosted server-side code, which is returning the browser the hosted client-side code, so the browser can display you the website.

# Client-Side Infrastructure

### Components

In the client-side code, components make up the individual pages of the application.

These files implement the Typescript code that drives the functionality of a particular page/view.

### Interceptors

Interceptors are http request lifecyle hooks. When the application makes a http request to the server that hosts the application, http interceptors can intercept that outgoing request and perform work on it before the request goes out.

Additionally they are the first pieces of code in the client-side application to receive a response and can perform additional logic against that incoming response before the response is returned to the piece of code that actually initiated the request.

The client side implements two http interceptors.

- StatusResponseInterceptor
- TandemTokenInterceptor

#### Status Response Interceptor Explained

The status response interceptors captures the request about to be made by the browser and forwards it to the server in a contained space that it can monitor.

When the response for that request comes back the StatusResponse Interceptor checks to see if that response has a header titled, "Status" and if it does it takes the value of that header, converts it from a string of text into a object, and stores that object in a session store that other pieces of code can access.

This enables a piece of code to make a request, receive a response, then check the session store to see what status details came back for the request.

So say the login endpoint is called from the browser. Typically a request made to this endpoint results in a string being returned that contains a new user token for the browser to use with subsequent requests.

If the code that made the request identified that no token was returned, it can then check the StatusResponse obj's values to identify why the request was unsuccessful and parrot that information to the user, or act on it in another way if necessary.

This centralized functionality occurrs for every outgoing request in one spot so no other code has to worry about reading these headers, the interceptor has got them covered.

#### Tandem Token Interceptor Explained

This interceptor captures all outgoing requests from the application, and applies the User Token stored in the browser's cookies to the headers of the outgoing request.

The benefit is that this occurrs in a centralized location so no matter where in the application code that a http request is made, that piece of code doesn't have to worry about applying the User Token to the request, the interceptor has it covered.

### APIs

In the client-side application there are two APIs. The PlayerAPI, and the TriviaAPI. The PlayerAPI is the centralized location from which all available http requests related to player data that can be made to server are dispatched. Likewise, the TriviaAPI is the centralized location from which all available http requests related to trivia data that can be made to server are dispatched.

APIs are utilized by Components to fetch data to display in the application pages as well as send data from the application views to the server to be stored in the data sources of the server.

# Server-Side Infrastructure
    
### DI/IoC Setup

For a .NET Core application, all the dependency injection to be used by the server-side code is hosted out of the Startup.cs file.

During a given request if a class' constructor requires a resource, the Dependency Injection system will identify the resource(s) required, identify if a instance of that class has been registered with the dependency container via Startup.cs, and if so will return that instance to the invoking class.

This makes Startup.cs a good place to see at a glance what the primarily used classes are throughout the application as, more-often-than-not they will be registered with the DI container in Startup.cs

### Request Pipeline hooks

The server-side code is reached via http requests from the browser. When a http request is made to the server, the request can be funneled through a series of lifecycle hooks before reaching the code related to the endpoint the request was made to.

This application uses the two following in-house lifecycle hooks

- TokenValidationMiddleware
- StatusResponseMiddleware

#### Token Middleware Explained

This application uses token authentication for the publicly exposed endpoints that are related to account-holder functionality, such as fetching a non-guest trivia round, or updating a account username/password.

If you look in PlayerController.cs or TriviaContoller.cs, and public method that is not affixed with the [NoToken] attribute will cause the TokenValidationMiddleware to be run against the incoming HttpRequest **before** the request reaches the Controller code associated with the endpoint the request was made to.

The TokenValidationMiddleware checks three things

- That a token was sent in the headers of the incoming request
- That the signature portion of the token can be recreated via the body portion of the token using the secret token generating functionality that cannot be replicated
- That the token sent has not expired

If these three conditions are validated successfully then the http request can continue on to execute the code associated with the request.

Tokens are issued to the client-side and stored as cookie during the three following events.

- When a account is created
- When a user logs into their account
- When a user updates their username/password

#### Status Response Middleware Explained

This application returns Status Codes and Descriptions in the response of every http request made to it. This is accomplished using a consistent model that contains a StatusCode and StatusDesc property to represent the ultimate result of the call.

As well as a StatusDetails property where additional more minute details about the ultimate status of the call can be relayed.

For example, if a user was chaning their password, and they set their new password to be the same value as their current password the following StatusResponse would be returned.

```json
status: {
 statusCode: 420,
 statusDesc: "Business Rule Error",
 statusDetails: [
  statusDetail: {
   code: 401,
   desc: "Your new password cannot be the same as your old password"
  }
 ]
}
```
In this example the 420 code and corresponding message indicate the overall result of the call. The request has failed to a business rule violation. In the statusDetails it goes on to explain explicitly what error occurred that caused this outcome.

The StatusResponseMiddleware works by capturing the incoming request and then continuing its execution from within a space that the middleware controls and can monitor. This way it knows when the request has finished and the response is starting.

Once the response process has started it will take the StatusResponse object that all files have access to, and it will apply whatever its current value is to the headers of the outgoing response.

This enables any piece of code in the request pipeline to update the StatusResponse obj without having to worry about, or be responsible for ensuring the values of the StatusResponse object get added to the headers of the outgoing response. The middleware handles this so nothing else has to.

### Model Types/Relationships

In the server-side code there are three types of models.

- Business Models
- Business Entities
- Domain Entities

#### Business Models
These are the models that are publicly exposed by the server-side API. These are the models that the API returns to those that use it, as well as the models the API may except for any one of its publicly exposed endpoints that a request can be made to.

It is common that a Business Entity will often be a subset of data from a Business Entity. Certain information may be excluded from the data used during business logic processing when returning data to a user. For example the Player Business Entity contains information such as PasswordHash, PasswordSalt, and PasswordPepper which are used when processing credential information.

This information would never be transposed onto a BusinessModel and returned to a user in a API response. This information is internal information used for internal processes that has no business being returned to users of the API.

#### Business Entities
These are the models used internally by the server-side code. These models could be a compilation of multiple different values from multiple different models stored under one object for convenience of processing. They are not strictly what will be returned to the user that has made a request, and they are not strictly the exact structure of the data that may be stored into any data source.

They are models used solely during business logic validation, processing, calculations, etc.

#### Domain Entities
These models are the 1:1 representation of your data structures as they will be stored in your data sources.

They are strict, they are not often changed. It is common that a Business Entity will often be a subset of data from a Domain Entity, certain information may be excluded from the Db when transformed into a Business Entity to perform processing.

### Controllers

Controller files are the entry point to the server for incoming Http requests made from the browser via the client-side code.

When a piece of client-side code makes a web request to Player/ChangeUsername/1337?newUsername=superAwesomeUsername what it's doing is making a request to the PlayerController.cs file, specifically it's triggering the ChangeUsername(string newUsername) method in that file, and passing the value, "superAwesomeUsername" to the, newUsername parameter of that method.

Controllers work exclusively with Business Models. They receive business model objects as payloads and they exclusively return Business Model objects as response payloads.

Controller methods do not perform any logic besides transforming a Business Entity object into a Business Model object for returning to the user that has made a request.

### Adapters

Adapter files are the centralized place where business logic is located.

Adapters work exclusively with Business Entities. Adapter methods receive Business Entity objects and return Business Entity objects.

Any method calls to external services such as Facades will result in a Business Entity being returned to the Adapter method.

### Facades

Facade files are the sole files responsible for contacting the database.

They are the only files who's methods are allowed to work with Domain Entities.

They never return a Domain Entity to a calling method. They receive Business Entities, and return Business Entities.

Facades do not perform any more logic than is necessary to fetch data that they are setup to fetch and return, or update data that they are setup to update.

If an Adapter needs data from the database to perform business logic against, the Adapter would request that a Facade file communicate with the Database to retrieve that Domain Entity then convert that Domain Entity to a Business Entity that the Adapter file can work with.

### Repositories

Repository files are a part of the Data Access Layer (DAL) and the sole files that actually retrieve raw data from the database or write data to the database.

Repositories work exclusive in Domain Entities. They receive Domain Entities from Facades and return Domain Entities to Facades.

Outside of the DAL Facades are the files responsible for grabbing data, they accomplish the task by having Repositories retrieve raw Db data and convert it to Domain Entity objects that the Facade can work with on behalf of the Adapter.

### Data Structure

So I liked the Tandem data, but I thought it was a little flat. I hope no one minds, but I decided to regularize it to more-so match the kind of data structure I would expect to see in a relational database such a SQL.

Because I thought it would be too much to ask for you guys to install SQL Server and run a script to generate tables, and sprocs, and such, I opted to use text files as data sources. These files are included in the solution, you can see their data under \Source\Data\Tandem.Data\DataFiles

In Visual Studio 2019 if you press Ctrl+K followed by Ctrl+D it will format the json data into a indented structure that is more parseable.

**If you do this, please please please use Ctrl+Z when you are done** reading the data to return it to its minified single-line state. Otherwise you will throw off the hard-delete prevention logic in the DataProxy **and will break the ability to insert data into the files.**

There are 6 data sources for the application

- Question.json
- Answer.json
- Player.json
- PlayerQuestion.json
- PlayerAnswer.json
- PlayerHistory.json

Question and Answer are just the questions and answers provided from the initial data sheet split up.

Every Question Entity has a QuestionID, and every Answer Entity has a QuestionID field that links to the Question that Answer is a part of.

Player is just some minimal data such as player name, and PlayerID, as well as come credential management data like their one-way HMAC SHA256 hashed password, the salt used after the intiial hash, and the pepper used after that hash.

PlayerHistory contains information about a specific round of Trivia that was played, such as when the round was started and finished.

PlayerQuestion is an entity that contains a PlayerHistoryID in it so that is can track back to a PlayerHistory entity. At the start of a round 10 Question Entities are selected at random then associated with a Player History Entity via being transformed into a PlayerQuestion.

As a Player plays a round of trivia every answer they pick for a question gets saved as a PlayerAnswer. This is a simple lookup object containing a AnswerID that maps to the Answer table where the Answer text and isCorrect value can be found, a QuestionID that maps to the question the Answer is a part of, and a PlayerHistoryID that designated what particular round of Trivia this answer was made for.

### In-House NuGets

The server-side code implements two in-house code libraries that under normal circumstances would be developed outside of the application, be compiled as NuGet packages, and installed into the server-side application.

These two libraries would be the StatusResponse and DataProxy project.

#### Data Proxy Explained

The Data Proxy project is the abstraction layer that sits on top of and accesses the database.

A Repository is responsible for receiving raw data and parsing it to a Domain Entity that can be worked with, the Data Proxy is the system that retrieves that raw data.

The purpose of this abstraction layer is to have a centralized library of code that accesses whatever flavor of Db you're using. This way if you migrate from say SQL to Mongo, you modify your DataProxy to not longer user Dapper, and begin using MongoDriver.

Upstream your Repos are still calling InsertEntity, GetEntity, UpdateEntity, and all they know is that when they call InsertEntity and pass an object it's going to a Db. They don't know what Db, they don't know how the data is getting into that database, and it's not their responsibility to know how, they just know it's happening.

This means that you can migrate data sources by changing code in one location and then have multiple other locations just upgrade to the latest data proxy and they will beging to write data to the new data source without having to change their code as well. They just keep on calling InsertData like nothing has changed.

#### Status Response Project Explained

The Status Response Project is a centralized project that contains a myriad of Status Codes, and corresponding Status Messages.

This is where you would establish your business wide codes such that any application would just simply install this package and then begin using these pre-established error codes, response codes, business rule codes, etc.

The package is designed with develope convenience in mind as it provides Scoped model management that enables any piece of code to update the StatusResponse object, as well as Request Pipeline Middleware to ensure that the state of the Status Response obj is always places on the outgoing request header without having to explicitly do so.

This enables a developer to quickly and easily code for explicit failure scenarios by simply setting their Status Response obj instance to the necessary values then exiting the current executing code. And the set Status will be applied to the outgoing response without any extra work.

# App Features
  
### Account Management

This application implements account management. You can create and sign into an account that enables your trivia history to be tracked and enables the ability to review the games of trivia you've played.

### Token Management

This application uses Json Web Tokens to validate requests made to the server on behalf of a user. They are generated via HMAC SHA256 encryption and stored browser-side in the cookies of the application.

They are passed on every outgoing web request to the server, and validated by the server for all incoming requests except for requests made to the Login, CreateAccount, and GetGuestTriviaRound endpoints.

### Game History

Mentioned briefly above, every game of trivia you play via a created account is logged in the db (text files)

You can access the Game History page to, as the app puts it, "relive your greatest(and dimmest) moments"

### Relational Data Structures

I decided while it would be straight forward to implement the data given, it would be a nice extra step to regularize it into a fashion that resembled more close the kinda of relational data structures that I would typically utilize in a data source such a SQL

### Data Sources

As mentioned above, the application stores data in text files meant to mimic relational database tables so that trivia games can be retained and revisted over time for account holders. These are .json files with raw json content stored. So more akin to Mongo than SQL, but I figured it was better than asking that you install SQL Server to run my trivia app, haha.

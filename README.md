# Tandem Code Challenge
 
 # Table Of Contents
 
 - 1: [Getting Started](#Getting-Started)
   - [Server-Side Resources](#Server-Side-Resources)
   - [Client-Side Resources](#Client-Side-Resources)
   - [Piecing it all together](#Piecing-it-all-together)
   - [What's happening under the hood](#What's-happening-under-the-hood)
 - 2: Client-Side Infrastructure
   - Components
   - Interceptors
   - APIs
 - 3: [Infrastructure](#Infrastructure)
   - DI/IoC Setup
   - Request Pipeline hooks
   - Controllers
   - Adapters
   - Facades
   - Repositories
   - In-House NuGets
     - Data Proxy
     - Status Responses
 - 4: App Features
   - Account Management
   - Token Management
   - Game History Management
   - Relational Data Structures
   - Data Sources
  
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

Jump down to [Piecing it all together](###-Piecing-it-all-together) from here

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





# App Features

### DI/IoC Setup

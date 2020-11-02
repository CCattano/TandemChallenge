﻿# Tandem Code Challenge
 
 # Table Of Contents
 
 - 1: [Getting Started](#Getting-Started)
    - [Server-Side Resources](###-Server--Side-Resources)
    - [Client-Side Resources](###-Client--Side-Resources)
    - [Piecing it all together](###-Piecing-it-all-together)
    - [What's happening under the hood](###-What's-happening-under-the-hood)
  - 2: [Infrastructure](#Getting-Started)
    - DI/IoC Setup
    - Request Pipeline hooks
    - Client-Side Infrastructure
      - Components
      - Interceptors
      - APIs
      - Viewmodels
    - Controllers
    - Adapters
    - Facades
    - Repositories
    - In-House NuGets
      - Data Proxy
      - Status Responses
  - 3 App Features
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
 
# Infrastructure

### TODO: Write this bit

# App Features

### TODO: Write this bit

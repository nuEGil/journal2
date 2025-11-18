# journal2
Why make a notepad application? They're everywhere. Microsoft word, Google Docs, Samsung + Apple Notes, Obsidian, Visual Studio, emacs, vim... the list goes on. 

I chose to do this in MAUI because .NET MAUI enables cross platform deployment of your application with one C# code base. Microsoft also has Blazor for web development, so I'm sure some of the C# code would cary over to future projects. 

# Repo map
1. Platforms : platform specific objects
2. Properties : 
3. Resources : assets that are used in the app. this includes the fonts, the app icons, style sheets, etc. Has some raw text files. In this application I have some chapters of the Jungle from WikiSource since it's under a creative commons liscence. this is just to preload the app with some text so that we have something to debug with and dont have to hand write a bunch of notes.

4. ViewModels : c# classes that give the UI something to bind to. in the .xaml files like under MainPage.xaml you will see {Binding Files}, that reads from MainPage.xaml.cs BindingContext = ... ViewmModels pass data from the services to that binding context so you can do dynamic pages. 

5. Services : These are classes and funcitonality that you want to run in the background of the app but like use within the pages. So here I have a service for SQLite database initialization, one to read and write from the database, then a service to keep track of files that the user generates since the datbase just stores lines of interest, a key word service so that we can check the text for key words and use those to search the note list as it grows,  then a file seeder service to preload the app with some texts. 

# Code anatomy

.xaml files just tell the app how to populate the page with things like text, buttons, and writing areas. xaml.cs files give functionality. 

App.xaml(.cs) is the part of the app that runs on start up. so here is where I have tasks to iniitalize the database and seed the app with text. 

AppShell.xaml(.cs) has code for navigation to different pages within the app. AppShell creates a new instance of a page the first time you navigate to it, then reuses the old instance every time you navigate back to it. 

MauiProgram.cs --> those lines that say builder.Services.AddSingleton<interface, implementation>(); those are giving you a single instance of that particular service class from journal2.Services. If you use transient you get a new instance of the service every time it is requested. So this part initializes, then the pages use service injection to use the services for content. See MainPage.xaml.cs for the file explorer service, WritingArea.xaml.cs and the SearhPage.xaml.cs use the keyword and database services. 

# language notes
Interfaces tell the code that you need to include specific methods and data stuctures for this class to be considered a specific type. it will not compile if you have not implemented everything you said you would in the interface. its a contract basically. 

you have class, struct, and record for your user defined types.

1. Classes are reference types - they store references to objects in the heap (unkown memory need, so you allocate - look for available addresses) -- only the reference is copied not the object itself.... the other thing is this one is nullable, it fully supports inheritance, it is mutable by default, and they are equal only if they reference the same object in memory

2. structs are value types --they directly store the data and are typically put on the stack -- you have known memory needs here. structs use value equality by default -- readonly enforces immutability -- structs do not support inherritance. --> use this for small simple data like points, datetime, color. These are non nullable

3. record struct -- value type, like structs, but include valu equality, ToString override (so you can print them) with expressions for non destructive mutation. -- no inheritance. 

# some known bugs
1. page state needs to be manages better. if you open an old note from the home page you can go back to the home page. but if you start a new writing area you have to use the flyout menu to navigate. 

2. Do a check on the data base. make sure that when the user opens an old file and deletes a line of text that contains the keywords we are looking out for that that entry in the database also gets removed. 

3. add back in the button on the home page to generate new text files -- the + button. --> In an old version this button pressed would push a new instance of a page to the page stack - hence the pop command that is left behind in the back button on the writing area .xaml.cs. 

4. need to add in encryption.
5. need to add in some http request functionality to serve algos and data from a server. real apps have some amount of local and some amount of server functionality, and they just swap between the 2 rapidly. 



# some dotnet commands (add more here)
install visual studio to get all the dotnet goodies, and nuget, and then never open it again. vim, emacs, vscode.

    dotnet workload list
    dotnet workload install maui 
    dotnet workload install android
    dotnet build -t:Run -f:net9.0-android
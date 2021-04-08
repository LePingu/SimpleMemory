# SimpleMemory
A Simple cache experiment in dotnet core

# Project Description

This project aims to try using basic dotnet knowledge to create a thread-safe memory cache with any possible method from an object needing to be recorded. 
It also aims to record past memory access **while the project is running**. 

Areas of improvement are various: 

- Access to the Sngleton can be developped though an extension instead of simple Instance call
- Explore the idea of hashing for keys with less memory than simple strings
- Avoid usage of LINQ for sorting with a better time record obect tracker
- Finish threading safety testing
- And more

This project uses [NodaTime](https://nodatime.org/) over DateTime usual object as NodaTime keeps better track of instants and zoned time travel.

Another area of exploration should be [Rx.net](https://github.com/dotnet/reactive) usage but this should be an entire new subject. 

# Jump-start the project

It can be started in two ways : 

- Through docker
- Through dotnet 5.0 if installed locally. 

No self-contained package is pushed.

Scripts have been placed in the project to help start from command line easily. 

# CQRS API

ASP.NET Core MVC lean controllers for commands and queries.

This is very much work in progress.

## Main concepts

Often, when using CQRS-style API (that is, exposing endpoints for commands and queries), the MVC controllers are just taking the command/query from the request and passing them into the dispatcher.

The idea behind the project is to autogenerate MVC controllers based on available command and query classes. These controllers' responsibility is to pass the command/query object from the request to the appropriate dispatcher and return results to the caller (in case of queries).

The CQRS infrastructure uses [CQRS Essentials](https://github.com/michaldudak/cqrs-essentials) with its Autofac integration.

Generic controller implementation taken from https://github.com/aspnet/Entropy/tree/dev/samples/Mvc.GenericControllers

# SignalR Chat Room

## Project Description

This project is a simple chat application built using ASP.NET Core SignalR and Blazor. Users can communicate with other users, join specific rooms and engage in group chats.

## Usage

When the application starts, users can join chat rooms by selecting a username on the main page. They can then join different rooms and chat with other users.

## Screen Recordings

https://github.com/BetulTugce/SignalRChatRoom/assets/79140478/884fcdfc-5ccf-4e08-ad04-603488999dcf

## Technological Infrastructure

The server application is built with ASP.NET Core using .NET 8, while the client application is developed using Blazor Web App also with .NET 8. This application was developed for the purpose of learning SignalR. SignalR is an open-source library that enables real-time functionality in web applications using WebSocket technology. Data is stored in memory, but you have the option to enhance the project by adding support for a database and incorporating various functionalities. If you wish to move communication with clients outside of the hub, you can utilize the [IHubContext](https://learn.microsoft.com/en-us/aspnet/core/signalr/hubcontext?view=aspnetcore-8.0) interface. Essentially, this interface allows us to use the SignalR architecture outside of 'Hub' classes, thereby enabling interaction between clients and servers from different places such as controllers, classes, etc.

## Installation

1. **Clone the project:**

    ```bash
    git clone https://github.com/BetulTugce/SignalRChatRoom.git
    ```

2. **Navigate to the project directory:**

    ```bash
    cd SignalRChatRoom
    ```

3. **Run the project:**

   Right-click on the solution and select `Configure Startup Projects`. In the opened window, select `Multiple startup projects`, mark SignalRChatRoom.Client and SignalRChatRoom.Server projects as `Start`, then click `OK` and press `F5` to run the project.

## Dependencies

- [Blazor.Bootstrap](https://docs.blazorbootstrap.com/) v2.0.0
- [Microsoft.AspNetCore.SignalR.Client](https://dotnet.microsoft.com/en-us/apps/aspnet) v8.0.2

## Support and Communication

If you encounter any issues or have feedback while using the project, feel free to [create an issue on GitHub](https://github.com/BetulTugce/SignalRChatRoom/issues). Moreover, if you wish to support or contribute to the project, you can do so by starring the [GitHub repository](https://github.com/BetulTugce/SignalRChatRoom) or making contributions to the codebase.
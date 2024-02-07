using System.Reflection;
using Fleck;
using lib;

var builder = WebApplication.CreateBuilder(args);

// STEP 1
var clientEventHandlers = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());

var app = builder.Build();
var server = new WebSocketServer("ws://0.0.0.0:8181");

var wsConnections = new List<IWebSocketConnection>();

server.Start(ws =>
{
    ws.OnOpen = () =>
    {
        wsConnections.Add(ws);
    };
    ws.OnMessage = message =>
    {
        try
        { //STEP 2:  INVOKE THE EVENT HANDLER
            app.InvokeClientEventHandler(clientEventHandlers, ws, message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.InnerException);
            
            
        }
    };
    
});
Console.ReadLine();


using System.Reflection;
using Fleck;
using lib;
using websocket;

var builder = WebApplication.CreateBuilder(args);

// STEP 1
var clientEventHandlers = builder.FindAndInjectClientEventHandlers(Assembly.GetExecutingAssembly());

var app = builder.Build();

var server = new WebSocketServer("ws://0.0.0.0:8181");
//list of connection
//kiping state of connection
var wsConnections = new List<IWebSocketConnection>();

// sent a message to the server
server.Start(ws =>
{
    ws.OnOpen = () =>
    {
        StateService.AddConnection(ws);
    };
    ws.OnMessage = async message =>
    {
        try
        { //STEP 2:  INVOKE THE EVENT HANDLER
           await app.InvokeClientEventHandler(clientEventHandlers, ws, message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.InnerException);
            Console.WriteLine(e.StackTrace);
            
            
        }
    };
    
});
Console.ReadLine();


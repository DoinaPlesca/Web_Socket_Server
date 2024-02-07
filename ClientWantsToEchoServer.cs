using System.Text.Json;
using Fleck;
using lib;

namespace websocket;
//model for deserialization
public class ClientWantsToEchoServerDto : BaseDto
{
    //transfer object
    public string messageContent { get; set; }
    
}
//STEP 3: ADD EVENTS BY EXTENDING BaseEventHandler<T> WHERE T IS YOUR DEFINED DTO
public class ClientWantsToEchoServer : BaseEventHandler<ClientWantsToEchoServerDto>
{
    //STEP 4: IMPLEMENT THE Handle(dto, socket) METHOD DEFINED BY BaseEventHandler
    public override Task Handle(ClientWantsToEchoServerDto dto, IWebSocketConnection socket)
    {
        var echo = new ServerEchosClient()
        {
            echoValue = "echo: " + dto.messageContent
        };
        var messageToClient = JsonSerializer.Serialize(echo);
        socket.Send(messageToClient);
        return Task.CompletedTask;
    }
}

public class ServerEchosClient : BaseDto
{
    public string echoValue { get; set; }
}
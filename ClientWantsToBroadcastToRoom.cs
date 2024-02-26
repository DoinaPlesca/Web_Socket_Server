using System.Text.Json;
using Fleck;
using lib;

namespace websocket;

public class ClientWantsToBroadcastToRoomDto : BaseDto
{
    public string message { get; set; }
    public int roomId { get; set; }
}

public class ClientWantsToBroadcastToRoom : BaseEventHandler<ClientWantsToBroadcastToRoomDto>
{
    public override Task Handle(ClientWantsToBroadcastToRoomDto dto, IWebSocketConnection socket)
    {
        var message = new ServerBroadcastMessageWithUsername()
        {
            message = dto.message,
            username = StateService.Connections[socket.ConnectionInfo.Id].Username
        };
        StateService.BroadcastToRoom(dto.roomId, JsonSerializer.Serialize(message));
        return Task.CompletedTask;
    }
}

public class ServerBroadcastMessageWithUsername : BaseDto
{
    public string message { get; set; }
    public string username { get; set; }
}

using Fleck;
using lib;

namespace websocket;

public class ClientWantsToSingInDto : BaseDto
{
    public string Username { get; set; }
}

public class ClientWantsToSingIn : BaseEventHandler<ClientWantsToSingInDto>
{
    public override Task Handle(ClientWantsToSingInDto dto, IWebSocketConnection socket)
    {
        StateService.Connections[socket.ConnectionInfo.Id].Username = dto.Username;
        return Task.CompletedTask;
    }
}
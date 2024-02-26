using Fleck;

namespace websocket;
//hold webSocketConnection and do what is asociate with conn
public class WebSocketMetaData(IWebSocketConnection connection)
{
    public IWebSocketConnection Connection { get; set; } = connection;
    public string Username { get; set; } //
}

public static class StateService
{
    //single source connection
    public static Dictionary<Guid, WebSocketMetaData> Connections = new();

    //Guid...refer just to 1 connection,inside of the tum 'int'
    // int = number of room
    public static Dictionary<int, HashSet<Guid>> Rooms = new();

    public static bool AddConnection(IWebSocketConnection ws)
    {
        return Connections.TryAdd(ws.ConnectionInfo.Id,
            new WebSocketMetaData(ws));
    }

    //'room' is the key of the Room
    public static bool AddToRoom(IWebSocketConnection ws, int room)
    {
        if (!Rooms.ContainsKey(room))
            Rooms.Add(room, new HashSet<Guid>());

        return Rooms[room].Add(ws.ConnectionInfo.Id);
    }

    //we go over each GuidID in the room and broadcast to the webSocket
    public static void BroadcastToRoom(int room, string message)
    {
        if (Rooms.TryGetValue(room, out var guids))
        {
            foreach (var guid in guids)
            {
                if (Connections.TryGetValue(guid, out var webSocketConnection))
                    webSocketConnection.Connection.Send(message);
            }
        }
    }
}
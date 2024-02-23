using SignalRChatRoom.Server.Models;

namespace SignalRChatRoom.Server.InMemoryData
{
    public class ClientSource
    {
        public static List<Client> Clients { get; } = new List<Client>(); // Property ReadOnly olarak tanımlandı.
    }
}

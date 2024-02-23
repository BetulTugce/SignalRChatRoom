using Microsoft.AspNetCore.SignalR;
using SignalRChatRoom.Server.InMemoryData;
using SignalRChatRoom.Server.Models;

namespace SignalRChatRoom.Server.Hubs
{
    // Bir sınıfın hub sınıfı olduğunu anlamak ve sorumlulukları yüklemek için hub sınıfından türetilmesi gerekir..
    public class ChatHub : Hub
    {
        public async Task GetUsernameAsync(string username)
        {
            Client client = new Client { ConnectionId = Context.ConnectionId, Username = username };

            // Callerı (Sisteme dahil olan kullanıcıyı) mevcuttaki tüm clientların tutulduğu listeye ekler.
            ClientSource.Clients.Add(client);

            // Sisteme bir clientın dahil olduğunu caller (dahil olan client) dışındaki tüm clientlara bildiriyor..
            await Clients.Others.SendAsync("clientJoined", username);

            // Yeni kullanıcının da eklendiği güncel listeyi tüm clientlara bildirir..
            await GetClientsAsync();
        }

        public async Task GetClientsAsync()
        {
            // Sisteme dahil olan clientı kendisi de dahil olmak üzere tüm clientlara bildirir..
            await Clients.All.SendAsync("clients", ClientSource.Clients);
        }
    }
}

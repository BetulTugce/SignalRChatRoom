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

            // Sisteme eklenmiş oda/grup listesi sisteme giriş yapan kullanıcıya (caller) bildirilir..
            await Clients.Caller.SendAsync("groups", GroupSource.Groups);
        }

        public async Task GetClientsAsync()
        {
            // Sisteme dahil olan clientı kendisi de dahil olmak üzere tüm clientlara bildirir..
            await Clients.All.SendAsync("clients", ClientSource.Clients);
        }

        // Caller bir clienta mesaj gönderecekse tetiklenir..
        public async Task SendMessageAsync(string message, Client client)
        {
            // Caller bilgisi alınıyor..
            Client senderClient = ClientSource.Clients.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);

            // Clientta tanımlı receiveMessage fonksiyonunu tetikler.Mesaj, senderClient ve receiverClient değerleri döndürülür..receiveMessage fonk..4 değer bekliyor..
            // 3.dönüş değeri bir Client beklemekte, grup mesajlaşmalarında bir clienta gönderilmediği için bu değer null olarak döndürülüyor..
            // 4.dönüş değeri ise string beklemekte, bu da karşılıklı mesajlaşma durumunda null döndürülüyor..
            await Clients.Client(client.ConnectionId).SendAsync("receiveMessage", message, senderClient, client, null);
        }

        // Grup oluşturma işlemini herhangi bir client yapacağı için ilk etapta gruba, grubu oluşturan client (caller) subscribe edilir..
        public async Task AddGroupAsync(string groupName)
        {
            // Grupların içinde hangi clientların olduğunun bilgisi server tarafından tutuluyor. ViewModel vs. kullanmaya gerek yok..
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            Group group = new Group { GroupName = groupName };

            // Group altında tutulan listeye client bilgisi ekleniyor..
            group.Clients.Add(ClientSource.Clients.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId));

            // Sistemde hangi grupların olduğu bilgisini depolamak gerekiyor..
            GroupSource.Groups.Add(group);

            // Sisteme bir grup/oda eklendiğini tüm clientlara bildiriyor..
            await Clients.All.SendAsync("groupAdded", groupName);

            await GetGroupsAsync();
        }

        public async Task GetGroupsAsync()
        {
            // Sisteme eklenen oda/grup oluşturan client dahil olmak üzere tüm clientlara bildirir.. Güncel grup listesini tüm clientlara bildirir..
            await Clients.All.SendAsync("groups", GroupSource.Groups);
        }

        // Parametrede bildirilen odalara callerı dahil eder..
        public async Task AddClientToGroupsAsync(IEnumerable<string> groupNames)
        {
            // İstekte bulunan (caller) client bilgisi alınıyor..
            Client client = ClientSource.Clients.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);

            foreach (var groupName in groupNames)
            {
                // groupName üzerinden GroupSourcedaki group listesinden ilgili grup bulunuyor..
                Group _group = GroupSource.Groups.FirstOrDefault(g => g.GroupName == groupName);

                // Caller ilgili groupa subscribe mı kontrol ediliyor..
                var result = _group.Clients.Any(c => c.ConnectionId == Context.ConnectionId);
                if (!result) // İlgili gruba dahil değilse..
                {
                    // Group nesnesinde tutulan client listesine caller client ekleniyor..
                    _group.Clients.Add(client);

                    // İlgili client (caller) gruba dahil ediliyor..
                    await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

                    // İlgili gruba ait tüm clientların listesini döndürür..
                    await GetClientsOfGroupAsync(groupName);
                }
            }
        }

        // Parametrede bildirilen odaya callerı dahil eder..
        public async Task AddClientToGroupAsync(string groupName)
        {
            // İstekte bulunan (caller) client bilgisi alınıyor..
            Client client = ClientSource.Clients.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId);

            // groupName üzerinden GroupSourcedaki group listesinden ilgili grup bulunuyor..
            Group _group = GroupSource.Groups.FirstOrDefault(g => g.GroupName == groupName);

            // Caller ilgili groupa subscribe mı kontrol ediliyor..
            var result = _group.Clients.Any(c=> c.ConnectionId == Context.ConnectionId);
            if (!result) // İlgili gruba dahil değilse..
            {
                // Group nesnesinde tutulan client listesine caller client ekleniyor..
                _group.Clients.Add(client);

                // İlgili client (caller) gruba dahil ediliyor..
                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

                // İlgili gruba ait tüm clientların listesini döndürür..
                await GetClientsOfGroupAsync(groupName);
            }
            
        }

        // İlgili gruba ait tüm clientların listesini döndürür..
        public async Task GetClientsOfGroupAsync(string groupName)
        {
            Group group = GroupSource.Groups.FirstOrDefault(g => g.GroupName.Equals(groupName));

            //await Clients.Caller.SendAsync("clientsOfGroup", group.Clients);
            await Clients.Groups(groupName).SendAsync("clientsOfGroup", group.Clients);
        }

        // İlgili gruba mesajı gönderir..
        public async Task SendMessageToGroupAsync(string groupName, string message)
        {
            //Clienttaki receiveMessage fonk. tetiklenir. Mesaj, senderClient ve groupName değerleri döndürülür.. 
            //receiveMessage fonk.. 4 değer bekliyor.. 3. dönüş değeri bir Client beklemekte, grup mesajlaşmalarında bir clienta gönderilmediği için bu değer null olarak döndürülüyor..
            //4. dönüş değeri ise string beklemekte, bu da karşılıklı mesajlaşma durumunda null döndürülüyor..
            await Clients.Groups(groupName).SendAsync("receiveMessage", message, ClientSource.Clients.FirstOrDefault(c => c.ConnectionId == Context.ConnectionId), null, groupName);
        }
    }
}

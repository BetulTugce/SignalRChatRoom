using SignalRChatRoom.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// CORS politikalarý ayarlarý..
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(origin => true)));

// Sisteme WebSocket protokolünü kullanacaðý bildiriliyor yani SignalR modülü devreye sokuluyor..
builder.Services.AddSignalR();

var app = builder.Build();

// Ayarlanan CORS politikasýný kullanabilmek için ilgili middleware çaðýrýlýyor..
app.UseCors();

// Hubýn hangi endpointte kullanýlacaðý bildiriliyor..
app.MapHub<ChatHub>("/chathub");

app.Run();

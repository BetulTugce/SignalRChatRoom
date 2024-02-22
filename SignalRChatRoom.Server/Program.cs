using SignalRChatRoom.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// CORS politikalar� ayarlar�..
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed(origin => true)));

// Sisteme WebSocket protokol�n� kullanaca�� bildiriliyor yani SignalR mod�l� devreye sokuluyor..
builder.Services.AddSignalR();

var app = builder.Build();

// Ayarlanan CORS politikas�n� kullanabilmek i�in ilgili middleware �a��r�l�yor..
app.UseCors();

// Hub�n hangi endpointte kullan�laca�� bildiriliyor..
app.MapHub<ChatHub>("/chathub");

app.Run();

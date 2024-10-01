using Library.Api.Commons.Interfaces;
using Library.Api.Persistance;

var builder = WebApplication.CreateBuilder(args);

#region Add Services Here
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDbConnnectionFactory>(_ => 
new SqliteConnectionFactory(builder.Configuration.GetValue<string>("Database:ConnectionString")!));
builder.Services.AddSingleton<DatabaseInitializer>();
#endregion Add Services Here

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Add Database initializer here
var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitilizeDatabaseAsync();

app.Run();


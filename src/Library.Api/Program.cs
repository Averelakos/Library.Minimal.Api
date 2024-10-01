using Library.Api.Commons.Interfaces.Persistance;
using Library.Api.Commons.Interfaces.Services;
using Library.Api.Entities;
using Library.Api.Persistance;
using Library.Api.Services;

var builder = WebApplication.CreateBuilder(args);

#region Add Services Here
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDbConnnectionFactory>(_ => 
new SqliteConnectionFactory(builder.Configuration.GetValue<string>("Database:ConnectionString")!));
builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddSingleton<IBookService, BookService>();
#endregion Add Services Here

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("books", async (Book book, IBookService bookService) =>
{
    var created = await bookService.CreateAsync(book);
    if (!created)
        return Results.BadRequest(new {errorMessage = "A book with ISBN-13 already exists."});

    return Results.Created($"/books/{book.Isbn}", book);
});


// Add Database initializer here
var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitilizeDatabaseAsync();

app.Run();


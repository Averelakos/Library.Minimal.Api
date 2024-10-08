using FluentValidation;
using FluentValidation.Results;
using Library.Api.Commons.Interfaces.Persistance;
using Library.Api.Commons.Interfaces.Services;
using Library.Api.Entities;
using Library.Api.Persistance;
using Library.Api.Services;

var builder = WebApplication.CreateBuilder(args);

//Add Configuration files
//builder.Configuration.AddJsonFile("appsetting.local.json", true, true);

#region Add Services Here
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDbConnnectionFactory>(_ => 
new SqliteConnectionFactory(builder.Configuration.GetValue<string>("Database:ConnectionString")!));
builder.Services.AddSingleton<DatabaseInitializer>();
builder.Services.AddSingleton<IBookService, BookService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
#endregion Add Services Here

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("books", async (Book book, IBookService bookService, IValidator<Book> validator) =>
{
    var validationResult = await validator.ValidateAsync(book);

    if (!validationResult.IsValid)
        return Results.BadRequest(validationResult.Errors);

    var created = await bookService.CreateAsync(book);
    if (!created)
        return Results.BadRequest(new List<ValidationFailure> 
        { 
            new ("Isbn","A book with ISBN-13 already exists.")
        });

    return Results.Created($"/books/{book.Isbn}", book);
});

app.MapGet("books", async (IBookService bookService, string? searchTerm) =>
{
    if(searchTerm is not null && !string.IsNullOrWhiteSpace(searchTerm))
    {
        var matchedBooks = await bookService.SearchByTitleAsync(searchTerm);
        return Results.Ok(matchedBooks);
    }

    var books = await bookService.GetAllAsync();
    return Results.Ok(books);
});

app.MapGet("books/{isbn}", async (string isbn, IBookService bookService) =>
{
    var book = await bookService.GetByIsbnAsync(isbn);
    return book is not null ? Results.Ok(book) : Results.NotFound();
});

app.MapPut("books/{isbn}", async (string isbn, Book book, IBookService bookService, IValidator<Book> validator) =>
{
    book.Isbn = isbn;

    var validationResult = await validator.ValidateAsync(book);

    if (!validationResult.IsValid)
        return Results.BadRequest(validationResult.Errors);

    var updatedBook = await bookService.UpdateAsync(book);

    return updatedBook ? Results.Ok(updatedBook) : Results.NotFound();
});

app.MapDelete("book/{isbn}", async (string isbn, IBookService bookService) =>
{
    var deleted = await bookService.DeleteAsync(isbn);
    return deleted ? Results.NoContent() : Results.NotFound();
});


// Add Database initializer here
var databaseInitializer = app.Services.GetRequiredService<DatabaseInitializer>();
await databaseInitializer.InitilizeDatabaseAsync();

app.Run();


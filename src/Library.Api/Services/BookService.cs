using Dapper;
using Library.Api.Commons.Interfaces.Persistance;
using Library.Api.Commons.Interfaces.Services;
using Library.Api.Entities;

namespace Library.Api.Services
{
    public class BookService : IBookService
    {
        private readonly IDbConnnectionFactory _dbConnnectionFactory;

        public BookService(IDbConnnectionFactory dbConnnectionFactory)
        {
            _dbConnnectionFactory = dbConnnectionFactory;
        }

        public async Task<bool> CreateAsync(Book book)
        {
            //var existingBook = await GetByIsbnAsync(book.Isbn);
            //if (existingBook is not null) 
            //{
            //    return false;
            //}

            using var connection = await _dbConnnectionFactory.CreateConnectionAsync();
            var result = await connection.ExecuteAsync(
                @"INSERT INTO Books(Isbn, Title, Author, ShortDescription, PageCount, ReleaseDate)
                    VALUES(@Isbn, @Title, @Author, @ShortDescription, @PageCount, @ReleaseDate)", 
                book);

            return result > 0;
        }

        public Task<bool> DeleteAsync(string isbn)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Book>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Book?> GetByIsbnAsync(string isbn)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Book>> SearchByTitleAsync(string searchTerm)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Book book)
        {
            throw new NotImplementedException();
        }
    }
}

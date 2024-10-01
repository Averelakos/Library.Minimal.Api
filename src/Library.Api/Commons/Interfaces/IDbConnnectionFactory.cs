using System.Data;

namespace Library.Api.Commons.Interfaces
{
    public interface IDbConnnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync();
    }
}

using System.Data;

namespace Library.Api.Commons.Interfaces.Persistance
{
    public interface IDbConnnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync();
    }
}

using System.Threading.Tasks;

namespace PowerwallSniffer.DatabaseProvider.TimescaleDB.Repository
{
    public interface IRepository<in TEntity>
    {
        Task Insert(TEntity item);
    }
}
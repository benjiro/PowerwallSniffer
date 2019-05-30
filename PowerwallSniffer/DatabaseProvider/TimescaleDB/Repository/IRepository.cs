namespace PowerwallSniffer.DatabaseProvider.TimescaleDB.Repository
{
    public interface IRepository<in TEntity>
    {
        void Insert(TEntity item);
    }
}
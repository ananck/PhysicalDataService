using Core.IRepository;
using Core.Model;

namespace Core.Repository
{
    public class TradeRepository : BaseRepository<Trade>, ITradeRepository
    {
        public TradeRepository(MyDbContext myDbContext) : base(myDbContext)
        {
        }
    }
}
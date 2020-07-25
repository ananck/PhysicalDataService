using Core.IRepository;
using Core.IService;
using Core.Model;

namespace Core.Service
{
    public class TradeService : BaseService<Trade>, ITradeService
    {
        public TradeService(IUnitOfWork unitOfWork, IBaseRepository<Trade> currentRepository) : base(unitOfWork, currentRepository)
        {
        }
    }
}
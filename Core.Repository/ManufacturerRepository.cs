using Core.IRepository;
using Core.Model;

namespace Core.Repository
{
    public class ManufacturerRepository : BaseRepository<Manufacturer>, IRepository.ManufacturerRepository
    {
        public ManufacturerRepository(MyDbContext myDbContext) : base(myDbContext)
        {
        }
    }
}
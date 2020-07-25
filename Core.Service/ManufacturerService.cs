using Core.IRepository;
using Core.IService;
using Core.Model;
using System.Threading.Tasks;

namespace Core.Service
{
    public class ManufacturerService : BaseService<Manufacturer>, IService.IManufacturerService
    {

        public ManufacturerService(IUnitOfWork unitOfWork, IBaseRepository<Manufacturer> currentRepository) : base(unitOfWork, currentRepository)

        {

        }

        //public async Task<bool> UOW(Manufacturer student, Teacher teacher)
        //{
        //    await currentRepository.Insert(student);
        //    await teacherRepository.Insert(teacher);

        //    await unitOfWork.SaveChangesAsync();

        //    return true;
        //}
    }
}
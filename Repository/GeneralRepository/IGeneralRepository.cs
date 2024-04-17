using Flight.Model;
using Flight.Model.DTO;

namespace Flight.Repository.GeneralRepository
{
    public interface IGeneralRepository
    {
        Task CreateGeneral(GeneralModel model);
        Task UpDateGeneral(int id ,GeneralModel model);
        Task<GeneralDTO> GetGeneral(int id);
    }
}

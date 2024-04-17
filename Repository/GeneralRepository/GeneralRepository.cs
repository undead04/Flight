using Flight.Data;
using Flight.Model;
using Flight.Model.DTO;
using Flight.Service.FileService;
using Microsoft.EntityFrameworkCore;

namespace Flight.Repository.GeneralRepository
{
    public class GeneralRepository : IGeneralRepository
    {
        private readonly MyDbContext context;
        private readonly IFileService fileService;

        public GeneralRepository(MyDbContext context,IFileService fileService) 
        {
            this.context = context;
            this.fileService= fileService;
        }
        public async Task CreateGeneral(GeneralModel model)
        {
            var general = new General
            {
                Theme= model.Theme,
            };
            
            if(model.Logo!=null)
            {
                var nameLogo = await fileService.UploadFile(null, "Logo", model.Logo);
                general.Logo = nameLogo;
            }
            await context.generals.AddAsync(general);
            await context.SaveChangesAsync();
        }

        public async Task<GeneralDTO> GetGeneral(int id)
        {
            var general = await context.generals.FirstOrDefaultAsync(ge => ge.Id == id);
            if (general == null)
            {
                return null;
            }
            return new GeneralDTO
            {
                id = general.Id,
                Theme = general.Theme,
                UrlLogo = fileService.GetUrlFile("Logo",general.Logo, null)
            };
        }

        public async Task UpDateGeneral(int id, GeneralModel model)
        {
            var general = await context.generals.FirstOrDefaultAsync(ge => ge.Id == id);
            if (general != null)
            {
                general.Theme = model.Theme;
                if(model.Logo!=null)
                {
                    fileService.DeleteFile("Logo", general.Logo, null);
                    general.Logo = await fileService.UploadFile(null, "Logo", model.Logo);
                    await context.SaveChangesAsync();
                }
            }
            
        }
    }
}

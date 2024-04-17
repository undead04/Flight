using Flight.Data;
using Flight.Model;
using Flight.Service.FileService;
using Microsoft.EntityFrameworkCore;

namespace Flight.Service.ConfirmDocumentService
{
    public class ConfirmDocumentService : IConfirmDocumentService
    {
        private readonly MyDbContext context;
        private readonly IFileService fileService;

        public ConfirmDocumentService(MyDbContext context,IFileService fileService) 
        {
            this.context = context;
            this.fileService = fileService;
        }
        public async Task ConfirmDocument(ConfirmDocumentModel model)
        {
            var flight = await context.flight.FirstOrDefaultAsync(fl => fl.Id == model.FlightId);
            if (flight != null&&flight.IsConfirm==false)
            {
                flight.IsConfirm = true;
                if(model.Signature!=null)
                {
                    flight.Signature = await fileService.UploadFile(null, "Signature", model.Signature);
                }
                await context.SaveChangesAsync();
            }
        }
    }
}

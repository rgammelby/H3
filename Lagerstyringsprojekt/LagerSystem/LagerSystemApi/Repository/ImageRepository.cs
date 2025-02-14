using LagerSystemApi.Interfaces;
using LagerSystemApi.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace LagerSystemApi.Repository
{
    public class ImageRepository: IImageRepository
    {
        private readonly Context _context;

        public ImageRepository(Context context)
        {
            _context = context;
        }

        public async Task<string> GetImage(int id)
        {
            try
            {
                DeviceOverview overview = await _context.DeviceOverview.Where(db => db.id == id).FirstOrDefaultAsync();

                if (overview == null)
                {
                    return null;
                }
                return overview.image;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

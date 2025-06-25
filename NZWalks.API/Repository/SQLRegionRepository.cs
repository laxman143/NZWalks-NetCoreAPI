using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repository
{
    public class SQLRegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContextcs dbContext;
        public SQLRegionRepository(NZWalksDbContextcs dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var extingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (extingRegion == null)
            {
                return null;
            }
            
            extingRegion.Code = region.Code;
            extingRegion.Name = region.Name;
            extingRegion.RegionImageUrl = region.RegionImageUrl;
             await dbContext.SaveChangesAsync();
            return extingRegion;
            
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null)
            {
                return null;
            }
            dbContext.Regions.Remove(existingRegion);
            await dbContext.SaveChangesAsync();
            return existingRegion;
        }
     }
}

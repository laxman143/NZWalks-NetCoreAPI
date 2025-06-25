using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Linq;

namespace NZWalks.API.Repository
{
    public class SQLWalkRepository : IWalkRepository
    {

         private readonly NZWalksDbContextcs dbContext; 
    
        public SQLWalkRepository(NZWalksDbContextcs dbContext)
        {
            this.dbContext = dbContext;
        }

       

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
         
            var existing = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) {
                return null;
            }
            dbContext.Walks.Remove(existing);
            await dbContext.SaveChangesAsync();
            return existing;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
            string? sortBy = null, bool? isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //Filtering 
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false) {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase)) {
                    walks = walks.Where(x=>x.Name.Contains(filterQuery));
                }
                if (filterOn.Equals("Description", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Description.Contains(filterQuery));
                }
             }

            // Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false) { 

                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase)) {

                    walks = isAscending == true ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("LengthInKm", StringComparison.OrdinalIgnoreCase)) {
                    walks = isAscending == true ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            //Pagination
            var skipResults = (pageNumber - 1) * pageSize;


            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
            //return await dbContext.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dbContext.Walks
                .Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var walkModel = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (walkModel == null) {
                return null;
             }
            walkModel.Name = walk.Name;
            walkModel.Description = walk.Description;
            walkModel.LengthInKm = walk.LengthInKm;
            walkModel.WalkImageUrl = walk.WalkImageUrl;
            walkModel.DifficultyId = walk.DifficultyId;
            walkModel.RegionId = walk.RegionId;
            await dbContext.SaveChangesAsync();
            return walkModel;

        }
    }
}

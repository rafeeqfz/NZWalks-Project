using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class SQLRegionRepository :IRegionRepository
    {
        private readonly NZWalksDbContext _dbContext;

        public SQLRegionRepository(NZWalksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Region> CreateAsync(Region region)
        {
           await _dbContext.Regions.AddAsync(region);
           _dbContext.SaveChangesAsync();
          return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
           var regionDelete= await _dbContext.Regions.FirstOrDefaultAsync(x=>x.Id == id);

            if(regionDelete ==null)
            {
                return null;
            }
            _dbContext.Regions.Remove(regionDelete);
           await _dbContext.SaveChangesAsync();
            return regionDelete;
            
        }

        public async Task<List<Region>> GetAllAsync()
        {
           return await _dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Regions.FirstOrDefaultAsync(a=>a.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
           var existingRegion= await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if(existingRegion==null)
            {
                return null;
            }
            existingRegion.Code = region.Code;
            existingRegion.Name= region.Name;
            existingRegion.RegionImageUrl= region.RegionImageUrl;
            return existingRegion;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _dbContext;
        public SQLWalkRepository(NZWalksDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _dbContext.Walks.AddAsync(walk);
            _dbContext.SaveChanges();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalks = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingWalks == null)
            {
                return null;
            }

            _dbContext.Walks.Remove(existingWalks);
            await _dbContext.SaveChangesAsync();
            return existingWalks;

        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null,
            bool isAscendingOrder = true, int pageNumber = 1, int pageSize = 1000)
        {
            //filtering
            var walk = _dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walk = walk.Where(a => a.Name.Contains(filterQuery));
                }
            }
            //sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walk = isAscendingOrder ? walk.OrderBy(x => x.Name) : walk.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("LengthInKm", StringComparison.OrdinalIgnoreCase))
                {
                    walk = isAscendingOrder ? walk.OrderBy(a => a.LengthInKm) : walk.OrderByDescending(a => a.LengthInKm);
                }

            }
            //paging
            var skipResult = (pageNumber - 1) * pageSize;

            return await walk.Skip(skipResult).Take(pageSize).ToListAsync();

        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var result = await _dbContext.Walks.Include("Region").Include("Difficulty").FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var exisitngWalk = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (exisitngWalk == null)
            {
                return null;
            }

            exisitngWalk.Name = walk.Name;
            exisitngWalk.Description = walk.Description;
            exisitngWalk.WalkImageUrl = walk.WalkImageUrl;
            exisitngWalk.DifficultyId = walk.DifficultyId;
            exisitngWalk.RegionId = walk.RegionId;

            await _dbContext.SaveChangesAsync();
            return exisitngWalk;

        }
    }

}

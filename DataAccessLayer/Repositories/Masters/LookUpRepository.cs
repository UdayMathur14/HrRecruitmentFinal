using DataAccess.Domain.Masters.LookUpMst;
using DataAccess.Interfaces.Masters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.RequestModels.Masters.LookUp;

namespace DataAccess.Repositories.Masters
{
    public class LookupRepository(ApplicationDbContext _context) : ILookupReporsitory
    {
        public async Task<Guid> AddAsync(LookupMstEntity entity)
        {
            _context.LookupMstEntities.Add(entity);
            _context.LookupMstEntities.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<LookupMstEntity?> FindAsync(Guid id)
        {
            return await _context.LookupMstEntities
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        }

        public async Task<LookupMstEntity?> FindByValue(LookUpRequestModel requestModel)
        {
            return await _context.LookupMstEntities
               .Where(x => x.Value == requestModel.Value && x.Description == requestModel.Description)
               .FirstOrDefaultAsync();
        }

        public async Task<LookupSearchResponseEntity> SearchLookUpAsync(LookUpSearchRequestModel requestModel, string? offset, string count)
        {
            var response = new LookupSearchResponseEntity();
            try
            {
                var query = _context.LookupMstEntities.AsQueryable();

                if (!string.IsNullOrWhiteSpace(requestModel.Type))
                {
                    query = query.Where(t => t.Type.ToLower().Contains(requestModel.Type.ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(requestModel.Value))
                {
                    query = query.Where(t => t.Value.ToLower().Contains(requestModel.Value.ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(requestModel.Status))
                {
                    query = query.Where(t => t.Status.ToLower().Contains(requestModel.Status.ToLower()));
                }

                response.Paging.Total = await query.AsNoTracking().CountAsync();

                // Try parsing offset and count
                int parsedOffset = int.TryParse(offset, out int tempOffset) ? tempOffset : 0;
                int parsedCount = int.TryParse(count, out int tempCount) ? tempCount : 10; // Default count if parsing fails

                if (parsedCount == 0)
                {
                    response.LookUps = await query.ToListAsync();

                    // Set pagination values
                    response.Paging.TotalPages = 0;
                    response.Paging.CurrentPage = 0;
                    response.Paging.Results = 0;
                    response.Paging.NextOffset = null;
                    response.Paging.NextPage = null;
                    response.Paging.PrevPage = null;
                }
                else
                {
                    response.LookUps = await query.Skip(parsedOffset).Take(parsedCount).ToListAsync();

                    response.Paging.TotalPages = (int)Math.Ceiling((double)response.Paging.Total / parsedCount);
                    response.Paging.CurrentPage = (parsedOffset / parsedCount) + 1;
                    response.Paging.Results = response.LookUps.Count();

                    int nextOffset = parsedOffset + parsedCount;
                    response.Paging.NextOffset = (response.Paging.Total > nextOffset) ? nextOffset.ToString() : null;

                    response.Paging.NextPage = response.Paging.NextOffset != null
                        ? $"?offset={nextOffset}&count={parsedCount}"
                        : null;

                    response.Paging.PrevPage = response.Paging.CurrentPage > 1
                        ? $"?offset={(parsedOffset - parsedCount)}&count={parsedCount}"
                        : null;

                    // Fetch distinct filter values
                    response.Filters = new Dictionary<string, List<string>>
            {
                { "Type", await _context.LookupMstEntities.Select(a => a.Type).Distinct().ToListAsync() },
                { "Status", await _context.LookupMstEntities.Select(a => a.Status).Distinct().ToListAsync() },
                { "Value", await _context.LookupMstEntities.Select(a => a.Value).Distinct().ToListAsync() },

                    };
                }


                response.responseCode = StatusCodes.Status200OK;
            }
            catch (Exception ex)
            {
                response.responseCode = StatusCodes.Status400BadRequest;
                response.message = ex.Message;
            }
            return response;
        }

        public async Task<Guid> UpdateAsync(LookupMstEntity entity)
        {
            _context.LookupMstEntities.Update(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }
    }
}

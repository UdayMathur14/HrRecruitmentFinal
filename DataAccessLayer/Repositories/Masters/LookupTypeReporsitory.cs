using DataAccess.Domain.Masters.LookUpType;
using DataAccess.Interfaces.Masters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.RequestModels.Masters.LookUpType;

namespace DataAccess.Repositories.Masters
{
    public class LookupTypeReporsitory(ApplicationDbContext _context) : ILookupTypeReporsitory
    {
        public async Task<Guid> AddAsync(LookupTypeMstEntity entity)
        {
            _context.LookupTypeMstEntities.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<LookupTypeMstEntity?> FindById(Guid id)
        {
            return await _context.LookupTypeMstEntities
               .Where(x => x.Id == id)
               .FirstOrDefaultAsync();
        }

        public async Task<LookupTypeMstEntity?> FindByTypeAsync(LookUpTypeRequestModel requestModel)
        {
            return await _context.LookupTypeMstEntities
                .Where(x => x.Type == requestModel.Type)
                .FirstOrDefaultAsync();
        }

        public async Task<LookupTypeSearchResponseEntity> SearLookupTypeAsync(LookUpTypeSearchRequestModel requestModel, string? offset, string count)
        {
            var response = new LookupTypeSearchResponseEntity();
            try
            {
                var query = _context.LookupTypeMstEntities.AsQueryable();

                if (!string.IsNullOrWhiteSpace(requestModel.Type))
                {
                    query = query.Where(t => t.Type.ToLower().Contains(requestModel.Type.ToLower()));
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
                    response.LookUpTypes = await query.ToListAsync();

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
                    response.LookUpTypes = await query.Skip(parsedOffset).Take(parsedCount).ToListAsync();

                    response.Paging.TotalPages = (int)Math.Ceiling((double)response.Paging.Total / parsedCount);
                    response.Paging.CurrentPage = (parsedOffset / parsedCount) + 1;
                    response.Paging.Results = response.LookUpTypes.Count();

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
                { "Type", await _context.LookupTypeMstEntities.Select(a => a.Type).Distinct().ToListAsync() },
                { "Status", await _context.LookupTypeMstEntities.Select(a => a.Status).Distinct().ToListAsync() },
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

        public async Task<Guid> UpdateAsync(LookupTypeMstEntity entity)
        {
            _context.LookupTypeMstEntities.Update(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }
    }
}

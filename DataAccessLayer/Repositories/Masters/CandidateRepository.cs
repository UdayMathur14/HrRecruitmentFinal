using DataAccessLayer.Domain.Masters.Candidate;
using DataAccessLayer.Interfaces.Masters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.RequestModels.Masters.Candidate;

namespace DataAccessLayer.Repositories.Masters
{
    public class CandidateRepository(ApplicationDbContext context) : ICandidateRepository
    {
        public async Task<Guid> AddAsync(CandidateEntity entity)
        {
            context.Add(entity);
            await context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<CandidateEntity?> FindAsync(Guid id)
        {
            return await context.Set<CandidateEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<CandidateSearchResponseEntity> SearchCandidateAsync(CandidateSearchRequestModel requestModel, string? offset, string count)
        {
            var response = new CandidateSearchResponseEntity();
            try
            {
                var query = context.Set<CandidateEntity>().AsNoTracking().AsQueryable();

                if (requestModel.JobId.HasValue)
                    query = query.Where(t => t.JobId == requestModel.JobId.Value);

                if (requestModel.DeptId.HasValue)
                    query = query.Where(t => t.DeptId == requestModel.DeptId.Value);

                if (!string.IsNullOrWhiteSpace(requestModel.FullName))
                    query = query.Where(t => t.FullName.ToLower().Contains(requestModel.FullName.ToLower()));

                if (!string.IsNullOrWhiteSpace(requestModel.Email))
                    query = query.Where(t => t.Email.ToLower().Contains(requestModel.Email.ToLower()));

                if (!string.IsNullOrWhiteSpace(requestModel.Status))
                    query = query.Where(t => t.Status != null && t.Status.ToLower().Equals(requestModel.Status.ToLower()));

                if (!string.IsNullOrWhiteSpace(requestModel.CandidateStatus))
                    query = query.Where(t => t.CandidateStatus != null && t.CandidateStatus.ToLower().Equals(requestModel.CandidateStatus.ToLower()));

                if (!string.IsNullOrWhiteSpace(requestModel.Location))
                    query = query.Where(t => t.Location != null && t.Location.ToLower().Contains(requestModel.Location.ToLower()));

                if (!string.IsNullOrWhiteSpace(requestModel.Skills))
                    query = query.Where(t => t.Skills != null && t.Skills.ToLower().Contains(requestModel.Skills.ToLower()));

                response.Paging.Total = await query.CountAsync();

                int parsedOffset = int.TryParse(offset, out int tempOffset) ? tempOffset : 0;
                int parsedCount = int.TryParse(count, out int tempCount) ? tempCount : 10;

                if (parsedCount == 0)
                {
                    response.Candidates = await query.ToListAsync();

                    response.Paging.TotalPages = 0;
                    response.Paging.CurrentPage = 0;
                    response.Paging.Results = 0;
                    response.Paging.NextOffset = null;
                    response.Paging.NextPage = null;
                    response.Paging.PrevPage = null;
                }
                else
                {
                    response.Candidates = await query.Skip(parsedOffset).Take(parsedCount).ToListAsync();

                    var resultCount = response.Candidates.Count();
                    response.Paging.TotalPages = (int)Math.Ceiling((double)response.Paging.Total / parsedCount);
                    response.Paging.CurrentPage = (parsedOffset / parsedCount) + 1;
                    response.Paging.Results = resultCount;

                    int nextOffset = parsedOffset + parsedCount;
                    response.Paging.NextOffset = (response.Paging.Total > nextOffset) ? nextOffset.ToString() : null;

                    response.Paging.NextPage = response.Paging.NextOffset != null
                        ? $"?offset={nextOffset}&count={parsedCount}"
                        : null;

                    response.Paging.PrevPage = response.Paging.CurrentPage > 1
                        ? $"?offset={(parsedOffset - parsedCount)}&count={parsedCount}"
                        : null;
                }

                response.Filters = new Dictionary<string, List<string>>
                {
                    { "Status", await context.Set<CandidateEntity>().Where(a => a.Status != null).Select(a => a.Status!).Distinct().ToListAsync() },
                    { "Location", await context.Set<CandidateEntity>().Where(a => a.Location != null).Select(a => a.Location!).Distinct().ToListAsync() },
                    { "CandidateStatus", await context.Set<CandidateEntity>().Where(a => a.CandidateStatus != null).Select(a => a.CandidateStatus!).Distinct().ToListAsync() }
                };

                response.responseCode = StatusCodes.Status200OK;
            }
            catch (Exception ex)
            {
                response.responseCode = StatusCodes.Status400BadRequest;
                response.message = ex.Message;
            }

            return response;
        }

        public async Task<CandidateEntity> UpdateAsync(CandidateEntity entity)
        {
            context.Set<CandidateEntity>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}

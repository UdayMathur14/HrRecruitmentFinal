using DataAccessLayer.Domain.Masters.Job;
using DataAccessLayer.Interfaces.Masters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.RequestModels.Masters.Job;

namespace DataAccessLayer.Repositories.Masters
{
    public class JobRepository(ApplicationDbContext _context) : IJobRepository
    {
        public async Task<Guid> AddAsync(JobEntity entity)
        {
            _context.JobEntity.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<JobEntity?> FindAsync(Guid id)
        {
            return await _context.JobEntity
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(j => new JobEntity
                {
                    Id = j.Id,
                    DeptId = j.DeptId,
                    JobName = j.JobName,
                    Description = j.Description,
                    HeadCount = j.HeadCount,
                    Status = j.Status,
                    JobStage = j.JobStage,
                    JobOwnerId = j.JobOwnerId,
                    CreatedOn = j.CreatedOn,
                    CreatedBy = j.CreatedBy,
                    ModifiedOn = j.ModifiedOn,
                    ModifiedBy = j.ModifiedBy,
                    JobOwnerUser = j.JobOwnerUser == null ? null : new DataAccessLayer.Domain.Masters.User.UserEntity
                    {
                        Id = j.JobOwnerUser.Id,
                        FirstName = j.JobOwnerUser.FirstName,
                        LastName = j.JobOwnerUser.LastName
                    },
                    JobMembers = j.JobMembers.Select(m => new JobMembersEntity
                    {
                        UserId = m.UserId,
                        User = m.User == null ? null : new DataAccessLayer.Domain.Masters.User.UserEntity
                        {
                            Id = m.User.Id,
                            FirstName = m.User.FirstName,
                            LastName = m.User.LastName
                        }
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<JobSearchResponseEntity> SearchJobAsync(JobSearchRequestModel requestModel, string? offset, string count)
        {
            var response = new JobSearchResponseEntity();
            try
            {
                var query = _context.JobEntity.AsNoTracking().AsQueryable();

                if (requestModel.DeptId.HasValue)
                {
                    query = query.Where(t => t.DeptId == requestModel.DeptId.Value);
                }

                if (!string.IsNullOrWhiteSpace(requestModel.JobName))
                {
                    query = query.Where(t => t.JobName.ToLower().Equals(requestModel.JobName.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(requestModel.Status))
                {
                    query = query.Where(t => t.Status != null && t.Status.ToLower().Equals(requestModel.Status.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(requestModel.JobStage))
                {
                    query = query.Where(t => t.JobStage != null && t.JobStage.ToLower().Equals(requestModel.JobStage.ToLower()));
                }

                if (requestModel.JobOwnerId.HasValue)
                {
                    query = query.Where(t => t.JobOwnerId == requestModel.JobOwnerId.Value);
                }

                response.Paging.Total = await query.AsNoTracking().CountAsync();

                int parsedOffset = int.TryParse(offset, out int tempOffset) ? tempOffset : 0;
                int parsedCount = int.TryParse(count, out int tempCount) ? tempCount : 10;

                var projectedQuery = query.Select(j => new JobEntity
                {
                    Id = j.Id,
                    DeptId = j.DeptId,
                    JobName = j.JobName,
                    Description = j.Description,
                    HeadCount = j.HeadCount,
                    Status = j.Status,
                    JobStage = j.JobStage,
                    JobOwnerId = j.JobOwnerId,
                    CreatedOn = j.CreatedOn,
                    CreatedBy = j.CreatedBy,
                    ModifiedOn = j.ModifiedOn,
                    ModifiedBy = j.ModifiedBy,
                    JobOwnerUser = j.JobOwnerUser == null ? null : new DataAccessLayer.Domain.Masters.User.UserEntity
                    {
                        Id = j.JobOwnerUser.Id,
                        FirstName = j.JobOwnerUser.FirstName,
                        LastName = j.JobOwnerUser.LastName
                    },
                    JobMembers = j.JobMembers.Select(m => new JobMembersEntity
                    {
                        UserId = m.UserId,
                        User = m.User == null ? null : new DataAccessLayer.Domain.Masters.User.UserEntity
                        {
                            Id = m.User.Id,
                            FirstName = m.User.FirstName,
                            LastName = m.User.LastName
                        }
                    }).ToList()
                });

                if (parsedCount == 0)
                {
                    response.Jobs = await projectedQuery.ToListAsync();

                    response.Paging.TotalPages = 0;
                    response.Paging.CurrentPage = 0;
                    response.Paging.Results = 0;
                    response.Paging.NextOffset = null;
                    response.Paging.NextPage = null;
                    response.Paging.PrevPage = null;
                }
                else
                {
                    response.Jobs = await projectedQuery.Skip(parsedOffset).Take(parsedCount).ToListAsync();

                    response.Paging.TotalPages = (int)Math.Ceiling((double)response.Paging.Total / parsedCount);
                    response.Paging.CurrentPage = (parsedOffset / parsedCount) + 1;
                    response.Paging.Results = response.Jobs.Count();

                    int nextOffset = parsedOffset + parsedCount;
                    response.Paging.NextOffset = (response.Paging.Total > nextOffset) ? nextOffset.ToString() : null;

                    response.Paging.NextPage = response.Paging.NextOffset != null
                        ? $"?offset={nextOffset}&count={parsedCount}"
                        : null;

                    response.Paging.PrevPage = response.Paging.CurrentPage > 1
                        ? $"?offset={(parsedOffset - parsedCount)}&count={parsedCount}"
                        : null;

                    response.Filters = new Dictionary<string, List<string>>
                    {
                        { "JobName", await _context.JobEntity.Select(a => a.JobName).Distinct().ToListAsync() },
                        { "Status", await _context.JobEntity.Where(a => a.Status != null).Select(a => a.Status!).Distinct().ToListAsync() },
                        { "JobStage", await _context.JobEntity.Where(a => a.JobStage != null).Select(a => a.JobStage!).Distinct().ToListAsync() }
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

        public async Task<JobEntity> UpdateAsync(JobEntity entity)
        {
            _context.JobEntity.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task ReplaceMembersAsync(Guid jobId, List<JobMembersEntity> members)
        {
            var existingMembers = await _context.JobMembersEntity
                .Where(x => x.JobId == jobId)
                .ToListAsync();

            if (existingMembers.Count > 0)
            {
                _context.JobMembersEntity.RemoveRange(existingMembers);
            }

            if (members.Count > 0)
            {
                await _context.JobMembersEntity.AddRangeAsync(members);
            }
        }
    }
}

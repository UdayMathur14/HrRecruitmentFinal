using DataAccessLayer.Domain.Masters.Department;
using DataAccessLayer.Domain.Masters.Job;
using DataAccessLayer.Domain.Masters.User;
using DataAccessLayer.Interfaces.Masters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.RequestModels.Masters.Department;

namespace DataAccessLayer.Repositories.Masters
{
    public class DepartmentRepository(ApplicationDbContext _context) : IDepartmentRepository
    {
        public async Task<Guid> AddAsync(DepartmentEntity entity)
        {
            _context.DepartmentEntity.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<DepartmentEntity?> FindAsync(Guid id)
        {
            return await _context.DepartmentEntity
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Select(d => new DepartmentEntity
                {
                    Id = d.Id,
                    DeptName = d.DeptName,
                    Location = d.Location,
                    Description = d.Description,
                    Status = d.Status,
                    OwnerId = d.OwnerId,
                    CreatedOn = d.CreatedOn,
                    CreatedBy = d.CreatedBy,
                    ModifiedOn = d.ModifiedOn,
                    ModifiedBy = d.ModifiedBy,

                    OwnerUser = d.OwnerUser == null ? null : new UserEntity
                    {
                        Id = d.OwnerUser.Id,
                        FirstName = d.OwnerUser.FirstName,
                        LastName = d.OwnerUser.LastName
                    },

                    DepartmentMembers = d.DepartmentMembers.Select(m => new DepartmentMembersEntity
                    {
                        UserId = m.UserId,
                        User = m.User == null ? null : new UserEntity
                        {
                            Id = m.User.Id,
                            FirstName = m.User.FirstName,
                            LastName = m.User.LastName
                        }
                    }).ToList(),

                    // ?? CORRECT JOBS MAPPING
                    Jobs = d.Jobs.Select(j => new JobEntity
                    {
                        Id = j.Id,
                        JobName = j.JobName,
                        Description = j.Description,
                        HeadCount = j.HeadCount,
                        JobStage = j.JobStage,
                        Status = j.Status,

                        // ? CORRECT (j se lo, d se nahi)
                        JobOwnerUser = j.JobOwnerUser == null ? null : new UserEntity
                        {
                            Id = j.JobOwnerUser.Id,
                            FirstName = j.JobOwnerUser.FirstName,
                            LastName = j.JobOwnerUser.LastName
                        },

                        // ? CORRECT (j.JobMembers)
                        JobMembers = j.JobMembers.Select(m => new JobMembersEntity
                        {
                            UserId = m.UserId,
                            User = m.User == null ? null : new UserEntity
                            {
                                Id = m.User.Id,
                                FirstName = m.User.FirstName,
                                LastName = m.User.LastName
                            }
                        }).ToList()

                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<DepartmentEntity>> GetAllWithMembersAsync()
        {
            var departments = await _context.DepartmentEntity
                .AsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .Select(d => new DepartmentEntity
                {
                    Id = d.Id,
                    DeptName = d.DeptName,
                    Location = d.Location,
                    Description = d.Description,
                    Status = d.Status,
                    OwnerId = d.OwnerId,
                    CreatedOn = d.CreatedOn,
                    CreatedBy = d.CreatedBy,
                    ModifiedOn = d.ModifiedOn,
                    ModifiedBy = d.ModifiedBy,
                    OwnerUser = d.OwnerUser == null ? null : new DataAccessLayer.Domain.Masters.User.UserEntity
                    {
                        Id = d.OwnerUser.Id,
                        FirstName = d.OwnerUser.FirstName,
                        LastName = d.OwnerUser.LastName
                    },
                    DepartmentMembers = d.DepartmentMembers.Select(m => new DepartmentMembersEntity
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
                .ToListAsync();

            return departments;
        }

        public async Task<DepartmentSearchResponseEntity> SearchDeptAsync(DepartmentSearchRequestModel requestModel, string? offset, string count)
        {
            var response = new DepartmentSearchResponseEntity();
            try
            {
                var query = _context.DepartmentEntity.AsNoTracking().AsQueryable();

                if (!string.IsNullOrWhiteSpace(requestModel.DeptName))
                {
                    query = query.Where(t => t.DeptName.ToLower().Equals(requestModel.DeptName.ToLower()));
                }
                if (!string.IsNullOrWhiteSpace(requestModel.Status))
                {
                    query = query.Where(t => t.Status.ToLower().Equals(requestModel.Status.ToLower()));
                }

                response.Paging.Total = await query.AsNoTracking().CountAsync();

                int parsedOffset = int.TryParse(offset, out int tempOffset) ? tempOffset : 0;
                int parsedCount = int.TryParse(count, out int tempCount) ? tempCount : 10;

                var jobCounts = await _context.JobEntity.AsNoTracking().GroupBy(j => j.DeptId).Select(g => new { DeptId = g.Key, Count = g.Count() }).ToDictionaryAsync(x => x.DeptId, x => x.Count);

                var projectedQuery = query
                    .Select(d => new DepartmentEntity
                    {
                        Id = d.Id,
                        DeptName = d.DeptName,
                        Location = d.Location,
                        Description = d.Description,
                        Status = d.Status,
                        OwnerId = d.OwnerId,
                        CreatedOn = d.CreatedOn,
                        CreatedBy = d.CreatedBy,
                        ModifiedOn = d.ModifiedOn,
                        ModifiedBy = d.ModifiedBy,
                        JobCount = d.JobCount,
                        OwnerUser = d.OwnerUser == null ? null : new DataAccessLayer.Domain.Masters.User.UserEntity
                        {
                            Id = d.OwnerUser.Id,
                            FirstName = d.OwnerUser.FirstName,
                            LastName = d.OwnerUser.LastName
                        },
                        DepartmentMembers = d.DepartmentMembers.Select(m => new DepartmentMembersEntity
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
                    response.Departments = await projectedQuery.ToListAsync();

                    response.Paging.TotalPages = 0;
                    response.Paging.CurrentPage = 0;
                    response.Paging.Results = 0;
                    response.Paging.NextOffset = null;
                    response.Paging.NextPage = null;
                    response.Paging.PrevPage = null;
                }
                else
                {
                    response.Departments = await projectedQuery.Skip(parsedOffset).Take(parsedCount).ToListAsync();

                    response.Paging.TotalPages = (int)Math.Ceiling((double)response.Paging.Total / parsedCount);
                    response.Paging.CurrentPage = (parsedOffset / parsedCount) + 1;
                    response.Paging.Results = response.Departments.Count();

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
                        { "DeptName", await _context.DepartmentEntity.Select(a => a.DeptName).Distinct().ToListAsync() },
                        { "Status", await _context.DepartmentEntity.Select(a => a.Status).Distinct().ToListAsync() },
                    };
                }

                foreach (var dept in response.Departments ?? [])
                {
                    dept.JobCount = jobCounts.TryGetValue(dept.Id, out int countVal) ? countVal : 0;
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

        public async Task ReplaceMembersAsync(Guid deptId, List<DepartmentMembersEntity> members)
        {
            var existingMembers = await _context.DepartmentMembersEntity
                .Where(x => x.DeptId == deptId)
                .ToListAsync();

            if (existingMembers.Count > 0)
            {
                _context.DepartmentMembersEntity.RemoveRange(existingMembers);
            }

            if (members.Count > 0)
            {
                await _context.DepartmentMembersEntity.AddRangeAsync(members);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<DepartmentEntity> UpdateAsync(DepartmentEntity entity)
        {
            _context.DepartmentEntity.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}

using DataAccessLayer.Domain.Masters.User;
using DataAccessLayer.Interfaces.Masters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.RequestModels.Masters.User;

namespace DataAccessLayer.Repositories.Masters
{
    public class UserRepository(ApplicationDbContext _context) : IUserRepository
    {
        public async Task<Guid> AddAsync(UserEntity entity)
        {
            _context.UserEntity.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<UserEntity?> FindAsync(Guid id)
        {
            return await _context.UserEntity.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<UserSearchResponseEntity> SearchUserAsync(UserSearchRequestModel requestModel, string? offset, string count)
        {
            var response = new UserSearchResponseEntity();
            try
            {
                var query = _context.UserEntity.AsQueryable();

                if (!string.IsNullOrWhiteSpace(requestModel.FirstName))
                {
                    query = query.Where(t => t.FirstName.ToLower().Equals(requestModel.FirstName.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(requestModel.Email))
                {
                    query = query.Where(t => t.Email.ToLower().Equals(requestModel.Email.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(requestModel.Role))
                {
                    query = query.Where(t => t.Role != null && t.Role.ToLower().Equals(requestModel.Role.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(requestModel.Status))
                {
                    query = query.Where(t => t.Status != null && t.Status.ToLower().Equals(requestModel.Status.ToLower()));
                }

                response.Paging.Total = await query.AsNoTracking().CountAsync();

                int parsedOffset = int.TryParse(offset, out int tempOffset) ? tempOffset : 0;
                int parsedCount = int.TryParse(count, out int tempCount) ? tempCount : 10;

                if (parsedCount == 0)
                {
                    response.Users = await query.ToListAsync();

                    response.Paging.TotalPages = 0;
                    response.Paging.CurrentPage = 0;
                    response.Paging.Results = 0;
                    response.Paging.NextOffset = null;
                    response.Paging.NextPage = null;
                    response.Paging.PrevPage = null;
                }
                else
                {
                    response.Users = await query.Skip(parsedOffset).Take(parsedCount).ToListAsync();

                    response.Paging.TotalPages = (int)Math.Ceiling((double)response.Paging.Total / parsedCount);
                    response.Paging.CurrentPage = (parsedOffset / parsedCount) + 1;
                    response.Paging.Results = response.Users.Count();

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
                        { "FirstName", await _context.UserEntity.Select(a => a.FirstName).Distinct().ToListAsync() },
                        { "Email", await _context.UserEntity.Select(a => a.Email).Distinct().ToListAsync() },
                        { "Role", await _context.UserEntity.Where(a => a.Role != null).Select(a => a.Role!).Distinct().ToListAsync() },
                        { "Status", await _context.UserEntity.Where(a => a.Status != null).Select(a => a.Status!).Distinct().ToListAsync() },
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

        public async Task<UserEntity> UpdateAsync(UserEntity entity)
        {
            _context.UserEntity.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}

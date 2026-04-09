using DataAccessLayer.Domain.Common.Notes;
using DataAccessLayer.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.RequestModels.Common.Notes;

namespace DataAccessLayer.Repositories.Common
{
    public class NoteRepository(ApplicationDbContext context) : INoteRepository
    {
        public async Task<Guid> AddAsync(NoteEntity entity)
        {
            context.NoteEntity.Add(entity);
            await context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<NoteEntity?> FindAsync(Guid id)
        {
            return await context.NoteEntity.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<NoteSearchResponseEntity> SearchNoteAsync(NoteSearchRequestModel requestModel, string? offset, string count)
        {
            var response = new NoteSearchResponseEntity();
            try
            {
                var query = context.NoteEntity.AsQueryable();

                if (requestModel.ReferenceType.HasValue)
                    query = query.Where(x => x.ReferenceType == (int)requestModel.ReferenceType.Value);

                if (requestModel.ReferenceId.HasValue)
                    query = query.Where(x => x.ReferenceId == requestModel.ReferenceId.Value);

                if (!string.IsNullOrWhiteSpace(requestModel.Header))
                    query = query.Where(x => x.Header.ToLower().Equals(requestModel.Header.ToLower()));

                if (!string.IsNullOrWhiteSpace(requestModel.Status))
                    query = query.Where(x => x.Status != null && x.Status.ToLower().Equals(requestModel.Status.ToLower()));

                response.Paging.Total = await query.AsNoTracking().CountAsync();

                int parsedOffset = int.TryParse(offset, out int tempOffset) ? tempOffset : 0;
                int parsedCount = int.TryParse(count, out int tempCount) ? tempCount : 10;

                if (parsedCount == 0)
                {
                    response.Notes = await query.ToListAsync();

                    response.Paging.TotalPages = 0;
                    response.Paging.CurrentPage = 0;
                    response.Paging.Results = 0;
                    response.Paging.NextOffset = null;
                    response.Paging.NextPage = null;
                    response.Paging.PrevPage = null;
                }
                else
                {
                    response.Notes = await query
                        .OrderByDescending(x => x.CreatedOn)
                        .Skip(parsedOffset)
                        .Take(parsedCount)
                        .ToListAsync();

                    response.Paging.TotalPages = (int)Math.Ceiling((double)response.Paging.Total / parsedCount);
                    response.Paging.CurrentPage = (parsedOffset / parsedCount) + 1;
                    response.Paging.Results = response.Notes.Count();

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
                        { "Header", await context.NoteEntity.Select(a => a.Header).Distinct().ToListAsync() },
                        { "Status", await context.NoteEntity.Where(a => a.Status != null).Select(a => a.Status!).Distinct().ToListAsync() },
                        { "ReferenceType", await context.NoteEntity.Select(a => a.ReferenceType.ToString()).Distinct().ToListAsync() }
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

        public async Task<NoteEntity> UpdateAsync(NoteEntity entity)
        {
            context.NoteEntity.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}

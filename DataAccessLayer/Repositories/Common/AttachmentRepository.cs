using DataAccessLayer.Domain.Common.Attachments;
using DataAccessLayer.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Enums;
using Models.RequestModels.Common.Attachments;

namespace DataAccessLayer.Repositories.Common
{
    public class AttachmentRepository(ApplicationDbContext context) : IAttachmentRepository
    {
        public async Task<Guid> AddAsync(AttachmentEntity entity)
        {
            context.AttachmentEntity.Add(entity);
            await context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<AttachmentEntity?> FindAsync(Guid id)
        {
            return await context.AttachmentEntity.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AttachmentSearchResponseEntity> SearchAttachmentAsync(AttachmentSearchRequestModel requestModel, string? offset, string count)
        {
            var response = new AttachmentSearchResponseEntity();

            try
            {
                var query = context.AttachmentEntity.AsQueryable();

                if (requestModel.ReferenceType.HasValue)
                    query = query.Where(x => x.ReferenceType == (int)requestModel.ReferenceType.Value);

                if (requestModel.ReferenceId.HasValue)
                    query = query.Where(x => x.ReferenceId == requestModel.ReferenceId.Value);

                if (!string.IsNullOrWhiteSpace(requestModel.FileName))
                    query = query.Where(x => x.FileName != null && EF.Functions.Like(x.FileName.ToLower(), $"%{requestModel.FileName.ToLower()}%"));

                if (!string.IsNullOrWhiteSpace(requestModel.Status))
                    query = query.Where(x => x.Status != null && x.Status.ToLower().Equals(requestModel.Status.ToLower()));

                response.Paging.Total = await query.AsNoTracking().CountAsync();

                int parsedOffset = int.TryParse(offset, out int tempOffset) ? tempOffset : 0;
                int parsedCount = int.TryParse(count, out int tempCount) ? tempCount : 10;

                if (parsedCount == 0)
                {
                    response.Attachments = await query.ToListAsync();

                    response.Paging.TotalPages = 0;
                    response.Paging.CurrentPage = 0;
                    response.Paging.Results = 0;
                    response.Paging.NextOffset = null;
                    response.Paging.NextPage = null;
                    response.Paging.PrevPage = null;
                }
                else
                {
                    response.Attachments = await query
                        .OrderByDescending(x => x.CreatedOn)
                        .Skip(parsedOffset)
                        .Take(parsedCount)
                        .ToListAsync();

                    response.Paging.TotalPages = (int)Math.Ceiling((double)response.Paging.Total / parsedCount);
                    response.Paging.CurrentPage = (parsedOffset / parsedCount) + 1;
                    response.Paging.Results = response.Attachments.Count();

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
                        { "FileName", await context.AttachmentEntity.Where(a => a.FileName != null).Select(a => a.FileName!).Distinct().ToListAsync() },
                        { "Status", await context.AttachmentEntity.Where(a => a.Status != null).Select(a => a.Status!).Distinct().ToListAsync() },
                        {
                            "ReferenceType",
                            await context.AttachmentEntity
                                .Select(a => a.ReferenceType)
                                .Distinct()
                                .Select(a => Enum.IsDefined(typeof(ReferenceType), a)
                                    ? ((ReferenceType)a).ToString()
                                    : a.ToString())
                                .ToListAsync()
                        }
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

        public async Task<AttachmentEntity> UpdateAsync(AttachmentEntity entity)
        {
            context.AttachmentEntity.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}

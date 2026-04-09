using BusinessLayer.Interfaces.Common;
using BusinessLayer.Interfaces.Masters;
using BusinessLayer.Mappings.Masters;
using BusinessLayer.Mappings.Common;
using BusinessLayer.Services.Common;
using BusinessLayer.Services.Masters;
using BusinessLogic.Interfaces.Masters;
using BusinessLogic.Mappings.Masters;
using BusinessLogic.Services.Masters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BusinessLayer.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogicDependencies(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IDepartmentSummaryService, DepartmentSummaryService>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICandidateService, CandidateService>();
            services.AddAutoMapper(typeof(DepartmentMappingProfile).Assembly);
            services.AddAutoMapper(typeof(DepartmentSummaryMappingProfile).Assembly);
            services.AddAutoMapper(typeof(JobMappingProfile).Assembly);
            services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
            services.AddAutoMapper(typeof(CandidateMappingProfile).Assembly);

            services.AddScoped<ILookUpTypeService, LookUpTypeService>();
            services.AddAutoMapper(typeof(LookUpTypeMappingProfile).Assembly);

            services.AddScoped<ILookUpService, LookupService>();
            services.AddAutoMapper(typeof(LookUpMappingProfile).Assembly);
            services.AddScoped<INoteService, NoteService>();
            services.AddAutoMapper(typeof(NoteMappingProfile).Assembly);
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddAutoMapper(typeof(AttachmentMappingProfile).Assembly);

            return services;
        }
    }
}

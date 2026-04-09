using Models;
using Models.ResponseModels.BaseResponseSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Domain.Masters.Department
{
    public class DepartmentSearchResponseEntity :CommonResponseModel
    {
        public IEnumerable<DepartmentEntity>? Departments { get; set; } = new List<DepartmentEntity>();
        public PagingModel Paging { get; set; } = new PagingModel();
        public Dictionary<string, List<string>> Filters { get; set; } = new Dictionary<string, List<string>>();
    }
}

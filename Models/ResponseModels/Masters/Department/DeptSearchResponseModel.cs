using Models.ResponseModels.BaseResponseSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ResponseModels.Masters.Department
{
    public class DeptSearchResponseModel : SearchResponseBase<DepartmentReadResponseModel>
    {
        public List<DepartmentReadResponseModel> Departments => Results;
    }
}

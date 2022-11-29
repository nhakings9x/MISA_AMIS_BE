
using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.BL
{
    public class DepartmentBL : BaseBL<Department>, IDepartmentBL
    {
        public DepartmentBL(IBaseDL<Department> baseDL) : base(baseDL)
        {
        }
    }
}

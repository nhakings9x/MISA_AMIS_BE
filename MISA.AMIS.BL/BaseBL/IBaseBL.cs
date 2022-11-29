using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.BL
{
    public interface IBaseBL<T>
    {
        /// <summary>
        /// Lấy 1 bản ghi theo ID
        /// </summary>
        /// <param name="recordID">ID của bản ghi</param>
        /// <returns>1 bản ghi</returns>
        public IEnumerable<T> GetAllReCord();
    }
}

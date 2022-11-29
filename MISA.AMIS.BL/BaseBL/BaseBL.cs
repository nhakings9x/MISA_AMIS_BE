using MISA.AMIS.KeToan.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.BL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field
        private IBaseDL<T> _baseDL;
        #endregion

        #region Constructor
        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }
        #endregion

        /// lấy tất cả bản ghi
        /// </summary>
        /// <returns>danh sách tất cả bản ghi</returns>
        /// CreatedBy:DTANH(16/112022)
        public IEnumerable<T> GetAllReCord()
        {
            return _baseDL.GetAllReCord();
        }
    }
}

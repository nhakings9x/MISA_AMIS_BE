using MISA.AMIS.KeToan.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.DL
{
    public interface IBaseDL<T>
    {
        /// <summary>
        /// API lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Author : NHANH (19/11/2022)
        public IEnumerable<T> GetAllReCord();

        /// <summary>
        /// API lấy bản ghi theo ID
        /// </summary>
        /// <returns>Bản ghi theo ID</returns>
        /// Author : NHANH (19/11/2022)
        public T GetRecordByID(Guid recordId);

        /// <summary>
        /// API Xóa bản ghi
        /// </summary>
        /// <returns>Số bản ghi bị xóa</returns>
        /// Author : NHANH (19/11/2022)
        public int DeleteRecord(Guid recordID);

        /// <summary>
        /// API Thêm mới bản ghi
        /// </summary>
        /// <returns>True False và Mã nhân viên đã thêm mới</returns>
        /// Author : NHANH (19/11/2022)
        public ResponseData InsertRecord (T record);
    }
}

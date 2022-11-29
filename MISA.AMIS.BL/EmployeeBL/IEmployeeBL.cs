using MISA.AMIS.KeToan.Common;
using MISA.AMIS.KeToan.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.BL
{
    public interface IEmployeeBL
    {
        /// <summary>
        /// Lấy danh sách tất cả nhân viên
        /// </summary>
        /// <returns>Danh sách tất cả nhân viên</returns>
        /// Create by: NHANH (16/11/2022)
        public IEnumerable<dynamic> GetAllEmployees();

        /// <summary>
        /// Lấy thông tin 1 nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID của nhân viên muốn lấy</param>
        /// <returns>Thông tin của 1 nhân viên muốn lấy</returns>
        /// Create by: NHANH (16/11/2022)
        public Employee GetEmployeesByID(Guid employeeID);

        /// <summary>
        /// API Lấy danh sách nhân viên theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="departmentID"></param>
        /// <param name="positionID"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns>Danh sách nhân vien và tổng số bản ghi</returns>
        public object GetEmployeesByFilterAndPaging(
            string? keyword,
            string? sort,
            int limit = 10,
            int offset = 0
            );

        ///// <summary>
        ///// API Thêm mới 1 nhân viên        
        ///// </summary>
        ///// <param name="employee">Đối tượng nhân viên cần thêm mới</param>
        ///// <returns>ID của nhân viên cần thêm mới</returns>
        ///// Created by: NHAnh (01/11/2022)
        public ResponseData InsertEmployee(Employee employee);

        ///// <summary>
        ///// API sửa thông tin nhân vieen theo ID
        ///// </summary>
        ///// <param name="employeeID">ID nhân viên muốn sửa</param>
        ///// <param name="employee">Đối tượng nhân viên muốn sửa</param>
        ///// <returns>ID nhân viên vừa sửa </returns>
        ///// Created by: NHAnh (01/11/2022)
        public ResponseData UpdateEmployee(Guid employeeID, Employee employee);

        ///// <summary>
        ///// API xóa 1 nhân viên theo ID
        ///// </summary>
        ///// <param name="employeeID">ID nhân viên muốn xóa</param>
        ///// <returns>ID nhân viên vừa xóa</returns>
        ///// Created by: NHAnh (01/11/2022)
        public int DeleteEmployee(Guid employeeID);

        ///// <summary>
        ///// API xóa nhiều nhân viên theo danh sách ID
        ///// </summary>
        ///// <param name="listEmployeeID">Danh sách ID của các nhân viên bị xóa</param>
        ///// <returns>Status code 200</returns>
        ///// Created by: NHAnh (01/11/2022)
        public int DeleteMultipleEmployee(ListEmployeeID listEmployeeID);

        /// <summary>
        /// API lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// Author: NHANH(17/11/2022)
        public string NewEmployeeCode();

        /// <summary>
        /// kiểm tra trùng mã
        /// </summary>
        /// <param name="employeeCode">mã nhân viên</param>
        /// <returns>true, recordID và recordCode</returns>
        /// Author: NHANH(19/11/2022)
        public ResponseData CheckDuplicate(string employeeCode);

        /// <summary>
        /// API xuất khẩu excel
        /// </summary>
        /// <returns>File excel</returns>
        public ResponseData ExportExcel();
    }
}

using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.KeToan.BL;
using MISA.AMIS.KeToan.Common;
using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.Common.Entities.Enums;
using MISA.AMIS.KeToan.DL;
using MySqlConnector;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Data;

namespace MISA.AMIS.KeToan.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        #region Field

        private IEmployeeBL _employeeBL;

        #endregion

        #region Contructor

        public EmployeeController(IEmployeeBL employeeBL)
        {
            _employeeBL = employeeBL;
        }

        #endregion

        #region Methods
        /// <summary>
        /// API lấy danh sách tất cả nhân viên
        /// </summary>
        /// <returns>Danh sách tất cả nhân viên</returns>
        /// Created by: NHAnh (01/11/2022)
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            try
            {
                var employees = _employeeBL.GetAllEmployees();

                // Xử lý kết quả trả về
                if (employees != null)
                {
                    return StatusCode(StatusCodes.Status200OK, employees);
                }

                return StatusCode(StatusCodes.Status200OK, new List<Employee>());


            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                new
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = Resource.DevMsg_Exception,
                    UserMsg = Resource.UserMsg_GetAll_Exception,
                    MoreInfo = Resource.More_Info
                });
            };

            // Try catch exception
        }

        /// <summary>
        /// API lấy nhân viên theo ID
        /// </summary>
        /// <returns></returns>
        /// Created by: NHAnh (01/11/2022)
        [HttpGet("{employeeID}")]
        public IActionResult GetEmployeesByID([FromRoute] Guid employeeID)
        {
            try
            {
                var employee = _employeeBL.GetEmployeesByID(employeeID);

                // Xử lý kết quả trả về
                if (employee != null)
                {
                    return StatusCode(StatusCodes.Status200OK, employee);
                }

                return StatusCode(StatusCodes.Status404NotFound);

            }
            catch (Exception er)
            {
                Console.WriteLine(er);
                return StatusCode(StatusCodes.Status500InternalServerError,
                new
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = Resource.DevMsg_Exception,
                    UserMsg = Resource.UserMsg_GetAll_Exception,
                    MoreInfo = Resource.More_Info
                });
            }

            // Try catch ễcption
        }

        /// <summary>
        /// API Lấy danh sách nhân viên theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="departmentID"></param>
        /// <param name="positionID"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns>Danh sách nhân vien và tổng số bản ghi</returns>
        [HttpGet("filter")]
        public IActionResult GetEmployeesByFilterAndPaging(
            [FromQuery] string? keyword,
            [FromQuery] string? sort,
            [FromQuery] int limit = 10,
            [FromQuery] int offset = 0
            )
        {
            try
            {
                // Khởi tạo kết nối với DB MySQL
                var result = _employeeBL.GetEmployeesByFilterAndPaging(keyword, sort, limit, offset);

                // Thực hiện gọi vào DB

                // Xử lý kết quả trả về

                // Thành công: Trả về dữ liệu cho FE
                if (result != null)
                {
                    return StatusCode(StatusCodes.Status200OK, result);
                }

                return StatusCode(StatusCodes.Status200OK, new List<Employee>());

                // Thất bại: Trả về lỗi
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = Resource.Dev_Msg_Filter,
                    UserMsg = Resource.UserMsg_Filter_Exception,
                    MoreInfo = Resource.More_Info
                });
            }

        }

        ///// <summary>
        ///// API Thêm mới 1 nhân viên        
        ///// </summary>
        ///// <param name="employee">Đối tượng nhân viên cần thêm mới</param>
        ///// <returns>ID của nhân viên cần thêm mới</returns>
        ///// Created by: NHAnh (01/11/2022)
        [HttpPost]
        public IActionResult InsertEmployee([FromBody] Employee employee)
        {
            try
            {
                // Thực hiện gọi vào DB
                ResponseData numberOfRowsAffected = _employeeBL.InsertEmployee(employee);

                // Xử lý kết quả trả về
                if (numberOfRowsAffected.Success)
                {
                    return StatusCode(StatusCodes.Status201Created, numberOfRowsAffected);
                }else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, numberOfRowsAffected.Data);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = Resource.DevMsg_Exception,
                    UserMsg = Resource.UserMsg_Insert_Exception,
                    MoreInfo = Resource.More_Info
                });
            }
        }

        /// <summary>
        /// API sửa thông tin nhân vieen theo ID
        /// </summary>
        /// <param name="employeeID">ID nhân viên muốn sửa</param>
        /// <param name="employee">Đối tượng nhân viên muốn sửa</param>
        /// <returns>ID nhân viên vừa sửa </returns>
        /// Created by: NHAnh (01/11/2022)
        [HttpPut("{employeeID}")]
        public IActionResult UpdateEmployee([FromRoute] Guid employeeID, [FromBody] Employee employee)
        {


            try
            {
                // Thực hiện gọi vào DB
                ResponseData numberOfRowsAffected = _employeeBL.UpdateEmployee(employeeID, employee);
                // Xử lý kết quả trả về

                // Thành công: Trả về dữ liệu cho FE

                // Xử lý kết quả trả về
                if (numberOfRowsAffected.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, numberOfRowsAffected);
                }
                return StatusCode(StatusCodes.Status400BadRequest, numberOfRowsAffected.Data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = Resource.DevMsg_Exception,
                    UserMsg = Resource.UserMsg_Insert_Exception,
                    MoreInfo = Resource.More_Info,
                });
            }
        }

        /// <summary>
        /// API xóa 1 nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID nhân viên muốn xóa</param>
        /// <returns>ID nhân viên vừa xóa</returns>
        /// Created by: NHAnh (01/11/2022)
        [HttpDelete("{employeeID}")]
        public IActionResult DeleteEmployee([FromRoute] Guid employeeID)
        {
            try
            {
                var employees = _employeeBL.DeleteEmployee(employeeID);

                // Xử lý kết quả trả về

                // Thành công: Trả về dữ liệu cho FE
                if (employees != 0)
                {
                    return StatusCode(StatusCodes.Status200OK, employees);
                }

                // Thất bại: Trả về lỗi
                return StatusCode(StatusCodes.Status400BadRequest, new
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = "Delete fail",
                    UserMsg = "Xóa thất bại",
                    MoreInfo = Resource.More_Info
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = Resource.DevMsg_Exception,
                    UserMsg = Resource.UserMsg_Insert_Exception,
                    MoreInfo = Resource.More_Info
                });
            }

        }

        /// <summary>
        /// API xóa nhiều nhân viên theo danh sách ID
        /// </summary>
        /// <param name="listEmployeeID">Danh sách ID của các nhân viên bị xóa</param>
        /// <returns>Status code 200</returns>
        /// Created by: NHAnh (01/11/2022)
        [HttpPost("deleteBatch")]
        public int DeleteMultipleEmployee(ListEmployeeID listEmployeeID)
        {
            var employees = _employeeBL.DeleteMultipleEmployee(listEmployeeID);
            return employees;
        }

        /// <summary>
        /// kiểm tra trùng mã
        /// </summary>
        /// <param name="employeeCode">mã nhân viên</param>
        /// <returns>true, recordID và recordCode</returns>
        /// Author: NHANH(19/11/2022)
        [HttpGet("newEmployeeCode")]
        public IActionResult NewEmployeeCode()
        {
            try
            {
                // Khởi tạo kết nối với DB MySQL
               
                string newCode = _employeeBL.NewEmployeeCode();

                if (newCode != null)
                {
                    return StatusCode(StatusCodes.Status200OK, newCode);
                }
                return StatusCode(StatusCodes.Status200OK, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                new
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = Resource.DevMsg_Exception,
                    UserMsg = Resource.UserMsg_NewCode_Exception,
                    MoreInfo = Resource.More_Info
                });
            }

        }

        /// <summary>
        /// API xuất khẩu excel
        /// </summary>
        /// <returns>File excel</returns>
        [HttpPost("exportExcel")]
        public IActionResult ExportExcel()
        {
            try
            {
                
                var result = _employeeBL.ExportExcel();
                if (result.Success)
                {
                    string excelName = $"Employee-{DateTime.Now.ToString("ddMMyyyyHHmmssfff")}.xlsx";
                    return File((MemoryStream)(result.Data), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, new ErrorResult(
                        AMISErrorCode.Exception,
                        Resource.DevMsg_Exception,
                        Resource.UserMsg_GetAll_Exception,
                        moreInfo: Resource.More_Info,
                        traceId: HttpContext.TraceIdentifier));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResult(
                        AMISErrorCode.Exception,
                        Resource.DevMsg_Exception,
                        Resource.UserMsg_GetAll_Exception,
                        moreInfo: Resource.More_Info,
                        traceId: HttpContext.TraceIdentifier
                        ));
            }
        }

        #endregion


    }
}

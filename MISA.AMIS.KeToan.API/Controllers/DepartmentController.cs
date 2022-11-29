using Microsoft.AspNetCore.Mvc;
using MISA.AMIS.KeToan.BL;
using MISA.AMIS.KeToan.Common;
using MISA.AMIS.KeToan.Common.Entities;

namespace MISA.AMIS.KeToan.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        #region Field

        private IDepartmentBL _IDepartmentBL;

        #endregion

        #region Contructor

        public DepartmentController(IDepartmentBL DepartmentBL)
        {
            _IDepartmentBL = DepartmentBL;
        }
        #endregion

        /// <summary>
        /// API lấy tất cả phòng ban
        /// </summary>
        /// <returns>Danh sách phòng ban</returns>
        /// Author : NHANH (19/11/2022)
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            try
            {
                var departments = _IDepartmentBL.GetAllReCord();

                // Xử lý kết quả trả về
                if (departments != null)
                {
                    return StatusCode(StatusCodes.Status200OK, departments);
                }

                return StatusCode(StatusCodes.Status200OK, new List<String>());


            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                return StatusCode(StatusCodes.Status500InternalServerError,
                new
                {
                    ErrorCode = AMISErrorCode.Exception,
                    DevMsg = Resource.DevMsg_Exception,
                    UserMsg = Resource.UserMsg_Get_Department,
                    MoreInfo = Resource.More_Info,
                });
            };
        }
    }
}

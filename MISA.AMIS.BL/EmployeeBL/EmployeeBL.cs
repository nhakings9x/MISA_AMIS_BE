using MISA.AMIS.KeToan.Common;
using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.Common.Entities.Enums;
using MISA.AMIS.KeToan.DL;
using MISA.AMIS.KeToan.DL;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace MISA.AMIS.KeToan.BL
{
    public class EmployeeBL : IEmployeeBL
    {
        #region Field

        private IEmployeeDL _employeeDL;

        #endregion

        #region Contructor
        public EmployeeBL(IEmployeeDL employeeBL)
        {
            _employeeDL = employeeBL;
        }


        ///// <summary>
        ///// API xóa 1 nhân viên theo ID
        ///// </summary>
        ///// <param name="employeeID">ID nhân viên muốn xóa</param>
        ///// <returns>ID nhân viên vừa xóa</returns>
        ///// Created by: NHAnh (01/11/2022)
        public int DeleteEmployee(Guid employeeID)
        {
            return _employeeDL.DeleteRecord(employeeID);
        }

        ///// <summary>
        ///// API xóa nhiều nhân viên theo danh sách ID
        ///// </summary>
        ///// <param name="listEmployeeID">Danh sách ID của các nhân viên bị xóa</param>
        ///// <returns>Status code 200</returns>
        ///// Created by: NHAnh (01/11/2022)
        public int DeleteMultipleEmployee(ListEmployeeID listEmployeeID)
        {
            return _employeeDL.DeleteMultipleEmployee(listEmployeeID);
        }

        #endregion

        /// <summary>
        /// Lấy danh sách tất cả nhân viên
        /// </summary>
        /// <returns>Danh sách tất cả nhân viên</returns>
        /// Create by: NHANH (16/11/2022)
        public IEnumerable<dynamic> GetAllEmployees()
        {
            return _employeeDL.GetAllReCord();
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
        public object GetEmployeesByFilterAndPaging(string? keyword, string? sort, int limit = 10, int offset = 0)
        {
            return _employeeDL.GetEmployeesByFilterAndPaging(keyword, sort, limit, offset);
        }

        /// <summary>
        /// Lấy thông tin 1 nhân viên theo ID
        /// </summary>
        /// <param name="employeeID">ID của nhân viên muốn lấy</param>
        /// <returns>Thông tin của 1 nhân viên muốn lấy</returns>
        /// Create by: NHANH (16/11/2022)
        public Employee GetEmployeesByID(Guid employeeID)
        {
            return _employeeDL.GetRecordByID(employeeID);
        }

        ///// <summary>
        ///// API Thêm mới 1 nhân viên        
        ///// </summary>
        ///// <param name="employee">Đối tượng nhân viên cần thêm mới</param>
        ///// <returns>ID của nhân viên cần thêm mới</returns>
        ///// Created by: NHAnh (01/11/2022)
        public ResponseData InsertEmployee(Employee employee)
        {
            ResponseData validate = ValidateData(null, employee);

            if (validate.Success)
            {
                return new ResponseData (true, _employeeDL.InsertRecord(employee));

            }
            else
            {
                return validate;
            }

            //Thông báo lỗi ở đây

        }

        /// <summary>
        /// API sửa thông tin nhân vieen theo ID
        /// </summary>
        /// <param name="employeeID">ID nhân viên muốn sửa</param>
        /// <param name="employee">Đối tượng nhân viên muốn sửa</param>
        /// <returns>ID nhân viên vừa sửa </returns>
        /// Created by: NHAnh (01/11/2022)
        public ResponseData UpdateEmployee(Guid employeeID, Employee employee)
        {
            //ResponseData validate = ValidateData(employeeID, employee);

            //if (validate.Success)
            //{
                return _employeeDL.UpdateEmployee(employeeID, employee);
            //}
            //else
            //{
            //    return validate;
            //}
        }

        /// <summary>
        /// Validate dữ liệu
        /// </summary>
        /// <param name="record">nhân viên</param>
        /// <returns>true hoặc false</returns>
        /// Created by: NHAnh (15/11/2022)
        public ResponseData ValidateData(Guid? recordID, Employee record)
        {
            //lấy ra tất cả attribute có attribute là "IsNotNullOrEmptyAttribute"
            var properties = record.GetType().GetProperties();
            //var properties = record.GetType().GetProperties();
            foreach (var prop in properties)
            {
                var propName = prop.Name;
                // lấy giá trị của property truyền lên
                var propValue = prop.GetValue(record);

                var isUniqueCode = Attribute.IsDefined(prop, typeof(UniCodeAttribute));

                // Kiểm tra xem property có attribute là endWithNumber không
                var isEndWithNumber = Attribute.IsDefined(prop, typeof(EndWithNumberAttribute));

                // Kiểm tra xem property có attribute là isNotNullOrEmpty không
                var isNotNullOrEmpty = Attribute.IsDefined(prop, typeof(IsNotNullOrEmptyAttribute));

                // Kiểm tra xem property có attribute là isEmail không
                var isEmail = Attribute.IsDefined(prop, typeof(IsEmailAttribute));

                // Kiểm tra xem property có attribute là BirhOfDate không
                var BirhOfDate = Attribute.IsDefined(prop, typeof(BirhOfDateAttribute));

                // Kiểm tra xem property có attribute là OnlyNumber không
                var isRegexFormat = Attribute.IsDefined(prop, typeof(FormatRegexAttribute));


                if (isUniqueCode == true)
                {
                    //Kiểm tra nếu recordID là null thì gán cho record ID bằng Guid Empty
                    if(recordID == null)
                    {
                        recordID = Guid.Empty;
                    }
                    //lấy ra bản ghi trước khi chỉnh sửa , ép kiểu Guid cho recordID, = null thì là 000-0000-000-000-000
                    var oldRecord = _employeeDL.GetRecordByID((Guid)recordID);
                    bool compareCode = false;
                    // Lấy ra mã trước lúc chỉnh sửa
                    if (oldRecord != null)
                    {
                        var oldRecordCode = oldRecord.GetType().GetProperty(propName).GetValue(oldRecord);
                        compareCode = CompareCode(oldRecordCode.ToString(), propValue.ToString(), 2);

                    }

                    // so sánh 2 mã  ( true nếu mã cũ lớn hơn mã mới)
                    if (compareCode)
                    {
                        return new ResponseData(false, new ErrorResult(
                                AMISErrorCode.DuplicateCode,
                                Resource.DevMsg_Validate,
                                Resource.UserMsg_Code_LessThan,
                                moreInfo: Resource.More_Info));
                    }
                    var data = _employeeDL.CheckDuplicate(propValue.ToString()).Data;
                    var codeData = data.GetType().GetProperty("recordCode").GetValue(data)?.ToString();
                    var attribute = prop.GetCustomAttributes(typeof(UniCodeAttribute), true).FirstOrDefault();
                    var errorMessage = (attribute as UniCodeAttribute).ErrorMessage;
                    // nếu có ID =>  là sửa
                    if (recordID != Guid.Empty)
                    {
                        if (data != null)
                        {

                            // Lấy ra id của bản ghi trong db
                            var idData = data.GetType().GetProperty("recordID")?.GetValue(data)?.ToString();
                            if (idData != recordID.ToString())
                            {
                                return new ResponseData(false, new ErrorResult(
                                AMISErrorCode.DuplicateCode,
                                Resource.DevMsg_Validate,
                                errorMessage,
                                moreInfo: Resource.More_Info
                            ));
                            }
                        }
                    }
                    else
                    {
                        if (data != null)
                        {

                            if (codeData == propValue.ToString())
                            {
                                return new ResponseData(false, new ErrorResult(
                                AMISErrorCode.DuplicateCode,
                                Resource.DevMsg_Validate,
                                errorMessage,
                                moreInfo: Resource.More_Info
                            ));
                            }
                        }
                    }

                    // Kiểm tra nếu không phải sửa thì có trùng với mã trong database không

                    // Nếu là sửa thì mã được phép trùng nhưng ID phải giống nhau. ID khác nhau đưa ra cảnh báo
                }

                if (isNotNullOrEmpty == true)
                {
                    var attribute = prop.GetCustomAttributes(typeof(IsNotNullOrEmptyAttribute), true).FirstOrDefault();
                    var errorMessage = (attribute as IsNotNullOrEmptyAttribute).ErrorMessage;
                    if (propValue == null || propValue.ToString().Trim() == "" || propValue.ToString() == Guid.Empty.ToString())
                    {
                        return new ResponseData(false, new ErrorResult(
                        AMISErrorCode.Validate,
                                Resource.DevMsg_Validate,
                                errorMessage,
                                moreInfo: Resource.More_Info

                        ));
                    }
                }

                if (isEmail == true)
                {
                    var attribute = prop.GetCustomAttributes(typeof(IsEmailAttribute), true).FirstOrDefault();
                    var errorMessage = (attribute as IsEmailAttribute).ErrorMessage;
                    bool checkEmail = IsValidEmail(propValue?.ToString());
                    if (propValue != null && !checkEmail)
                    {
                        return new ResponseData(false, new ErrorResult(
                        AMISErrorCode.Validate,
                                Resource.DevMsg_Validate,
                                errorMessage,
                                moreInfo: Resource.More_Info
                        ));
                    }
                }

                if (BirhOfDate == true)
                {
                    var attribute = prop.GetCustomAttributes(typeof(BirhOfDateAttribute), true).FirstOrDefault();
                    var errorMessage = (attribute as BirhOfDateAttribute).ErrorMessage;
                    if (propValue != null && !IsValidDate(propValue.ToString()))
                    {
                        return new ResponseData(false, new ErrorResult(
                        AMISErrorCode.Validate,
                                Resource.DevMsg_Validate,
                                errorMessage,
                                moreInfo: Resource.More_Info
                        ));
                    }
                }

                if (isRegexFormat == true)
                {
                    //lấy ra attribute
                    var attribute = prop.GetCustomAttributes(typeof(FormatRegexAttribute), true).FirstOrDefault();

                    // lấy ra regex 
                    var regex = new Regex((attribute as FormatRegexAttribute).Format);

                    var errorMessage = (attribute as FormatRegexAttribute).ErrorMessage;

                    if (propValue != null && !regex.IsMatch(propValue.ToString()))
                    {
                        return new ResponseData(false, new ErrorResult(
                                AMISErrorCode.Validate,
                                Resource.DevMsg_Validate,
                                errorMessage,
                                moreInfo: Resource.More_Info
                        ));
                    }
                }
            }
            return new ResponseData(true, null);
        }

        /// <summary>
        /// validate Email
        /// </summary>
        /// <param name="email">giá trị email truyền vào</param>
        /// <returns>true nếu là email, false nếu không phải email</returns>
        private static bool IsValidEmail(string email)
        {
            if (email == null) return false;
            if (email.Trim().EndsWith("."))
            {
                return false;
            }
            try
            {
                MailAddress mail = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
                throw;
            }
        }

        /// <summary>
        /// validate ngày sinh
        /// </summary>
        /// <param name="date">Ngày sinh</param>
        /// <returns>true: nếu nhỏ hơn ngày hiện tại và tuổi bé hơn 200 ngược lại false </returns>
        public static bool IsValidDate(string date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                if (dt > DateTime.Now)
                {
                    return false;
                }
                else if (dt.Year < 1900)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// So sánh 2 mã
        /// </summary>
        /// <param name="oldCode"></param>
        /// <param name="newCode"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool CompareCode(string oldCode, string newCode, int index)
        {
            int oldCodeSub = int.Parse(oldCode.Substring(index));
            int newCodeSub = int.Parse(newCode.Substring(index));
            return oldCodeSub > newCodeSub;
        }

        /// <summary>
        /// API lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// Author: NHANH(17/11/2022)
        public string NewEmployeeCode()
        {
            return _employeeDL.NewEmployeeCode();
        }

        /// <summary>
        /// kiểm tra trùng mã
        /// </summary>
        /// <param name="employeeCode">mã nhân viên</param>
        /// <returns>true, recordID và recordCode</returns>
        /// Author: NHANH(19/11/2022)
        public ResponseData CheckDuplicate(string employeeCode)
        {
            return _employeeDL.CheckDuplicate(employeeCode);
        }

        /// <summary>
        /// API xuất khẩu excel
        /// </summary>
        /// <returns>File excel</returns>
        public ResponseData ExportExcel()
        {

            var result = GetAllEmployees();
            if (result == null || result.Count() == 0)
            {
                return new ResponseData(false, new ErrorResult(
                        AMISErrorCode.Exception,
                        Resource.DevMsg_Exception,
                        Resource.UserMsg_GetAll_Exception,
                        moreInfo: Resource.More_Info
                        )
            );
            }
                var stream = new MemoryStream();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    // Danh sách cột
                    string listColName = "ABCDEFGHI";
                    // Title của bảng
                    var workSheet = package.Workbook.Worksheets.Add("DANH SÁCH NHÂN VIÊN");

                    #region Style cho các ô header
                    workSheet.Cells.Style.Font.SetFromFont("Times New Roman", 11);
                    workSheet.Cells["A1:I1"].Merge = true;
                    workSheet.Cells["A2:I2"].Merge = true;
                    workSheet.Cells["A1"].Value = "Danh sách nhân viên";
                    workSheet.Cells["A1"].Style.Font.SetFromFont("Arial", 16, true);
                    workSheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells["A1"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    workSheet.Cells["A3:I3"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    workSheet.Cells["A3:I3"].Style.Fill.BackgroundColor.SetColor(OfficeOpenXml.Drawing.eThemeSchemeColor.Accent3);
                    workSheet.Cells["A3:I3"].Style.Font.SetFromFont("Arial", 10, true);
                    workSheet.Cells["A3:I3"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    workSheet.Cells["A3:I3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion

                    #region Đặt chiều rộng các cột 
                    workSheet.Column(1).Width = 5;
                    workSheet.Column(2).Width = 15;
                    workSheet.Column(3).Width = 26;
                    workSheet.Column(4).Width = 12;
                    workSheet.Column(5).Width = 15;
                    workSheet.Column(6).Width = 26;
                    workSheet.Column(7).Width = 26;
                    workSheet.Column(8).Width = 16;
                    workSheet.Column(9).Width = 26;
                    #endregion

                    #region Header các cột
                    workSheet.Cells["A3"].Value = "STT";
                    workSheet.Cells["B3"].Value = "Mã nhân viên";
                    workSheet.Cells["C3"].Value = "Tên Nhân viên";
                    workSheet.Cells["D3"].Value = "Giới tính";
                    workSheet.Cells["E3"].Value = "Ngày sinh";
                    workSheet.Cells["F3"].Value = "Chức Danh";
                    workSheet.Cells["G3"].Value = "Tên đơn vị";
                    workSheet.Cells["H3"].Value = "Số tài khoản";
                    workSheet.Cells["I3"].Value = "Tên ngân hàng";
                    workSheet.Cells["A3:I3"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    #endregion

                    int rowStart = 3;
                    foreach (var text in listColName)
                    {
                        workSheet.Cells[$"{text}{rowStart}"].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        workSheet.Cells[$"{text}{rowStart}"].Style.WrapText = true;
                    }
                    foreach (var val in result.Select((value, i) => new { i, value }))
                    {
                        for (int col = 1; col <= 9; col++)
                        {
                            workSheet.Cells[val.i + 1 + rowStart, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                            workSheet.Cells[val.i + 1 + rowStart, col].Style.WrapText = true;
                            if (col == 5)
                            {
                                workSheet.Cells[val.i + 1 + rowStart, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            }
                        }

                        workSheet.Cells[val.i + 1 + rowStart, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                        workSheet.Cells[val.i + 1 + rowStart, 1].Value = val.i + 1;
                        workSheet.Cells[val.i + 1 + rowStart, 2].Value = val.value.EmployeeCode.ToString();
                        workSheet.Cells[val.i + 1 + rowStart, 3].Value = val.value.EmployeeName.ToString();
                        workSheet.Cells[val.i + 1 + rowStart, 4].Value = val.value.Gender == Gender.Male ? Resource.Gender_Male : val.value.Gender == Gender.Female ? Resource.Gender_Female : Resource.Gender_Other;
                        workSheet.Cells[val.i + 1 + rowStart, 5].Value = val.value.DateofBirth?.Year.ToString() == "0001" ? "" : val.value.DateofBirth?.ToString("dd/MM/yyyy");
                        workSheet.Cells[val.i + 1 + rowStart, 6].Value = val.value.JobPositionName == null ? "" : val.value.JobPositionName.ToString();
                        workSheet.Cells[val.i + 1 + rowStart, 7].Value = val.value.DepartmentName == null ? "" : val.value.DepartmentName.ToString();
                        workSheet.Cells[val.i + 1 + rowStart, 8].Value = val.value.BankNumber == null ? "" : val.value.BankNumber.ToString();
                        workSheet.Cells[val.i + 1 + rowStart, 9].Value = val.value.BankName == null ? "" : val.value.BankName.ToString();

                    }
                    package.SaveAs(stream);
                }
                stream.Position = 0;
                return new ResponseData(true, stream);
            
        }
    }
}

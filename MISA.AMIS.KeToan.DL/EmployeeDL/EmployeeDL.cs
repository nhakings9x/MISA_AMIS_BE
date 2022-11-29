using Dapper;
using MISA.AMIS.KeToan.Common;
using MISA.AMIS.KeToan.Common.Entities;
using MISA.AMIS.KeToan.DL;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.DL
{
    public class EmployeeDL : BaseDL<Employee>, IEmployeeDL
    {

        /// <summary>
        /// API Lấy danh sách nhân viên theo bộ lọc và phân trang
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="departmentID"></param>
        /// <param name="positionID"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns>Danh sách nhân vien và tổng số bản ghi</returns>
        public object GetEmployeesByFilterAndPaging(string? keyword, string? sort, int limit = 10, int offset = 1)
        {
            // Khởi tạo kết nối với DB MySQL
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            // Chuẩn bị câu lệnh SQL
            string sqlCommand = "Proc_employee_filter";


            // Chuẩn bị tham số đầu vào
            var parammeters = new DynamicParameters();
            parammeters.Add("@keyword", keyword);
            parammeters.Add("@limit", limit);
            parammeters.Add("@offset", (offset - 1) * limit);
            parammeters.Add("@sort", sort);

            var multipleResults = mySqlConnection.QueryMultiple(sqlCommand, parammeters, commandType: System.Data.CommandType.StoredProcedure);

            var result = new PaggingResult();
            if (multipleResults != null)
            {
                var TotalPage = 0;
                var employees = multipleResults.Read<Employee>().ToList();
                var totalRecord = multipleResults.Read<int>().Single();
                if (totalRecord == 0)
                {
                    TotalPage = 1;
                }
                else
                {
                    TotalPage = (int)Math.Ceiling((double)totalRecord / (double)limit);
                }

                result.TotalPage = TotalPage;
                result.Data = employees;
                result.TotalCount = totalRecord;

            }
            return result;
        }

        ///// <summary>
        ///// API sửa thông tin nhân vieen theo ID
        ///// </summary>
        ///// <param name="employeeID">ID nhân viên muốn sửa</param>
        ///// <param name="employee">Đối tượng nhân viên muốn sửa</param>
        ///// <returns>ID nhân viên vừa sửa </returns>
        ///// Created by: NHAnh (01/11/2022)
        public ResponseData UpdateEmployee(Guid employeeID, Employee employee)
        {
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            // Chuẩn bị câu lệnh SQL
            string storedProcedureName = "Proc_employee_Update";

            // Chuẩn bị tham số đầu vào
            //var parammeters = new DynamicParameters();

            //parammeters.Add("@EmployeeID", employeeID);
            //parammeters.Add("@EmployeeCode", employee.EmployeeCode);
            //parammeters.Add("@EmployeeName", employee.EmployeeName);
            //parammeters.Add("@DateofBirth", employee.DateofBirth);
            //parammeters.Add("@Gender", employee.Gender);
            //parammeters.Add("@IdentityNumber", employee.IdentityNumber);
            //parammeters.Add("@IdentityDate", employee.IdentityDate);
            //parammeters.Add("@IdentityPlace", employee.IdentityPlace);
            //parammeters.Add("@JobPositionID", employee.JobPositionName);
            //parammeters.Add("@Salary", employee.Salary);
            //parammeters.Add("@CreatedDate", employee.CreatedDate);
            //parammeters.Add("@CreatedBy", employee.CreatedBy);
            //parammeters.Add("@DepartmentID", employee.DepartmentID);
            //parammeters.Add("@Adress", employee.Adress);
            //parammeters.Add("@TelephoneNumber", employee.TelephoneNumber);
            //parammeters.Add("@PhoneNumber", employee.PhoneNumber);
            //parammeters.Add("@Email", employee.Email);
            //parammeters.Add("@BankNumber", employee.BankNumber);
            //parammeters.Add("@BankName", employee.BankName);
            //parammeters.Add("@BankBranch", employee.BankBranch);
            //parammeters.Add("@ModifiedBy", employee.ModifiedBy);

            // Thực hiện gọi vào DB


            int numberOfRowsAffected = mySqlConnection.Execute(storedProcedureName, employee, commandType: System.Data.CommandType.StoredProcedure);
            // Xử lý kết quả trả về

            // Thành công: Trả về dữ liệu cho FE

            // Xử lý kết quả trả về
            if(numberOfRowsAffected > 0)
            {
                    return new ResponseData(true, numberOfRowsAffected);
            }
            return new ResponseData(false, numberOfRowsAffected);
        }

        ///// <summary>
        ///// API xóa nhiều nhân viên theo danh sách ID
        ///// </summary>
        ///// <param name="listEmployeeID">Danh sách ID của các nhân viên bị xóa</param>
        ///// <returns>Status code 200</returns>
        ///// Created by: NHAnh (01/11/2022)
        public int DeleteMultipleEmployee(ListEmployeeID listEmployeeID)
        {
            var listRemove = "";
            // Khời tạo kết nối tới DB MySQL
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);
            MySqlTransaction transaction = null;

            var storedProcedureName = "Proc_employee_DeleteMultiple";

            var ids = listEmployeeID.EmployeeIDs.Select(x => $"'{x}'").ToList();
            listRemove = string.Join(",", ids);



            //Chuẩn bị câu lệnh SQL
            var parammeters = new DynamicParameters();
            parammeters.Add("listId", listRemove);
            parammeters.Add("countRows", dbType: DbType.Int32, direction: ParameterDirection.Output);
            //string sql = $" DELETE FROM employee WHERE EmployeeID IN ('{str}');";

            mySqlConnection.Open();
            int numberOfRowsAffected = 0;
            try
            {
                transaction = mySqlConnection.BeginTransaction();
                //Thực hiện gọi vào DB
                numberOfRowsAffected = mySqlConnection.Execute(storedProcedureName, parammeters, transaction: transaction, commandType: System.Data.CommandType.StoredProcedure);
                int countRows = parammeters.Get<int>("countRows");

                if (countRows == listEmployeeID.EmployeeIDs.Count)
                {
                    transaction.Commit();
                    return countRows;
                }
                else
                {
                    transaction.Rollback();
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            finally
            {
                mySqlConnection.Close();
            }
            return numberOfRowsAffected;
        }

        /// <summary>
        /// API lấy mã nhân viên mới
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        /// Author: NHANH(17/11/2022)
        public string NewEmployeeCode()
        {
            // Khởi tạo kết nối với DB MySQL
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            // Chuẩn bị câu lệnh SQL
            string sqlCommand = "Proc_employee_getMaxCode";
            var parammeters = new DynamicParameters();
            parammeters.Add("out_number", dbType: DbType.String, direction: ParameterDirection.Output);

            // Chuẩn bị tham số đầu vào
            mySqlConnection.QueryFirstOrDefault(sqlCommand, parammeters, commandType: System.Data.CommandType.StoredProcedure);
            string newCode = parammeters.Get<string>("out_number");

            if (newCode != null)
            {
                return newCode;
            }
            return null;
        }

        /// <summary>
        /// kiểm tra trùng mã
        /// </summary>
        /// <param name="employeeCode">mã nhân viên</param>
        /// <returns>true, recordID và recordCode</returns>
        /// Author: NHANH(19/11/2022)
        public ResponseData CheckDuplicate(string employeeCode)
        {
            //lấy chuỗi kết nối
            var connectionString = DatabaseContext.ConnectionString;

            //lấy tên Procedure 
            string sqlCommand = "Proc_employee_FindByCode";

            var parameters = new DynamicParameters();
            parameters.Add("recordCode", employeeCode);

            parameters.Add("empID", dbType: DbType.String, direction: ParameterDirection.InputOutput);
            parameters.Add("empCode", dbType: DbType.String, direction: ParameterDirection.InputOutput);

            using (var mySqlConnection = new MySqlConnection(connectionString))
            {
                var multipleResults = mySqlConnection.Query(sqlCommand, parameters, commandType: CommandType.StoredProcedure);
                Guid? recordID = parameters.Get<Guid?>("empID");
                string? recordCode = parameters.Get<string?>("empCode");
                return new ResponseData(true, new
                {
                    recordID = recordID,
                    recordCode = recordCode,
                });
            }
        }
    }
}

using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using MISA.AMIS.KeToan.Common.Entities;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using MISA.AMIS.KeToan.Common;

namespace MISA.AMIS.KeToan.DL
{
    public class BaseDL<T> : IBaseDL<T>
    {
        /// <summary>
        /// API lấy tất cả bản ghi
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// Author : NHANH (19/11/2022)
        public IEnumerable<T> GetAllReCord()
        {
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);
            if(mySqlConnection.State != ConnectionState.Open)
            {
                mySqlConnection.Open();
            }
            // Chuẩn bị câu lệnh SQL
            string sqlCommand = String.Format("Proc_{0}_GetAll",typeof(T).Name);


            // Chuẩn bị tham số đầu vào
            var record = mySqlConnection.Query<T>(sqlCommand, commandType: System.Data.CommandType.StoredProcedure);

            // Xử lý kết quả trả về
            if (record != null)
            {
                return record;
            }
            return new List<T>();
        }

        /// <summary>
        /// API lấy bản ghi theo ID
        /// </summary>
        /// <returns>Bản ghi theo ID</returns>
        /// Author : NHANH (19/11/2022)
        public T GetRecordByID(Guid recordId)
        {
            // Khởi tạo kết nối với DB MySQL
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            // Chuẩn bị câu lệnh SQL
            string storedProcedureName = String.Format("Proc_{0}_Get{0}ByID", typeof(T).Name);

            // Chuẩn bị tham số đầu vào
            var parammeters = new DynamicParameters();
            parammeters.Add($"@{typeof(T).Name}ID", recordId);

            // Thực hiện gọi vào DB
            var employee = mySqlConnection.QueryFirstOrDefault<T>(storedProcedureName, parammeters, commandType: System.Data.CommandType.StoredProcedure);

            // Xử lý kết quả trả về
            return employee;
        }

        /// <summary>
        /// API Xóa bản ghi
        /// </summary>
        /// <returns>Số bản ghi bị xóa</returns>
        /// Author : NHANH (19/11/2022)
        public int DeleteRecord(Guid recordID)
        {
            // Khởi tạo kết nối với DB MySQL
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);

            // Chuẩn bị câu lệnh SQL
            string sqlCommand = String.Format("Proc_{0}_Delete{0}", typeof(T).Name);

            // Chuẩn bị tham số đầu vào
            var parammeters = new DynamicParameters();
            parammeters.Add($"@{typeof(T).Name}ID", recordID);

            // Thực hiện gọi vào DB
            var records = mySqlConnection.Execute(sqlCommand, parammeters, commandType: System.Data.CommandType.StoredProcedure);

            // Xử lý kết quả trả về

            // Thành công: Trả về dữ liệu cho FE
            return records;
        }

        /// <summary>
        /// API Thêm mới bản ghi
        /// </summary>
        /// <returns>True False và Mã nhân viên đã thêm mới</returns>
        /// Author : NHANH (19/11/2022)
        public ResponseData InsertRecord(T record)
        {
            // Khởi tạo kết nối
            var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString);
            // Khai báo Proc
            string sqlCommand = String.Format("Proc_{0}_Insert", typeof(T).Name);
            // Lấy ra mảng tất cả Properties của  T
            var properties = record.GetType().GetProperties();

            //Khai báo biến cho proc
            var parameters = new DynamicParameters();

            //Tạo  1 id mới
            var newID = Guid.NewGuid();

            // lặp qua mảng properties 
            foreach(var property in properties)
            {
                // đặt biến kiểm tra attribute key
                var isPrimaryKey = Attribute.IsDefined(property, typeof(KeyAttribute));

                // Nếu có Key Attribute thì gán bằng newID, không có thì gán bằng property value
                if (isPrimaryKey)
                {
                    parameters.Add($"@{property.Name}", newID);
                }else
                {
                // Lấy tên và đặt param cho proc
                parameters.Add($"@{property.Name}", property.GetValue(record));
                }
            }
            // Thực hiện gọi vào DB
            var records = mySqlConnection.Execute(sqlCommand, parameters, commandType: System.Data.CommandType.StoredProcedure);

            // Trả kết quả về
            if (records > 0)
            {
                return new ResponseData(true, newID);
            }
            return new ResponseData(false, Guid.Empty);
        }

    }
}

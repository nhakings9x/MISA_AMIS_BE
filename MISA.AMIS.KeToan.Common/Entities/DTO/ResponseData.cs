using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.Common
{
    public class ResponseData
    {
        /// <summary>
        /// Tráng thái kết quả trả về
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Dữ liệu trả về
        /// </summary>
        public object? Data { get; set; }

        /// <summary>
        /// Data trả về 
        /// </summary>
        /// <param name="success">Tráng thái kết quả trả về</param>
        /// <param name="data">Dữ liệu trả về</param>
        public ResponseData(bool success, object? data)
        {
            Success = success;
            Data = data;
        }
    }
}

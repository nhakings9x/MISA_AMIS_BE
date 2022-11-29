using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AMIS.KeToan.Common
{
    public class ErrorResult
    {
        #region Contructor
        public ErrorResult()
        {

        }

        /// <summary>
        /// Lỗi trả về khi gọi API lỗi
        /// </summary>
        /// <param name="errorCode">Mã lỗi</param>
        /// <param name="devMsg">Lỗi trả về cho dev</param>
        /// <param name="userMsg">Lỗi trả về cho người dùng</param>
        /// <param name="moreInfo">Thông tin liên lạc để sửa lỗi</param>
        /// Author: NHANH (19/11/2022)
        public ErrorResult(AMISErrorCode errorCode, string? devMsg, string? userMsg, string? moreInfo)
        {
            ErrorCode = errorCode;
            DevMsg = devMsg;
            UserMsg = userMsg;
            MoreInfo = moreInfo;
        }

        /// <summary>
        /// Lỗi trả về khi gọi API lỗi
        /// </summary>
        /// <param name="errorCode">Mã lỗi</param>
        /// <param name="devMsg">Lỗi trả về cho dev</param>
        /// <param name="userMsg">Lỗi trả về cho người dùng</param>
        /// <param name="moreInfo">Thông tin liên lạc để sửa lỗi</param>
        /// <param name="traceId">Mã sinh ra khi gặp lỗi</param>
        /// Author: NHANH (19/11/2022)
        public ErrorResult(AMISErrorCode errorCode, string? devMsg, string? userMsg, string? moreInfo, string? traceId)
        {
            ErrorCode = errorCode;
            DevMsg = devMsg;
            UserMsg = userMsg;
            MoreInfo = moreInfo;
            TraceId = traceId;
        }
        #endregion

        #region property

        /// <summary>
        /// Mã lỗi
        /// Author: NHANH(22/11/2022)
        /// </summary>
        public AMISErrorCode ErrorCode { get; set; }

        /// <summary>
        /// Phải hồi cho dev
        /// Author: NHANH(22/11/2022)
        /// </summary>
        public string? DevMsg { get; set; }

        /// <summary>
        /// Phải hồi cho user
        /// Author: NHANH(22/11/2022)
        /// </summary>
        public string? UserMsg { get; set; }

        /// <summary>
        /// Thông tin thêm
        /// Author: NHANH(22/11/2022)
        /// </summary>
        public string? MoreInfo { get; set; }

        /// <summary>
        /// ID của lỗi
        /// Author: NHANH(22/11/2022)
        /// </summary>
        public string? TraceId { get; set; }
        #endregion

    }
}

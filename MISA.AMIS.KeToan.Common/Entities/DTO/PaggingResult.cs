namespace MISA.AMIS.KeToan.Common.Entities
{

    /// <summary>
    /// Kết quả trả về của APi lấy danh sách nhân viên
    /// </summary>
    public class PaggingResult
    {
        /// <summary>
        /// Tổng số bản ghi
        /// </summary>
        public long TotalCount { get; set; }

        /// <summary>
        /// Tổng số trang
        /// </summary>
        public long TotalPage { get; set; }

        /// <summary>
        /// DS nhân viên
        /// </summary>
        public List<Employee> Data { get; set; }

    }
}

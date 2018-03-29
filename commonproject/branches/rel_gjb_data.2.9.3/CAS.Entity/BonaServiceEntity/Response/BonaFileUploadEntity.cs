namespace CAS.Entity.BonaServiceEntity.Response
{
    /// <summary>
    /// 上传bona服务器返回对象
    /// </summary>
    public class BonaFileUploadEntity
    {
        public string filesize { get; set; }
        public string infilename { get; set; }

        /// <summary>
        /// 文件相对路径
        /// </summary>
        public string outfilename { get; set; }
    }
}

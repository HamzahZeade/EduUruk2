namespace EduUruk.Models.ViewModels
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ApiUserMobile { get; set; }
        public string ApiPassMobile { get; set; }

        public string ApiUserHOffice { get; set; }
        public string ApiPassHoffice { get; set; }


        public int BlobGuidLength { get; set; }

        public int DEFAULT_IMAGE_WIDTH { get; set; }
        public int DEFAULT_IMAGE_HEIGHT { get; set; }
    }
}

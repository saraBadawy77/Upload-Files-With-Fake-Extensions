namespace Upload_Files_With_Fake_Extensions.Model
{
    public class UploadFile
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? StoredFileName { get; set; }
        public string? ContentType { get; set; }
    }
}

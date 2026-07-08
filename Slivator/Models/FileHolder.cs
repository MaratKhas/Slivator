namespace Slivator.Models
{
    //public class FileHolder
    //{
    //    public Guid IdGuid { get; set; }

    //    public string FileName { get; set; }

    //    private string _directory => "UserData/Files";

    //    public string FilePath()
    //    {
    //        return Path.Combine(_directory, FileName);
    //    }
    //}
    public class FileHolder
    {
        public Guid Id { get; set; }
        public string OriginalFileName { get; set; }
        public string SavedFileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public string ContentType { get; set; }
        public DateTime UploadDate { get; set; }

    }
}

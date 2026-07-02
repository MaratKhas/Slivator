namespace Slivator.Models
{
    public class FileHolder
    {
        public Guid IdGuid { get; set; }

        public string FileName { get; set; }

        private string _directory => "TemFile";

        public string FilePath()
        {
            return Path.Combine(_directory, FileName);
        }
    }
}

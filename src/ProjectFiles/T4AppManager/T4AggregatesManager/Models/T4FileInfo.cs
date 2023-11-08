using System.Diagnostics;

namespace T4AggregatesManager.Models
{
    public class T4FileInfo
    {
        public T4FileInfo(string aggName, string path)
        {
            AggName = aggName;
            Path = string.Join(@"\", path.Split(@"\").SkipLast(2));
        }

        public string Path { get; set; }
        public string ShortPath => Path.Substring(Path.IndexOf("src"));
        public string AggName { get; set; }
        public bool Loading { get; set; }
        public Process Process { get; set; }
        
        public bool IsClicked { get; set; }
    }
}

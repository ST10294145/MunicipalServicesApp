namespace MunicipalServicesApp
{
    public class Issue
    {
        public int IssueID { get; set; }   // ✅ use this instead of Id
        public string Title { get; set; }
        public string Description { get; set; }
        public string Reporter { get; set; }
        public string Email { get; set; }
        public string Province { get; set; }
        public string Category { get; set; }
        public string FilePath { get; set; }
        public string Feedback { get; set; }
        public string Status { get; set; }
        public DateTime DateReported { get; set; }
        public DateTime SLADeadline { get; set; }
    }
}

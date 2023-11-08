namespace ToolSvcData.Models.Interfaces
{
    public interface ITSvcCondModel
    {
        string? Comment { get; set; }
        int Condition { get; set; }
        string? EnteredBy { get; set; }
        DateTime? EnteredDate { get; set; }
        string? LastChgBy { get; set; }
        DateTime? LastChgDate { get; set; }
        string? PartName { get; set; }
        int ToolPartId { get; set; }
        int ToolSvcId { get; set; }
        int TSvcCondId { get; set; }
    }
}
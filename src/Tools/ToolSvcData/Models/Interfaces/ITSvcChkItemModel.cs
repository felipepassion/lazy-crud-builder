namespace ToolSvcData.Models.Interfaces
{
    public interface ITSvcChkItemModel
    {
        int CheckItemId { get; set; }
        string? EnteredBy { get; set; }
        DateTime? EnteredDate { get; set; }
        string? LastChgBy { get; set; }
        DateTime? LastChgDate { get; set; }
        int Answer { get; set; }
        int ToolSvcId { get; set; }
        int TSvcChkItemId { get; set; }
    }
}
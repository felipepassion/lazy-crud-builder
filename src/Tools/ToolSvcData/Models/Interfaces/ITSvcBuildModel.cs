namespace ToolSvcData.Models.Interfaces
{
    public interface ITSvcBuildModel
    {
        string? ActualWO { get; set; }
        int BuildLineId { get; set; }
        string? BuildWO { get; set; }
        int ComponentId { get; set; }
        string? EnteredBy { get; set; }
        DateTime? EnteredDate { get; set; }
        string? LastChgBy { get; set; }
        DateTime? LastChgDate { get; set; }
        string? PartNum { get; set; }
        int ToolPartId { get; set; }
        int ToolSvcId { get; set; }
        int TSvcBuildId { get; set; }
    }
}
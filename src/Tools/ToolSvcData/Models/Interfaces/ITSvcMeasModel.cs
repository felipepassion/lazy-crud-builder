namespace ToolSvcData.Models.Interfaces
{
    public interface ITSvcMeasModel
    {
        decimal DecVal { get; set; }
        string? EnteredBy { get; set; }
        DateTime? EnteredDate { get; set; }
        int IntVal { get; set; }
        string? LastChgBy { get; set; }
        DateTime? LastChgDate { get; set; }
        int MeasurementId { get; set; }
        int ToolSvcId { get; set; }
        int TSvcMeasId { get; set; }
    }
}
namespace Niu.Nutri.Core.Application.DTO.Seedwork.ValueObjects
{
    public class AutoSaveSettingsDTO
    {
        public bool Enabled { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public void AddError(params string[] errors)
        {
            Errors.AddRange(errors);
        }

        public event Action OnUpdateEvent;
        public void OnUpdate() => OnUpdateEvent?.Invoke();

        public Func<Task> TriggerAutoSave = null!;
    }
}

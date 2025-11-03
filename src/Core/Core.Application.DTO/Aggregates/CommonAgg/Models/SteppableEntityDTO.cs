using System.Reflection;

namespace Lazy.Crud.Core.Application.DTO.Aggregates.CommonAgg.Models;

public interface ISteppableEntityDTO : IActivableEntityDTO
{
    int CurrentStep { get; set; }
    public int MaxSteps { get; set; }
    void ChangeStep(int newStep);
    bool RegisterDone { get; set; }
    bool PushStepToModelStep { get; set; }
}

public abstract class SteppableEntityDTO : ActivableEntityDTO, ISteppableEntityDTO
{
    public int CurrentStep { get; set; }

    int? _maxSteps;
    public bool RegisterDone { get; set; }

    public int MaxSteps
    {
        get
        {
            if (_maxSteps.HasValue)
                return _maxSteps.Value;

            _maxSteps = GetMaxSetpsCount();

            return _maxSteps.Value;
        }
        set
        {
            _maxSteps = value;
        }
    }

    public bool PushStepToModelStep { get; set; }

    public void ChangeStep(int newStep)
    {
        this.CurrentStep = newStep;
        if (!this.RegisterDone && this.IsRegisterDone()) { this.RegisterDone = true; }
    }

    public int GetMaxSetpsCount()
    {
        var myName = $"{this.GetMyTypeName()}";
     
        var _maxSteps = Assembly.GetEntryAssembly()
            .GetTypes()
            .Count(t => t.Name[..^1] == myName);

        return _maxSteps == 0 ? 1 : _maxSteps;
    }

    public bool IsRegisterDone(int? maxSteps = null) => CurrentStep >= (maxSteps ?? this.GetMaxSetpsCount());
}

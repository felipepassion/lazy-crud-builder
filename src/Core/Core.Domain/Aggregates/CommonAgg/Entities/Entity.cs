using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using LazyCrud.Core.Application.DTO.Attributes;
using LazyCrud.Core.Application.DTO.Extensions;
using LazyCrud.Core.Domain.Aggregates.CommonAgg.Events;
using LazyCrud.Core.Domain.Attributes.T4;
using LazyCrud.Core.Domain.Attributes.T4;

namespace LazyCrud.Core.Domain.Aggregates.CommonAgg.Entities
{
    public abstract class SteppableEntity : ActivableEntity
    {   
        public void ChangeStep(int newStep)
        {
            this.CurrentStep = newStep;
            if (!this.Active.HasValue && this.CurrentStep >= this.MaxSteps)
                this.Active = true;
        }
        public int CurrentStep { get; set; }
        public int? MaxSteps => this.GetType().GetCustomAttribute<Steppable>()?.Quantity;
        public bool? RegisterDone { get; set; }
    }

    public abstract class ActivableEntity : Entity
    {
        [DisplayName("Habilitado?")]
        public bool? Active { get; set; } = true;

        public void Enable()
        {

        }

        public void Disable()
        {
        }
    }

    public interface IEntity
    {
        public int Id { get; set; }
        public string ExternalId { get; set; }
        string GetTitle();
        string GetTitlePropName();
    }

    public class Entity : IEntity
    {
        [NotMapped]
        [IgnorePropertyT4]
        private List<BaseEvent> _domainEvents;

        public Entity()
        {
            CreatedAt = CreatedAt ?? DateTime.UtcNow;
            if (_externalId == null) _externalId = Guid.NewGuid().ToString();
        }

        [IgnorePropertyT4OnRequest]
        [DisplayName("Criado em")]
        public DateTime? CreatedAt { get; set; }

        [IgnorePropertyT4OnRequest]
        [DisplayName("Atualizado em")]
        public DateTime? UpdatedAt { get; set; }

        [IgnorePropertyT4OnRequest]
        [DisplayName("Deletado em")]
        public DateTime? DeletedAt { get; set; }

        protected virtual void Updated()
        {
            this.UpdatedAt = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string GetTitle()
        {
            var val = this.GetValueWithAttribute<Title>()?.ToString();
            if (string.IsNullOrWhiteSpace(val))
            {
                val = this.GetType().GetCustomAttribute<H1>()?.Title ?? null;
            }
            return val;
        }

        public string GetTitlePropName()
        {
            var property = this.GetFieldInfoByWithAttribute<Title>();
            var test = property?.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? property?.Name ?? "Título";
            return property?.Name;
        }

        string _externalId;
        public string ExternalId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_externalId))
                    _externalId = Guid.NewGuid().ToString();
                return _externalId;
            }
            set { this._externalId = value; }
        }

        [IgnorePropertyT4OnRequest]
        public bool IsDeleted { get; set; }

        [NotMapped]
        [IgnorePropertyT4]
        public List<BaseEvent> DomainEvents { get { return _domainEvents; } set { _domainEvents = _domainEvents ?? value; } }

        //[Column(TypeName = "jsonb"), NotMapped]
        //public List<EventsHistory>? EventsHistory { get; set; }
        public void Delete()
        {
            this.IsDeleted = true;
            this.DeletedAt = DateTime.UtcNow;
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public void AddDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents = _domainEvents ?? new List<BaseEvent>();
            //_domainEvents.Add(domainEvent);
        }

        public bool IsEmpty()
        {
            return this.IsEmpty();
        }

        public override bool Equals(object? obj)
        {
            if (obj is not IEntity) return false;

            return ((Entity)obj)?.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return this.Id;
        }
    }
}

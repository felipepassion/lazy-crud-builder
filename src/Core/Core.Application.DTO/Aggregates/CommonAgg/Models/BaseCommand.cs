using Lazy.Crud.Core.Domain.CrossCutting;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lazy.Crud.Core.Application.DTO.Aggregates.CommonAgg.Models
{
    public class BaseCommand : IRequest<DomainResponse>
    {
        public string LoggedUserId { get; set; }
    }
}

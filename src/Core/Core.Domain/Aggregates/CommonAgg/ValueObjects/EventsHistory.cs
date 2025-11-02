using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Niu.Nutri.Core.Domain.Aggregates.CommonAgg.ValueObjects
{
    [Keyless]
    public class EventsHistory
    {
        public EventsHistory(int userId, string userName)
        {
            UserId = userId;
            UpdatedAt = DateTime.UtcNow;
            UserName = userName;
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

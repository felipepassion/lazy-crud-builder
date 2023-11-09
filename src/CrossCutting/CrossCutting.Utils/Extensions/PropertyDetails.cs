﻿using System.Reflection;

namespace LazyCrudBuilder.Core.Application.DTO.Aggregates.CommonAgg.ValueObjects
{
    public class PropertyDetails
    {
        public PropertyDetails(PropertyInfo info, object value)
        {
            this.info = info;
            Value = value;
        }

        public PropertyInfo info { get; set; }
        public object Value { get; set; } 

    }
}

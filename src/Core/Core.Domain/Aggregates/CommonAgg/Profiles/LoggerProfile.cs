﻿using AutoMapper;
using Newtonsoft.Json;
using LazyCrudBuilder.CrossCutting.Infra.Log.Entries;
using LazyCrudBuilder.CrossCutting.Infra.Log.SeedWork;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Events;
using LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Notifications;

namespace LazyCrudBuilder.Core.Domain.Aggregates.CommonAgg.Profiles
{
    public partial class LoggerProfile : Profile
    {
        public LoggerProfile()
        {
            CreateMap<BaseNotification, LogEntry>()
                .ForMember(x => x.Title, opt => opt.MapFrom(notification => $"[{notification.Context.ShortLogId}] {notification.Title}"))
                .ForMember(x => x.Action, opt => opt.MapFrom(notification => notification.GetType().Name))
                .ForMember(x => x.Level, opt => opt.MapFrom(notification => notification.LogType))
                .ForMember(x => x.Properties, opt => opt.Ignore())
                .ForMember(x => x.LogId, opt => opt.MapFrom(notification => notification.Context.LogId != Guid.Empty ? notification.Context.LogId : LoggerFactory.ExecutionKey))
                .ForMember(x => x.Content, opt => opt.MapFrom(notification =>
                    JsonConvert.DeserializeObject<object>(JsonConvert.SerializeObject(notification, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects }))
                ))
            .PreserveReferences()
                //.ForMember(x => x.Properties, opt => opt.MapFrom(notification => notification.ExtractProperties().ToArray()))
            .IncludeAllDerived();

            CreateMap<ErrorEvent, LogEntry>();
        }
    }
}
 
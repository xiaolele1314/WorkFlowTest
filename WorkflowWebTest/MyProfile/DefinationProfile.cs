using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkflowCore.Models;
using WorkflowWebTest.Test;

namespace WorkflowWebTest.MyProfile
{
    public class DefinationProfile : Profile
    {
        public DefinationProfile()
        {
            this.CreateMap<WorkflowDefinition, MyWorkflowDefination>()
                .ForMember(d => d.Steps, m => m.MapFrom(s => s.Steps == null ? string.Empty : JsonConvert.SerializeObject(s.Steps)))
                .ForMember(d => d.DataType, m => m.MapFrom(s => s.DataType == null ? string.Empty : JsonConvert.SerializeObject(s.DataType)))
                .ForMember(d => d.OnPostMiddlewareError, m => m.MapFrom(s => s.OnPostMiddlewareError == null ? string.Empty : JsonConvert.SerializeObject(s.OnPostMiddlewareError)))
                .ForMember(d => d.DefaultErrorRetryInterval, m => m.MapFrom(s => !s.DefaultErrorRetryInterval.HasValue ? string.Empty : s.DefaultErrorRetryInterval.Value.Ticks.ToString()));
            this.CreateMap<MyWorkflowDefination, WorkflowDefinition>()
                .ForMember(s => s.Steps, m => m.MapFrom(d => string.IsNullOrEmpty(d.Steps) ? null : JsonConvert.DeserializeObject<WorkflowDefinition>(d.Steps)))
                .ForMember(s => s.DataType, m => m.MapFrom(d => string.IsNullOrEmpty(d.DataType) ? null : JsonConvert.DeserializeObject<Type>(d.DataType)))
                .ForMember(s => s.OnPostMiddlewareError, m => m.MapFrom(d => string.IsNullOrEmpty(d.OnPostMiddlewareError) ? null : JsonConvert.DeserializeObject<Type>(d.OnPostMiddlewareError)))
                .ForMember(s => s.DefaultErrorRetryInterval, m => m.MapFrom(d => string.IsNullOrEmpty(d.DefaultErrorRetryInterval) ? null : new TimeSpan(long.Parse(d.DefaultErrorRetryInterval)) as TimeSpan?));

        }
    }
}

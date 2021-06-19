﻿using AutoMapper;
using Automaton.Studio.Activity;
using Automaton.Studio.Activity.Metadata;
using Automaton.Studio.Models;
using Elsa.Models;

namespace Automaton.Studio.Profiles
{
    public class StudioProfile : Profile
    {
        public StudioProfile()
        {
            CreateMap<Runner, WorkflowRunner>();
            CreateMap<ActivityDescriptor, TreeActivity>();
            CreateMap<TreeActivity, StudioActivity>();
            CreateMap<WorkflowNew, WorkflowDefinition>();
            CreateMap<ActivityDefinition, StudioActivity>();
            CreateMap<StudioActivity, ActivityDefinition>();
            CreateMap<WorkflowDefinition, StudioWorkflow>();
            CreateMap<StudioWorkflow, WorkflowDefinition>();
            CreateMap<WorkflowDefinition, WorkflowInfo>();
        }
    }
}
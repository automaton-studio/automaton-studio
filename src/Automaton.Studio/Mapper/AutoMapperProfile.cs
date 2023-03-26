using AutoMapper;
using Automaton.Client.Auth.Models;
using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Automaton.Studio.Factories;
using Automaton.Studio.Models;
using Automaton.Studio.Pages.CustomSteps;
using Automaton.Studio.Pages.FlowDesigner.Components.FlowExplorer;
using Automaton.Studio.Pages.Flows;
using Automaton.Studio.Pages.Login;
using Automaton.Studio.Steps.ExecuteFlow;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Studio.Mapper;

public class AutoMapperProfile : Profile
{
    private readonly IServiceProvider serviceProvider;

    public AutoMapperProfile(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;

        CreateMap<StudioFlow, Flow>();
        CreateMap<StudioStep, Step>();
        CreateMap<Step, StudioStep>();
        CreateMap<StudioDefinition, Definition>();
        
        CreateMap<StudioDefinition, FlowExplorerDefinition>();
        CreateMap<LoginModel, LoginDetails>();

        CreateMap<FlowInfo, ExecuteFlowModel>();

        CreateMap<FlowInfo, FlowModel>();
        CreateMap<FlowModel, FlowInfo>();

        CreateMap<CustomStep, CustomStepModel>();   
    }
}

﻿using AutoMapper;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Pages.FlowDesigner.Components.FlowExplorer;

public class FlowExplorerViewModel
{
    private StudioFlow flow;
    private readonly IMapper mapper;

    public IList<FlowExplorerDefinition> ExplorerDefinitions { get; set; }
    public IEnumerable<string> DefinitionNames => ExplorerDefinitions.Select(x => x.Name);

    public FlowExplorerViewModel(IMapper mapper)
    {
        this.mapper = mapper;
    }

    public void LoadDefinitions(StudioFlow flow)
    {
        this.flow = flow;
        SetExplorerDefinitions();
        SetStartupDefinition();
    }

    public void RenameDefinition(string definitionId, string workflowName)
    {
        var definition = ExplorerDefinitions.SingleOrDefault(x => x.Id == definitionId);
        definition.Name = workflowName;
    }

    public void RefreshDefinitions()
    {
        mapper.Map(flow.Definitions, ExplorerDefinitions);
    }

    public void SetStartupDefinition(string definitionId)
    {
        var startupDefinition = ExplorerDefinitions.SingleOrDefault(x => x.IsStartup);
        if (startupDefinition != null)
        {
            startupDefinition.IsStartup = false;
        }

        var newSartupDefinition = ExplorerDefinitions.SingleOrDefault(x => x.Id == definitionId);
        newSartupDefinition.IsStartup = true;

        flow.StartupDefinitionId = definitionId;
    }

    private void SetExplorerDefinitions()
    {
        ExplorerDefinitions = mapper.Map<IEnumerable<StudioDefinition>, IList<FlowExplorerDefinition>>(flow.Definitions);
    }

    private void SetStartupDefinition()
    {
        var startupDefinition = ExplorerDefinitions.SingleOrDefault(x => x.Id == flow.StartupDefinitionId);
        startupDefinition.IsStartup = true;
    }

    public void DeleteDefinition(FlowExplorerDefinition definition)
    {
        ExplorerDefinitions.Remove(definition);
        flow.RemoveDefinition(definition.Id);
    }
}

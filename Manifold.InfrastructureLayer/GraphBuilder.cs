using Manifold.DomainLayer.Entities;
using Manifold.InfrastructureLayer.DTOs;
using Manifold.InfrastructureLayer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Manifold.InfrastructureLayer;
public class GraphBuilder : IGraphBuilder
{
    private readonly string graphConfigPath;

    public GraphBuilder(string graphConfigPath)
    {
        this.graphConfigPath = graphConfigPath;
    }

    public List<GraphElement> BuildGraph()
    {
        // Read config file
        string json = File.ReadAllText(graphConfigPath);

        Graph? graph = JsonSerializer.Deserialize<Graph>(json) 
            ?? throw new JsonException("Failed to retrive graph elements from the configurationn file.");

        // Store all nodes in a dictionary for finding them easily later
        Dictionary<string, GraphElement> nodes = new();

        foreach (var dto in graph.SourceValves)
        {
            var entity = MapHelper.MapToEntity(dto);

            nodes.Add(entity.Id, entity);
        }

        foreach (var dto in graph.Valves)
        {
            var entity = MapHelper.MapToEntity(dto);

            nodes.Add(entity.Id, entity);
        }

        foreach (var dto in graph.WasteValves)
        {
            var entity = MapHelper.MapToEntity(dto);

            nodes.Add(entity.Id, entity);
        }

        foreach (var dto in graph.Chambers)
        {
            var entity = MapHelper.MapToEntity(dto);

            nodes.Add(entity.Id, entity);
        }

        // Crate pipes
        List<Pipe> pipes = new();

        foreach (PipeDto p in graph.Pipes)
        {
            var from = nodes[p.From];
            var to = nodes[p.To];

            Pipe pipe = new Pipe(p.Id, from, to);

            pipes.Add(pipe);
        }

        // Connect pipes to the graph elements
        foreach (Pipe pipe in pipes)
        {
            pipe.From.ConnectedPipes.Add(pipe);

            pipe.To.ConnectedPipes.Add(pipe);
        }

        return nodes.Values.ToList();
    }
}

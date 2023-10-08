using System;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using MyFreezer.API.GraphQL.Queries;

namespace MyFreezer.API.GraphQL.Schemas;

public class GraphQLSchema : Schema
{
    public GraphQLSchema(IServiceProvider provider):base(provider)
    {
        Query = provider.GetRequiredService<GraphQLQuery>();
        Mutation = provider.GetRequiredService<GraphQLMutation>();
    }
}
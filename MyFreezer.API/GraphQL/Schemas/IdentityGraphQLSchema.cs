using System;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using MyFreezer.API.GraphQL.Queries;

namespace MyFreezer.API.GraphQL.Schemas;

public class IdentityGraphQLSchema : Schema
{
    
    public IdentityGraphQLSchema(IServiceProvider serviceProvider):base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<IdentityGraphQLQuery>();
    }
}
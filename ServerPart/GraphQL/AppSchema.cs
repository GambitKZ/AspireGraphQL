using GraphQL.Types;

namespace ServerPart.GraphQL;

public class AppSchema : Schema
{
    public AppSchema(UniversityQuery query, UniversityMutation mutation, UniversitySubscription subscription)
    {
        this.Query = query;
        this.Mutation = mutation;
        this.Subscription = subscription;
    }
}

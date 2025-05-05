using AspireGraphQL.ServiceDefaults.Models;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;

namespace ServerPart.HotChocolate.Schema
{
    public class UniversitySubscription : ObjectType
    {
        protected override void Configure(IObjectTypeDescriptor descriptor)
        {
            descriptor.Name(OperationTypeNames.Subscription);

            descriptor
                .Field("TeacherAdded")
                .Resolve(context => context.GetEventMessage<Teacher>())
                .Subscribe(async context =>
                {
                    var receiver = context.Service<ITopicEventReceiver>();

                    ISourceStream stream =
                    await receiver.SubscribeAsync<Teacher>("TeacherAdded");

                    return stream;
                });
        }
    }
}

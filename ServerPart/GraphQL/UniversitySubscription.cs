using GraphQL;
using GraphQL.MicrosoftDI;
using GraphQL.Server.Transports.AspNetCore.WebSockets.GraphQLWs;
using GraphQL.Types;
using ServerPart.GraphQL.Subscription;
using ServerPart.Models;

namespace ServerPart.GraphQL
{
    public class UniversitySubscription : ObjectGraphType
    {
        public UniversitySubscription()
        {
            Name = "Subscription";


            Field<MessageType, Message>("courseUpdated")
                .Resolve()
                .WithService<INotificationEventService>()
                .ResolveStream((context, eventService) => ResolveStream(context, eventService));
        }

        private IObservable<Message> ResolveStream(IResolveFieldContext context, INotificationEventService eventService)
        {
            return eventService.GetLatestCourses();
        }
    }
}

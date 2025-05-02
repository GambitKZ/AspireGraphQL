using System.Collections.Concurrent;
using ServerPart.Models;

namespace ServerPart.GraphQL.Subscription
{
    public interface INotificationEventService
    {
        ConcurrentStack<Message> AllMessages { get; }

        Message CourseUpdate(Message courseDetails);
        IObservable<Message> GetLatestCourses();
    }
}
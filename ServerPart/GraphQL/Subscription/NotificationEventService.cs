using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ServerPart.Models;

namespace ServerPart.GraphQL.Subscription;

public class NotificationEventService : INotificationEventService
{
    private readonly ISubject<Message> _eventStream = new ReplaySubject<Message>(1);
    public ConcurrentStack<Message> AllMessages { get; }

    public NotificationEventService()
    {
        AllMessages = new ConcurrentStack<Message>();
    }

    public IObservable<Message> GetLatestCourses()
    {
        return _eventStream.Select(course =>
        {
            return course;
        }).AsObservable();
    }

    public Message CourseUpdate(Message courseDetails)
    {
        AllMessages.Push(courseDetails);
        _eventStream.OnNext(courseDetails);
        return courseDetails;
    }

    public void AddError(Exception exception)
    {
        _eventStream.OnError(exception);
    }
}

using GraphQL.Types;
using ServerPart.Models;

namespace ServerPart.GraphQL.Types
{
    public class MessageType : ObjectGraphType<Message>
    {
        public MessageType()
        {
            Field(m => m.Id);
            Field(m => m.Name);
        }
    }
}

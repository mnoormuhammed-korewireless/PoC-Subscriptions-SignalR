using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQL.Subscription;
using GraphQL.Resolvers;
using PoC.Data.Service;
using PoC.Api.GraphQL.Types;
using PoC.Data.Model;
using System.Reactive.Linq;

namespace PoC.Api.GraphQL.Subscriptions
{
    public class MessageSubscription : ObjectGraphType<object>
    {
        private readonly IMessageService _events;
        public MessageSubscription(IMessageService events)
        {
            _events = events;
            Name = "MessageSubscription";
            AddField(new EventStreamFieldType
            {
                Name = "new_message",
                Type = typeof(MessageType),
                Resolver = new FuncFieldResolver<Message>(ResolveMessageEvent),
                Subscriber = new EventStreamResolver<Message>(Subscribe)
            });
        }

        private IObservable<Message> Subscribe(ResolveEventStreamContext ctx)
        {
            return _events.EventStream();
        }

        private Message ResolveMessageEvent(ResolveFieldContext ctx)
        {
            var message = ctx.Source as Message;
            return message;
        }
    }
}

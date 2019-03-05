using GraphQL.Types;
using PoC.Api.GraphQL.InputTypes;
using PoC.Api.GraphQL.Types;
using PoC.Data.Model;
using PoC.Data.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoC.Api.GraphQL.Mutations
{
    public class MessageMutation : ObjectGraphType<object>
    {
        public MessageMutation(IMessageService messageService)
        {
            Name = "MessageMutation";
            Field<MessageType>(
                "addMessage",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<MessageInputType>> { Name = "message" }),
                resolve: context =>
                {
                    var message = context.GetArgument<Message>("message");
                    return messageService.AddMessage(message);
                });
        }
    }
}

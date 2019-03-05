using GraphQL;
using GraphQL.Types;
using PoC.Api;
using PoC.Api.GraphQL;
using PoC.Api.GraphQL.Mutations;
using PoC.Api.GraphQL.Queries;
using PoC.Api.GraphQL.Subscriptions;
using PoC.Data.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Poc.Api
{
    public class PoCSchema : Schema
    {
        public PoCSchema(IMessageService messageService)
        {
            //Query = resolver.Resolve<PoCQuery>();
            //Mutation = resolver.Resolve<PoCMutation>();
            //Subscription = resolver.Resolve<MessageSubscription>();

            Query = new MessageQuery(messageService);
            Mutation = new MessageMutation(messageService);
            Subscription = new MessageSubscription(messageService);
        }
    }
}

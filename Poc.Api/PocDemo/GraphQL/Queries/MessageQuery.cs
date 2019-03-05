﻿using GraphQL.Types;
using PoC.Api.GraphQL.Types;
using PoC.Data.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoC.Api.GraphQL.Queries
{
    public class MessageQuery : ObjectGraphType<object>
    {
        public MessageQuery(IMessageService messageService)
        {
            Name = "MessageQuery";
            Field<ListGraphType<MessageType>>(
                name: "get_data",
                description: "Get data from cassandra table",
                resolve: context => {
                    return messageService.GetCassandraData();
                    });
        }
    }
}

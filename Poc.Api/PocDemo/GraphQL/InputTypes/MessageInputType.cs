using GraphQL.Types;
using PoC.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoC.Api.GraphQL.InputTypes
{
    public class MessageInputType : InputObjectGraphType
    {
        public MessageInputType()
        {
            Name = "InputMessage";
            foreach(var prop in typeof(Message).GetProperties())
            {
                Field(prop.Name, x => prop.GetValue(x), type: Helper.Map[prop.PropertyType]);
            }
        }
    }
}

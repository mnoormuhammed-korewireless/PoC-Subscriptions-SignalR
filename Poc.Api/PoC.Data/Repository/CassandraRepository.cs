using Cassandra;
using Cassandra.Mapping;
using PoC.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoC.Data.Repository
{
    public class CassandraRepository : ICassandraRepository
    {
        private readonly string END_POINT = "127.0.0.1";
        private readonly string KEY_SPACE = "poc";
        private readonly string TABLE_NAME = "messages";

        private Cluster cluster;
        private ISession session;
        private IMapper mapper;

        public CassandraRepository()
        {
            cluster = Cluster.Builder().AddContactPoint(END_POINT).WithPort(9042).Build();
            session = cluster.Connect(KEY_SPACE);
            mapper = new Mapper(session);
        }

        public Message AddMessage(Message message)
        {
            message.id = Guid.NewGuid();
            var query = $"INSERT INTO {KEY_SPACE}.{TABLE_NAME}" +
                $"(id,message)" +
                $" VALUES" +
                $"({message.id},'{message.message}');";
            var result = session.Execute(query);
            return message;
        }

        public IEnumerable<T> Get<T>()
        {
            var query = $"select * from {KEY_SPACE}.{TABLE_NAME};";
            IEnumerable<T> result = mapper.Fetch<T>(query);
            return result;
        }

    }
}

using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Poc.Data.Hubs;
using PoC.Data.Model;
using PoC.Data.Repository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace PoC.Data.Service
{
    public class MessageService : IMessageService
    {
        ICassandraRepository _cassandraRepository;
        IHubContext<MessageHub> _hubContext;

        public MessageService(ICassandraRepository cassandraRepository, IHubContext<MessageHub> hubContext)
        {
            _cassandraRepository = cassandraRepository;
            _hubContext = hubContext;
            AllEvents = new ConcurrentStack<Message>();
        }
        public IEnumerable<Message> GetCassandraData()
        {
            return _cassandraRepository.Get<Message>();
        }

        // Method called for mutation
        public Message AddMessage(Message message)
        {
            // Saving the data to the database
            var new_message = _cassandraRepository.AddMessage(message);
            // Pushing the data to event stream for subscription
            this.AddEvent(new_message);
            // Pushing data to the hub for SignalR
            var data = JsonConvert.SerializeObject(new_message);
            _hubContext.Clients.All.SendAsync("BroadcastMessage", data);
            return message;
        }


        private readonly System.Reactive.Subjects.ISubject<Message> _eventStream = new ReplaySubject<Message>(1);
        public ConcurrentStack<Message> AllEvents { get; }

        public void AddError(Exception exception)
        {
            _eventStream.OnError(exception);
        }

        public Message AddEvent(Message message)
        {
            AllEvents.Push(message);
            _eventStream.OnNext(message);
            return message;
        }

        public IObservable<Message> EventStream()
        {
            return _eventStream.AsObservable();
        }

    }
}

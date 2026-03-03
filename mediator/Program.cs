using System;
using System.Collections.Generic;

namespace ChatMediatorAdvanced
{
    public interface IMediator
    {
        void SendMessage(string channel, string message, IUser sender);
        void RegisterUser(string channel, IUser user);
        void RemoveUser(string channel, IUser user);
    }

    public interface IUser
    {
        string Name { get; }
        void Receive(string message);
    }

    public class ChannelMediator : IMediator
    {
        private Dictionary<string, List<IUser>> channels =
            new Dictionary<string, List<IUser>>();

        public void RegisterUser(string channel, IUser user)
        {
            if (!channels.ContainsKey(channel))
                channels[channel] = new List<IUser>();

            channels[channel].Add(user);
            Broadcast(channel, $"{user.Name} вошел в канал");
        }

        public void RemoveUser(string channel, IUser user)
        {
            if (channels.ContainsKey(channel))
            {
                channels[channel].Remove(user);
                Broadcast(channel, $"{user.Name} покинул канал");
            }
        }

        public void SendMessage(string channel, string message, IUser sender)
        {
            if (!channels.ContainsKey(channel))
            {
                Console.WriteLine("Канал не существует!");
                return;
            }

            if (!channels[channel].Contains(sender))
            {
                Console.WriteLine("Вы не состоите в этом канале!");
                return;
            }

            foreach (var user in channels[channel])
                if (user != sender)
                    user.Receive($"{sender.Name}: {message}");
        }

        private void Broadcast(string channel, string message)
        {
            foreach (var user in channels[channel])
                user.Receive("[Система] " + message);
        }
    }

    public class User : IUser
    {
        public string Name { get; }
        private IMediator mediator;

        public User(string name, IMediator med)
        {
            Name = name;
            mediator = med;
        }

        public void Send(string channel, string message)
        {
            mediator.SendMessage(channel, message, this);
        }

        public void Receive(string message)
        {
            Console.WriteLine($"{Name} получил: {message}");
        }
    }

    class Program
    {
        static void Main()
        {
            IMediator mediator = new ChannelMediator();

            var alice = new User("Alice", mediator);
            var bob = new User("Bob", mediator);

            mediator.RegisterUser("General", alice);
            mediator.RegisterUser("General", bob);

            alice.Send("General", "Привет!");
            mediator.RemoveUser("General", bob);

            bob.Send("General", "Меня слышно?");
        }
    }
}

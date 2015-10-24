using System;
using System.Collections.Generic;

namespace Entities
{
    public class DistributeList
    {
        private string _title;
        private string _description;

        public Guid Id { get; private set; }

        public string Title
        {
            get { return _title; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException();

                _title = value;
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException();

                _description = value;
            }
        }

        public List<User> SubscribersList { get; private set; }

        public DistributeList(string title, string description)
        {
            SubscribersList = new List<User>();

            Title = title;
            Description = description;

            Id = new Guid();
            Id = Guid.NewGuid();
        }

        public DistributeList(Guid id, string title, string description)
        {
            SubscribersList = new List<User>();

            Title = title;
            Description = description;

            Id = new Guid();
            Id = id;
        }
    }
}

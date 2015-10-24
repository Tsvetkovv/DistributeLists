using System;

namespace Entities
{
    public class User
    {
        private string _login;
        private string _firstName;
        private string _lastName;
        private string _middleName;

        public Guid Id { get; private set; }

        public string Login
        {
            get { return _login; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException();

                _login = value;
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException();

                _firstName = value;
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException();

                _lastName = value;
            }
        }

        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException();

                _middleName = value;
            }
        }

        public User(string login, string firstName, string lastName, string middleName)
        {
            Login = login;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Id = new Guid();
            Id = Guid.NewGuid();
        }

        public User(Guid id, string login, string firstName, string lastName, string middleName)
        {
            Login = login;
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Id = new Guid();
            Id = id;
        }
    }
}

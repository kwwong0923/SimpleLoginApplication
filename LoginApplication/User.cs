using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginApplication
{
    class User: IComparable<User>
    {
        private readonly string _username;
        private readonly string _hashPassword;
        private readonly byte[] _salt;

        public string Username { get { return _username; } }
        public byte[] Salt { get { return _salt; } }
        public string HashPassword { get { return _hashPassword; } }

        public User(string username, string hashPassword, byte[] salt)
        {
            this._username = username;
            this._hashPassword = hashPassword;
            this._salt = salt;
        }
        public int CompareTo(User other)
        {
            if (this.Username == other.Username) return 1;
            return -1;
        }
    }
}

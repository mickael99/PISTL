using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Interface
{
    internal interface IServerParameterRepository
    {
        ICollection<ServerParameter> GetServerParameters();

        public bool DeleteServerParameter(ServerParameter s);

        public bool UpdateServerParameter(ServerParameter sp);

        public bool CreateServerParameter(ServerParameter sp);

        public bool ServerParameterExists(String key);

        public bool Save();

        public ServerParameter GetServerParameter(string key);


    }
}

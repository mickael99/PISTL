using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Interface;


namespace Project.Repository
{
    internal class ServerParameterRepository : IServerParameterRepository
    {
        private readonly DatContext _context;

        public ServerParameterRepository(DatContext context)
        {
            _context = context;
        }

        public ServerParameter GetServerParameter(string key)
        {
            return _context.ServerParameters.Where(sp => sp.ParameterKey == key).FirstOrDefault();
        }

        public ICollection<ServerParameter> GetServerParameters()
        {
            // return _context.ServerParameters.OrderBy(s => s.ParameterKey).ToList();
            return _context.ServerParameters.ToList();

        }

        public bool ServerParameterExists(String key)
        {
            return _context.ServerParameters.Any(sp => sp.ParameterKey == key);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            Console.WriteLine("-------->Save success" + saved);
            return saved > 0 ? true : false;
        }

        public bool CreateServerParameter(ServerParameter sp)
        {
            _context.Add(sp);
            return Save();
        }

        public bool UpdateServerParameter(ServerParameter sp)
        {
            _context.Update(sp);
            return Save();
        }

        public bool DeleteServerParameter(ServerParameter s)
        {
            _context.Remove(s);
            return Save();
        }


    }
}

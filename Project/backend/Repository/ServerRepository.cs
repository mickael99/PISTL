using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Interface;
using Project.interfaces;


namespace Project.Repository
{
    public class ServerRepository: IServerRepository
    {
        private readonly DatContext _context;

        public ServerRepository(DatContext context)
        {
            _context = context;
        }

        public Server GetServer(int id)
        {
            return _context.Servers.Where(s => s.ServerId == id).FirstOrDefault();
        }

        public ICollection<Server> GetServers()
        {
            return _context.Servers.OrderBy(s => s.ServerId).ToList();
        }

        public bool ServerExists(int Id)
        {
            return _context.Servers.Any(s => s.ServerId == Id);
        }

        public bool ServerExists(string ServerName)
        {
            return _context.Servers.Any(s => s.Name == ServerName);
        }

        public bool CreateServer(Server db)
        {
            _context.Add(db);
            return Save();
        }

        public bool UpdateServer(Server s)
        {
            _context.Update(s);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool DeleteServer(Server s)
        {
            _context.Remove(s);
            return Save();
        }
    }
}


        
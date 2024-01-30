using Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.Interface;
using Microsoft.EntityFrameworkCore;


namespace Project.Repository
{
    public class ServerRepository : IServerRepository
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

        public Server GetServer(string name)
        {
            return _context.Servers.Where(s => s.Name == name).FirstOrDefault();
        }

        public Server GetServerWithAddress(string address)
        {
            return _context.Servers.Where(s => s.Address == address).FirstOrDefault();
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

        public bool ServerExistsWithAddress(string ServerAddress)
        {
            return _context.Servers.Any(s => s.Address == ServerAddress);
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

        public int GetServerCount()
        {
            return _context.Servers.Count();
        }

        public int GetUnusedMinServerId()
        {
            int minServerId = 1;
            while (_context.Servers.Any(s => s.ServerId == minServerId))
            {
                minServerId++;
            }
            return minServerId;
        }

        public bool AddDatabaseToServer(int databaseId, int serverId)
        {
            var server = GetServer(serverId);

            var database = _context.Databases.FirstOrDefault(d => d.DatabaseId == databaseId);

            if (server != null && database != null)
            {
                server.Databases.Add(database);
                UpdateServer(server);

                _context.Entry(server).State = EntityState.Modified;

                return Save();
            }

            return false;
        }
    }
}



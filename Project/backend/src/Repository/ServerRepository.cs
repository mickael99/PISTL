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
            Console.WriteLine("-------->Save success" + saved);
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
            Console.WriteLine("-------->AddDatabaseToServer begin");
            var server = GetServer(serverId);

            Console.WriteLine("-------->server found ");
            Console.WriteLine("--------> Total databases: " + server.Databases.Count );
            Console.WriteLine("--------> Last entity ID: " + server.Databases.LastOrDefault()?.DatabaseId);
            var database = _context.Databases.FirstOrDefault(d => d.DatabaseId == databaseId);
 
            Console.WriteLine("-------->database found ");
            if (server != null && database != null)
            {
                server.Databases.Add(database);
                UpdateServer(server);
                
                Console.WriteLine("--------> Total databases: " + server.Databases.Count );
                Console.WriteLine("--------> Last entity ID: " + server.Databases.FirstOrDefault()?.DatabaseId);

                Console.WriteLine("--------> Last entity ID: " + server.Databases.LastOrDefault()?.DatabaseId);
                _context.Entry(server).State = EntityState.Modified; 

                Console.WriteLine("-------->AddDatabaseToServer success");
                return Save();
            }

            return false;
        }
    }
}


        
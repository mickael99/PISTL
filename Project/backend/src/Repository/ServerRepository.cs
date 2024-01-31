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

        /// <summary>
        /// Retrieves a server by its ID.
        /// </summary>
        /// <param name="id">The ID of the server to retrieve.</param>
        /// <returns>The server with the specified ID, or null if not found.</returns>
        public Server GetServer(int id)
        {
            return _context.Servers.Where(s => s.ServerId == id).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves a server by its name.
        /// </summary>
        /// <param name="name">The name of the server.</param>
        /// <returns>The server object if found, otherwise null.</returns>
        public Server GetServer(string name)
        {
            return _context.Servers.Where(s => s.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves a server with the specified address.
        /// </summary>
        /// <param name="address">The address of the server.</param>
        /// <returns>The server with the specified address, or null if not found.</returns>
        public Server GetServerWithAddress(string address)
        {
            return _context.Servers.Where(s => s.Address == address).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves a collection of servers from the database.
        /// </summary>
        /// <returns>A collection of servers.</returns>
        public ICollection<Server> GetServers()
        {
            return _context.Servers.OrderBy(s => s.ServerId).ToList();
        }

        /// <summary>
        /// Checks if a server with the specified Id exists in the repository.
        /// </summary>
        /// <param name="Id">The Id of the server to check.</param>
        /// <returns>True if a server with the specified Id exists, otherwise false.</returns>
        public bool ServerExists(int Id)
        {
            return _context.Servers.Any(s => s.ServerId == Id);
        }

        /// <summary>
        /// Checks if a server with the specified name exists in the repository.
        /// </summary>
        /// <param name="ServerName">The name of the server to check.</param>
        /// <returns>True if a server with the specified name exists, otherwise false.</returns>
        public bool ServerExists(string ServerName)
        {
            return _context.Servers.Any(s => s.Name == ServerName);
        }

        /// <summary>
        /// Checks if a server with the specified address exists in the repository.
        /// </summary>
        /// <param name="ServerAddress">The address of the server to check.</param>
        /// <returns>True if a server with the specified address exists, otherwise false.</returns>
        public bool ServerExistsWithAddress(string ServerAddress)
        {
            return _context.Servers.Any(s => s.Address == ServerAddress);
        }

        /// <summary>
        /// Creates a new server in the database.
        /// </summary>
        /// <param name="db">The server object to be created.</param>
        /// <returns>True if the server is successfully created, otherwise false.</returns>
        public bool CreateServer(Server db)
        {
            _context.Add(db);
            return Save();
        }

        /// <summary>
        /// Updates a server in the repository.
        /// </summary>
        /// <param name="s">The server to be updated.</param>
        /// <returns>True if the server was successfully updated, false otherwise.</returns>
        public bool UpdateServer(Server s)
        {
            _context.Update(s);
            return Save();
        }

        /// <summary>
        /// Saves changes made to the context.
        /// </summary>
        /// <returns>True if changes were successfully saved, otherwise false.</returns>
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        /// <summary>
        /// Deletes a server from the repository.
        /// </summary>
        /// <param name="s">The server to be deleted.</param>
        /// <returns>True if the server was successfully deleted, false otherwise.</returns>
        public bool DeleteServer(Server s)
        {
            _context.Remove(s);
            return Save();
        }

        /// <summary>
        /// Gets the count of servers in the repository.
        /// </summary>
        /// <returns>The count of servers.</returns>
        public int GetServerCount()
        {
            return _context.Servers.Count();
        }

        /// <summary>
        /// Gets the minimum unused server ID.
        /// </summary>
        /// <returns>The minimum unused server ID.</returns>
        public int GetUnusedMinServerId()
        {
            int minServerId = 1;
            while (_context.Servers.Any(s => s.ServerId == minServerId))
            {
                minServerId++;
            }
            return minServerId;
        }

        /// <summary>
        /// Adds a database to a server.
        /// </summary>
        /// <param name="databaseId">The ID of the database to add.</param>
        /// <param name="serverId">The ID of the server to add the database to.</param>
        /// <returns>True if the database was successfully added to the server, false otherwise.</returns>
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



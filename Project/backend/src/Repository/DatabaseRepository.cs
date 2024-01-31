using System.Data.Common;
using Project.Models;
using Project.Interface;
using System.Security.Cryptography;
using System.Text;


namespace Project.Repository
{
    public class DatabaseRepository : IDatabaseRepository
    {

        private readonly DatContext _context;

        public DatabaseRepository(DatContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a database from the repository based on the specified ID.
        /// </summary>
        /// <param name="id">The ID of the database to retrieve.</param>
        /// <returns>The database with the specified ID, or null if not found.</returns>
        public Database GetDatabase(int id)
        {
            return _context.Databases.Where(db => db.DatabaseId == id).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves a database by its name.
        /// </summary>
        /// <param name="name">The name of the database.</param>
        /// <returns>The database object.</returns>
        public Database GetDatabase(string name){
            return _context.Databases.Where(db => db.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// Retrieves a collection of databases from the repository.
        /// </summary>
        /// <returns>A collection of databases.</returns>
        public ICollection<Database> GetDataBases(){
            return _context.Databases.OrderBy(db => db.DatabaseId).ToList();
        }

        /// <summary>
        /// Checks if a database with the specified Id exists in the repository.
        /// </summary>
        /// <param name="Id">The Id of the database to check.</param>
        /// <returns>True if the database exists, otherwise false.</returns>
        public bool DatabaseExists(int Id)
        {
            return _context.Databases.Any(db => db.DatabaseId == Id);
        }

        /// <summary>
        /// Checks if a database with the specified name exists.
        /// </summary>
        /// <param name="Name">The name of the database to check.</param>
        /// <returns>True if a database with the specified name exists, otherwise false.</returns>
        public bool DatabaseExists(string Name)
        {
            return _context.Databases.Any(db => db.Name == Name);
        }

        


        /// <summary>
        /// Creates a new database in the repository.
        /// </summary>
        /// <param name="db">The database to be created.</param>
        /// <returns>True if the database was created successfully, false otherwise.</returns>
        public bool CreateDatabase(Database db)
        {
            _context.Add(db);
            return Save();
        }

        /// <summary>
        /// Saves changes made to the database.
        /// </summary>
        /// <returns>True if changes were successfully saved, otherwise false.</returns>
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        /// <summary>
        /// Updates the specified database in the repository.
        /// </summary>
        /// <param name="db">The database to be updated.</param>
        /// <returns>True if the update was successful, false otherwise.</returns>
        public bool UpdateDatabase(Database db)
        {
            _context.Update(db);
            return Save();
        }

        /// <summary>
        /// Deletes a database from the repository.
        /// </summary>
        /// <param name="db">The database to be deleted.</param>
        /// <returns>True if the database was successfully deleted, false otherwise.</returns>
        public bool DeleteDatabase(Database db)
        {
            _context.Remove(db);
            return Save();
        }

        /// <summary>
        /// Gets the count of databases in the repository.
        /// </summary>
        /// <returns>The count of databases.</returns>
        public int GetDatabaseCount()
        {
            return _context.Databases.Count();
        }

        /// <summary>
        /// Gets the minimum unused database ID.
        /// </summary>
        /// <returns>The minimum unused database ID.</returns>
        public int GetUnusedMinDatabaseId()
        {
            int minDatabaseId = 1;
            while (_context.Databases.Any(db => db.DatabaseId == minDatabaseId))
            {
                minDatabaseId++;
            }
            return minDatabaseId;
        }

        /***************************************************************************************/
        /// <summary>
        /// Generates a random salt.
        /// </summary>
        /// <param name="size">The size of the salt.</param>
        /// <returns>The generated salt.</returns>
        public string GetSalt(int size)
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(size));
        }

        /***************************************************************************************/
        /// <summary>
        /// Encrypts the password using SHA512 algorithm. // TODO change to PBKDF2
        /// </summary>
        /// <param name="password">The password to be encrypted.</param>
        /// <param name="salt">The salt to be used.</param>
        /// <returns>The encrypted password.</returns>
        public string EncryptPassword(string password)
        {
            var bytes = Encoding.UTF8.GetBytes(password);
            var encryptedBytes = SHA512.HashData(bytes);
            return BitConverter.ToString(encryptedBytes).Replace("-", "");
        }
    }
}
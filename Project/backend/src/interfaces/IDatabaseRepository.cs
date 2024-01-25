using Project.Models;
using System.Collections.Generic;

namespace Project.Interface 
{
    public interface IDatabaseRepository
    {
        Database GetDatabase(int id);

        ICollection<Database> GetDataBases();

        bool DatabaseExists(int Id);

        bool DatabaseExists(string DatabaseName);

        bool CreateDatabase(Database db);

        bool Save();

        bool UpdateDatabase(Database db);

        bool DeleteDatabase(Database db);

        public int GetDatabaseCount();

        public int GetUnusedMinDatabaseId();

        public string EncryptPassword(string input);

    }
}

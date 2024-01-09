using Project.Models;
using System.Collections.Generic;

namespace Project.Interface 
{
    public interface IDatabaseRepository
    {
        Database GetDatabase(int id);

        ICollection<Database> GetDataBases();

        bool DatabaseExists(int Id);

        bool CreateDatabase(Database db);

        bool Save();

        bool UpdateDatabase(Database db);

        bool DeleteDatabase(Database db);
    }
}
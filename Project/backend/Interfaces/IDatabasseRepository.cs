using Microsoft.EntityFrameworkCore.Storage;

namespace Project.Interface 
{
    public interface IDatabaseRepository
    {
        Database GetDatabase();
    }
}

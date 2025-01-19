using System;

namespace PersonalCoachingApp.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable //Thanks to this method, we can clean the RAM.
    {
       Task<int> SaveChangesAsync(); //Returns how many records it affected.

        Task BeginTransaction(); //Void method, no async.

        Task CommitTransaction();

        Task RollBackTransaction(); //Undoes the action.
    }
}

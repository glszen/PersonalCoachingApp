using Microsoft.EntityFrameworkCore.Storage;
using PersonalCoachingApp.Data.Context;
using System;

namespace PersonalCoachingApp.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoachingAppDbContext _db;
        private IDbContextTransaction? _transaction;

        public UnitOfWork(CoachingAppDbContext db)
        {
            _db = db;
        }

        public async Task BeginTransaction()
        {
            _transaction = await _db.Database.BeginTransactionAsync();
        }

        public async Task CommitTransaction()
        { 
           await _transaction.CommitAsync();
        }

        public async Task RollBackTransaction()
        {
           await _transaction.RollbackAsync();
        }

        public async Task <int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}

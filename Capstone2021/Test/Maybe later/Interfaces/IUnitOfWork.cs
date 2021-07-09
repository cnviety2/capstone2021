using Capstone2021.DTO;
using System;

namespace Capstone2021.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Recruiter> RecruiterRepository { get; }
        int Save();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Application.DTOs;
using PGManagementSystem.Domain.Entities;

namespace PGManagementSystem.Application.Interfaces
{
    public interface IPGService
    {
        Task AddPG(PG pg);
    }
}

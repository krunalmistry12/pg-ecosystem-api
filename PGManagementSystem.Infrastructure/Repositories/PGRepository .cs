using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Domain.Entities;
using PGManagementSystem.Infrastructure.Data;

namespace PGManagementSystem.Infrastructure.Repositories
{
    public class PGRepository : IPGService
    {
        private readonly AppDbContext _context;

        public PGRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddPG(PG pg)
        {
            //_context.PGs.Add(pg);
            //await _context.SaveChangesAsync();
        }
    }
}

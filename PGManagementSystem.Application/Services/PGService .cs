using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PGManagementSystem.Application.DTOs;
using PGManagementSystem.Application.Interfaces;
using PGManagementSystem.Domain.Entities;

namespace PGManagementSystem.Application.Services
{
    public class PGService 
    {
        private readonly ILogger<PGService> _logger;
        private readonly IPGService _pGService;
        private readonly AuthService _jwt;

        public PGService(IPGService repo,AuthService jwt)
        {
            _pGService = repo;
            _jwt = jwt;
        }

        public async Task CreatePG(CreatePGDto dto, int ownerId)
        {
            var pg = new PG
            {
                Name = dto.Name,
                Location = dto.Location,
                OwnerId = ownerId
            };

            await _pGService.AddPG(pg);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGManagementSystem.Domain.Entities
{
    public class PG
    {
        public int PGId { get; set; }

        public string Name { get; set; }
        public string Location { get; set; }

        public int OwnerId { get; set; }   // 🔥 Admin UserId
        public UserMaster Owner { get; set; }
    }
}

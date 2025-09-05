using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AESServerAPP.Controllers;
using Microsoft.EntityFrameworkCore;

namespace AESServerAPP.Models
{
    public class KeyManagementService
    {
         private readonly ApplicationDbContext _context;
        public KeyManagementService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ClientKeyIV?> GetKeyAndIVAsync(string clientId)
        {
            // Lấy bản ghi ClientKeyIV khớp với clientId được cung cấp
            return await _context.ClientKeyIVs
                .FirstOrDefaultAsync(c => c.ClientId.ToLower() == clientId.ToLower());
        }
    }
}
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Extensions
{
    public static class DbContextExtensions
    {
        //public static async Task<List<long>> GetActiveDescendantsIdAsync(this EFContextNAG context, long ancestorId)
        //{
        //    return await context.AgentHierarchies
        //        .Where(h => h.AncestorId == ancestorId && h.IsActive)
        //        .Select(h => h.DescendantId)
        //        .ToListAsync();
        //}
    }
}

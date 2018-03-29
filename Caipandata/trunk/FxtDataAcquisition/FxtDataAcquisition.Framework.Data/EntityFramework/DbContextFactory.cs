using FxtDataAcquisition.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace FxtDataAcquisition.Framework.Data.EntityFramework
{
    public class DbContextFactory : IDbContextFactory
    {
        private readonly DbContext _context;

        public DbContextFactory()
        {
            _context = new FxtDataAcquisitionContext();
        }

        public DbContext GetDbContext()
        {
            return _context;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerstyringClassLibrary
{
    public class ContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseSqlServer("Server=SUS-EL-TWAN1;Database=Lagerstyring;Trusted_Connection=True;TrustServerCertificate=True");

            return new Context(optionsBuilder.Options);
        }

    }
}

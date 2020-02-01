using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace VulnusCloud.Models
{
    public class EntityFrameWorkMagicContext : DbContext
    {
        public EntityFrameWorkMagicContext (DbContextOptions<EntityFrameWorkMagicContext> options)
            : base(options)
        {
        }

        public DbSet<VulnusCloud.Models.FileUploadViewModel> FileUploadViewModel { get; set; }
    }
}

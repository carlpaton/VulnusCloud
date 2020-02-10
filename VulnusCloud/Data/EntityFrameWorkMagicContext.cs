using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VulnusCloud.Models;

namespace VulnusCloud.Models
{
    public class EntityFrameWorkMagicContext : DbContext
    {
        public EntityFrameWorkMagicContext (DbContextOptions<EntityFrameWorkMagicContext> options)
            : base(options)
        {
        }

        public DbSet<VulnusCloud.Models.FileUploadViewModel> FileUploadViewModel { get; set; }

        public DbSet<VulnusCloud.Models.ProjectViewModel> ProjectViewModel { get; set; }

        public DbSet<VulnusCloud.Models.ReportByProjectViewModel> ReportViewModel { get; set; }
    }
}

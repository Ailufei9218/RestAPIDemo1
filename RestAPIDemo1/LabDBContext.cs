using Microsoft.EntityFrameworkCore;

namespace RestAPIDemo1
{
    public class LabDBContext : DbContext
    {
        public LabDBContext(DbContextOptions<LabDBContext> options) : base(options) { }

        public DbSet<UserInfo> Users { get; set; }
    }
}

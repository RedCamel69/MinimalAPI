using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data.Models;

namespace MinimalAPI.Data.Context
{
    public class YogaDb : DbContext
    {
        public YogaDb(DbContextOptions<YogaDb> options)
            : base(options) { }

        public DbSet<Pose> Poses => Set<Pose>();
    }
}

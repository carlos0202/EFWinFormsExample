using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using EFWinFormsExample.Models.Mapping;

namespace EFWinFormsExample.Models
{
    public partial class DBContext : DbContext
    {
        static DBContext()
        {
            Database.SetInitializer<DBContext>(null);
        }

        public DBContext()
            : base("Name=DBContext")
        {
        }

        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ClientMap());
        }
    }
}

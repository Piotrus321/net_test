using Microsoft.EntityFrameworkCore;

namespace net_test
{
    internal class AccessDatabaseContext : DbContext
    {
        // DbSet maps directly to a table in a given database
        // Name MUST be the same as table name, here 'Table1'
        public DbSet<TableRecord> Table1 { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO: set correct path to the database file on Azure
            optionsBuilder.UseJetOleDb(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=./../../../test_db.accdb;Persist Security Info=True");
        }
    }
}

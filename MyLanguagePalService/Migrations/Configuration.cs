// DropCreateDatabaseIfModelChanges doesn't work if a migration exists
// So enable migrations compilation only in release mode
// See http://stackoverflow.com/questions/19430502/dropcreatedatabaseifmodelchanges-ef6-results-in-system-invalidoperationexception


#if !DEBUG
namespace MyLanguagePalService.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<DAL.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "MyLanguagePalService.DAL.ApplicationDbContext";
        }

        protected override void Seed(DAL.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
#endif
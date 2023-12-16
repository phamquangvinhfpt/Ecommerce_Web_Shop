using System.Data.Entity.Migrations;

namespace DoAn_LapTrinhWeb.Migrations
{
    public class Configuration : DbMigrationsConfiguration<DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}
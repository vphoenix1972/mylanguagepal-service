namespace MyLanguagePalService.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Tags : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tags",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.TagsPhrases",
                c => new
                {
                    TagDal_Id = c.Int(nullable: false),
                    PhraseDal_Id = c.Int(nullable: false),
                })
                .PrimaryKey(t => new { t.TagDal_Id, t.PhraseDal_Id })
                .ForeignKey("dbo.Tags", t => t.TagDal_Id)
                .ForeignKey("dbo.Phrases", t => t.PhraseDal_Id)
                .Index(t => t.TagDal_Id)
                .Index(t => t.PhraseDal_Id);

            // Add default tags
            Sql("INSERT INTO Tags (Name) VALUES ('noun'), ('verb')");
        }

        public override void Down()
        {
            DropForeignKey("dbo.TagsPhrases", "PhraseDal_Id", "dbo.Phrases");
            DropForeignKey("dbo.TagsPhrases", "TagDal_Id", "dbo.Tags");
            DropIndex("dbo.TagsPhrases", new[] { "PhraseDal_Id" });
            DropIndex("dbo.TagsPhrases", new[] { "TagDal_Id" });
            DropTable("dbo.TagsPhrases");
            DropTable("dbo.Tags");
        }
    }
}

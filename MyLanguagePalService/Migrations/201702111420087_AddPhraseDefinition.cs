namespace MyLanguagePalService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddPhraseDefinition : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Phrases", "Definition", c => c.String(nullable: true, unicode: true));
        }

        public override void Down()
        {
            DropColumn("dbo.Phrases", "Definition");
        }
    }
}

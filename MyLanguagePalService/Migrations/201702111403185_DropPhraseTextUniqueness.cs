namespace MyLanguagePalService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropPhraseTextUniqueness : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Phrases", "UX_Text");
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Phrases", "Text", unique: true, name: "UX_Text");
        }
    }
}

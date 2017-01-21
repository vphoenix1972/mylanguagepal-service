namespace MyLanguagePalService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitialRelease : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Languages",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Phrases",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Text = c.String(maxLength: 255, storeType: "nvarchar"),
                    LanguageId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.LanguageId)
                .Index(t => t.Text, unique: true, name: "UX_Text")
                .Index(t => t.LanguageId);

            CreateTable(
                "dbo.Translations",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ForPhraseId = c.Int(nullable: false),
                    TranslationPhraseId = c.Int(nullable: false),
                    Prevalence = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Phrases", t => t.TranslationPhraseId)
                .ForeignKey("dbo.Phrases", t => t.ForPhraseId)
                .Index(t => t.ForPhraseId)
                .Index(t => t.TranslationPhraseId);

            CreateTable(
                "dbo.KnowledgeLevels",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TaskId = c.Int(nullable: false),
                    PhraseId = c.Int(nullable: false),
                    LastRepetitonTime = c.DateTime(nullable: false, precision: 0),
                    CurrentLevel = c.Double(nullable: false),
                    PreviousLevel = c.Double(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Phrases", t => t.PhraseId)
                .Index(t => t.PhraseId);


            CreateTable(
                "dbo.TaskSettings",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TaskId = c.Int(nullable: false),
                    SettingsJson = c.String(unicode: false),
                })
                .PrimaryKey(t => t.Id);


            // Add default languages
            Sql("INSERT INTO Languages (Name) VALUES ('English'), ('Русский')");

        }

        public override void Down()
        {
            DropForeignKey("dbo.Translations", "ForPhraseId", "dbo.Phrases");
            DropForeignKey("dbo.Translations", "TranslationPhraseId", "dbo.Phrases");
            DropForeignKey("dbo.Phrases", "LanguageId", "dbo.Languages");
            DropForeignKey("dbo.KnowledgeLevels", "PhraseId", "dbo.Phrases");

            DropIndex("dbo.Translations", new[] { "TranslationPhraseId" });
            DropIndex("dbo.Translations", new[] { "ForPhraseId" });
            DropIndex("dbo.Phrases", new[] { "LanguageId" });
            DropIndex("dbo.Phrases", "UX_Text");
            DropIndex("dbo.KnowledgeLevels", new[] { "PhraseId" });

            DropTable("dbo.TaskSettings");
            DropTable("dbo.Translations");
            DropTable("dbo.Languages");
            DropTable("dbo.Phrases");
            DropTable("dbo.KnowledgeLevels");
        }
    }
}

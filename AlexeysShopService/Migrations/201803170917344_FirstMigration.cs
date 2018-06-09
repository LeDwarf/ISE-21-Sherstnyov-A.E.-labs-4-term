namespace AlexeysShopService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleParts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArticleId = c.Int(nullable: false),
                        PartId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Parts", t => t.PartId, cascadeDelete: true)
                .Index(t => t.ArticleId)
                .Index(t => t.PartId);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArticleName = c.String(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Contracts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        ArticleId = c.Int(nullable: false),
                        BuilderId = c.Int(),
                        Count = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Status = c.Int(nullable: false),
                        DateBegin = c.DateTime(nullable: false),
                        DateBuilt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .ForeignKey("dbo.Builders", t => t.BuilderId)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.ArticleId)
                .Index(t => t.BuilderId);
            
            CreateTable(
                "dbo.Builders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BuilderFIO = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerFIO = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Parts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PartName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StorageParts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StorageId = c.Int(nullable: false),
                        PartId = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Parts", t => t.PartId, cascadeDelete: true)
                .ForeignKey("dbo.Storages", t => t.StorageId, cascadeDelete: true)
                .Index(t => t.StorageId)
                .Index(t => t.PartId);
            
            CreateTable(
                "dbo.Storages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StorageName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StorageParts", "StorageId", "dbo.Storages");
            DropForeignKey("dbo.StorageParts", "PartId", "dbo.Parts");
            DropForeignKey("dbo.ArticleParts", "PartId", "dbo.Parts");
            DropForeignKey("dbo.Contracts", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Contracts", "BuilderId", "dbo.Builders");
            DropForeignKey("dbo.Contracts", "ArticleId", "dbo.Articles");
            DropForeignKey("dbo.ArticleParts", "ArticleId", "dbo.Articles");
            DropIndex("dbo.StorageParts", new[] { "PartId" });
            DropIndex("dbo.StorageParts", new[] { "StorageId" });
            DropIndex("dbo.Contracts", new[] { "BuilderId" });
            DropIndex("dbo.Contracts", new[] { "ArticleId" });
            DropIndex("dbo.Contracts", new[] { "CustomerId" });
            DropIndex("dbo.ArticleParts", new[] { "PartId" });
            DropIndex("dbo.ArticleParts", new[] { "ArticleId" });
            DropTable("dbo.Storages");
            DropTable("dbo.StorageParts");
            DropTable("dbo.Parts");
            DropTable("dbo.Customers");
            DropTable("dbo.Builders");
            DropTable("dbo.Contracts");
            DropTable("dbo.Articles");
            DropTable("dbo.ArticleParts");
        }
    }
}

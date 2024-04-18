namespace MyPassionProjectW2024n01605783.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class blog_workout : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Blogs", "BlogFeaturedImageUrl");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Blogs", "BlogFeaturedImageUrl", c => c.String());
        }
    }
}

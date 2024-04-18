namespace MyPassionProjectW2024n01605783.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class users : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        UserDescription = c.String(),
                        UserAge = c.String(),
                        UserWeight = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            AddColumn("dbo.Workouts", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Workouts", "UserId");
            AddForeignKey("dbo.Workouts", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Workouts", "UserId", "dbo.Users");
            DropIndex("dbo.Workouts", new[] { "UserId" });
            DropColumn("dbo.Workouts", "UserId");
            DropTable("dbo.Users");
        }
    }
}

namespace MyPassionProjectW2024n01605783.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class workouts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Workouts",
                c => new
                    {
                        WorkoutId = c.Int(nullable: false, identity: true),
                        WorkoutName = c.String(),
                        WorkoutDescription = c.String(),
                        WorkoutDay = c.String(),
                        WorkoutStatus = c.String(),
                    })
                .PrimaryKey(t => t.WorkoutId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Workouts");
        }
    }
}

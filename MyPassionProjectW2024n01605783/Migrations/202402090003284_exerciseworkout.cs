namespace MyPassionProjectW2024n01605783.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class exerciseworkout : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exercises", "WorkoutId", c => c.Int(nullable: false));
            CreateIndex("dbo.Exercises", "WorkoutId");
            AddForeignKey("dbo.Exercises", "WorkoutId", "dbo.Workouts", "WorkoutId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Exercises", "WorkoutId", "dbo.Workouts");
            DropIndex("dbo.Exercises", new[] { "WorkoutId" });
            DropColumn("dbo.Exercises", "WorkoutId");
        }
    }
}

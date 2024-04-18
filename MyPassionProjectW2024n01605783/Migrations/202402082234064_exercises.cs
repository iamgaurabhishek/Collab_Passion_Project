namespace MyPassionProjectW2024n01605783.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class exercises : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Exercises",
                c => new
                    {
                        ExerciseId = c.Int(nullable: false, identity: true),
                        ExerciseName = c.String(),
                        ExerciseDescription = c.String(),
                        NumberOfSets = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ExerciseId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Exercises");
        }
    }
}

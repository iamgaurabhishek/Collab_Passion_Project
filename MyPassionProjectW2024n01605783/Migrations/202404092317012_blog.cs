namespace MyPassionProjectW2024n01605783.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class blog : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Blogs", name: "ExeciseId", newName: "ExerciseId");
            RenameIndex(table: "dbo.Blogs", name: "IX_ExeciseId", newName: "IX_ExerciseId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Blogs", name: "IX_ExerciseId", newName: "IX_ExeciseId");
            RenameColumn(table: "dbo.Blogs", name: "ExerciseId", newName: "ExeciseId");
        }
    }
}

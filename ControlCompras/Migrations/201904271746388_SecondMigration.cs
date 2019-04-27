namespace ControlCompras.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productos", "Cantidad", c => c.Single());
            AddColumn("dbo.Productos", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Productos", "Discriminator");
            DropColumn("dbo.Productos", "Cantidad");
        }
    }
}

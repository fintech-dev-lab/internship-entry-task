using FluentMigrator;

namespace Segrom.СrossesProject.PostgresRepository.Migrations;

[Migration(2025_07_09__13_22_00, "Create tables")]
// ReSharper disable once UnusedType.Global
public sealed class SetupMigration: Migration
{
	public override void Up()
	{
		Execute.Sql(
			"""
			CREATE TABLE games (
			    id UUID NOT NULL PRIMARY KEY,
			    created_at TIMESTAMP NOT NULL,
			    updated_at TIMESTAMP NOT NULL,
			    
			    state SMALLINT NOT NULL,
			    winner SMALLINT NULL,
			    current_player SMALLINT NOT NULL,
			    
			    cells SMALLINT[][] NOT NULL,
			    length_for_win INT NOT NULL
			);
			""");
	}

	public override void Down()
	{
		Execute.Sql("DROP TABLE games;");
	}
}
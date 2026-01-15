using FluentMigrator;

namespace StatisticsServiceProject.Infrastructure.Driven.Migrations.Postgres;

[Migration(2026011502)]
public class CreatePointsSpendingsTable : Migration
{
    public override void Up()
    {
        Execute.Sql(
            """
            CREATE TABLE points_spendings (
                id SERIAL PRIMARY KEY,
                order_id BIGINT NOT NULL,
                user_id BIGINT NOT NULL,
                points_amount BIGINT NOT NULL CHECK (points_amount > 0),
                spent_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );

            CREATE INDEX idx_points_spendings_spent_at
                ON points_spendings (spent_at);
            """);
    }

    public override void Down()
    {
        Execute.Sql("DROP TABLE IF EXISTS points_spendings");
    }
}
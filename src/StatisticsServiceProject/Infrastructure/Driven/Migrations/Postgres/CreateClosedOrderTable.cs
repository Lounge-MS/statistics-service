using FluentMigrator;

namespace StatisticsServiceProject.Infrastructure.Driven.Migrations.Postgres;

[Migration(2026011500)]
public class CreateClosedOrderTable : Migration
{
    public override void Up()
    {
        Execute.Sql(
            """
            CREATE TABLE closed_orders (
                id SERIAL PRIMARY KEY,
                user_id BIGINT NOT NULL,
                price_original DECIMAL(12, 2) NOT NULL,
                price_discounted DECIMAL(12, 2) NOT NULL,
                closed_at TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
            );

            CREATE INDEX idx_closed_orders_closed_at
                ON closed_orders (closed_at);
            """);
    }

    public override void Down()
    {
        Execute.Sql("DROP TABLE IF EXISTS closed_orders");
    }
}
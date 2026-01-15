using FluentMigrator;

namespace StatisticsServiceProject.Infrastructure.Driven.Migrations.Postgres;

[Migration(2026011501)]
public class CreateBoughtOrderPositionsTable : Migration
{
    public override void Up()
    {
        Execute.Sql(
            """
            CREATE TABLE bought_order_positions (
                id SERIAL PRIMARY KEY,
                order_id BIGINT NOT NULL,
                product_id BIGINT NOT NULL,
                product_name VARCHAR(255) NOT NULL,
                quantity BIGINT NOT NULL CHECK (quantity > 0),
                bought_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
            );

            CREATE INDEX idx_bopp_product_id
                ON bought_order_positions (product_id);

            CREATE INDEX idx_bopp_product_name
                ON bought_order_positions (product_name);

            CREATE INDEX idx_bought_order_positions_bought_at
                ON bought_order_positions (bought_at);
            """);
    }

    public override void Down()
    {
        Execute.Sql("DROP TABLE IF EXISTS bought_order_positions");
    }
}
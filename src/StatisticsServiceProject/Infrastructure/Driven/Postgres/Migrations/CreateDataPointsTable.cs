using FluentMigrator;

namespace StatisticsServiceProject.Infrastructure.Driven.Postgres.Migrations;

[Migration(2026011502)]
public class CreateDataPointsTable : Migration
{
    public override void Up()
    {
        Execute.Sql(
            """
            CREATE TABLE data_points (
                id SERIAL PRIMARY KEY,
                data_type VARCHAR(32) NOT NULL,
                value DOUBLE PRECISION NOT NULL,
                metainfo JSONB NOT NULL DEFAULT '{}'::jsonb,
                timestamp TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP
            );

            CREATE INDEX idx_data_points_timestamp
                ON data_points (timestamp);

            CREATE INDEX idx_data_points_data_type
                ON data_points (data_type);

            CREATE INDEX idx_data_points_product_name
                ON data_points (((metainfo->>'product_name')::varchar));

            CREATE INDEX idx_data_points_metainfo_gin
                ON data_points USING gin (metainfo);
            """);
    }

    public override void Down()
    {
        Execute.Sql("DROP TABLE IF EXISTS data_points");
    }
}
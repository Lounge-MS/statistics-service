using FluentMigrator;

namespace StatisticsServiceProject.Infrastructure.Driven.Migrations.Postgres;

[Migration(2026011503)]
public class CreateExpensesTable : Migration
{
    public override void Up()
    {
        Execute.Sql(
            """
            CREATE TABLE expenses (
                id SERIAL PRIMARY KEY,
                amount NUMERIC(12,2) NOT NULL CHECK (amount >= 0),
                created_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
            );

            CREATE INDEX idx_expenses_created_at
                ON expenses (created_at);
            """);
    }

    public override void Down()
    {
        Execute.Sql("DROP TABLE IF EXISTS expenses");
    }
}
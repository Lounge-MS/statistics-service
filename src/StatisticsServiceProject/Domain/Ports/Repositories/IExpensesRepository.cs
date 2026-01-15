using StatisticsServiceProject.Domain.Entities;
using StatisticsServiceProject.Domain.Entities.Dto.Repositories;

namespace StatisticsServiceProject.Domain.Ports.Repositories;

public interface IExpensesRepository
{
    Task<Expense> CreateAsync(CreateExpenseRequest request);

    Task<IEnumerable<Expense>> GetAsync(
        DateTime startTimestamp,
        DateTime endTimestamp,
        DateTime stepTimestamp);
}
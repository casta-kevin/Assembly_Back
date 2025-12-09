namespace AssemblyApi.Domain.ValueObjects;

public record DateRange
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }

    private DateRange(DateTime startDate, DateTime endDate)
    {
        StartDate = startDate;
        EndDate = endDate;
    }

    public static DateRange Create(DateTime startDate, DateTime endDate)
    {
        if (endDate < startDate)
            throw new ArgumentException("La fecha de fin debe ser posterior a la fecha de inicio");

        return new DateRange(startDate, endDate);
    }

    public bool Contains(DateTime date) => date >= StartDate && date <= EndDate;
}

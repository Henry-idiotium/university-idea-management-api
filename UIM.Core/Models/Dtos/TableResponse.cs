namespace UIM.Core.Models.Dtos;

public class TableResponse
{
    public TableResponse(IEnumerable<object> rows, int? index, int total)
    {
        Rows = rows;
        Index = index ?? 1;
        Total = total;
    }

    public int Index { get; private set; }
    public int Total { get; private set; }
    public IEnumerable<object> Rows { get; private set; }
}
namespace SemestralniPraceDB2.Models.Entities;

public class DBTable
{
    public string TableName { get; set; }
    public int RowCount { get; set; }

    public DBTable(string tableName, int rowsCount)
    {
        TableName = tableName;
        RowCount = rowsCount;
    }

    public DBTable()
    {
    }

    public override string ToString()
    {
        return TableName;
    }
}

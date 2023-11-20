namespace SemestralniPraceDB2.Models.Entities;

internal class DBTable
{
    public string TableName { get; set; }
    public int RowCount { get; set; }

    public DBTable(string tableName, int rowsCount)
    {
        TableName = tableName;
        RowCount = rowsCount;
    }

    public override string ToString()
    {
        return TableName;
    }
}

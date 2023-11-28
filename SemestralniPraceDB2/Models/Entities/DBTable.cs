using CommunityToolkit.Mvvm.ComponentModel;

namespace SemestralniPraceDB2.Models.Entities;

public partial class DBTable : ObservableObject
{
    public string TableName { get; set; }

    [ObservableProperty]
    public int rowCount;

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

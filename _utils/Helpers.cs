using System.Text;

namespace GenerateRawTextToPrint._utils;

internal class Field
{
    public string DisplayLabel { get; set; }
    public string Value { get; set; }
}

internal class FieldSetting
{
    public string Position { get; set; }
    public string DisplaySeq { get; set; }
}

internal static class Helper
{
    public static float ConvertMmToInches(float mm)
    {
        return mm / 25.4f;
    }

    // Function to format a row of data
    public static string FormatRow(string[] columns)
    {
        StringBuilder rowBuilder = new StringBuilder("|");
        foreach (var column in columns)
        {
            rowBuilder.Append($" {column,-15} |"); // Adjust width as needed
        }
        return rowBuilder.ToString();
    }

    // Function to generate a separator row
    public static string FormatSeparator(int columnsCount)
    {
        StringBuilder separatorBuilder = new StringBuilder("|");
        for (int i = 0; i < columnsCount; i++)
        {
            separatorBuilder.Append(new string('-', 17) + "|"); // Adjust width as needed
        }
        return separatorBuilder.ToString();
    }
}



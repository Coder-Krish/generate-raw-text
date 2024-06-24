using GenerateRawTextToPrint._utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace GenerateRawTextToPrint;

public class InvoiceService
{
    public string GenerateInvoiceText(string patientAndInvoiceInfo, string patientAndInvoiceInfoSettings, string invoiceItemsData, float paperWidthMm)
    {
        var patientAndInvoiceInfoData = JsonConvert.DeserializeObject<Dictionary<string, Field>>(patientAndInvoiceInfo);
        var patientAndInvoiceInfoSectionSettings = JsonConvert.DeserializeObject<Dictionary<string, FieldSetting>>(patientAndInvoiceInfoSettings);
        var invoiceItems = JsonConvert.DeserializeObject(invoiceItemsData);
        if(patientAndInvoiceInfoData is null || patientAndInvoiceInfoSectionSettings is null)
        {
            return string.Empty;
        }
        return GenerateRawText(patientAndInvoiceInfoData, patientAndInvoiceInfoSectionSettings, invoiceItems, paperWidthMm);
    }

    private string GenerateRawText(Dictionary<string, Field> data, Dictionary<string, FieldSetting> settings, dynamic invoiceItems, float paperWidthMm)
    {
        // Create a list of items with position and sequence
        var printItems = data.Select(d => new
        {
            DisplayLabel = d.Value.DisplayLabel,
            Value = d.Value.Value,
            Position = settings[d.Key].Position,
            DisplaySeq = int.Parse(settings[d.Key].DisplaySeq)
        }).OrderBy(item => item.DisplaySeq).ToList();

        // Define paper dimensions for A5 in landscape (210mm x 148.5mm)
        float halfPaperWidthMm = paperWidthMm / 2f;
        float fixedRightOffsetMm = halfPaperWidthMm + 20f; // Fixed offset for the right side properties (beyond the midpoint)
        float leftColumnX = 20f; // Fixed X position for left side properties in mm

        // Initialize a dictionary to store the text at specific positions
        var textPositions = new Dictionary<int, List<(int x, string text)>>();

        foreach (var item in printItems)
        {
            var position = item.Position.Split(';');
            float yPos = Helper.ConvertMmToInches(float.Parse(position[1].Replace("mm", ""))) * 10;

            int xPos;
            if (item.DisplaySeq % 2 == 1)
            {
                // Odd DisplaySeq: Place on the left side at fixed X position
                xPos = (int)(Helper.ConvertMmToInches(leftColumnX) * 10);
            }
            else
            {
                // Even DisplaySeq: Place symmetrically on the right side with a fixed offset
                xPos = (int)(Helper.ConvertMmToInches(fixedRightOffsetMm) * 10);
            }

            int row = (int)yPos; // Using yPos as row key

            if (!textPositions.ContainsKey(row))
            {
                textPositions[row] = new List<(int x, string text)>();
            }

            string text = $"{item.DisplayLabel}: {item.Value}";
            textPositions[row].Add((xPos, text));
        }

        // Construct the raw text string
        StringBuilder rawTextBuilder = new StringBuilder();
        foreach (var row in textPositions.OrderBy(kvp => kvp.Key))
        {
            var sortedTexts = row.Value.OrderBy(item => item.x);

            // Track the last x-position to determine the space needed between left and right texts
            int lastXPos = int.MinValue;

            foreach (var item in sortedTexts)
            {
                // Calculate the number of spaces needed before the text
                int spacesCount = item.x - lastXPos;
                if (spacesCount > 0)
                {
                    rawTextBuilder.Append(' ', spacesCount);
                }
                rawTextBuilder.Append(item.text);

                // Update lastXPos
                lastXPos = item.x + item.text.Length;
            }
            rawTextBuilder.AppendLine();
        }

        //Add Invoice Items in the raw text
        if(invoiceItems is not null && invoiceItems.Count > 0)
        {
            rawTextBuilder.AppendLine("\n");
            // Dynamically generate headers based on properties in the first item
            var firstItem = (JObject)invoiceItems.First;
            List<string> headers = new List<string>();
            foreach (var prop in firstItem.Properties())
            {
                headers.Add(prop.Name);
            }

            // Print headers
            rawTextBuilder.AppendLine(Helper.FormatRow(headers.ToArray()));

            // Print separator row
            rawTextBuilder.AppendLine(Helper.FormatSeparator(headers.Count));

            // Print data rows
            foreach (var item in invoiceItems)
            {
                List<string> rowData = new List<string>();
                foreach (var prop in headers)
                {
                    rowData.Add(item[prop].ToString());
                }
                rawTextBuilder.AppendLine(Helper.FormatRow(rowData.ToArray()));
            }
        }

        return rawTextBuilder.ToString();
    }   
}


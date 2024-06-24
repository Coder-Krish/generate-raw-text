using GenerateRawTextToPrint;

namespace HospitalInvoicePrinter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define paper dimensions for A5 in landscape (210mm x 148.5mm)
            float paperWidthMm = 210f;
            // Sample JSON data
            var patientAndinvoiceDetails = @"{
                'HospitalNo': { 'DisplayLabel': 'HospitalNo', 'value': '24000624' },
                'PatientName': { 'DisplayLabel': 'PatientName', 'value': 'Monday New Patient' },
                'Address': { 'DisplayLabel': 'Address', 'value': 'Dillibazar, Kathmandu' },
                'AgeSex': { 'DisplayLabel': 'Age/Sex', 'value': '19Y/M' },
                'Contact': { 'DisplayLabel': 'Phone Number', 'value': '980000000' },
                'PolicyNo': { 'DisplayLabel': 'Policy/Member No', 'value': '1720256' },
                'Type': { 'DisplayLabel': 'Scheme', 'value': 'General' },
                'InvoiceNo': { 'DisplayLabel': 'InvoiceNo', 'value': '80/81-BL23456' },
                'InvoiceDate': { 'DisplayLabel': 'InvoiceDate', 'value': '2024-06-24' },
                'ClaimCode': { 'DisplayLabel': 'ClaimCode', 'value': '12365478' },
                'Department': { 'DisplayLabel': 'Dept', 'value': 'ENT' },
                'PaymentMode': { 'DisplayLabel': 'Method Of Payment', 'value': 'Cash' }
            }";

            // Sample settings
            var patientAndInvoiceSettings = @"{
                'HospitalNo': { 'Position': '20mm;20mm', 'DisplaySeq': '1' },
                'PatientName': { 'Position': '20mm;40mm', 'DisplaySeq': '3' },
                'Address': { 'Position': '20mm;60mm', 'DisplaySeq': '5' },
                'AgeSex': { 'Position': '20mm;80mm', 'DisplaySeq': '7' },
                'Contact': { 'Position': '20mm;100mm', 'DisplaySeq': '9' },
                'PolicyNo': { 'Position': '20mm;120mm', 'DisplaySeq': '11' },
                'Type': { 'Position': '20mm;140mm', 'DisplaySeq': '13' },
                'InvoiceNo': { 'Position': '105mm;20mm', 'DisplaySeq': '2' },
                'InvoiceDate': { 'Position': '105mm;40mm', 'DisplaySeq': '4' },
                'ClaimCode': { 'Position': '105mm;60mm', 'DisplaySeq': '6' },
                'Department': { 'Position': '105mm;80mm', 'DisplaySeq': '8' },
                'PaymentMode': { 'Position': '105mm;100mm', 'DisplaySeq': '10' }
            }";

            var invoiceItemsData = @"[
		{
			""SN"": 1,
			""ItemName"": ""CBC"",
			""Quantity"": 1,
			""DiscountPercent"":0,
			""DiscountAmount"": 0,
			""TotalAmount"": 250
		},
		{
			""SN"": 2,
			""ItemName"": ""RFT"",
			""Quantity"": 1,
			""DiscountPercent"":0,
			""DiscountAmount"": 0,
			""TotalAmount"": 250
		},
		{
			""SN"": 3,
			""ItemName"": ""Bilirubin"",
			""Quantity"": 1,
			""DiscountPercent"":0,
			""DiscountAmount"": 0,
			""TotalAmount"": 250
		},
		{
			""SN"": 4,
			""ItemName"": ""X-Ray"",
			""Quantity"": 1,
			""DiscountPercent"":0,
			""DiscountAmount"": 0,
			""TotalAmount"": 250
		},
		{
			""SN"": 5,
			""ItemName"": ""Ultra Sound"",
			""Quantity"": 1,
			""DiscountPercent"":0,
			""DiscountAmount"": 0,
			""TotalAmount"": 250
		}			
	]";

            InvoiceService invoiceService = new InvoiceService();
            string invoiceText = invoiceService.GenerateInvoiceText(patientAndinvoiceDetails, patientAndInvoiceSettings, invoiceItemsData, paperWidthMm);

            Console.WriteLine(invoiceText);
        }
    }
}

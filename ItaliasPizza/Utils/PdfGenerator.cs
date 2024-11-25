using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;

namespace ItaliasPizza.Utils
{
	public class PdfGenerator
	{
		public static void GeneratePdfReport(List<SupplyInventoryReportDetails> supplies)
		{
			var fileName = $"ReporteInventario_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
			var filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + fileName;

			// -- DEFINE CUSTOM COLUMN NAMES -- //
			var columns = new Dictionary<string, string>()
			{
				{ "SupplyName", "Nombre insumo"},
				{ "MeasurementUnit", "Unidad de Medida" },
				{ "ExpectedAmount", "Cantidad Esperada" },
				{ "ReportedAmount", "Cantidad Reportada" },
				{ "DifferingAmountReason", "Razón de Diferencia" }
			};

			PdfWriter writer = new PdfWriter(filePath);
			PdfDocument pdf = new PdfDocument(writer);
			Document document = new Document(pdf);

			document.Add(new Paragraph("Italias's Pizza - Reporte de Inventario")
				.SetTextAlignment(TextAlignment.CENTER)
				.SetFontSize(20));

			document.Add(new Paragraph("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy"))
				.SetTextAlignment(TextAlignment.CENTER)
				.SetFontSize(14)
				.SetMarginBottom(20));

			var properties = typeof(SupplyInventoryReportDetails).GetProperties();

			Table table = new Table(columns.Count, true);
			table.SetWidth(UnitValue.CreatePercentValue(100));

			// -- ADD COLUMNS -- //
			foreach (var column in columns.Values)
			{
				table.AddHeaderCell(new Cell()
					.Add(new Paragraph(column))
					.SetBackgroundColor(ColorConstants.LIGHT_GRAY)
					.SetTextAlignment(TextAlignment.CENTER));
			}

			// -- ADD DATA -- //
			foreach (var supply in supplies)
			{
				foreach (var column in columns.Keys)
				{
					var propertyInfo = typeof(SupplyInventoryReportDetails).GetProperty(column);

					if (propertyInfo != null)
					{
						string value = propertyInfo.GetValue(supply).ToString() ?? "";

						if (column == "ReportedAmount" && value == "0")
							value = "";

						table.AddCell(
							new Cell()
								.Add(new Paragraph(value))
								.SetTextAlignment(TextAlignment.CENTER));
					}
				}
			}

			document.Add(table);

			// -- Observations section -- //
			document.Add(
				new Paragraph("Observaciones:")
					.SetTextAlignment(TextAlignment.LEFT)
					.SetFontSize(14)
					.SetMarginTop(20));

			var rectangle = new Table(1)
				.SetWidth(UnitValue.CreatePercentValue(100))
				.SetHeight(80)
				.SetBorder(new SolidBorder(1));

			rectangle.AddCell(
				new Cell()
					.SetBorder(Border.NO_BORDER));

			document.Add(rectangle);

			document.Close();
			pdf.Close();
			writer.Close();
		}
	}
}

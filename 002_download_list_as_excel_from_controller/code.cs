//EPPlus 5.0.4

[HttpGet("ExportToExcel")]
public async Task<IActionResult> ExportToExcel(string parameter)
{
    var data = await _service.GetData(parameter);
    var stream = new MemoryStream();
    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//or commercial if you have a license
    
    using (var package = new ExcelPackage(stream))
    {
        var workSheet = package.Workbook.Worksheets.Add("Sheet1");
        workSheet.Cells.LoadFromCollection(data, true);
        package.Save();
    }
    
    stream.Position = 0;
    string excelName = $"thelist-{DateTime.Now.ToString("yyyyMMdd")}.xlsx";

    return File(stream, "application/octet-stream", excelName);
}

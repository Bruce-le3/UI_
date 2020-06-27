using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using NPOI.HSSF.UserModel;
//using NPOI.XSSF.UserModel;
using System.Data;
using System.IO;
//using NPOI.SS.UserModel;
using System.Windows.Forms;
//using NPOI.HPSF;
//using NPOI.POIFS.FileSystem;

namespace Justech
{
    class ReadXls
    {

        //public static DataTable GetDgvToTable(DataGridView dgv)
        //{
        //    DataTable dt = new DataTable();

        //    // 列强制转换
        //    for (int count = 0; count < dgv.Columns.Count; count++)
        //    {
        //        DataColumn dc = new DataColumn(dgv.Columns[count].Name.ToString());
        //        dt.Columns.Add(dc);
        //    }

        //    // 循环行
        //    for (int count = 0; count < dgv.Rows.Count; count++)
        //    {
        //        DataRow dr = dt.NewRow();
        //        for (int countsub = 0; countsub < dgv.Columns.Count; countsub++)
        //        {
        //            dr[countsub] = Convert.ToString(dgv.Rows[count].Cells[countsub].Value);
        //        }
        //        dt.Rows.Add(dr);
        //    }
        //    return dt;
        //}

        ////数据路径
        //public static string filePath = null;
        ///// <summary>

        ///// Excel转换成DataTable（.xls）

        ///// </summary>

        ///// <param name="filePath">Excel文件路径</param>

        ///// <returns></returns>

        //public static DataTable ExcelToDataTable()
        //{

        //    var dt = new DataTable();

        //    using (var file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        //    {

        //        //var hssfworkbook = new HSSFWorkbook(file);

        //        var sheet = hssfworkbook.GetSheetAt(0);

        //        for (var j = 0; j < 5; j++)
        //        {

        //            dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());

        //        }

        //        var rows = sheet.GetRowEnumerator();

        //        while (rows.MoveNext())
        //        {

        //            var row = (HSSFRow)rows.Current;

        //            var dr = dt.NewRow();

        //            for (var i = 0; i < row.LastCellNum; i++)
        //            {

        //                var cell = row.GetCell(i);

        //                if (cell == null)
        //                {

        //                    dr[i] = null;

        //                }

        //                else
        //                {

        //                    switch (cell.CellType)
        //                    {

        //                        case CellType.BLANK:

        //                            dr[i] = "[null]";

        //                            break;

        //                        case CellType.BOOLEAN:

        //                            dr[i] = cell.BooleanCellValue;

        //                            break;

        //                        case CellType.NUMERIC:

        //                            dr[i] = cell.ToString();

        //                            break;

        //                        case CellType.STRING:

        //                            dr[i] = cell.StringCellValue;

        //                            break;

        //                        case CellType.ERROR:

        //                            dr[i] = cell.ErrorCellValue;

        //                            break;

        //                        case CellType.FORMULA:

        //                            try
        //                            {

        //                                dr[i] = cell.NumericCellValue;

        //                            }

        //                            catch
        //                            {

        //                                dr[i] = cell.StringCellValue;

        //                            }

        //                            break;

        //                        default:

        //                            dr[i] = "=" + cell.CellFormula;

        //                            break;

        //                    }

        //                }

        //            }

        //            dt.Rows.Add(dr);

        //        }

        //    }

        //    return dt;

        //}



        ///// <summary>

        ///// Excel转换成DataSet（.xlsx/.xls）

        ///// </summary>

        ///// <param name="filePath">Excel文件路径</param>

        ///// <param name="strMsg"></param>

        ///// <returns></returns>

        //public static DataSet ExcelToDataSet(out string strMsg)
        //{

        //    strMsg = "";

        //    DataSet ds = new DataSet();

        //    DataTable dt = new DataTable();

        //    string fileType = Path.GetExtension(filePath).ToLower();

        //    string fileName = Path.GetFileName(filePath).ToLower();

        //    try
        //    {

        //        ISheet sheet = null;

        //        int sheetNumber = 0;

        //        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        //        if (fileType == ".xlsx")
        //        {

        //            // 2007版本

        //            XSSFWorkbook workbook = new XSSFWorkbook(fs);

        //            sheetNumber = workbook.NumberOfSheets;

        //            for (int i = 0; i < sheetNumber; i++)
        //            {

        //                string sheetName = workbook.GetSheetName(i);

        //                sheet = workbook.GetSheet(sheetName);

        //                if (sheet != null)
        //                {

        //                    dt = GetSheetDataTable(sheet, out strMsg);

        //                    if (dt != null)
        //                    {

        //                        dt.TableName = sheetName.Trim();

        //                        ds.Tables.Add(dt);

        //                    }

        //                    else
        //                    {

        //                        //  MessageBox.Show("Sheet数据获取失败，原因：" + strMsg);

        //                    }

        //                }

        //            }

        //        }

        //        else if (fileType == ".xls")
        //        {

        //            // 2003版本

        //            HSSFWorkbook workbook = new HSSFWorkbook(fs);

        //            sheetNumber = workbook.NumberOfSheets;

        //            for (int i = 0; i < sheetNumber; i++)
        //            {

        //                string sheetName = workbook.GetSheetName(i);

        //                sheet = workbook.GetSheet(sheetName);

        //                if (sheet != null)
        //                {

        //                    dt = GetSheetDataTable(sheet, out strMsg);

        //                    if (dt != null)
        //                    {

        //                        dt.TableName = sheetName.Trim();

        //                        ds.Tables.Add(dt);

        //                    }

        //                    else
        //                    {

        //                        //   MessageBox.Show("Sheet数据获取失败，原因：" + strMsg);

        //                    }

        //                }

        //            }

        //        }

        //        return ds;

        //    }

        //    catch (Exception ex)
        //    {

        //        strMsg = ex.Message;

        //        return null;

        //    }

        //}

        ///// <summary>

        ///// 获取sheet表对应的DataTable

        ///// </summary>

        ///// <param name="sheet">Excel工作表</param>

        ///// <param name="strMsg">弹框信息</param>

        ///// <returns></returns>

        //public static DataTable GetSheetDataTable(ISheet sheet, out string strMsg)
        //{

        //    strMsg = "";

        //    DataTable dt = new DataTable();

        //    string sheetName = sheet.SheetName;

        //    int startIndex = 0;// sheet.FirstRowNum;

        //    int lastIndex = sheet.LastRowNum;

        //    //最大列数

        //    int cellCount = 0;

        //    IRow maxRow = sheet.GetRow(0);

        //    for (int i = startIndex; i <= lastIndex; i++)
        //    {

        //        IRow row = sheet.GetRow(i);

        //        if (row != null && cellCount < row.LastCellNum)
        //        {

        //            cellCount = row.LastCellNum;

        //            maxRow = row;

        //        }

        //    }

        //    //列名设置

        //    try
        //    {

        //        for (int i = 0; i < maxRow.LastCellNum; i++)//maxRow.FirstCellNum
        //        {

        //            dt.Columns.Add(Convert.ToChar(((int)'A') + i).ToString());

        //            //DataColumn column = new DataColumn("Column" + (i + 1).ToString());

        //            //dt.Columns.Add(column);

        //        }

        //    }

        //    catch
        //    {

        //        strMsg = "工作表" + sheetName + "中无数据";

        //        return null;

        //    }

        //    //数据填充

        //    for (int i = startIndex; i <= lastIndex; i++)
        //    {

        //        IRow row = sheet.GetRow(i);

        //        DataRow drNew = dt.NewRow();

        //        if (row != null)
        //        {

        //            for (int j = row.FirstCellNum; j < row.LastCellNum; ++j)
        //            {

        //                if (row.GetCell(j) != null)
        //                {

        //                    ICell cell = row.GetCell(j);

        //                    switch (cell.CellType)
        //                    {

        //                        case CellType.BLANK:

        //                            drNew[j] = "";

        //                            break;

        //                        case CellType.NUMERIC:

        //                            short format = cell.CellStyle.DataFormat;

        //                            //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理 

        //                            if (format == 14 || format == 31 || format == 57 || format == 58)

        //                                drNew[j] = cell.DateCellValue;

        //                            else

        //                                drNew[j] = cell.NumericCellValue;

        //                            if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 188)

        //                                drNew[j] = cell.NumericCellValue.ToString("#0.00");

        //                            break;

        //                        case CellType.STRING:

        //                            drNew[j] = cell.StringCellValue;

        //                            break;

        //                        case CellType.FORMULA:

        //                            try
        //                            {

        //                                drNew[j] = cell.NumericCellValue;

        //                                if (cell.CellStyle.DataFormat == 177 || cell.CellStyle.DataFormat == 178 || cell.CellStyle.DataFormat == 188)

        //                                    drNew[j] = cell.NumericCellValue.ToString("#0.00");

        //                            }

        //                            catch
        //                            {

        //                                try
        //                                {

        //                                    drNew[j] = cell.StringCellValue;

        //                                }

        //                                catch { }

        //                            }

        //                            break;

        //                        default:

        //                            drNew[j] = cell.StringCellValue;

        //                            break;

        //                    }

        //                }

        //            }

        //        }

        //        dt.Rows.Add(drNew);

        //    }

        //    return dt;

        //}
        //public static string Read_data(string Sheet, int Row, int Col)
        //{
        //    string rtn_data = "";
        //    try
        //    {
        //        string msg = "";
        //         rtn_data = ExcelToDataSet(out msg).Tables[Sheet].Rows[Row][Col].ToString();
        //    }
        //    catch 
        //    {
        //        rtn_data = "Exception";
        //    }

        //    return rtn_data;
        //}

        //static ICell cell = null;
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="sheetname">Sheet</param>
        ///// <param name="row">行</param>
        ///// <param name="column">列</param>
        ///// <param name="value">内容</param>
        ///// <returns></returns>
        //public static bool setExcelCellValue(String sheetname, int row, int column, String value)
        //{
        //    bool returnb = false;
        //    try
        //    {
        //        IWorkbook wk = null;
        //        using (FileStream fs = File.Open(filePath, FileMode.Open,
        //        FileAccess.Read, FileShare.ReadWrite))
        //        {
        //            if (filePath.Contains(".xlsx"))
        //            {

        //                wk = new XSSFWorkbook(fs);
        //            }
        //            else
        //            {
        //                //把xls文件读入workbook变量里，之后就可以关闭了  
        //                wk = new HSSFWorkbook(fs);
        //            }
        //            fs.Close();
        //        }
        //        //把xls文件读入workbook变量里，之后就可以关闭了  
        //        ISheet sheet = wk.GetSheet(sheetname);



        //        GetCellValue(cell);



        //        cell = sheet.GetRow(row).GetCell(column);
        //        cell.SetCellValue(value);
        //        using (FileStream fileStream = File.Create(filePath))
        //        {
        //            wk.Write(fileStream);
        //            fileStream.Close();
        //        }
        //        returnb = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        returnb = false;
        //        //  throw;
        //    }
        //    return returnb;
        //}
        //public static String getExcelCellValue(String sheetname, int row, int column)
        //{
        //    String returnStr = null;
        //    try
        //    {
        //        IWorkbook wk = null;
        //        using (FileStream fs = File.Open(filePath, FileMode.Open,
        //        FileAccess.Read, FileShare.ReadWrite))
        //        {
        //            //把xls文件读入workbook变量里，之后就可以关闭了  
        //            if (filePath.Contains(".xlsx"))
        //            {

        //                wk = new XSSFWorkbook(fs);
        //            }
        //            else
        //            {
        //                //把xls文件读入workbook变量里，之后就可以关闭了  
        //                wk = new HSSFWorkbook(fs);
        //            }
        //            fs.Close();
        //        }
        //        //把xls文件读入workbook变量里，之后就可以关闭了  
        //        ISheet sheet = wk.GetSheet(sheetname);
        //        ICell cell = sheet.GetRow(row).GetCell(column);
        //        returnStr = cell.ToString();
        //    }
        //    catch (Exception)
        //    {
        //        returnStr = "Exception";
        //     //   throw;
        //    }
        //    return returnStr;
        //}

        ////获取cell的数据，并设置为对应的数据类型
        //public static object GetCellValue(ICell cell)
        //{
        //    object value = null;
        //    try
        //    {
        //        if (cell.CellType != CellType.BLANK)
        //        {
        //            switch (cell.CellType)
        //            {
        //                case CellType.NUMERIC:
        //                    // Date comes here
        //                    if (DateUtil.IsCellDateFormatted(cell))
        //                    {
        //                        value = cell.DateCellValue;
        //                    }
        //                    else
        //                    {
        //                        // Numeric type
        //                        value = cell.NumericCellValue;
        //                    }
        //                    break;
        //                case CellType.BOOLEAN:
        //                    // Boolean type
        //                    value = cell.BooleanCellValue;
        //                    break;
        //                case CellType.FORMULA:
        //                    value = cell.CellFormula;
        //                    break;
        //                default:
        //                    // String type
        //                    value = cell.StringCellValue;
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        value = "";
        //    }

        //    return value;
        //}
        ////根据数据类型设置不同类型的cell
        //public static void SetCellValue(ICell cell, object obj)
        //{
        //    if (obj.GetType() == typeof(int))
        //    {
        //        cell.SetCellValue((int)obj);
        //    }
        //    else if (obj.GetType() == typeof(double))
        //    {
        //        cell.SetCellValue((double)obj);
        //    }
        //    else if (obj.GetType() == typeof(IRichTextString))
        //    {
        //        cell.SetCellValue((IRichTextString)obj);
        //    }
        //    else if (obj.GetType() == typeof(string))
        //    {
        //        cell.SetCellValue(obj.ToString());
        //    }
        //    else if (obj.GetType() == typeof(DateTime))
        //    {
        //        cell.SetCellValue((DateTime)obj);
        //    }
        //    else if (obj.GetType() == typeof(bool))
        //    {
        //        cell.SetCellValue((bool)obj);
        //    }
        //    else
        //    {
        //        cell.SetCellValue(obj.ToString());
        //    }
        //}



        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="path">路径</param>
        ///// <param name="SheetName">Sheetname</param>
        //public static void Creat_excel(string path,string[] SheetName)
        //{
         
        //    try
        //    {

        //        IWorkbook wk = null;
        //        if (path.Contains(".xlsx"))
        //        {

        //            wk = new XSSFWorkbook();
        //        }
        //        else
        //        {
                   
        //            wk = new HSSFWorkbook();
        //        }


        //        wk.CreateSheet(SheetName[0]);  //新建1个Sheet工作表

        //        if (SheetName.Length > 1)
        //        {

        //            wk.CreateSheet(SheetName[1]);
        //            wk.CreateSheet(SheetName[2]);
        //        }
        //        FileStream file_excel = new FileStream(path, FileMode.Create);
        //        wk.Write(file_excel);
        //        file_excel.Close();  //关闭文件流
        //    }
        //    catch 
        //    {
        //    }
        //}
    }
}

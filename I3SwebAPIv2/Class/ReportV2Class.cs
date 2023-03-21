using I3SwebAPIv2.Resources;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;

namespace I3SwebAPIv2.Class
{
    public class ReportV2Class
    {
        public string ExportClassAttendance(DataTable dtExcel, int year, int month, string create_by, string culture)
        {
            string value = string.Empty;
            CultureInfo ci = new CultureInfo(culture);

            object misValue = System.Reflection.Missing.Value;
            string school_name = string.Empty;
            string class_name = string.Empty;
            string prev_name = string.Empty;

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                try
                {
                    ExcelWorksheet worKsheeT = package.Workbook.Worksheets.Add(WebApiResources.Report);

                    int days = DateTime.DaysInMonth(year, month);
                    int totalCols = days + 6;
                    int scndLastCols = totalCols - 1;

                    Bitmap image = new Bitmap(HttpContext.Current.Server.MapPath("~") + "\\Images\\ic_logo_small.png");
                    var pic = worKsheeT.Drawings.AddPicture("logo", image);
                    pic.SetPosition(1, 0, 27, 0); // Position Row, RowOffsetPixels, Column, ColumnOffsetPixels


                    worKsheeT.Cells[5, 1,5, 2].Merge = true;
                    worKsheeT.Cells[5, 1].Value = WebApiResources.PreparedBy;
                    worKsheeT.Cells[5, 3].Value = ":";
                    worKsheeT.Cells[5, 4,5, 14].Merge = true;
                    worKsheeT.Cells[5, 4].Value = create_by;

                    worKsheeT.Cells[3, 16, 3, 18].Merge = true;
                    worKsheeT.Cells[3, 16].Value = WebApiResources.Month;
                    worKsheeT.Cells[4, 16, 4, 18].Merge = true;
                    worKsheeT.Cells[4, 16].Value = WebApiResources.Year;
                    worKsheeT.Cells[5, 16, 5, 18].Merge = true;
                    worKsheeT.Cells[5, 16].Value = WebApiResources.PrintedDateTime;

                    worKsheeT.Cells[3, 19].Value = ":";
                    worKsheeT.Cells[4, 19].Value = ":";
                    worKsheeT.Cells[5, 19].Value = ":";

                    worKsheeT.Cells[3, 20, 3, 22].Merge = true;
                    worKsheeT.Cells[3, 20].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                    worKsheeT.Cells[4, 20, 4, 22].Merge = true;
                    worKsheeT.Cells[4, 20].Value = year;
                    worKsheeT.Cells[4, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worKsheeT.Cells[5, 20, 5, 22].Merge = true;
                    worKsheeT.Cells[5, 20].Value = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt", ci);
                    worKsheeT.Cells[5, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    worKsheeT.Cells.Style.Font.Size = 12;
                    worKsheeT.Cells[8, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worKsheeT.Cells[8, 1].Value = WebApiResources.No;
                    worKsheeT.Column(1).Width = 4;
                    worKsheeT.Cells[8, 2].Value = WebApiResources.StudentName;
                    worKsheeT.Column(2).Width = 30;

                    int d = 1;

                    for (int i = 3; i < (days + 3); i++)
                    {
                        DateTime dt = DateTime.ParseExact(d.ToString().PadLeft(2,'0') + "/" + month.ToString().PadLeft(2, '0') + "/" + year.ToString(), "dd/MM/yyyy" , ci);
                        DayOfWeek dow = dt.DayOfWeek;
                        worKsheeT.Cells[7, i].Value = dt.ToString("ddd", ci);
                        worKsheeT.Cells[7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worKsheeT.Cells[8, i].Value = d;
                        worKsheeT.Cells[8, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worKsheeT.Column(i).Width = 4;
                        if (dow == DayOfWeek.Sunday || dow == DayOfWeek.Saturday)
                        {
                            worKsheeT.Cells[7, i].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                            worKsheeT.Cells[8, i].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                        }
                        else
                        {
                            worKsheeT.Cells[7, i].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                            worKsheeT.Cells[8, i].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        }

                        d++;
                    }

                    worKsheeT.Cells[7, totalCols - 3,7, totalCols].Merge = true;
                    worKsheeT.Cells[8, totalCols - 3].Value = "X";
                    worKsheeT.Cells[8, totalCols - 2].Value = "E";
                    worKsheeT.Cells[8, totalCols - 1].Value = "L";
                    worKsheeT.Cells[8, totalCols].Value = "\u2713";
                    worKsheeT.Cells[8, totalCols - 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worKsheeT.Cells[8, totalCols - 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worKsheeT.Cells[8, totalCols - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worKsheeT.Cells[8, totalCols].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worKsheeT.Column(totalCols - 3).Width = 4;
                    worKsheeT.Column(totalCols - 2).Width = 4;
                    worKsheeT.Column(totalCols - 1).Width = 4;
                    worKsheeT.Column(totalCols).Width = 4;

                    int rowno = 1;
                    int rowcount = 9;
                    int X = 0;
                    int E = 0;
                    int L = 0;
                    int P = 0;

                    foreach (DataRow datarow in dtExcel.Rows)
                    {
                        school_name = datarow[3].ToString();
                        class_name = datarow[4].ToString();
                        prev_name = (string)worKsheeT.Cells[rowcount - 1, 2].Value;

                        if (prev_name == datarow[0].ToString())
                        {
                            DateTime att = Convert.ToDateTime(datarow[1].ToString());
                            int eDay = att.Day;
                            if (datarow[2].ToString() == "P")
                            {
                                worKsheeT.Cells[rowcount - 1, eDay + 2].Value = "\u2713";
                            }
                            else if (datarow[2].ToString() == "U")
                            {
                                worKsheeT.Cells[rowcount - 1, eDay + 2].Value = "X";
                            }
                            else
                            {
                                worKsheeT.Cells[rowcount - 1, eDay + 2].Value = datarow[2].ToString();
                            }

                            worKsheeT.Cells[rowcount - 1, eDay + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            worKsheeT.Cells[rowcount - 1, totalCols - 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worKsheeT.Cells[rowcount - 1, totalCols - 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worKsheeT.Cells[rowcount - 1, totalCols - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worKsheeT.Cells[rowcount - 1, totalCols].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            if (datarow[2].ToString() == "X")
                            {
                                X++;
                            }
                            else if (datarow[2].ToString() == "E")
                            {
                                E++;
                            }
                            else if (datarow[2].ToString() == "L")
                            {
                                L++;
                            }
                            else if (datarow[2].ToString() == "P")
                            {
                                P++;
                            }
                            worKsheeT.Cells[rowcount - 1, totalCols - 3].Value = X;
                            worKsheeT.Cells[rowcount - 1, totalCols - 2].Value = E;
                            worKsheeT.Cells[rowcount - 1, totalCols - 1].Value = L;
                            worKsheeT.Cells[rowcount - 1, totalCols].Value = P;
                        }
                        else
                        {
                            L = 0;
                            X = 0;
                            E = 0;
                            P = 0;
                            worKsheeT.Cells[rowcount, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worKsheeT.Cells[rowcount, 1].Value = rowno.ToString();
                            worKsheeT.Cells[rowcount, 2].Value = datarow[0].ToString();
                            DateTime att = Convert.ToDateTime(datarow[1].ToString());
                            int eDay = att.Day;
                            if (datarow[2].ToString() == "P")
                            {
                                worKsheeT.Cells[rowcount, eDay + 2].Value = "\u2713";
                            }
                            else if (datarow[2].ToString() == "U")
                            {
                                worKsheeT.Cells[rowcount, eDay + 2].Value = "X";
                            }
                            else
                            {
                                worKsheeT.Cells[rowcount, eDay + 2].Value = datarow[2].ToString();
                            }

                            worKsheeT.Cells[rowcount, eDay + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            worKsheeT.Cells[rowcount, totalCols - 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worKsheeT.Cells[rowcount, totalCols - 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worKsheeT.Cells[rowcount, totalCols - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worKsheeT.Cells[rowcount, totalCols].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            if (datarow[2].ToString() == "X")
                            {
                                X++;
                            }
                            else if (datarow[2].ToString() == "E")
                            {
                                E++;
                            }
                            else if (datarow[2].ToString() == "L")
                            {
                                L++;
                            }
                            else if (datarow[2].ToString() == "P")
                            {
                                P++;
                            }
                            worKsheeT.Cells[rowcount, totalCols - 3].Value = X;
                            worKsheeT.Cells[rowcount, totalCols - 2].Value = E;
                            worKsheeT.Cells[rowcount, totalCols - 1].Value = L;
                            worKsheeT.Cells[rowcount, totalCols].Value = P;

                            rowcount++;
                            rowno++;
                        }

                    }

                    worKsheeT.Cells[3, 1,3, 2].Merge = true;
                    worKsheeT.Cells[3, 1].Value = WebApiResources.SchoolName;
                    worKsheeT.Cells[4, 1,4, 2].Merge = true;
                    worKsheeT.Cells[4, 1].Value = WebApiResources.ClassName;

                    worKsheeT.Cells[3, 3].Value = ":";
                    worKsheeT.Cells[4, 3].Value = ":";

                    worKsheeT.Cells[3, 4,3, 14].Merge = true;
                    worKsheeT.Cells[3, 4].Value = school_name;
                    worKsheeT.Cells[4, 4,4, 14].Merge = true;
                    worKsheeT.Cells[4, 4].Value = class_name;

                    worKsheeT.Cells[rowcount + 1, 1,rowcount + 1, totalCols].Merge = true;
                    worKsheeT.Cells[rowcount + 1, 1].Value = WebApiResources.Enter + ": X = " + WebApiResources.Absent + ", E = " + WebApiResources.Excused + ", L = " + WebApiResources.Late + " or \u2713 = " + WebApiResources.Present;
                    worKsheeT.Cells[rowcount + 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worKsheeT.Cells[7, 1, rowcount - 1, totalCols].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worKsheeT.Cells.AutoFitColumns();

                    value = "/Reports/Attendances/" + class_name.Replace(" ", "_") + "_" + month.ToString().PadLeft(2, '0') + "" + year + "_" + DateTime.Now.ToString("ddMMyyHHmmss", ci) + ".xlsx";
                    package.SaveAs(new FileInfo(HttpContext.Current.Server.MapPath("~") + "\\Reports\\Attendances\\" + class_name.Replace(" ", "_") + "_" + month.ToString().PadLeft(2, '0') + "" + year + "_" + DateTime.Now.ToString("ddMMyyHHmmss", ci) + ".xlsx"));
                }
                catch (Exception ex)
                {
                    value = ex.Message;
                }
                finally
                {

                }
                return value;
            }
            
        }

        public string ExportStaffAttendance(DataTable dtExcel, int year, int month, string create_by, string culture)
        {
            CultureInfo ci = new CultureInfo(culture);

            object misValue = System.Reflection.Missing.Value;
            string school_name = string.Empty;
            string shift_code = string.Empty;
            string prev_name = string.Empty;
            string value = string.Empty;

            // If you use EPPlus in a noncommercial context
            // according to the Polyform Noncommercial license:
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                try
                {

                    ExcelWorksheet worKsheeT = package.Workbook.Worksheets.Add(WebApiResources.Report);

                    int days = DateTime.DaysInMonth(year, month);
                    int totalCols = days + 6;
                    int scndLastCols = totalCols - 1;

                    Bitmap image = new Bitmap(HttpContext.Current.Server.MapPath("~") + "\\Images\\ic_logo_small.png");
                    var pic = worKsheeT.Drawings.AddPicture("logo", image);
                    pic.SetPosition(1, 0, 27, 0); // Position Row, RowOffsetPixels, Column, ColumnOffsetPixels

                    worKsheeT.Cells[5, 1, 5, 2].Merge = true;
                    worKsheeT.Cells[5, 1].Value = WebApiResources.PreparedBy;
                    worKsheeT.Cells[5, 3].Value = ":";
                    worKsheeT.Cells[5, 4, 5, 14].Merge = true;
                    worKsheeT.Cells[5, 4].Value = create_by;

                    worKsheeT.Cells[3, 16, 3, 18].Merge = true;
                    worKsheeT.Cells[3, 16].Value = WebApiResources.Month;
                    worKsheeT.Cells[4, 16, 4, 18].Merge = true;
                    worKsheeT.Cells[4, 16].Value = WebApiResources.Year;
                    worKsheeT.Cells[5, 16, 5, 18].Merge = true;
                    worKsheeT.Cells[5, 16].Value = WebApiResources.PrintedDateTime;

                    worKsheeT.Cells[3, 19].Value = ":";
                    worKsheeT.Cells[4, 19].Value = ":";
                    worKsheeT.Cells[5, 19].Value = ":";

                    worKsheeT.Cells[3, 20, 3, 22].Merge = true;
                    worKsheeT.Cells[3, 20].Value = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
                    worKsheeT.Cells[4, 20, 4, 22].Merge = true;
                    worKsheeT.Cells[4, 20].Value = year;
                    worKsheeT.Cells[4, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    worKsheeT.Cells[5, 20, 5, 22].Merge = true;
                    worKsheeT.Cells[5, 20].Value = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt", ci);
                    worKsheeT.Cells[5, 20].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

                    worKsheeT.Cells.Style.Font.Size = 12;

                    worKsheeT.Cells[7, 1, 7, 2].Merge = true;
                    worKsheeT.Cells[8, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worKsheeT.Cells[8, 1].Value = WebApiResources.No;
                    worKsheeT.Column(1).Width = 4;
                    worKsheeT.Cells[8, 2].Value = WebApiResources.StaffName;
                    worKsheeT.Column(2).Width = 30;

                    int d = 1;

                    for (int i = 3; i < (days + 3); i++)
                    {
                        DateTime dt = DateTime.ParseExact(d.ToString().PadLeft(2, '0') + "/" + month.ToString().PadLeft(2, '0') + "/" + year.ToString(), "dd/MM/yyyy", ci);
                        DayOfWeek dow = dt.DayOfWeek;
                        worKsheeT.Cells[7, i].Value = dt.ToString("ddd", ci);
                        worKsheeT.Cells[7, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worKsheeT.Cells[8, i].Value = d;
                        worKsheeT.Cells[8, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worKsheeT.Column(i).Width = 4;
                        if (dow == DayOfWeek.Sunday || dow == DayOfWeek.Saturday)
                        {
                            worKsheeT.Cells[7, i].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                            worKsheeT.Cells[8, i].Style.Font.Color.SetColor(System.Drawing.Color.Red);
                        }
                        else
                        {
                            worKsheeT.Cells[7, i].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                            worKsheeT.Cells[8, i].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        }

                        d++;
                    }

                    worKsheeT.Cells[7, totalCols - 3, 7, totalCols].Merge = true;
                    worKsheeT.Cells[8, totalCols - 3].Value = "X";
                    worKsheeT.Cells[8, totalCols - 2].Value = "E";
                    worKsheeT.Cells[8, totalCols - 1].Value = "L";
                    worKsheeT.Cells[8, totalCols].Value = "\u2713";
                    worKsheeT.Cells[8, totalCols - 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worKsheeT.Cells[8, totalCols - 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worKsheeT.Cells[8, totalCols - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worKsheeT.Cells[8, totalCols].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worKsheeT.Column(totalCols - 3).Width = 4;
                    worKsheeT.Column(totalCols - 2).Width = 4;
                    worKsheeT.Column(totalCols - 1).Width = 4;
                    worKsheeT.Column(totalCols).Width = 4;

                    int rowno = 1;
                    int rowcount = 9;
                    int X = 0;
                    int E = 0;
                    int L = 0;
                    int P = 0;

                    foreach (DataRow datarow in dtExcel.Rows)
                    {
                        school_name = datarow[3].ToString();
                        shift_code = datarow[4].ToString();
                        prev_name = (string)worKsheeT.Cells[rowcount - 1, 2].Value;

                        if (prev_name == datarow[0].ToString())
                        {
                            DateTime att = Convert.ToDateTime(datarow[1].ToString());
                            int eDay = att.Day;
                            if (datarow[2].ToString() == "P")
                            {
                                worKsheeT.Cells[rowcount - 1, eDay + 2].Value = "\u2713";
                            }
                            else if (datarow[2].ToString() == "U")
                            {
                                worKsheeT.Cells[rowcount - 1, eDay + 2].Value = "X";
                            }
                            else
                            {
                                worKsheeT.Cells[rowcount - 1, eDay + 2].Value = datarow[2].ToString();
                            }

                            worKsheeT.Cells[rowcount - 1, eDay + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            worKsheeT.Cells[rowcount - 1, totalCols - 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worKsheeT.Cells[rowcount - 1, totalCols - 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worKsheeT.Cells[rowcount - 1, totalCols - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worKsheeT.Cells[rowcount - 1, totalCols].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            if (datarow[2].ToString() == "X")
                            {
                                X++;
                            }
                            else if (datarow[2].ToString() == "E")
                            {
                                E++;
                            }
                            else if (datarow[2].ToString() == "L")
                            {
                                L++;
                            }
                            else if (datarow[2].ToString() == "P")
                            {
                                P++;
                            }
                            worKsheeT.Cells[rowcount - 1, totalCols - 3].Value = X;
                            worKsheeT.Cells[rowcount - 1, totalCols - 2].Value = E;
                            worKsheeT.Cells[rowcount - 1, totalCols - 1].Value = L;
                            worKsheeT.Cells[rowcount - 1, totalCols].Value = P;
                        }
                        else
                        {
                            L = 0;
                            X = 0;
                            E = 0;
                            P = 0;
                            worKsheeT.Cells[rowcount, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worKsheeT.Cells[rowcount, 1].Value = rowno.ToString();
                            worKsheeT.Cells[rowcount, 2].Value = datarow[0].ToString();
                            DateTime att = Convert.ToDateTime(datarow[1].ToString());
                            int eDay = att.Day;
                            if (datarow[2].ToString() == "P")
                            {
                                worKsheeT.Cells[rowcount, eDay + 2].Value = "\u2713";
                            }
                            else if (datarow[2].ToString() == "U")
                            {
                                worKsheeT.Cells[rowcount, eDay + 2].Value = "X";
                            }
                            else
                            {
                                worKsheeT.Cells[rowcount, eDay + 2].Value = datarow[2].ToString();
                            }

                            worKsheeT.Cells[rowcount, eDay + 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            worKsheeT.Cells[rowcount, totalCols - 3].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worKsheeT.Cells[rowcount, totalCols - 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worKsheeT.Cells[rowcount, totalCols - 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            worKsheeT.Cells[rowcount, totalCols].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                            if (datarow[2].ToString() == "X")
                            {
                                X++;
                            }
                            else if (datarow[2].ToString() == "E")
                            {
                                E++;
                            }
                            else if (datarow[2].ToString() == "L")
                            {
                                L++;
                            }
                            else if (datarow[2].ToString() == "P")
                            {
                                P++;
                            }
                            worKsheeT.Cells[rowcount, totalCols - 3].Value = X;
                            worKsheeT.Cells[rowcount, totalCols - 2].Value = E;
                            worKsheeT.Cells[rowcount, totalCols - 1].Value = L;
                            worKsheeT.Cells[rowcount, totalCols].Value = P;

                            rowcount++;
                            rowno++;
                        }

                    }

                    worKsheeT.Cells[3, 1,3, 2].Merge = true;
                    worKsheeT.Cells[3, 1].Value = WebApiResources.SchoolName;
                    worKsheeT.Cells[4, 1, 4, 2].Merge = true;
                    worKsheeT.Cells[4, 1].Value = WebApiResources.ShiftCode;

                    worKsheeT.Cells[3, 3].Value = ":";
                    worKsheeT.Cells[4, 3].Value = ":";

                    worKsheeT.Cells[3, 4, 3, 14].Merge = true;
                    worKsheeT.Cells[3, 4].Value = school_name;
                    worKsheeT.Cells[4, 4, 4, 14].Merge = true;
                    worKsheeT.Cells[4, 4].Value = shift_code;

                    worKsheeT.Cells[rowcount + 1, 1, rowcount + 1, totalCols].Merge = true;
                    worKsheeT.Cells[rowcount + 1, 1].Value = WebApiResources.Enter + ": X = " + WebApiResources.Absent + ", E = " + WebApiResources.Excused + ", L = " + WebApiResources.Late + " or \u2713 = " + WebApiResources.Present;
                    worKsheeT.Cells[rowcount + 1, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worKsheeT.Cells[7, 1, rowcount - 1, totalCols].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    worKsheeT.Cells.AutoFitColumns();
                    
                    value = "/Reports/Attendances/" + shift_code.Replace(" ", "_") + "_" + month.ToString().PadLeft(2, '0') + "" + year + "_" + DateTime.Now.ToString("ddMMyyHHmmss", ci) + ".xlsx";
                    package.SaveAs(new FileInfo(HttpContext.Current.Server.MapPath("~") + "\\Reports\\Attendances\\" + shift_code.Replace(" ", "_") + "_" + month.ToString().PadLeft(2, '0') + "" + year + "_" + DateTime.Now.ToString("ddMMyyHHmmss", ci) + ".xlsx"));

                    //var excelInteropExcelToPdfConverter = new ExcelInteropExcelToPdfConverter();

                    //try
                    //{
                    //    excelInteropExcelToPdfConverter.ConvertToPdf(HttpContext.Current.Server.MapPath("~") + "\\Reports\\Attendances\\" + shift_code.Replace(" ", "_") + "_" + month.ToString().PadLeft(2, '0') + "" + year + "_" + DateTime.Now.ToString("ddMMyyHHmmss", ci) + ".xlsx");
                    //}
                    //catch (Exception ex)
                    //{
                    //    value = ex.Message;
                    //    Environment.ExitCode = -1;
                    //}
                }
                catch (Exception ex)
                {
                    value = ex.Message;
                }
                finally
                {

                }
                return value;
            }
        }

        public string ExportOrderHistoryProduct(DataTable dtExcel, string pickup_date, string create_by)
        {
            string value = string.Empty;
            DataTable dt = new DataTable();
            string school_name = string.Empty;
            string company_name = string.Empty;

            dt.Columns.AddRange(new DataColumn[6] {
                                    new DataColumn(WebApiResources.No),
                                    new DataColumn(WebApiResources.CategoryName),
                                    new DataColumn(WebApiResources.ProductName),
                                    new DataColumn(WebApiResources.Qty),
                                    new DataColumn(WebApiResources.UnitPrice),
                                    new DataColumn(WebApiResources.Total)
                                });

            if (dtExcel.Rows.Count > 0)
            {
                school_name = dtExcel.Rows[0]["school_name"].ToString();
                company_name = dtExcel.Rows[0]["company_name"].ToString();

                for (int i = 0; i < dtExcel.Rows.Count; i++)
                {
                    dt.Rows.Add(i + 1,
                        dtExcel.Rows[i]["category_name"],
                        dtExcel.Rows[i]["product_name"],
                        dtExcel.Rows[i]["product_qty"],
                        dtExcel.Rows[i]["unit_price"],
                        dtExcel.Rows[i]["total_amount"]
                    );
                }
            }

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td align='right' style='background-color: #18B5F0' colspan='2'><img src='" + HttpContext.Current.Server.MapPath("~") + "\\Images\\ic_logo.png' height='30%' width='100%' align='center'/></td></tr>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan='2'><font size='2'>" + WebApiResources.ProductDailyOrderReport + "</td></tr>");
                    sb.Append("<tr><td colspan = '3'></td></tr>");
                    sb.Append("<tr><td><font size='1'>" + WebApiResources.CompanyName + ": ");
                    sb.Append(company_name);
                    sb.Append("</font></td><td align='right'><font size='1'>" + WebApiResources.PrintedDateTime + ": ");
                    sb.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                    sb.Append("</font></td></tr>");
                    sb.Append("<tr><td colspan = '3'><font size='1'>" + WebApiResources.SchoolName + ": ");
                    sb.Append(school_name);
                    sb.Append("</font></td></tr>");
                    sb.Append("<tr><td colspan = '3'><font size='1'>" + WebApiResources.OrderDate + ": ");
                    sb.Append(Convert.ToDateTime(pickup_date).ToString("dd/MM/yyyy"));
                    sb.Append("</font></td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br />");
                    sb.Append("<table border = '1'>");
                    sb.Append("<tr>");

                    foreach (DataColumn column in dt.Columns)
                    {
                        sb.Append("<th style='background-color:#D20B0C;color:#aaaaaa'><font color='#000000' align='center' size='1' >");
                        sb.Append(column.ColumnName);
                        sb.Append("</font></th>");
                    }
                    sb.Append("</tr>");

                    foreach (DataRow row in dt.Rows)
                    {
                        sb.Append("<tr>");
                        foreach (DataColumn column in dt.Columns)
                        {
                            if (column.ColumnName == WebApiResources.No)
                            {
                                sb.Append("<td align='center' width='5%'><font size='1'>");
                                sb.Append(row[column]);
                                sb.Append("</font></td>");
                            }
                            else if (column.ColumnName == WebApiResources.CategoryName)
                            {
                                sb.Append("<td align='left' width='25%'><font size='1'>");
                                sb.Append(row[column]);
                                sb.Append("</font></td>");
                            }
                            else if (column.ColumnName == WebApiResources.ProductName)
                            {
                                sb.Append("<td align='left' width='30%'><font size='1'>");
                                sb.Append(row[column]);
                                sb.Append("</font></td>");
                            }
                            else if (column.ColumnName == WebApiResources.Qty)
                            {
                                sb.Append("<td align='center' width='10%'><font size='1'>");
                                sb.Append(row[column]);
                                sb.Append("</font></td>");
                            }
                            else if (column.ColumnName == WebApiResources.UnitPrice)
                            {
                                sb.Append("<td align='right' width='15%'><font size='1'>");
                                sb.Append(row[column]);
                                sb.Append("</font></td>");
                            }
                            else if (column.ColumnName == WebApiResources.Total)
                            {
                                sb.Append("<td align='right' width='15%'><font size='1'>");
                                sb.Append(row[column]);
                                sb.Append("</font></td>");
                            }

                        }
                        sb.Append("</tr>");
                    }
                    //footer
                    sb.Append("</table>");

                    StringReader sr = new StringReader(sb.ToString());

                    Document pdfDoc = new Document(PageSize.A4, 50f, 50f, 50, 50f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    value = "/Reports/Orders/" + company_name.Replace(" ", "_") + "_" + school_name.Replace(" ", "_") + "_" + Convert.ToDateTime(pickup_date).ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("ddMMyyHHmmss") + ".pdf";
                    PdfWriter.GetInstance(pdfDoc, new FileStream(HttpContext.Current.Server.MapPath("~") + "\\Reports\\Orders\\" + company_name.Replace(" ", "_") + "_" + school_name.Replace(" ", "_") + "_" + Convert.ToDateTime(pickup_date).ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("ddMMyyHHmmss") + ".pdf", FileMode.Create));
                    pdfDoc.Open();

                    htmlparser.Parse(sr);
                    pdfDoc.Close();

                    return value;
                }
            }
        }

        public string ExportOrderHistoryClass(DataTable dtExcel, string pickup_date, string create_by)
        {
            string value = string.Empty;
            DataTable dt = new DataTable();
            string school_name = string.Empty;
            string company_name = string.Empty;
            string class_name = string.Empty;

            dt.Columns.AddRange(new DataColumn[6] {
                                    new DataColumn(WebApiResources.No),
                                    new DataColumn(WebApiResources.ClassName),
                                    new DataColumn(WebApiResources.ProductName),
                                    new DataColumn(WebApiResources.Qty),
                                    new DataColumn(WebApiResources.UnitPrice),
                                    new DataColumn(WebApiResources.Total)
                                });

            if (dtExcel.Rows.Count > 0)
            {
                school_name = dtExcel.Rows[0]["school_name"].ToString();
                company_name = dtExcel.Rows[0]["company_name"].ToString();

                for (int i = 0; i < dtExcel.Rows.Count; i++)
                {
                    dt.Rows.Add(i + 1,
                        dtExcel.Rows[i]["class_name"],
                        dtExcel.Rows[i]["product_name"],
                        dtExcel.Rows[i]["product_qty"],
                        dtExcel.Rows[i]["unit_price"],
                        dtExcel.Rows[i]["total_amount"]
                    );
                }
            }

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td align='right' style='background-color: #18B5F0' colspan='2'><img src='" + HttpContext.Current.Server.MapPath("~") + "\\Images\\ic_logo.png' height='30%' width='100%' align='center'/></td></tr>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan='2'><font size='2'>" + WebApiResources.ClassDailyOrderReportTitle + "</td></tr>");
                    sb.Append("<tr><td colspan = '3'></td></tr>");
                    sb.Append("<tr><td><font size='1'>" + WebApiResources.CompanyName + ": ");
                    sb.Append(company_name);
                    sb.Append("</font></td><td align='right'><font size='1'>" + WebApiResources.PrintedDateTime + ": ");
                    sb.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                    sb.Append("</font></td></tr>");
                    sb.Append("<tr><td colspan = '3'><font size='1'>" + WebApiResources.SchoolName + ": ");
                    sb.Append(school_name);
                    sb.Append("</font></td></tr>");
                    sb.Append("<tr><td colspan = '3'><font size='1'>" + WebApiResources.OrderDate + ": ");
                    sb.Append(Convert.ToDateTime(pickup_date).ToString("dd/MM/yyyy"));
                    sb.Append("</font></td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br />");
                    sb.Append("<table border = '1'>");
                    sb.Append("<tr>");

                    foreach (DataColumn column in dt.Columns)
                    {
                        sb.Append("<th style='background-color:#D20B0C;color:#aaaaaa'><font color='#000000' align='center' size='1' >");
                        sb.Append(column.ColumnName);
                        sb.Append("</font></th>");
                    }
                    sb.Append("</tr>");

                    //foreach (DataRow row in dt.Rows)
                    //{
                    sb.Append("<tr>");

                    int rowNo = 1;

                    for (int index = 0; index < dt.Rows.Count; ++index)
                    {
                        foreach (DataColumn column in dt.Columns)
                        {
                            if (column.ColumnName == WebApiResources.No)
                            {
                                string rowStr = "";

                                if (index > 0)
                                {
                                    string firstName = string.Empty;
                                    if (index == 1)
                                    {
                                        firstName = dt.Rows[0][1].ToString();
                                    }
                                    else
                                    {
                                        firstName = dt.Rows[index - 1][1].ToString();
                                    }

                                    string secondName = dt.Rows[index][1].ToString();

                                    if (firstName != secondName)
                                    {
                                        rowNo++;
                                        rowStr = rowNo.ToString();
                                    }
                                }
                                else
                                {
                                    rowStr = rowNo.ToString();
                                }

                                sb.Append("<td align='center' width='5%'><font size='1'>");
                                sb.Append(rowStr);
                                sb.Append("</font></td>");
                            }
                            else if (column.ColumnName == WebApiResources.ClassName)
                            {
                                string className = string.Empty;

                                if (index > 0)
                                {
                                    string firstName = string.Empty;
                                    if (index == 1)
                                    {
                                        firstName = dt.Rows[0][1].ToString();
                                    }
                                    else
                                    {
                                        firstName = dt.Rows[index - 1][1].ToString();
                                    }

                                    string secondName = dt.Rows[index][1].ToString();

                                    if (firstName != secondName)
                                    {
                                        className = dt.Rows[index][1].ToString();
                                    }
                                }
                                else if (index == 0)
                                {
                                    className = dt.Rows[index][1].ToString();
                                }

                                sb.Append("<td align='left' width='25%'><font size='1'>");
                                sb.Append(className);
                                sb.Append("</font></td>");
                            }
                            else if (column.ColumnName == WebApiResources.ProductName)
                            {
                                sb.Append("<td align='left' width='30%'><font size='1'>");
                                sb.Append(dt.Rows[index][2].ToString());
                                sb.Append("</font></td>");
                            }
                            else if (column.ColumnName == WebApiResources.Qty)
                            {
                                sb.Append("<td align='center' width='10%'><font size='1'>");
                                sb.Append(dt.Rows[index][3].ToString());
                                sb.Append("</font></td>");
                            }
                            else if (column.ColumnName == WebApiResources.UnitPrice)
                            {
                                sb.Append("<td align='right' width='15%'><font size='1'>");
                                sb.Append(dt.Rows[index][4].ToString());
                                sb.Append("</font></td>");
                            }
                            else if (column.ColumnName == WebApiResources.Total)
                            {
                                sb.Append("<td align='right' width='15%'><font size='1'>");
                                sb.Append(dt.Rows[index][5].ToString());
                                sb.Append("</font></td>");
                            }
                        }
                        sb.Append("</tr>");
                    }
                    //}
                    //footer
                    sb.Append("</table>");

                    StringReader sr = new StringReader(sb.ToString());

                    Document pdfDoc = new Document(PageSize.A4, 50f, 50f, 50, 50f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    value = "/Reports/Orders/" + company_name.Replace(" ", "_") + "_" + school_name.Replace(" ", "_") + "_Class_" + Convert.ToDateTime(pickup_date).ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("ddMMyyHHmmss") + ".pdf";
                    PdfWriter.GetInstance(pdfDoc, new FileStream(HttpContext.Current.Server.MapPath("~") + "\\Reports\\Orders\\" + company_name.Replace(" ", "_") + "_" + school_name.Replace(" ", "_") + "_Class_" + Convert.ToDateTime(pickup_date).ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("ddMMyyHHmmss") + ".pdf", FileMode.Create));
                    pdfDoc.Open();

                    htmlparser.Parse(sr);
                    pdfDoc.Close();

                    return value;
                }
            }
        }

        public string ExportOrderHistoryStudent(DataTable dtExcel, string pickup_date, string create_by)
        {
            DataTable dt = new DataTable();
            string school_name = string.Empty;
            string company_name = string.Empty;
            string class_name = string.Empty;
            string value = string.Empty;

            dt.Columns.AddRange(new DataColumn[4] {
                                    new DataColumn(WebApiResources.No),
                                    new DataColumn(WebApiResources.StudentName),
                                    new DataColumn(WebApiResources.ProductName),
                                    new DataColumn(WebApiResources.Qty)
                                });

            if (dtExcel.Rows.Count > 0)
            {
                school_name = dtExcel.Rows[0]["school_name"].ToString();
                company_name = dtExcel.Rows[0]["company_name"].ToString();
                class_name = dtExcel.Rows[0]["class_name"].ToString();

                for (int i = 0; i < dtExcel.Rows.Count; i++)
                {
                    dt.Rows.Add(i + 1,
                        dtExcel.Rows[i]["full_name"],
                        dtExcel.Rows[i]["product_name"],
                        dtExcel.Rows[i]["product_qty"]
                    );
                }
            }

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td align='right' style='background-color: #18B5F0' colspan='2'><img src='" + HttpContext.Current.Server.MapPath("~") + "\\Images\\ic_logo.png' height='30%' width='100%' align='center'/></td></tr>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan='2'><font size='2'>" + WebApiResources.StudentDailyOrderReport + "</td></tr>");
                    sb.Append("<tr><td colspan = '3'></td></tr>");
                    sb.Append("<tr><td><font size='1'>" + WebApiResources.CompanyName + ": ");
                    sb.Append(company_name);
                    sb.Append("</font></td><td align='right'><font size='1'>" + WebApiResources.PrintedDateTime + ": ");
                    sb.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                    sb.Append("</font></td></tr>");
                    sb.Append("<tr><td colspan = '3'><font size='1'>" + WebApiResources.SchoolName + ": ");
                    sb.Append(school_name);
                    sb.Append("</font></td></tr>");
                    sb.Append("<tr><td colspan = '3'><font size='1'>" + WebApiResources.ClassName + ": ");
                    sb.Append(class_name);
                    sb.Append("</font></td></tr>");
                    sb.Append("<tr><td colspan = '3'><font size='1'>" + WebApiResources.OrderDate + ": ");
                    sb.Append(Convert.ToDateTime(pickup_date).ToString("dd/MM/yyyy"));
                    sb.Append("</font></td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br />");
                    sb.Append("<table border = '1'>");
                    sb.Append("<tr>");

                    foreach (DataColumn column in dt.Columns)
                    {
                        sb.Append("<th style='background-color:#D20B0C;color:#aaaaaa'><font color='#000000' align='center' size='1' >");
                        sb.Append(column.ColumnName);
                        sb.Append("</font></th>");
                    }
                    sb.Append("</tr>");

                    //foreach (DataRow row in dt.Rows)
                    //{
                        sb.Append("<tr>");


                        int rowNo = 1;


                        for (int index = 0; index < dt.Rows.Count; ++index)
                        {
                            foreach (DataColumn column in dt.Columns)
                            {
                                if (column.ColumnName == WebApiResources.No)
                                {
                                    string rowStr = "";

                                    if (index > 0)
                                    {
                                        string firstName = string.Empty;
                                        if (index == 1)
                                        {
                                            firstName = dt.Rows[0][1].ToString();
                                        }
                                        else
                                        {
                                            firstName = dt.Rows[index - 1][1].ToString();
                                        }

                                        string secondName = dt.Rows[index][1].ToString();

                                        if (firstName != secondName)
                                        {
                                            rowNo++;
                                            rowStr = rowNo.ToString();
                                        }
                                    }
                                    else 
                                    {
                                        rowStr = rowNo.ToString();
                                    }

                                    sb.Append("<td align='center' width='5%'><font size='1'>");
                                    sb.Append(rowStr);
                                    sb.Append("</font></td>");
                                }
                                else if (column.ColumnName == WebApiResources.StudentName)
                                {
                                    string student_name = string.Empty;

                                    if (index > 0)
                                    {
                                        string firstName = string.Empty;
                                        if (index == 1)
                                        {
                                            firstName = dt.Rows[0][1].ToString();
                                        }
                                        else {
                                            firstName = dt.Rows[index - 1][1].ToString();
                                        }

                                        string secondName = dt.Rows[index][1].ToString();

                                        if (firstName != secondName)
                                        {
                                            student_name = dt.Rows[index][1].ToString();
                                        }
                                    }
                                    else if(index == 0)
                                    {
                                        student_name = dt.Rows[index][1].ToString();
                                    }

                                    sb.Append("<td align='left' width='25%'><font size='1'>");
                                    sb.Append(student_name);
                                    sb.Append("</font></td>");
                                }
                                else if (column.ColumnName == WebApiResources.ProductName)
                                {
                                    sb.Append("<td align='left' width='30%'><font size='1'>");
                                    sb.Append(dt.Rows[index][2].ToString());
                                    sb.Append("</font></td>");
                                }
                                else if (column.ColumnName == WebApiResources.Qty)
                                {
                                    sb.Append("<td align='center' width='10%'><font size='1'>");
                                    sb.Append(dt.Rows[index][3].ToString());
                                    sb.Append("</font></td>");
                                }
                                else if (column.ColumnName == WebApiResources.UnitPrice)
                                {
                                    sb.Append("<td align='right' width='15%'><font size='1'>");
                                    sb.Append(dt.Rows[index][4].ToString());
                                    sb.Append("</font></td>");
                                }
                                else if (column.ColumnName == WebApiResources.Total)
                                {
                                    sb.Append("<td align='right' width='15%'><font size='1'>");
                                    sb.Append(dt.Rows[index][5].ToString());
                                    sb.Append("</font></td>");
                                }
                            }
                            sb.Append("</tr>");
                        }
                    //}
                    //footer
                    sb.Append("</table>");

                    StringReader sr = new StringReader(sb.ToString());

                    Document pdfDoc = new Document(PageSize.A4, 50f, 50f, 50, 50f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    value = "/Reports/Orders/" + company_name.Replace(" ", "_") + "_" + school_name.Replace(" ", "_") + "_" + class_name.Replace(" ", "_") + "_" + Convert.ToDateTime(pickup_date).ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("ddMMyyHHmmss") + ".pdf";
                    PdfWriter.GetInstance(pdfDoc, new FileStream(HttpContext.Current.Server.MapPath("~") + "\\Reports\\Orders\\" + company_name.Replace(" ", "_") + "_" + school_name.Replace(" ", "_") + "_" + class_name.Replace(" ", "_") + "_" + Convert.ToDateTime(pickup_date).ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("ddMMyyHHmmss") + ".pdf", FileMode.Create));
                    pdfDoc.Open();

                    htmlparser.Parse(sr);
                    pdfDoc.Close();

                    return value;
                }
            }
        }

        public string ExportOrderHistoryStaff(DataTable dtExcel, string pickup_date, string create_by)
        {
            DataTable dt = new DataTable();
            string school_name = string.Empty;
            string company_name = string.Empty;
            string class_name = string.Empty;
            string value = string.Empty;

            dt.Columns.AddRange(new DataColumn[8] {
                                    new DataColumn(WebApiResources.No),
                                    new DataColumn(WebApiResources.StaffName),
                                    new DataColumn(WebApiResources.ServiceMethod),
                                    new DataColumn(WebApiResources.OrderTiming),
                                    new DataColumn(WebApiResources.ProductName),
                                    new DataColumn(WebApiResources.Qty),
                                    new DataColumn(WebApiResources.UnitPrice),
                                    new DataColumn(WebApiResources.Total)
                                });

            if (dtExcel.Rows.Count > 0)
            {
                school_name = dtExcel.Rows[0]["school_name"].ToString();
                company_name = dtExcel.Rows[0]["company_name"].ToString();
                class_name = dtExcel.Rows[0]["class_name"].ToString();

                for (int i = 0; i < dtExcel.Rows.Count; i++)
                {
                    dt.Rows.Add(i + 1,
                        dtExcel.Rows[i]["full_name"],
                        dtExcel.Rows[i]["service_method_id"] + "#" + dtExcel.Rows[i]["delivery_location"],
                        dtExcel.Rows[i]["pickup_time"],
                        dtExcel.Rows[i]["product_name"],
                        dtExcel.Rows[i]["product_qty"],
                        dtExcel.Rows[i]["unit_price"],
                        dtExcel.Rows[i]["total_amount"]
                    );
                }
            }

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td align='right' style='background-color: #18B5F0' colspan='2'><img src='" + HttpContext.Current.Server.MapPath("~") + "\\Images\\ic_logo.png' height='30%' width='100%' align='center'/></td></tr>");
                    sb.Append("<tr><td align='center' style='background-color: #18B5F0' colspan='2'><font size='2'>" + WebApiResources.StaffDailyOrderReportTitle + "</td></tr>");
                    sb.Append("<tr><td colspan = '3'></td></tr>");
                    sb.Append("<tr><td><font size='1'>" + WebApiResources.CompanyName + ": ");
                    sb.Append(company_name);
                    sb.Append("</font></td><td align='right'><font size='1'>" + WebApiResources.PrintedDateTime + ": ");
                    sb.Append(DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                    sb.Append("</font></td></tr>");
                    sb.Append("<tr><td colspan = '3'><font size='1'>" + WebApiResources.SchoolName + ": ");
                    sb.Append(school_name);
                    sb.Append("</font></td></tr>");
                    sb.Append("<tr><td colspan = '3'><font size='1'>" + WebApiResources.OrderDate + ": ");
                    sb.Append(Convert.ToDateTime(pickup_date).ToString("dd/MM/yyyy"));
                    sb.Append("</font></td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br />");
                    sb.Append("<table border = '1'>");
                    sb.Append("<tr>");

                    foreach (DataColumn column in dt.Columns)
                    {
                        sb.Append("<th style='background-color:#D20B0C;color:#aaaaaa'><font color='#000000' align='center' size='1' >");
                        sb.Append(column.ColumnName);
                        sb.Append("</font></th>");
                    }
                    sb.Append("</tr>");

                    //foreach (DataRow row in dt.Rows)
                    //{
                    sb.Append("<tr>");

                    int rowNo = 1;

                    for (int index = 0; index < dt.Rows.Count; ++index)
                    {
                        foreach (DataColumn column in dt.Columns)
                        {
                            if (column.ColumnName == WebApiResources.No)
                            {
                                string rowStr = "";

                                if (index > 0)
                                {
                                    string firstName = string.Empty;
                                    if (index == 1)
                                    {
                                        firstName = dt.Rows[0][1].ToString();
                                    }
                                    else
                                    {
                                        firstName = dt.Rows[index - 1][1].ToString();
                                    }

                                    string secondName = dt.Rows[index][1].ToString();

                                    if (firstName != secondName)
                                    {
                                        rowNo++;
                                        rowStr = rowNo.ToString();
                                    }
                                }
                                else if (index == 0)
                                {
                                    rowStr = rowNo.ToString();
                                }

                                sb.Append("<td align='center' width='5%'><font size='1'>");
                                sb.Append(rowStr);
                                sb.Append("</font></td>");
                            }
                            else if (column.ColumnName == WebApiResources.StaffName)
                            {
                                string staff_name = string.Empty;

                                if (index > 0)
                                {
                                    string firstName = string.Empty;
                                    if (index == 1)
                                    {
                                        firstName = dt.Rows[0][1].ToString();
                                    }
                                    else
                                    {
                                        firstName = dt.Rows[index - 1][1].ToString();
                                    }

                                    string secondName = dt.Rows[index][1].ToString();

                                    if (firstName != secondName)
                                    {
                                        staff_name = dt.Rows[index][1].ToString();
                                    }
                                }
                                else if (index == 0)
                                {
                                    staff_name = dt.Rows[index][1].ToString();
                                }

                                sb.Append("<td align='left' width='20%'><font size='1'>");
                                sb.Append(staff_name);
                                sb.Append("</font></td>");
                            }
                            else if (column.ColumnName == WebApiResources.ServiceMethod)
                            {
                                string service_method = string.Empty;
                                string delivery_location = string.Empty;
                                if (index > 0)
                                {
                                    string firstName = string.Empty;
                                    if (index == 1)
                                    {
                                        firstName = dt.Rows[0][1].ToString();
                                    }
                                    else
                                    {
                                        firstName = dt.Rows[index - 1][1].ToString();
                                    }

                                    string secondName = dt.Rows[index][1].ToString();

                                    if (firstName != secondName)
                                    {
                                        service_method = dt.Rows[index][2].ToString().Split('#')[0];
                                    }
                                }
                                else if (index == 0)
                                {
                                    service_method = dt.Rows[index][2].ToString().Split('#')[0];
                                    delivery_location = dt.Rows[index][2].ToString().Split('#')[1];
                                }

                                if (service_method == "1")
                                {
                                    service_method = WebApiResources.Delivery + " (" + delivery_location + ")";
                                }
                                else if (service_method == "2")
                                {
                                    service_method = WebApiResources.SelfPickup;
                                }

                                sb.Append("<td align='left' width='20%'><font size='1'>");
                                sb.Append(service_method);
                                sb.Append("</font></td>");
                            }
                            else if (column.ColumnName == WebApiResources.OrderTiming)
                            {
                                string order_timing = string.Empty;

                                if (index > 0)
                                {
                                    string firstName = string.Empty;
                                    if (index == 1)
                                    {
                                        firstName = dt.Rows[0][1].ToString();
                                    }
                                    else
                                    {
                                        firstName = dt.Rows[index - 1][1].ToString();
                                    }

                                    string secondName = dt.Rows[index][1].ToString();

                                    if (firstName != secondName)
                                    {
                                        order_timing = Convert.ToDateTime(dt.Rows[index][3]).ToString("hh:mm:ss tt");
                                    }
                                }
                                else if (index == 0)
                                {
                                    order_timing = Convert.ToDateTime(dt.Rows[index][3]).ToString("hh:mm:ss tt");
                                }

                                sb.Append("<td align='center' width='10%'><font size='1'>");
                                sb.Append(order_timing);
                                sb.Append("</font></td>");
                            }
                            else if (column.ColumnName == WebApiResources.ProductName)
                            {
                                sb.Append("<td align='left' width='20%'><font size='1'>");
                                sb.Append(dt.Rows[index][4].ToString());
                                sb.Append("</font></td>");
                            }
                            else if (column.ColumnName == WebApiResources.Qty)
                            {
                                sb.Append("<td align='center' width='5%'><font size='1'>");
                                sb.Append(dt.Rows[index][5].ToString());
                                sb.Append("</font></td>");
                            }
                            else if (column.ColumnName == WebApiResources.UnitPrice)
                            {
                                sb.Append("<td align='right' width='10%'><font size='1'>");
                                sb.Append(dt.Rows[index][6].ToString());
                                sb.Append("</font></td>");
                            }
                            else if (column.ColumnName == WebApiResources.Total)
                            {
                                sb.Append("<td align='right' width='10%'><font size='1'>");
                                sb.Append(dt.Rows[index][7].ToString());
                                sb.Append("</font></td>");
                            }
                        }
                        sb.Append("</tr>");
                    }
                    //}
                    //footer
                    sb.Append("</table>");

                    StringReader sr = new StringReader(sb.ToString());

                    Document pdfDoc = new Document(PageSize.A4.Rotate(), 50f, 50f, 50, 50f);
                    HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    value = "/Reports/Orders/" + company_name.Replace(" ", "_") + "_" + school_name.Replace(" ", "_") + "_Staff_" + Convert.ToDateTime(pickup_date).ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("ddMMyyHHmmss") + ".pdf";
                    PdfWriter.GetInstance(pdfDoc, new FileStream(HttpContext.Current.Server.MapPath("~") + "\\Reports\\Orders\\" + company_name.Replace(" ", "_") + "_" + school_name.Replace(" ", "_") + "_Staff_" + Convert.ToDateTime(pickup_date).ToString("ddMMyyyy") + "_" + DateTime.Now.ToString("ddMMyyHHmmss") + ".pdf", FileMode.Create));
                    pdfDoc.Open();

                    htmlparser.Parse(sr);
                    pdfDoc.Close();

                    return value;
                }
            }
        }
    }
}
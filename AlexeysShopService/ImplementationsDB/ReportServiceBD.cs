﻿using AlexeysShopService.BindingModels;
using AlexeysShopService.Interfaces;
using AlexeysShopService.ViewModels;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;

namespace AlexeysShopService.ImplementationsBD
{
    public class ReportServiceBD : IReportService
    {
        private AlexeysDbContext context;

        public ReportServiceBD(AlexeysDbContext context)
        {
            this.context = context;
        }

        public void SaveArticlePrice(ReportBindingModel model)
        {
            if (File.Exists(model.FileName))
            {
                File.Delete(model.FileName);
            }

            var winword = new Microsoft.Office.Interop.Word.Application();
            try
            {
                object missing = System.Reflection.Missing.Value;
                //создаем документ
                Microsoft.Office.Interop.Word.Document document =
                    winword.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                //получаем ссылку на параграф
                var paragraph = document.Paragraphs.Add(missing);
                var range = paragraph.Range;
                //задаем текст
                range.Text = "Прайс изделий";
                //задаем настройки шрифта
                var font = range.Font;
                font.Size = 16;
                font.Name = "Times New Roman";
                font.Bold = 1;
                //задаем настройки абзаца
                var paragraphFormat = range.ParagraphFormat;
                paragraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                paragraphFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                paragraphFormat.SpaceAfter = 10;
                paragraphFormat.SpaceBefore = 0;
                //добавляем абзац в документ
                range.InsertParagraphAfter();

                var products = context.Articles.ToList();
                //создаем таблицу
                var paragraphTable = document.Paragraphs.Add(Type.Missing);
                var rangeTable = paragraphTable.Range;
                var table = document.Tables.Add(rangeTable, products.Count, 2, ref missing, ref missing);

                font = table.Range.Font;
                font.Size = 14;
                font.Name = "Times New Roman";

                var paragraphTableFormat = table.Range.ParagraphFormat;
                paragraphTableFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                paragraphTableFormat.SpaceAfter = 0;
                paragraphTableFormat.SpaceBefore = 0;

                for (int i = 0; i < products.Count; ++i)
                {
                    table.Cell(i + 1, 1).Range.Text = products[i].ArticleName;
                    table.Cell(i + 1, 2).Range.Text = products[i].Cost.ToString();
                }
                //задаем границы таблицы
                table.Borders.InsideLineStyle = WdLineStyle.wdLineStyleInset;
                table.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;

                paragraph = document.Paragraphs.Add(missing);
                range = paragraph.Range;

                range.Text = "Дата: " + DateTime.Now.ToLongDateString();

                font = range.Font;
                font.Size = 12;
                font.Name = "Times New Roman";

                paragraphFormat = range.ParagraphFormat;
                paragraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                paragraphFormat.LineSpacingRule = WdLineSpacing.wdLineSpaceSingle;
                paragraphFormat.SpaceAfter = 10;
                paragraphFormat.SpaceBefore = 10;

                range.InsertParagraphAfter();
                //сохраняем
                object fileFormat = WdSaveFormat.wdFormatXMLDocument;
                document.SaveAs(model.FileName, ref fileFormat, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing,
                    ref missing);
                document.Close(ref missing, ref missing, ref missing);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                winword.Quit();
            }
        }

        public List<StoragesLoadViewModel> GetStoragesLoad()
        {
            return context.Storages
                            .ToList()
                            .GroupJoin(
                                    context.StorageParts
                                                .Include(r => r.Part)
                                                .ToList(),
                                    stock => stock,
                                    stockPart => stockPart.Storage,
                                    (stock, stockCompList) =>
            new StoragesLoadViewModel
            {
                StorageName = stock.StorageName,
                TotalCount = stockCompList.Sum(r => r.Count),
                Parts = stockCompList.Select(r => new Tuple<string, int>(r.Part.PartName, r.Count))
            })
                            .ToList();
        }

        public void SaveStoragesLoad(ReportBindingModel model)
        {
            var excel = new Microsoft.Office.Interop.Excel.Application();
            try
            {
                //или создаем excel-файл, или открываем существующий
                if (File.Exists(model.FileName))
                {
                    excel.Workbooks.Open(model.FileName, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                        Type.Missing);
                }
                else
                {
                    excel.SheetsInNewWorkbook = 1;
                    excel.Workbooks.Add(Type.Missing);
                    excel.Workbooks[1].SaveAs(model.FileName, XlFileFormat.xlExcel8, Type.Missing,
                        Type.Missing, false, false, XlSaveAsAccessMode.xlNoChange, Type.Missing,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                }

                Sheets excelsheets = excel.Workbooks[1].Worksheets;
                //Получаем ссылку на лист
                var excelworksheet = (Worksheet)excelsheets.get_Item(1);
                //очищаем ячейки
                excelworksheet.Cells.Clear();
                //настройки страницы
                excelworksheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
                excelworksheet.PageSetup.CenterHorizontally = true;
                excelworksheet.PageSetup.CenterVertically = true;
                //получаем ссылку на первые 3 ячейки
                Microsoft.Office.Interop.Excel.Range excelcells = excelworksheet.get_Range("A1", "C1");
                //объединяем их
                excelcells.Merge(Type.Missing);
                //задаем текст, настройки шрифта и ячейки
                excelcells.Font.Bold = true;
                excelcells.Value2 = "Загруженность складов";
                excelcells.RowHeight = 25;
                excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                excelcells.Font.Name = "Times New Roman";
                excelcells.Font.Size = 14;

                excelcells = excelworksheet.get_Range("A2", "C2");
                excelcells.Merge(Type.Missing);
                excelcells.Value2 = "на" + DateTime.Now.ToShortDateString();
                excelcells.RowHeight = 20;
                excelcells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                excelcells.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                excelcells.Font.Name = "Times New Roman";
                excelcells.Font.Size = 12;

                var dict = GetStoragesLoad();
                if (dict != null)
                {
                    excelcells = excelworksheet.get_Range("C1", "C1");
                    foreach (var elem in dict)
                    {
                        //спускаемся на 2 ячейку вниз и 2 ячейкт влево
                        excelcells = excelcells.get_Offset(2, -2);
                        excelcells.ColumnWidth = 15;
                        excelcells.Value2 = elem.StorageName;
                        excelcells = excelcells.get_Offset(1, 1);
                        //обводим границы
                        if (elem.Parts.Count() > 0)
                        {
                            //получаем ячейкт для выбеления рамки под таблицу
                            var excelBorder =
                                excelworksheet.get_Range(excelcells,
                                            excelcells.get_Offset(elem.Parts.Count() - 1, 1));
                            excelBorder.Borders.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                            excelBorder.Borders.Weight = Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin;
                            excelBorder.HorizontalAlignment = Constants.xlCenter;
                            excelBorder.VerticalAlignment = Constants.xlCenter;
                            excelBorder.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                                                    Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                                                    Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic,
                                                    1);

                            foreach (var listElem in elem.Parts)
                            {
                                excelcells.Value2 = listElem.Item1;
                                excelcells.ColumnWidth = 10;
                                excelcells.get_Offset(0, 1).Value2 = listElem.Item2;
                                excelcells = excelcells.get_Offset(1, 0);
                            }
                        }
                        excelcells = excelcells.get_Offset(0, -1);
                        excelcells.Value2 = "Итого";
                        excelcells.Font.Bold = true;
                        excelcells = excelcells.get_Offset(0, 2);
                        excelcells.Value2 = elem.TotalCount;
                        excelcells.Font.Bold = true;
                    }
                }
                //сохраняем
                excel.Workbooks[1].Save();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //закрываем
                excel.Quit();
            }
        }

        public List<CustomerContractsModel> GetCustomerContracts(ReportBindingModel model)
        {
            return context.Contracts
                            .Include(rec => rec.Customer)
                            .Include(rec => rec.Article)
                            .Where(rec => rec.DateBegin >= model.DateFrom && rec.DateBegin <= model.DateTo)
                            .Select(rec => new CustomerContractsModel
                            {
                                CustomerName = rec.Customer.CustomerFIO,
                                DateCreate = SqlFunctions.DateName("dd", rec.DateBegin) + " " +
                                            SqlFunctions.DateName("mm", rec.DateBegin) + " " +
                                            SqlFunctions.DateName("yyyy", rec.DateBegin),
                                ArticleName = rec.Article.ArticleName,
                                Count = rec.Count,
                                Cost = rec.Cost,
                                Status = rec.Status.ToString()
                            })
                            .ToList();
        }

        public void SaveCustomerContracts(ReportBindingModel model)
        {
            //из ресрусов получаем шрифт для кирилицы
            if (!File.Exists("TIMCYR.TTF"))
            {
                File.WriteAllBytes("TIMCYR.TTF", Properties.Resources.TIMCYR);
            }
            //открываем файл для работы
            FileStream fs = new FileStream(model.FileName, FileMode.OpenOrCreate, FileAccess.Write);
            //создаем документ, задаем границы, связываем документ и поток
            iTextSharp.text.Document doc = new iTextSharp.text.Document();
            doc.SetMargins(0.5f, 0.5f, 0.5f, 0.5f);
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);

            doc.Open();
            BaseFont baseFont = BaseFont.CreateFont("TIMCYR.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            //вставляем заголовок
            var phraseTitle = new Phrase("Заказы клиентов",
                new iTextSharp.text.Font(baseFont, 16, iTextSharp.text.Font.BOLD));
            iTextSharp.text.Paragraph paragraph = new iTextSharp.text.Paragraph(phraseTitle)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 12
            };
            doc.Add(paragraph);

            var phrasePeriod = new Phrase("c " + model.DateFrom.Value.ToShortDateString() +
                                    " по " + model.DateTo.Value.ToShortDateString(),
                                    new iTextSharp.text.Font(baseFont, 14, iTextSharp.text.Font.BOLD));
            paragraph = new iTextSharp.text.Paragraph(phrasePeriod)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 12
            };
            doc.Add(paragraph);

            //вставляем таблицу, задаем количество столбцов, и ширину колонок
            PdfPTable table = new PdfPTable(6)
            {
                TotalWidth = 800F
            };
            table.SetTotalWidth(new float[] { 160, 140, 160, 100, 100, 140 });
            //вставляем шапку
            PdfPCell cell = new PdfPCell();
            var fontForCellBold = new iTextSharp.text.Font(baseFont, 10, iTextSharp.text.Font.BOLD);
            table.AddCell(new PdfPCell(new Phrase("ФИО клиента", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Дата создания", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Изделие", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Количество", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Сумма", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            table.AddCell(new PdfPCell(new Phrase("Статус", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_CENTER
            });
            //заполняем таблицу
            var list = GetCustomerContracts(model);
            var fontForCells = new iTextSharp.text.Font(baseFont, 10);
            for (int i = 0; i < list.Count; i++)
            {
                cell = new PdfPCell(new Phrase(list[i].CustomerName, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].DateCreate, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].ArticleName, fontForCells));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].Count.ToString(), fontForCells));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].Cost.ToString(), fontForCells));
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(list[i].Status, fontForCells));
                table.AddCell(cell);
            }
            //вставляем итого
            cell = new PdfPCell(new Phrase("Итого:", fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Colspan = 4,
                Border = 0
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase(list.Sum(rec => rec.Cost).ToString(), fontForCellBold))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                Border = 0
            };
            table.AddCell(cell);
            cell = new PdfPCell(new Phrase("", fontForCellBold))
            {
                Border = 0
            };
            table.AddCell(cell);
            //вставляем таблицу
            doc.Add(table);

            doc.Close();
        }
    }
}

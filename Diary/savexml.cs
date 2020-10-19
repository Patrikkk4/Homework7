using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Windows;
using ClosedXML.Excel;
using Newtonsoft.Json;
using Microsoft.Win32;

namespace Diary
{
    /// <summary>
    /// Структура создает заголовок таблицы и сохраняет xml файл с данными и разметкой таблицы
    /// </summary>
    struct savexml
    {
        // Инициализация экземпляра класса таблицы вводимых данных
        public static DataTable Dtable = new DataTable();

        /// <summary>
        /// Метод создает заголовок таблицы если отсутствует файл "myaffairs.xml"
        /// </summary>
        public static DataTable dataTable()
        {
            // Условие проверяет наличие файла myaffairs.xml. Если файл отсутствует, то создается новый заголовок таблицы
            if (!File.Exists(@"myaffair.json"))
            {
                // Создание заголовков таблицы вводимых данных
                DataColumn Coldate = new DataColumn("Дата");
                DataColumn ColnameHappening = new DataColumn("Имя события");
                DataColumn ColHappening = new DataColumn("Событие");
                DataColumn ColPhone = new DataColumn("Номер телефона");
                DataColumn ColPriority = new DataColumn("Важность");

                // добавление заголовков таблицы вводимых данных
                Dtable.Columns.Add(Coldate);
                Dtable.Columns.Add(ColnameHappening);
                Dtable.Columns.Add(ColHappening);
                Dtable.Columns.Add(ColPhone);
                Dtable.Columns.Add(ColPriority);
            }
            return Dtable;
        }

        /// <summary>
        /// Метод сохраняет таблицу с введенными данными
        /// </summary>
        public static void saveData()
        {
            //// временное имя таблицы для сохранения таблицы в xml файл
            //Dtable.TableName = "temp";

            //// сериализация таблицы в xml файл
            //Dtable.WriteXml(@"myaffairs.xml", XmlWriteMode.WriteSchema);

            string jsontable = JsonConvert.SerializeObject(Dtable);

            File.WriteAllText(@"myaffair.json", jsontable);
        }

        public static void saveXLS()
        {
            SaveFileDialog saveXLS = new SaveFileDialog();

            string SaveFilePath = string.Empty;

            saveXLS.Filter = "Таблица(*.xlsx)|*.xlsx|Таблица(*.xls)|*.xls|Все файлы(*.*)|*.*";

            if (MainWindow.Selfref.table.SelectedItems.Count != 0)
            {
                if (saveXLS.ShowDialog() == true)
                {
                    SaveFilePath = saveXLS.FileName;

                    using (DataTable exportDt = Dtable.Clone())
                    {
                        foreach (DataRowView t in MainWindow.Selfref.table.SelectedItems)
                        {
                            DataRow row = t.Row;
                            exportDt.ImportRow(row);
                        }

                        exportDt.TableName = "temp";

                        using (var exportBook = new XLWorkbook())
                        {  
                            
                            exportBook.Worksheets.Add(exportDt);                            
                            exportBook.SaveAs(SaveFilePath);
                        }
                    }

                }
            }
            else
            {
                MessageBox.Show("Не выбранно ни одной строки");
            }
        }
    }
}

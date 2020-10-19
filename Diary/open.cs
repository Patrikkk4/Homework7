using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.IO;
using Microsoft.Win32;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;


namespace Diary
{
    /// <summary>
    /// Структура импортирует файл xls или xlsx для добавления в таблицу событий
    /// </summary>
    struct open
    {
        /// <summary>
        /// Метод импорта файла в таблицу. Поддерживаются файлы xls и xlsx
        /// </summary>
        public static void openFile()
        {
            
            // Очистка лэйбла "Сохранено"
            MainWindow.Selfref.lblSaved.Content = null;

            // Переменная для выбранного пути файла XLS/XLSX
            string FilePath = string.Empty;

            // Инициализация экземпляра окна выбора файла
            OpenFileDialog opn = new OpenFileDialog();

            // Фильтр выбора файла по 
            opn.Filter = "Таблица(*.xlsx; *.xls)|*.xlsx;*.xls|Все файлы(*.*)|*.*";

            // Условие при положительном результате выбора файла
            if (opn.ShowDialog() == true)
            {
                // Присвоение переменной пути путь выбранного файла 
                FilePath = opn.FileName;
            }

            #region Переменные
            // Переменная колличества добавляемых строк
            int xlsRows = 0;
            // Переменная колличества добавляемых столбцов
            int xlsCol = 0;

            // Инициализация класса считывания данных из книги xls
            HSSFWorkbook book = null;

            // Инициализация класса считывания данных из книги xlsx
            XSSFWorkbook bookX = null;

            // Инициализация класса считывания данных из листа книги xls
            HSSFSheet sheet = null;

            // Инициализация класса считывания данных из листа книги xlsx
            XSSFSheet sheetX = null;
            #endregion

            #region Условия
            // Условие при отмене выбора файла
            if (FilePath == string.Empty)
            {

            }
            // Условие при выбранном файле xls
            else if (Path.GetExtension(FilePath) == ".xls")
            {
                try
                {
                    // Считывание книги xls по выбранному пути
                    book = new HSSFWorkbook(File.Open(FilePath, FileMode.Open));


                    // Считывание листа книги xls по выбранному пути
                    sheet = (HSSFSheet)book.GetSheetAt(0);

                    // Присвоение переменной значения количества строк 
                    xlsRows = sheet.PhysicalNumberOfRows;

                    // Присвоение переменной значения количества ячеек
                    xlsCol = sheet.GetRow(0).PhysicalNumberOfCells;
                }
                catch
                {
                    MessageBox.Show("Файл занят. Закройте импортируемый файл.");
                }

                //Вызов метода добавления импортированных данных в таблицу
                addRows(sheet, xlsRows, xlsCol);
            }
            // Условие при выбранном файле xlsx
            else if (Path.GetExtension(FilePath) == ".xlsx")
            {
                try
                {
                    // Считывание книги xlsx по выбранному пути
                    bookX = new XSSFWorkbook(File.Open(FilePath, FileMode.Open));

                    // Считывание листа книги xlsx по выбранному пути
                    sheetX = (XSSFSheet)bookX.GetSheetAt(0);

                    // Присвоение переменной значения количества строк 
                    xlsRows = sheetX.PhysicalNumberOfRows;

                    // Присвоение переменной значения количества ячеек
                    xlsCol = sheetX.GetRow(0).PhysicalNumberOfCells;
                }
                catch
                {
                    MessageBox.Show("Файл занят. Закройте импортируемый файл.");
                }

                //Вызов метода добавления импортированных данных в таблицу
                addRows(sheetX, xlsRows, xlsCol);
            }
            // Условие при выборе файла не соответствующего расширениям xls и xlsx
            else if (Path.GetExtension(FilePath) != ".xlsx" || Path.GetExtension(FilePath) != ".xls")
            {
                MessageBox.Show("Файл не поддерживается");
            }
            #endregion
        }

        #region Методы добавления строк

        /// <summary>
        /// Метод принимает считанные данные из книги xls и добавляет данные в таблицу Dtable
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="xlsRows"></param>
        /// <param name="xlsCol"></param>
        static void addRows(HSSFSheet sheet, int xlsRows, int xlsCol)
        {
            // Цикл добавления строк
            for (int l = 0; l < xlsRows; l++)
            {
                // формирование строки в соответствии с основной таблицей
                DataRow dRow = savexml.Dtable.NewRow();
                try
                {
                    // Цикл формирования строки 
                    for (int k = 0; k < xlsCol; k++)
                    {
                        // Заполнение строки данными 
                        dRow[k] = sheet.GetRow(l).GetCell(k).ToString();
                    }
                    // Добавление строки в таблицу
                    savexml.Dtable.Rows.Add(dRow);
                }
                catch
                {
                    MessageBox.Show("Файл имеет неверный формат. Проверьте количество столбцов, их должно быть не больше 5.");
                }
            }
        }

        /// <summary>
        /// Метод принимает считанные данные из книги xlsx и добавляет данные в таблицу Dtable
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="xlsRows"></param>
        /// <param name="xlsCol"></param>
        static void addRows(XSSFSheet sheet, int xlsRows, int xlsCol)
        {
            // Цикл добавления строк
            for (int l = 0; l < xlsRows; l++)
            {
                // формирование строки в соответствии с основной таблицей
                DataRow dRow = savexml.Dtable.NewRow();
                try
                {
                    // Цикл формирования строки 
                    for (int k = 0; k < xlsCol; k++)
                    {
                        // Заполнение строки данными 
                        dRow[k] = sheet.GetRow(l).GetCell(k);
                    }
                    // Добавление строки в таблицу
                    savexml.Dtable.Rows.Add(dRow);
                }
                catch
                {
                    MessageBox.Show("Файл имеет неверный формат. Проверьте количество столбцов, их должно быть не больше 5.");
                }
            }
            
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Data;

namespace Diary
{
    /// <summary>
    /// Структура записи вводимых данных
    /// </summary>
    struct basicData
    {
        #region Поля
        // поле ввода даты
        public string Date { get; set; }

        // поле ввода имени события
        public string NameHappening { get; set; }

        // поле ввода описания собития
        public string Happening { get; set; }

        // поле номера телефона
        public string PhoneNumber { get; set; }

        // поле важности
        public string Priority { get; set; }
       
        #endregion
       
        /// <summary>
        /// Метод обращается к классу basicData и формирует запись в файл xml
        /// </summary>
        public static void newHappening()
        {
            // Присвоение переменной выбранной даты в ChouseDate
            string date = MainWindow.Selfref.ChouseDate.SelectedDate.Value.Date.ToShortDateString();
            // Присвоение переменной выбранной времени в ChouseTime
            string time = MainWindow.Selfref.ChouseTime.Text.ToString();

            // Инициализация экземпляра класса ввода данных
            basicData save = new basicData();

            // Присвоение даты и времени
            if (MainWindow.Selfref.ChouseTime.Value != null)
            {
                save.Date = date + " в " + time;
            }
            else
            {
                save.Date = date;
            }

            // Переменная ввода имени события
            save.NameHappening = MainWindow.Selfref.txbNameHappening.Text;

            // Переменная ввода тела события
            save.Happening = MainWindow.Selfref.txbHappening.Text;

            // Переменная ввода телефонного номера
            save.PhoneNumber = MainWindow.Selfref.txbPhoneNumber.Text;

            // Переменная ввода важности события
            save.Priority = MainWindow.Selfref.cmbPriority.Text;

            // Создание новой строки таблицы записи введенных данных
            DataRow row = savexml.Dtable.NewRow();

            // Создание ячеек соответственно столбцам таблицы
            row["Дата"] = save.Date;
            row["Имя события"] = save.NameHappening;
            row["Событие"] = save.Happening;
            row["Номер телефона"] = save.PhoneNumber;
            row["Важность"] = save.Priority;

            // Добавление новой строки в таблицу
            savexml.Dtable.Rows.Add(row);
        }              
    }
}

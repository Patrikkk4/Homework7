using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Data;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Diary
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Переменные
        // Массив уровня приоритета. Присваивается cmbPriority
        string[] priority = new string[] { "Важно", "Нормально", "Неважно" };

        public bool CheckDataCell = false;

        // Объявление главного окна статическим классом для доступа к элементам управления
        public static MainWindow Selfref { get; set; }

        #endregion

        #region Главное окно
        public MainWindow()
        {
            InitializeComponent();
            Selfref = this;
            // присваение списка значений к combobox приоритета события
            cmbPriority.ItemsSource = priority;
            // Присваивание текущей даты полю выбора даты
            ChouseDate.SelectedDate = DateTime.Today;

            ChouseTime.Text = DateTime.Now.ToString("HH:mm");

            // Блокировка прошедших дат
            ChouseDate.BlackoutDates.AddDatesInPast();

            // Условие проверки наличия файла myaffairs.xml
            if (File.Exists(@"myaffair.json"))
            {
                // Действие при условии = true. Загружает файл xml с данными и разметкой страницы
                //savexml.Dtable.ReadXml(@"myaffairs.xml");

                // Считывание файла с данными json
                string json = File.ReadAllText(@"myaffair.json");

                // Действие при условии = true. Загружает файл json с данными и разметкой страницы
                savexml.Dtable = (DataTable)JsonConvert.DeserializeObject(json, (typeof(DataTable)));
            }

            // Присвоение источника таблицы
            table.ItemsSource = savexml.dataTable().DefaultView;
        }
        #endregion

        #region События

        
        /// <summary>
        /// Событие записи данных в файл. Нажатие на кнопку "добавить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            //if (ChouseTime.Value != null)
            //{
                // Вызов метода добавления события
                basicData.newHappening();
                // Очистка лэйбла сохранения
                lblSaved.Content = null;
            //}
            //else
            //{
            //    MessageBox.Show("Не выбрана дата и время");
            //}
        }

        /// <summary>
        /// Событие открытия основного файла с событиями
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFile_Click(object sender, RoutedEventArgs e)
        {
            // Вызов метода открытия основного файла
            open.openFile();
        }      
        
        /// <summary>
        /// Событие кнопки сохранения введенных данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Вызов метода сохранения введенных данных
            savexml.saveData();

            // Лэйбл указывает на время последнего сохранения данных
            lblSaved.Content = "Последнее сохранение в " + DateTime.Now.ToString("HH:mm");

            CheckDataCell = false;
        }

        /// <summary>
        /// Событие автогенерации столбцов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void table_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.Column.Header.ToString() == "Дата")
            {
                // Инициализация столбца
                DataGridColumn columnName = e.Column;
                // Задание размера столбца
                columnName.Width = 85;
            }
            if (e.Column.Header.ToString() == "Имя события")
            {
                // Инициализация столбца
                DataGridColumn columnName = e.Column;
                // Задание размера столбца
                columnName.Width = 85;
            }
            // Условие для столбца "Событие"
            if (e.Column.Header.ToString() == "Событие")
            {
                // Инициализация столбца
                DataGridColumn columnEvent = e.Column;
                // Задание размера столбца
                columnEvent.Width = 560;
            }
            if(e.Column.Header.ToString() == "Важность")
            {
                // Инициализация столбца
                DataGridColumn columnPriority = e.Column;
                // Задание размера столбца
                columnPriority.Width = 72;
            }
            

            // обработка столбца с текстовым содержанием
            var col = e.Column as DataGridTextColumn;

            // Запись стиля ячеек
            var style = new Style(typeof(TextBlock));

            // Пердача параметра переноса текста в ячейке
            style.Setters.Add(new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap));

            // Передача стиля элементам таблицы
            col.ElementStyle = style;
        }
        
        /// <summary>
        /// Событие кнопки очистить все
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClean_Click(object sender, RoutedEventArgs e)
        {
            // Инициализация экзкмпляра диалогового окна предупреждения
            AlarmWindow alarm = new AlarmWindow();

            // Вызов диалогового окна предупреждения
            alarm.Show();
        }

        /// <summary>
        /// Событие при закрытии главного окна. Событие обрабатывает три варианта из окна предупреждения: сохранить, не сохранять и отменить. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_ProgramClosing(object sender, CancelEventArgs e)
        {
            // Условие изменения количества строк в таблице. Если количество изменилось, то выводится предупреждение
            if (CheckDataCell == true)
            {
                // отмена закрытия программы
                e.Cancel = true;

                // Вывод предупреждения о сохранении данных
                MessageBoxResult result = MessageBox.Show("Убедитесь, что вы сохранили записи. Сохранить сейчас?", "Предупреждение", MessageBoxButton.YesNoCancel);

                // Условие при котором MessageBox возвращает "да" 
                if (result == MessageBoxResult.Yes)
                {
                    // Сохранение изменений в таблице
                    savexml.saveData();
                    // Выход из программы
                    e.Cancel = false;
                }
                // Условие при котором MessageBox возвращает "нет" 
                else if (result == MessageBoxResult.No)
                {
                    // Выход из программы
                    e.Cancel = false;
                }
            }
            // Условие изменения количества строк в таблице. Если количество не изменилось, то программа закрывается
            else
            {
                // Выход из программы
                e.Cancel = false;
            }
        }

        /// <summary>
        /// Событие добавления строки в таблицу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void table_CheckRows(object sender, DataGridRowEventArgs e)
        {
            // Присвоение переменной CheckDataCell значения true для вывода предупреждения о сохранения данных
            CheckDataCell = true;
        }

        /// <summary>
        /// Событие изменения выделенных ячеек и удаления строк
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void table_CheckCell(object sender, SelectedCellsChangedEventArgs e)
        {
            // Присвоение переменной CheckDataCell значения true для вывода предупреждения о сохранения данных
            CheckDataCell = true;
        }

        /// <summary>
        /// Событие нажатия кнопки "Экспорт в файл XLS; XLSX". Учитывается состояние CheckBox cbxExportAll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            // Условие, если галочка cbxExportAll установлено, то выделяется весь DataGrid table и экспортируется в XLS/XLSX файл
            if (cbxExportAll.IsChecked == false)
            {
                // Экспорт Таблицы в XLS/XLSX файл
                savexml.saveXLS();
            }
            else
            {
                // Выбор всех строк
                table.SelectAll();
                // Экспорт Таблицы в XLS/XLSX файл
                savexml.saveXLS();
            }
        }
        #endregion
    }
}

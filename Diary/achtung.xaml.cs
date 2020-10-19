using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace Diary
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class AlarmWindow : Window
    {
        /// <summary>
        /// Окно предупреждения
        /// </summary>
        public AlarmWindow()
        {
            InitializeComponent();
        }

        #region События
        /// <summary>
        /// Событие кнопки "отмена"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCansel_Click(object sender, RoutedEventArgs e)
        {
            // закрыть данную форму
            Close();
        }

        /// <summary>
        /// Событие кнопки "продолжить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            // Очистка основного файла
            //File.Delete(@"myaffairs.xml");
            File.Delete(@"myaffair.json");

            // Очистка таблицы table
            MainWindow.Selfref.table.ItemsSource = null;

            // Очистка таблицы Dtable
            savexml.Dtable.Clear();

            // Создание заголовка таблицы
            MainWindow.Selfref.table.ItemsSource = savexml.Dtable.DefaultView;

            // Значение переменной говорит о том, что не нужно проводить проверку на сохранение внесенных изменений
            MainWindow.Selfref.CheckDataCell = false;

            // закрыть данную форму
            Close();
        }
        #endregion
    }
}

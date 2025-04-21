using Microsoft.EntityFrameworkCore;
using PR20.Models;
using System.Windows;
using System.Windows.Controls;

namespace PR20
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDBInDataGrid();
        }
        private void LoadDBInDataGrid()
        {
            using (SpravochnikV5Context _db = new SpravochnikV5Context())
            {
                int selectIndex = dataGridMain.SelectedIndex;
                _db.DirectoryObjects.Load();
                _db.DirectoryCompletionWorks.Load();
                _db.DirectoryPrices.Load();
                _db.DirectoryTypeWorks.Load();
                _db.VolumeWorkObjects.Load();
                dataGridMain.ItemsSource = _db.VolumeWorkObjects.ToList();

                if (selectIndex != -1)
                {
                    if (selectIndex == dataGridMain.Items.Count) selectIndex--;
                    dataGridMain.SelectedIndex = selectIndex;
                    dataGridMain.ScrollIntoView(dataGridMain.SelectedIndex);
                }
                dataGridMain.Focus();
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as MenuItem;
            switch (s.Header)
            {
                case "Добавить": AddItem(); break;
                case "Редактировать": EditItem(); break;
                case "Удалить": DelItem(); break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var s = sender as Button;
            switch (s.Content)
            {
                case "Количество объектов в одном городе": LINQ(1); break;
                case "Итоговой стоимости всех работ одного вида": LINQ(2); break;
            }
        }

        private void AddItem()
        {
            Data.volumeWorkObject = null;
            AddDBItem f = new AddDBItem();
            f.Owner = this;
            f.ShowDialog();
            LoadDBInDataGrid();
        }
        private void EditItem()
        {
            if (dataGridMain.SelectedItem != null)
            {
                Data.volumeWorkObject = (VolumeWorkObject)dataGridMain.SelectedItem;
                AddDBItem f = new AddDBItem();
                f.Owner = this;
                f.ShowDialog();
                LoadDBInDataGrid();
            }
        }

        private void DelItem()
        {
            if (dataGridMain.SelectedItem != null)
            {
                MessageBoxResult result;
                result = MessageBox.Show("Удалить Запись?", " Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        VolumeWorkObject row = (VolumeWorkObject)dataGridMain.SelectedItem;
                        if (row != null)
                        {
                            using (SpravochnikV5Context _db = new SpravochnikV5Context())
                            {
                                _db.VolumeWorkObjects.Remove(row);
                                _db.SaveChanges();
                                LoadDBInDataGrid();
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка удаления");
                    }
                }
                else dataGridMain.Focus();
            }
            else MessageBox.Show("Выбери запись");
        }

        private void LINQ(int resultCase)
        {
            using (SpravochnikV5Context _db = new SpravochnikV5Context())
            {

                switch (resultCase)
                {
                    case 1:
                        var resultZp1 = _db.DirectoryObjects
                         .GroupBy(d => d.Town)
                         .Select(g => new
                         {
                             Town = g.Key,
                             Количество = g.Count()
                         });
                        dataGridSQL.ItemsSource = resultZp1.ToList();
                        ; break;

                    case 2:
                        var resultZp2 = (from работа in _db.DirectoryPrices
                                         join видРаботы in _db.DirectoryTypeWorks
                                             on работа.IdTypeWork equals видРаботы.IdTypeWork
                                         group работа by видРаботы.NameTypeWork into grouped
                                         select new
                                         {
                                             НаименованиеВидаРаботы = grouped.Key,
                                             ИтоговаяСтоимость = grouped.Sum(r => r.Price)
                                         });
                        dataGridSQL.ItemsSource = resultZp2.ToList();
                        ; break;
                }
            }
        }
    }
}
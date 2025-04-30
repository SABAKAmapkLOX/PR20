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

            var a = sender as Button;
            switch (a.Name)
            {
                case "btSQL1": LINQ(3); break;
                case "btSQL2": LINQ(4); break;
                case "btSQL3": LINQ(5); break;
                case "btSQL4": LINQ(6); break;
                case "btSQL5": LINQ(7); break;
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

                    //просмотр сведений обо всех объектах, на которых предусмотрено выполнение работ одного вида
                    case 3:
                        MessageBox.Show("Нема");
                        ; break;

                    //добавление записей в таблицу Справочник окончания работ
                    case 4:
                        var newEndRecords = new List<DirectoryCompletionWork>
                        {
                            new DirectoryCompletionWork { IdObject = 55 ,DateCompletionDate = new DateTime(2025, 9, 30) },
                            new DirectoryCompletionWork { IdObject = 56 ,DateCompletionDate = new DateTime(2025, 10, 15) },
                            new DirectoryCompletionWork { IdObject = 57 ,DateCompletionDate = new DateTime(2025, 11, 1) }
                        };
                        _db.DirectoryCompletionWorks.AddRange(newEndRecords);
                        _db.SaveChanges();
                        dataGridSQL.ItemsSource = newEndRecords.ToList();
                        ; break;

                    //подсчет количества месяцев строительства
                    case 5:
                        MessageBox.Show("Нема");
                        ; break;

                    //вывести список работ, срок окончания которых предусмотрен в 3 квартале этого года
                    case 6:
                        var query4 = (from price in _db.DirectoryPrices
                                      join volumeWorkObject in _db.VolumeWorkObjects
                                        on price.IdWork equals volumeWorkObject.IdWork
                                      join compWorks in _db.DirectoryCompletionWorks
                                        on volumeWorkObject.IdObject equals compWorks.IdObject
                                      where compWorks.DateCompletionDate >= new DateTime(2025, 7, 1) &&
                                      compWorks.DateCompletionDate <= new DateTime(2025, 9, 30)
                                      select new
                                      {
                                          НаименованиеРаботы = price.NameWork
                                      });
                        dataGridSQL.ItemsSource = query4.ToList();
                        ; break;

                    //вывести Наименование работ с максимальной расценкой
                    case 7:
                        var query5 = (from price in _db.DirectoryPrices
                                      join volumeWorkObject in _db.VolumeWorkObjects
                                      on price.IdWork equals volumeWorkObject.IdWork
                                      select new
                                      { 
                                          НаименованиеРаботы = price.NameWork,
                                          МаксимальнаяРасценка = _db.DirectoryPrices.Max(p => p.Price)
                                      });
                        dataGridSQL.ItemsSource = query5.ToList();
                        ; break;
                }
            }
        }
    }
}
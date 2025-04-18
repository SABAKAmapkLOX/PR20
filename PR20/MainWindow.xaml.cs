using Microsoft.EntityFrameworkCore;
using PR20.Models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                _db.DirectoryCompletionWorks.Load();
                _db.DirectoryObjects.Load();
                _db.DirectoryObject.Load();
                _db.DirectoryTypeWorks.Load();
                _db.VolumeWorkObjects.Load();
                dataGridMain.ItemsSource = _db.VolumeWorkObjects.ToList();

                if(selectIndex != -1)
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
            switch(s.Header)
            {
                case "Добавить": AddItem(); break;
                case "Редактировать": EditItem(); break;
                case "Удалить":; break;
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
    }
}
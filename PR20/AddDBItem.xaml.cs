using PR20.Models;
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

namespace PR20
{
    public partial class AddDBItem : Window
    {
        SpravochnikV5Context _db = new SpravochnikV5Context();
        VolumeWorkObject _volumeWorkObject;
        public AddDBItem()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbNameObject.ItemsSource = _db.DirectoryObjects.ToList();
            cbNameObject.DisplayMemberPath = "NameObject";
            cbNameWork.ItemsSource = _db.DirectoryPrices.ToList();
            cbNameWork.DisplayMemberPath = "NameWork";
            if (Data.volumeWorkObject != null)
            {
                this.Title = "Изменение записи";
                btAddItem.Content = "Изменение";
                _volumeWorkObject = _db.VolumeWorkObjects.Find(Data.volumeWorkObject.IdObject);
            }
            else
            {
                this.Title = "Добавление записи";
                btAddItem.Content = "Добавить";
                _volumeWorkObject = new VolumeWorkObject();

            }
            this.DataContext = _volumeWorkObject;
        }

        private void btAddItem_Click(object sender, EventArgs e)
        {
            StringBuilder error = new StringBuilder();

            if (cbNameWork.SelectedItem == null)
            {
                error.AppendLine("Введите название работы");
            } 
            if (cbNameObject.SelectedItem == null)
            {
                error.AppendLine("Введите название работы");
            }

            if (cbVolumeWork.Text == null)
            {
                error.Append("Введите текст");
            }
            try
            {
                if (Data.volumeWorkObject == null)
                {
                    _db.VolumeWorkObjects.Add(_volumeWorkObject);
                    _db.SaveChanges();
                }
                else
                {
                    _db.SaveChanges();
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }

        private void btClose_Click(object sender, EventArgs e) { Close(); }


    }
}

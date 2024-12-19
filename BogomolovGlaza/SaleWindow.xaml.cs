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

namespace BogomolovGlaza
{
    /// <summary>
    /// Логика взаимодействия для SaleWindow.xaml
    /// </summary>
    public partial class SaleWindow : Window
    {
        public SaleWindow()
        {
            InitializeComponent();
            var currentProducts = БогомоловГлазкиSaveEntities.GetContext().Product.ToList();

            ComboProduct.ItemsSource = currentProducts;
            

        }

        private void AddSale_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TBSaleCount.Text))
            {
                MessageBox.Show("Укажите количество пролукции");
                return;
            }
            if (Convert.ToInt32(TBSaleCount.Text) < 0)
            {
                MessageBox.Show("Укажите количество пролукции");
            }
            if (ComboProduct.SelectedItem == null)
            {
                MessageBox.Show("Выберите продукцию");
                return;
            }
            if (saleDate.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату продажи");
                return;
            }
            this.Close();
        }
    }
}

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BogomolovGlaza
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Agent currentAgent = new Agent();
        public AddEditPage(Agent SelectedAgent)
        {
            InitializeComponent();
            if (SelectedAgent != null)
                currentAgent = SelectedAgent;
            DataContext = currentAgent;
            var _current = БогомоловГлазкиSaveEntities.GetContext().AgentType.ToList();
            ComboTyp.ItemsSource= _current;

            var currentSales = БогомоловГлазкиSaveEntities.GetContext().ProductSale.ToList();
            currentSales = currentSales.Where(p=>p.AgentID==currentAgent.ID).ToList();
            SaleList.ItemsSource=currentSales;
        }

        private void ChangePictureBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                currentAgent.Logo = myOpenFileDialog.FileName;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(currentAgent.Title))
                errors.AppendLine("Укажите наменование агента");
            if (string.IsNullOrWhiteSpace(currentAgent.Address))
                errors.AppendLine("Укажите адрес агента");
            if (string.IsNullOrWhiteSpace(currentAgent.DirectorName))
                errors.AppendLine("Укажите ФИО директора");
            if (ComboTyp.SelectedIndex == -1)
                errors.AppendLine("Укажите тип агента");
            if (string.IsNullOrWhiteSpace(currentAgent.Priority.ToString()))
                errors.AppendLine("Укажите приоритет агента");
            if (currentAgent.Priority <= 0) 
                errors.AppendLine("Укажите положительный приоритет агента");
            if (string.IsNullOrWhiteSpace(currentAgent.INN))
                errors.AppendLine("Укажите ИНН агента");
            if (string.IsNullOrWhiteSpace(currentAgent.KPP))
                errors.AppendLine("Укажите КПП агента");
            if (string.IsNullOrWhiteSpace(currentAgent.Phone))
                errors.AppendLine("Укажите телефон агента");
            else
            {
                string ph = currentAgent.Phone.Replace("(", "").Replace("-", "").Replace("+", "").Replace(")", "").Replace(" ", "");
                if (((ph[1] == '9' || ph[1] == '4' || ph[1] == '8') && ph.Length != 11) || (ph[1] == '3' && ph.Length != 12))
                    errors.AppendLine("Укажите правильно телефон агента");
            }
            if (string.IsNullOrWhiteSpace(currentAgent.Email))
                errors.AppendLine("Укажите посту агента");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (ComboTyp.SelectedIndex == 0)
            {
                currentAgent.AgentTypeID = 1;
            }
            if (ComboTyp.SelectedIndex == 1)
            {
                currentAgent.AgentTypeID = 2;
            }
            if (ComboTyp.SelectedIndex == 2)
            {
                currentAgent.AgentTypeID = 3;
            }
            if (ComboTyp.SelectedIndex == 3)
            {
                currentAgent.AgentTypeID = 4;
            }
            if (ComboTyp.SelectedIndex == 4)
            {
                currentAgent.AgentTypeID = 5;
            }
            if (ComboTyp.SelectedIndex == 5)
            {
                currentAgent.AgentTypeID = 6;
            }
            if (currentAgent.ID == 0)
                БогомоловГлазкиSaveEntities.GetContext().Agent.Add(currentAgent);
            try
            {
                БогомоловГлазкиSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentAgent = (sender as Button).DataContext as Agent;
            var currentSake = БогомоловГлазкиSaveEntities.GetContext().ProductSale.ToList();
            currentSake =currentSake.Where(p=>p.AgentID==currentAgent.ID).ToList();
            if (currentSake.Count != 0)
            {
                MessageBox.Show("Невозможно удалить агента");
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        БогомоловГлазкиSaveEntities.GetContext().Agent.Remove(currentAgent);
                        БогомоловГлазкиSaveEntities.GetContext().SaveChanges();
                        Manager.MainFrame.Navigate(new ServicePage());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void AddSaleBtn_Click(object sender, RoutedEventArgs e)
        {
            SaleWindow myWindow = new SaleWindow();
            myWindow.ShowDialog();
            ProductSale newSale = new ProductSale();
            newSale.SaleDate = Convert.ToDateTime(myWindow.saleDate.Text);
            var currentProducts = БогомоловГлазкиSaveEntities.GetContext().Product.ToList();
            newSale.ProductID = myWindow.ComboProduct.SelectedIndex + 1;
            newSale.ProductCount = Convert.ToInt32(myWindow.TBSaleCount.Text.ToString());
            newSale.AgentID = currentAgent.ID;
            БогомоловГлазкиSaveEntities.GetContext().ProductSale.Add(newSale);
            try
            {
                БогомоловГлазкиSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");

                var currentSales = БогомоловГлазкиSaveEntities.GetContext().ProductSale.ToList();
                var currentSale = currentSales.Where(p => p.AgentID == currentAgent.ID).ToList()[0];
                БогомоловГлазкиSaveEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                SaleList.ItemsSource = БогомоловГлазкиSaveEntities.GetContext().ProductSale.ToList().Where(p => p.AgentID == currentAgent.ID).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void DeleteSaleBtn_Click(object sender, RoutedEventArgs e)
        {
            if(SaleList.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите продажу");
                return;
            }
            if (SaleList.SelectedItems.Count == 1)
            {
                var currentSales = БогомоловГлазкиSaveEntities.GetContext().ProductSale.ToList();
                var currentSale = currentSales.Where(p => p.AgentID == currentAgent.ID).ToList()[SaleList.SelectedIndex];
                БогомоловГлазкиSaveEntities.GetContext().ProductSale.Remove(currentSale);
                БогомоловГлазкиSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("Продажа удалена");
                БогомоловГлазкиSaveEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                SaleList.ItemsSource = БогомоловГлазкиSaveEntities.GetContext().ProductSale.ToList().Where(p => p.AgentID == currentAgent.ID).ToList();
                return;
            }
            else
            {
                MessageBox.Show("выберите 1 продажу");
                return;
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                БогомоловГлазкиSaveEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                SaleList.ItemsSource = БогомоловГлазкиSaveEntities.GetContext().ProductSale.ToList().Where(p=>p.AgentID == currentAgent.ID).ToList();   
            }
        }
    }
}

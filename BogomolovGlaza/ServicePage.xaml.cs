using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Xml.Serialization;

namespace BogomolovGlaza
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    
    public partial class ServicePage : Page
    {
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;
        List<Agent> CurrnetPageList = new List<Agent>();
        List<Agent> TableList;
        public ServicePage()
        {
            InitializeComponent();
            var currentServices = БогомоловГлазкиSaveEntities.GetContext().Agent.ToList();
            AgentListView.ItemsSource = currentServices;
            ComboSort.SelectedIndex = 0;
            ComboType.SelectedIndex = 0;
            UpdateServices();
        }


        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void TboxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void UpdateServices()
        {
            var currentSrvice = БогомоловГлазкиSaveEntities.GetContext().Agent.ToList();
            string searchText = TboxSearch.Text.ToLower();
            currentSrvice = currentSrvice.Where(p => p.Title.ToLower().Contains(searchText) || p.Email.ToLower().Contains(searchText) || p.Phone.Replace(" ","").Replace("+","").Replace("(","").Replace(")","").Replace("-","").ToLower().Contains(TboxSearch.Text.ToLower())).ToList();
            if (ComboSort.SelectedIndex == 0)
            {

            }
            if (ComboSort.SelectedIndex == 1)
            {
                currentSrvice = currentSrvice.OrderBy(p => p.Title).ToList();
            }
            if (ComboSort.SelectedIndex == 2)
            {
                currentSrvice = currentSrvice.OrderByDescending(p => p.Title).ToList();
            }
            if (ComboSort.SelectedIndex == 3)
            {
                currentSrvice = currentSrvice.OrderBy(p => p.Discount).ToList();
            }
            if (ComboSort.SelectedIndex == 4)
            {
                currentSrvice = currentSrvice.OrderByDescending(p => p.Discount).ToList();
            }
            if (ComboSort.SelectedIndex == 5)
            {
                currentSrvice = currentSrvice.OrderBy(p => p.Priority).ToList();
            }
            if (ComboSort.SelectedIndex == 6)
            {
                currentSrvice = currentSrvice.OrderByDescending(p => p.Priority).ToList();
            }
            AgentListView.ItemsSource = currentSrvice.ToList();

            if (ComboType.SelectedIndex == 0) { }
            if (ComboType.SelectedIndex == 1)
            {
                currentSrvice = currentSrvice.Where(p => (p.AgentTypeName == "МФО")).ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentSrvice = currentSrvice.Where(p => p.AgentTypeName == "ООО").ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentSrvice = currentSrvice.Where(p => p.AgentTypeName == "ЗАО").ToList();
            }
            if (ComboType.SelectedIndex == 4)
            {
                currentSrvice = currentSrvice.Where(p => p.AgentTypeName == "МКК").ToList();
            }
            if (ComboType.SelectedIndex == 5)
            {
                currentSrvice = currentSrvice.Where(p => p.AgentTypeName == "ОАО").ToList();
            }
            if (ComboType.SelectedIndex == 6)
            {
                currentSrvice = currentSrvice.Where(p => p.AgentTypeName == "ПАО").ToList();
            }
            AgentListView.ItemsSource = currentSrvice.ToList();
            TableList = currentSrvice;
            ChagePage(0, 0);
        }
        private void ChagePage(int direction, int? selecredPage)
        {
            CurrnetPageList.Clear();
            CountRecords = TableList.Count;
            if (CountRecords % 10 > 0)
            {
                CountPage = CountRecords / 10 + 1;
            }
            else
            {
                CountPage = CountRecords / 10;
            }
            Boolean Ifupdate = true;
            int min;
            if (selecredPage.HasValue)
            {
                if (selecredPage >= 0 && selecredPage <= CountPage)
                {
                    CurrentPage = (int)selecredPage;
                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    for (int i = CurrentPage * 10; i < min; i++)
                    {
                        CurrnetPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrnetPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrnetPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                }
            }
            if (Ifupdate)
            {
                PageLisxtBox.Items.Clear();
                for (int i = 1; i <= CountPage; i++)
                {
                    PageLisxtBox.Items.Add(i);
                }
                PageLisxtBox.SelectedIndex = CurrentPage;

                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                TBCount.Text = min.ToString();
                TBallRecords.Text = " из " + CountRecords.ToString();

                AgentListView.ItemsSource = CurrnetPageList;
                AgentListView.Items.Refresh();
            }
        }

        private void PageLisxtBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChagePage(0, Convert.ToInt32(PageLisxtBox.SelectedIndex.ToString()));
        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChagePage(1, null);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChagePage(2, null);
        }

        private void Redact_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Agent));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

            var currentAgent = (sender as Button).DataContext as Agent;
            var currentSake = БогомоловГлазкиSaveEntities.GetContext().ProductSale.ToList();
            currentSake = currentSake.Where(p => p.AgentID == currentAgent.ID).ToList();
            if (currentSake.Count != 0)
            {
                MessageBox.Show("Невозможно удалить агента");
            }
            else {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        БогомоловГлазкиSaveEntities.GetContext().Agent.Remove(currentAgent);
                        БогомоловГлазкиSaveEntities.GetContext().SaveChanges();
                        AgentListView.ItemsSource = БогомоловГлазкиSaveEntities.GetContext().Agent.ToList();
                        UpdateServices();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void Adbut_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                БогомоловГлазкиSaveEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                AgentListView.ItemsSource = БогомоловГлазкиSaveEntities.GetContext().Agent.ToList();
                UpdateServices();
            }
        }

        private void AgentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AgentListView.SelectedItems.Count > 1)
                ChangeProirity.Visibility = Visibility.Visible;
            else
                ChangeProirity.Visibility = Visibility.Hidden;
        }

        private void ChangeProirity_Click(object sender, RoutedEventArgs e)
        {
            int maxPriority = 0;
            foreach(Agent agent in AgentListView.SelectedItems)
            {
                if(agent.Priority > maxPriority)
                {
                    maxPriority = agent.Priority;
                }
            }
            SeWindow myWindow = new SeWindow(maxPriority);
            myWindow.ShowDialog();
            if (Convert.ToInt32(myWindow.TBPriority.Text) < 0)
                MessageBox.Show("Измените приоритет");
            else
            {
                int newPriotiry = Convert.ToInt32(myWindow.TBPriority.Text);
                foreach(Agent agent in AgentListView.SelectedItems)
                    agent.Priority = newPriotiry;
                try
                {
                    БогомоловГлазкиSaveEntities.GetContext().SaveChanges();
                    MessageBox.Show("Изменения сохранены");
                    UpdateServices();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}

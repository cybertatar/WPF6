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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BD.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        public ServicesPage()
        {
            InitializeComponent();

            ComboDiscount.SelectedIndex = 0;
            ComboSortBy.SelectedIndex = 0;
            UpdateServices();
            //LViewServices.ItemsSource = App.Context.Services.ToList();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void BtnAddService_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditPage());
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var currentService = (sender as Button).DataContext as Entities.Service;
            NavigationService.Navigate(new AddEditPage(currentService));
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var currentService = (sender as Button).DataContext as Entities.Service;

            if (MessageBox.Show($"Вы уверены, что хотите удалить услугу: {currentService.Title}?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                App.Context.Services.Remove(currentService);
                App.Context.SaveChanges();
                UpdateServices();
            }
        }

        private void ComboSortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void ComboDiscount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void UpdateServices()
        {
            var services = App.Context.Services.ToList();

            // Сортировка по цене
            if (ComboSortBy.SelectedIndex == 0)
                services = services.OrderBy(p => p.CostWithDiscount).ToList();
            else
                services = services.OrderByDescending(p => p.CostWithDiscount).ToList();

            // Фильтрация по размеру скидки
            switch(ComboDiscount.SelectedIndex)
            {
                case 1:
                    services = services.Where(p => p.Discount >= 0 && p.Discount < 0.05).ToList();
                    break;
                case 2:
                    services = services.Where(p => p.Discount >= 0.05 && p.Discount < 0.15).ToList();
                    break;
                case 3:
                    services = services.Where(p => p.Discount >= 0.15 && p.Discount < 0.30).ToList();
                    break;
                case 4:
                    services = services.Where(p => p.Discount >= 0.30 && p.Discount < 0.70).ToList();
                    break;
                case 5:
                    services = services.Where(p => p.Discount >= 0.70 && p.Discount <= 1).ToList();
                    break;
                default:
                    break;
            }

            // Поиск по названию (регистронезависимый)
            services = services.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            LViewServices.ItemsSource = services;
        }
    }
}

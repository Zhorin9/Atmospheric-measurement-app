using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System.Windows.Navigation;

namespace EngineeringThesis.ViewModel
{

    public class ViewModelLocator
    {
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public ButtonsViewModel Second
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ButtonsViewModel>();
            }
        }
        public StatisticsViewModel Statistics
        {
            get
            {
                return ServiceLocator.Current.GetInstance<StatisticsViewModel>();
            }
        }
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ButtonsViewModel>();
            SimpleIoc.Default.Register<StatisticsViewModel>();
        }
        
    }
}
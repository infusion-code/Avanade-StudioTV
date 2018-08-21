using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using AvanadeStudioTV.Views;
using AvanadeStudioTV.Models;
using AvanadeStudioTV.Network;
using Xamarin.Forms;
using System;
 

namespace AvanadeStudioTV.ViewModels
{
    public class VideoPageViewModel : INotifyPropertyChanged
    {

		
		private MasterDetailPage Master;

        private Item selectedItem = null;
        private INavigation Navigation;
        public Item SelectedItem
        {
            get => selectedItem;
            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    OnPropertyChanged("SelectedItem");
                 
                }
            }
        }
    
        public event PropertyChangedEventHandler PropertyChanged;

        public VideoPageViewModel()
        {
            //this.Master = master;          
            //Navigation = navigation;
        }
 

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

   
    }
}

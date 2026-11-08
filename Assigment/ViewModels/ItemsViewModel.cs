using Assigment.Commands;
using Assigment.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace Assigment.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public string _name;
        public string Name
        { 
            get { return _name; }
            set
            {
                _name = value;
                //Rasie the prop event
                if(value.Contains("_"))
                {
                    MessageBox.Show("UnderScore is not allowed");
                    value = value.Replace("_", " ");
                }
                OnPropertyChanged(nameof(Name));
            }
        }
        public string _details;
        public string Details
        {
            get { return _details; }
            set
            {
                _details = value;
                //Rasie the prop event
                OnPropertyChanged(nameof(Details));
            }
        }
        public double _price;
        public double Price
        {
            get { return _price; }
            set
            {
                _price = value;
                //Rasie the prop event
                OnPropertyChanged(nameof(Price));
            }
        }
        public string _measuringUnit;
        public string MeasuringUnit
        {
            get { return _measuringUnit; }
            set
            {
                _measuringUnit = value;
                //Rasie the prop event
                OnPropertyChanged(nameof(MeasuringUnit));
            }
        }
        public double _reOredrLevel;
        public double ReOrderLevel
        {
            get { return _reOredrLevel; }
            set
            {
                _reOredrLevel = value;
                //Rasie the prop event
                OnPropertyChanged(nameof(ReOrderLevel));
            }
        }
        public ObservableCollection<Item> _items;
        public ObservableCollection<Item> Items
        {

            get { return _items; }
            set
            {
                _items = value;
                //Rasie the prop event
                OnPropertyChanged(nameof(Items));
            }
        }
        public Item _selectedItem;
        public Item SelectedItem
        {

            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                //Rasie the prop event
                if (value != null) { 
                    Name = value.Name;
                    Details = value.Details;
                    Price = value.Price;
                    MeasuringUnit = value.MasureingUnite;
                    ReOrderLevel = value.ReOrederLevel;

                }
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand UpdateCommand { get; }
        public ItemsViewModel()
        {
            //set the defualt
            Name = "";
            Details = "";
            Price = 0;
            MeasuringUnit = "";
            ReOrderLevel = 10;
            SaveCommand = new RelyCommand(SaveItem, CanSaveItem);
            UpdateCommand = new RelyCommand(UpdateItem, CanUpdateItem);
            LoadItems();
        }
        private bool CanSaveItem(object obj)
        {
            return !string.IsNullOrEmpty(Name) && Price > 0 && SessionManager.LoggedInUser.PermissionLevel > 1;
        }
        private void SaveItem(object obj) {
            Item item = new Item
            {
                Name = Name,
                Details = Details,
                Price = Price,
                MasureingUnite = MeasuringUnit,
                ReOrederLevel = ReOrderLevel

            };
            IventoryDBContext db = new IventoryDBContext();
            db.Items.Add(item);
            db.SaveChanges();
            ClearFields();
            LoadItems();
            
        }
        private bool CanUpdateItem(object obj) {
            return SelectedItem != null && !string.IsNullOrEmpty(Name) && Price > 0 && SessionManager.LoggedInUser.PermissionLevel > 1;
        }
        private void UpdateItem(object obj)
        {
            IventoryDBContext db = new IventoryDBContext();
            Item item = db.Items.Find(SelectedItem.ItemId);
            if (item != null) {
                item.Name = Name;
                item.Details = Details;
                item.Price = Price;
                item.MasureingUnite = MeasuringUnit;
                item.ReOrederLevel = ReOrderLevel;
                db.SaveChanges();
            }
            ClearFields();
            LoadItems();
        }
        public void LoadItems()
        {
            IventoryDBContext db = new IventoryDBContext();
            Items = new ObservableCollection<Item>(db.Items.ToList()
                .Select(i =>
                {

                    i.StockValue = db.Stocks.Where(s => s.ItemId == i.ItemId).Select(s => s.StockValue).DefaultIfEmpty(0).Sum();
                    return i;
                }
                ).ToList());
        } 
        private void ClearFields()
        {
            Name = "";
            Details = "";
            Price = 0;
            MeasuringUnit = "";
            ReOrderLevel = 10;
        }
    }
}

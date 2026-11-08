using Assigment.Commands;
using Assigment.Commands;
using Assigment.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace Assigment.ViewModels
{
    public class AddSupplyViewModel : BaseViewModel
    {
        private IventoryDBContext db = new IventoryDBContext();
        public ObservableCollection<Supplier> _suppliers;
        public ObservableCollection<Supplier> Suppliers {
            get {  return _suppliers; }
            set { _suppliers = value; 
                OnPropertyChanged(nameof(Suppliers));
            }

        }
        private Item _selectedItem;
        public Item SelectedItem
        {
            get { return _selectedItem; }
            set {
                _selectedItem = value;
                if(value != null)
                {
                    MeausringUnitText = "(" + value.MasureingUnite + ")";
                }
            OnPropertyChanged(nameof(SelectedItem));}
        }
        private Supplier _selectedSupplier;
        public Supplier SelectedSupplier
        {
            get { return _selectedSupplier; }
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged(nameof(SelectedSupplier));
            }
        }
        public ObservableCollection<Item> _items;
        public ObservableCollection<Item> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }

        }
        private double _quantity;
        public double Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }
        private string _meausringUnitText;
        public string MeausringUnitText
        {
            get { return _meausringUnitText; }
            set
            {
                _meausringUnitText = value;
                OnPropertyChanged(nameof(MeausringUnitText));
            }
        }
        private double _purchasePrice;
        public double PurchasePrice
        { get { return _purchasePrice; }
        set
            {
                _purchasePrice = value;
                OnPropertyChanged(nameof(PurchasePrice));
            }
        }
        private ObservableCollection<SupplyDetail> _supplyDetails;
        public ObservableCollection<SupplyDetail> SupplyDetails
        {
            get { return _supplyDetails; }
            set { _supplyDetails = value;
                OnPropertyChanged(nameof(SupplyDetail));
            }
        }
        private double _totalAmount;
        public double TotalAmount
        {
            get { return _totalAmount; }
            set
            {
                _totalAmount = value;
                OnPropertyChanged(nameof(TotalAmount));
            }
        } 
        public ICommand SaveCommand { get; }
        public ICommand AddSupplyDetailCommand { get; }

        public AddSupplyViewModel()
        {
            SaveCommand = new RelyCommand(SaveSupply, CanSaveSupply);
            AddSupplyDetailCommand = new RelyCommand(AddSupplyDetail, CanAddSupplyDetail);
            TotalAmount = 0;
            SupplyDetails =  new ObservableCollection<SupplyDetail>();
        }
        private void LoadSuppliers()
        {
            Suppliers = new ObservableCollection<Supplier>(db.Suppliers);
        }
        private void LoadItems()
        {
 
            Items = new ObservableCollection<Item>(db.Items);
        }
        private bool CanSaveSupply(object obj)
        {
            return SelectedSupplier != null && SupplyDetails.Count > 0 && SessionManager.LoggedInUser.PermissionLevel > 1;
        }
        private void SaveSupply(object obj)
        {

          
            var transaction = db.Database.BeginTransaction();
            try
            {
                Supply supply = new Supply
                {
                    SupplierId = SelectedSupplier.SupplierId,
                    Date = DateTime.Now,
                    SupplyDetails = SupplyDetails

                };
                db.Supplies.Add(supply);
                db.SaveChanges(); // it is omportant to use it bucuse we eill use th id to save the sock
                foreach (var SupplyDetail in SupplyDetails)
                {
                    
                    var newStock = new Stock
                    {
                        ItemId = SupplyDetail.ItemId,
                        SupplyDetailId = SupplyDetail.SupplyDetailId,
                        StockValue = SupplyDetail.Quantity

                    };
                    db.Stocks.Add(newStock);
                }
         
                db.SaveChanges();
                transaction.Commit();
                MessageBox.Show("Supply saved successfully", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {

                transaction.Rollback();
                MessageBox.Show("An error occured whitle saving the supply","Eroor", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            

        }
        private bool CanAddSupplyDetail(object obj)
        {
            return SelectedItem != null && Quantity > 0 && PurchasePrice > 0 && SessionManager.LoggedInUser.PermissionLevel > 1;
        }
        private void AddSupplyDetail(object obj)
        {
            var SupplyDetail = new SupplyDetail
            {
                ItemId = SelectedItem.ItemId,
                Quantity = Quantity,
                PurchasePrice = PurchasePrice,
                Item = SelectedItem
            };
            SupplyDetails.Add(SupplyDetail);
            RefreshTotal();
        }
        
        private void RefreshTotal()
        {
            TotalAmount = SupplyDetails.Sum(sd => sd.Quantity * sd.PurchasePrice);
        }
      private bool CanDeleteSupplyDetail(object obj)
        {
            return SelectedItem != null && SupplyDetails.Count > 0;
        }
      


    }
}

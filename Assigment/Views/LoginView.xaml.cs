using Assigment.Model;
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

namespace Assigment.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        IventoryDBContext db = new IventoryDBContext();
        public LoginView()
        {
            InitializeComponent();
            txtUsername.Focus();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var user = db.Users.Find(txtUsername.Text);
            if(user != null)
            {
                if(user.Password == txtPassword.Password)
                {
                    MainWindow vm = new MainWindow();
                    Hide();
                    SessionManager.SetUser(user);
                    vm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Incoreect Password", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No Uer Found", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            txtUsername.Clear();
            txtPassword.Clear();

        }
    }
}

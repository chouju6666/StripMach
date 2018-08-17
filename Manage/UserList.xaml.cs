using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace RT
{
    /// <summary>
    /// Interaction logic for UserList.xaml
    /// </summary>
    public partial class UserList : Window
    {
        App _main = ((App)Application.Current);

        //<User>
        //  <name>chou</name>
        //  <password>l9FKFK9dDu40o9abnMfn7w==</password>
        //  <mail>Admin@ss.ss</mail>
        //  <role>Admin</role>
        //</User>

        public UserList()
        {
            InitializeComponent();
            Update();
        }

        public void Update()
        {   
            DgUser.SelectionChanged -= DgUser_SelectionChanged;
            MessageListener.Instance.UpdateRolesArray();
            DgUser.DataContext = AuthenticationService._users;
            DgUser.SelectionChanged += DgUser_SelectionChanged;

            txUserName.Text = string.Empty;
            txPassword.Password = string.Empty;
            txEmail.Text = string.Empty;
            txAutologoutTime.Text = string.Empty;
        }

        private void DgUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Add item means is selected item
            //Remove item means is previous item
            var SelectItem = ((AuthenticationService.InternalUserData)e.AddedItems[0]);
            txUserName.Text = SelectItem.Username;
            txPassword.Password = CryptorEngine.Decrypt(SelectItem.HashedPassword,true);
            txEmail.Text = SelectItem.Email;
            CbRoleSelected.SelectedItem = SelectItem.Roles[0];
            txAutologoutTime.Text = SelectItem.AutoLogoutTime;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Sure ?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
            try
            {
                XElement element = _main._RTaccount.Root.Element("Users");
                foreach (XElement el in element.Descendants("User"))
                {
                    if (el.Element("name").Value == txUserName.Text)
                    {
                        MessageBox.Show("Add failed, \nDuplicated User Name", "Error");
                        return;
                    }
                }

                _main._RTaccount.Root.Element("Users").Add(
                            new XElement("User",
                                new XElement("name", txUserName.Text),
                                new XElement("password", CryptorEngine.Encrypt(txPassword.Password, true)),
                                new XElement("mail", txEmail.Text),
                                new XElement("role", CbRoleSelected.SelectedItem),
                                new XElement("AutoLogoutTime", this.txAutologoutTime.Text)
                            ));

                _main._RTaccount.Save(_main.XmlFile);
                CommFeature.Instance.UpdateRTXml();
                MessageBox.Show("Add Success", "Warning");
                Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add failed, \n" + ex.Message, "Error");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (txUserName.Text == string.Empty)
            {
                MessageBox.Show("Please input user name", "Warning"); return;
            }
            else if (txPassword.Password == string.Empty)
            {
                MessageBox.Show("Please input password", "Warning"); return;
            }

            if (MessageBox.Show("Sure ?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            try
            {
                XElement element = _main._RTaccount.Root.Element("Users");
                foreach (XElement el in element.Descendants("User"))
                {
                    if (el.Element("name").Value == txUserName.Text)
                    {
                        el.Remove();
                        break;
                    }
                }

                _main._RTaccount.Root.Element("Users").Add(
                            new XElement("User",
                                new XElement("name", txUserName.Text),
                                new XElement("password", CryptorEngine.Encrypt(txPassword.Password,true)),
                                new XElement("mail", txEmail.Text),
                                new XElement("role", CbRoleSelected.SelectedItem),
                                new XElement("AutoLogoutTime", this.txAutologoutTime.Text)
                            ));

                _main._RTaccount.Save(_main.XmlFile);
                CommFeature.Instance.UpdateRTXml();
                MessageBox.Show("Save Success", "Warning");
                Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save failed, \n" + ex.Message, "Error");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Sure ?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
            try
            {
                XElement element = _main._RTaccount.Root.Element("Users");
                foreach (XElement el in element.Descendants("User"))
                {
                    if (el.Element("name").Value == txUserName.Text)
                    {
                        el.Remove();
                        break;
                    }
                }

                _main._RTaccount.Save(_main.XmlFile);
                CommFeature.Instance.UpdateRTXml();
                MessageBox.Show("Delete Success", "Warning");
                Update();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete failed, \n" + ex.Message, "Error");
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txEmail.Text = string.Empty;
            txUserName.Text = string.Empty;
            txPassword.Password = string.Empty;
            CbRoleSelected.SelectedItem = "Operator";
            txAutologoutTime.Text = "1";
        }

        private void PreviewTextInputHandler(object sender, TextCompositionEventArgs e)
        {
            // MIN Value is 0 and MAX value is 999
            var textBox = sender as TextBox;
            bool bFlag = false;
            if (!string.IsNullOrWhiteSpace(e.Text) && !string.IsNullOrWhiteSpace(textBox.Text))
            {
                string str = textBox.Text + e.Text;
                bFlag = str.Length <= 3 ? false : true;
            }
            e.Handled = (Regex.IsMatch(e.Text, "[^0-9]+") || bFlag);
        }
    }
}

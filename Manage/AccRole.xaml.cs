using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Windows.Controls;
using System.Collections;
using System;

namespace RT
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AccRole : Window
    {
        App _main = ((App)Application.Current);
        public AccRole()
        {
            InitializeComponent();
            Update();
        }
        private void Update()
        {
            CbRole.SelectionChanged -= ComboBox_SelectionChanged;
            MessageListener.Instance.UpdateRolesArray();
            DataContext = this;
            CbRole.SelectionChanged += ComboBox_SelectionChanged;
            CbRole.SelectedIndex = 0;
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                string selected = ((System.Windows.Controls.ComboBox)(sender)).SelectedItem.ToString();
                MessageListener.Instance.UpdateRolesPage(selected);
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Sure ?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.No) return;
            try
            {
                XElement element = _main._RTaccount.Root.Element("Roles");
                foreach (XElement el in element.Descendants("Role"))
                {
                    if (el.Attribute("name").Value == CbRole.SelectedValue.ToString())
                    {
                        el.Remove();
                        break;
                    }
                }

                List<XElement> SubAttr = new List<XElement>();
                foreach (DataGridRow dr in CommFeature.Instance.GetDataGridRows(Dg1))
                {
                    SubAttr.Add(new XElement("page",
                        new XAttribute("authority", ((AuthenticationService.InternalPage)(dr.Item)).authority),
                        ((AuthenticationService.InternalPage)(dr.Item)).pagename));
                }

                _main._RTaccount.Root.Element("Roles").Add(new XElement("Role",
                                new XAttribute("name", CbRole.SelectedValue.ToString()),
                                new XElement("pages", SubAttr)
                            ));


                _main._RTaccount.Save(_main.XmlFile);
                MessageBox.Show("Save Success", "Warning");
                Update();
            }catch (Exception ex)
            {
                MessageBox.Show("Save failed, \n" + ex.Message, "Error");
            }
        }      
    }

    public class PageLevel
    {
        public int PageId { get; set; }
        public string PageMsg { get; set; }
    }
}

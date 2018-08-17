using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace RT
{
    /// <summary>
    /// Common feature for each class used.
    /// </summary>
    class CommFeature : NotifyUIBase
    {
        App _main = ((App)Application.Current);
        public CommFeature()
        {
            _main._RTaccount = XDocument.Load(_main.XmlFile);
        }

        private static CommFeature mInstance;

        public static CommFeature Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new CommFeature();
                return mInstance;
            }
        }

        public void UpdateRTXml()
        {
            UpdateRoleTable();
            UpdateUserTable();
        }

        public void UpdateRoleTable()
        {
            AuthenticationService._roles = ((App)Application.Current)._RTaccount.Descendants("Role")
            .Select(r => new AuthenticationService.InternalRoleData
            {
                role = r.Attribute("name").Value,
                pages = r.Descendants("page").Select(x => new AuthenticationService.InternalPage
                {
                    authority = x.Attribute("authority").Value,
                    pagename = x.Value
                }).ToList()
            }).ToList();
        }

        public void UpdateUserTable()
        {
            AuthenticationService._users =
           ((App)Application.Current)._RTaccount.Descendants("User")
           .Select(r => new AuthenticationService.InternalUserData(
               r.Descendants("name").First().Value,
               r.Descendants("mail").First().Value,
               r.Descendants("password").First().Value,
               new string[] { r.Descendants("role").First().Value },
               r.Descendants("AutoLogoutTime").First().Value
              )
           ).ToList();
        }
        
        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }
    }
}

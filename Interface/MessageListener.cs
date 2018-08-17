using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace RT
{
    /// <summary>
    /// Message listener, singlton pattern.
    /// Inherit from DependencyObject to implement DataBinding.
    /// </summary>
    public class MessageListener : DependencyObject
    {
        /// <summary>
        /// 
        /// </summary>
        private static MessageListener mInstance;

        /// <summary>
        /// 
        /// </summary>
        private MessageListener()
        {

        }

        /// <summary>
        /// Get MessageListener instance
        /// </summary>
        public static MessageListener Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new MessageListener();
                return mInstance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void ReceiveMessage(string message, int processValue)
        {
            Message = message;
            ProcessValue = processValue;
            DispatcherHelper.DoEvents();
        }

        /// <summary>
        /// Get or set received message
        /// </summary>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        /// <summary>
        /// Get or set received message
        /// </summary>
        public int ProcessValue
        {
            get { return (int)GetValue(MessageProperty2); }
            set { SetValue(MessageProperty2, value); }
        }

        public void UpdateRolesArray()
        {
            if (RolesArray == null)
                RolesArray = new ObservableCollection<string>();
            foreach (var c in AuthenticationService._roles)
            {
                if (!RolesArray.Contains(c.role))
                    RolesArray.Add(c.role);
            }
        }

        public void UpdateRolesPage(string _Role)
        {
            RoleLevel = new ObservableCollection<PageLevel> {
                new PageLevel { PageMsg = "None", PageId = 0 },
                new PageLevel { PageMsg = "Read", PageId = 1 },
                new PageLevel { PageMsg = "ReadAndWrite", PageId = 2 }
            };

            foreach (var c in AuthenticationService._roles)
            {
                if (c.role.ToString() == _Role)
                {
                    RolesPage = c.pages;
                    break;
                }
            }     
        }

        public ObservableCollection<string> RolesArray
        {
            get { return (ObservableCollection<string>)GetValue(MessageProperty4); }
            set { SetValue(MessageProperty4, value); }
        }

        public List<AuthenticationService.InternalPage> RolesPage
        {
            get { return (List<AuthenticationService.InternalPage>)GetValue(MessageProperty5); }
            set { SetValue(MessageProperty5, value); }
        }

        public ObservableCollection<PageLevel> RoleLevel 
        {
            get { return (ObservableCollection<PageLevel>)GetValue(MessageProperty6); }
            set { SetValue(MessageProperty6, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MessageListener), new UIPropertyMetadata(null));

        public static readonly DependencyProperty MessageProperty2 =
            DependencyProperty.Register("ProcessValue", typeof(int), typeof(MessageListener), new UIPropertyMetadata(null));

        public static readonly DependencyProperty MessageProperty3 =
            DependencyProperty.Register("IsVisible", typeof(int), typeof(MessageListener), new UIPropertyMetadata(null));

        public static readonly DependencyProperty MessageProperty4 =
            DependencyProperty.Register("RolesArray", typeof(ObservableCollection<string>), typeof(MessageListener), new UIPropertyMetadata(null));
        
        public static readonly DependencyProperty MessageProperty5 =
            DependencyProperty.Register("RolesPage", typeof(List<AuthenticationService.InternalPage>), typeof(MessageListener), new UIPropertyMetadata(null));

        public static readonly DependencyProperty MessageProperty6 =
           DependencyProperty.Register("RoleLevel", typeof(ObservableCollection<PageLevel>), typeof(MessageListener), new UIPropertyMetadata(null));

    }
}

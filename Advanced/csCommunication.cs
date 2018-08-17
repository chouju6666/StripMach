using Clifton.Core.Pipes;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RT.Advanced
{
    public delegate void DelegateMessage(string Reply);
    class csCommunication
    {
        private string _pipeName;
        ClientPipe clientPipes;
        public event EventHandler<string> DataReceived;
        public event EventHandler<bool> IsDisConnected;
        private int ConnectretryCount = 3;
        private int CurrntretryCount = 1;
        public bool IsConnected = false;

        public void Listen(string PipeName)
        {
            try
            {
                clientPipes = new ClientPipe(".", PipeName, p => p.StartStringReaderAsync());
                clientPipes.DataReceived -= clientPipes_DataReceived;
                clientPipes.PipeClosed -= clientPipes_PipeClosed;
                clientPipes.DataReceived += clientPipes_DataReceived;
                clientPipes.PipeClosed += clientPipes_PipeClosed;
                clientPipes.Connect(1000);
                if (clientPipes.Isconnected)
                {
                    IsConnected = clientPipes.Isconnected;
                    CurrntretryCount = 1;
                    MessageListener.Instance.ReceiveMessage(string.Format("Connect {0} Successed", PipeName), 20);
                }
            }
            catch(TimeoutException et)
            {
                clientPipes = null;
                MessageListener.Instance.ReceiveMessage(string.Format(string.Format("Connect {0} timeout...{1}",PipeName, CurrntretryCount), 20), 20);
                CurrntretryCount++;
                if (CurrntretryCount > ConnectretryCount)
                {
                    CurrntretryCount = 1;
                    MessageListener.Instance.ReceiveMessage(string.Format("Connect {0} failed", PipeName), 20);
                }
                else
                    Listen(PipeName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void clientPipes_PipeClosed(object sender, EventArgs e)
        {
            if (IsDisConnected != null)
                IsDisConnected.Invoke(sender, true);
        }

        void clientPipes_DataReceived(object sender, PipeEventArgs e)
        {
            if (DataReceived != null)
                DataReceived.Invoke(sender, e.String);
        }
    }
}

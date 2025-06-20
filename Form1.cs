namespace Simulator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Logger.Instance.LogReceived += AppendLog;
            SocketHandler socketHandler = new SocketHandler();            
        }

        private void AppendLog(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendLog), message);
            }
            else
            {
                LogBox.AppendText(message + Environment.NewLine);
            }
        }
    }
}

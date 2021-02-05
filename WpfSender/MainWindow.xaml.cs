using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfSender.Tools;

namespace WpfSender
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            string strDlgTitle = "RecvMessage";
            strDlgTitle = "WindowsProject2";

            //接收端的窗口句柄
            IntPtr hwndRecvWindow = ImportFromDLL.FindWindow(null, strDlgTitle);
            if (hwndRecvWindow == IntPtr.Zero)
            {
                Console.WriteLine("请先启动接收消息程序");
                return;
            }

            //自己的窗口句柄
            IntPtr hwndSendWindow = ImportFromDLL.GetConsoleWindow();

            hwndSendWindow = WHwnd.GetWindowHwndSource(this);
            if (hwndSendWindow == IntPtr.Zero)
            {
                Console.WriteLine("获取自己的窗口句柄失败，请重试");
                return;
            }

            for (int i = 0; i < 1; i++)
            {
                string strText = DateTime.Now.ToString();

                byte[] imagedata = new byte[65536];
                byte[] data = new byte[65536];
                Random random = new Random();
                for (int index = 0; index < imagedata.Length; index++)
                {
                    int ccc = random.Next(0, byte.MaxValue);
                    imagedata[index] = (byte)44;
                    data[index] = (byte)ccc;
                }
                //填充COPYDATA结构
                ImportFromDLL.COPYDATASTRUCT copydata = new ImportFromDLL.COPYDATASTRUCT();
                //copydata.cbData = Encoding.Default.GetBytes(strText).Length; //长度 注意不要用strText.Length;
                //copydata.lpData = strText;                                   //内容

                // {
                ImportFromDLL.COPYDATASTRUCT cds = new ImportFromDLL.COPYDATASTRUCT();

                //cds.dwData = (IntPtr)flag;

                cds.cbData = data.Length;

                cds.lpData = Marshal.AllocHGlobal(data.Length);

                Marshal.Copy(data, 0, cds.lpData, data.Length);

                //SendMessage(WINDOW_HANDLER, WM_COPYDATA, 0, ref cds);
                // }

                //copydata.cbData = imagedata.Length; //长度 注意不要用strText.Length;
                //copydata.lpData = imagedata;                                   //内容

                ImportFromDLL.SendMessage(hwndRecvWindow, ImportFromDLL.WM_COPYDATA, hwndSendWindow, ref cds);

                Console.WriteLine(strText);
                Thread.Sleep(1000);
            }

        }
    }
}

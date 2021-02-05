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


        public class ImportFromDLL
        {
            public const int WM_COPYDATA = 0x004A;

            //启用非托管代码
            [StructLayout(LayoutKind.Sequential)]
            public struct COPYDATASTRUCT
            {
                public int dwData;    //not used
                public int cbData;    //长度
                [MarshalAs(UnmanagedType.LPStr)]
                public string lpData;
            }

            [DllImport("User32.dll")]
            public static extern int SendMessage(
                IntPtr hWnd,     // handle to destination window 
                int Msg,         // message
                IntPtr wParam,    // first message parameter 
                ref COPYDATASTRUCT pcd // second message parameter 
            );

            [DllImport("User32.dll", EntryPoint = "FindWindow")]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

            [DllImport("Kernel32.dll", EntryPoint = "GetConsoleWindow")]
            public static extern IntPtr GetConsoleWindow();

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

            hwndSendWindow= WHwnd.GetWindowHwndSource(this);
            if (hwndSendWindow == IntPtr.Zero)
            {
                Console.WriteLine("获取自己的窗口句柄失败，请重试");
                return;
            }

            for (int i = 0; i < 10; i++)
            {
                string strText = DateTime.Now.ToString();
                //填充COPYDATA结构
                ImportFromDLL.COPYDATASTRUCT copydata = new ImportFromDLL.COPYDATASTRUCT();
                copydata.cbData = Encoding.Default.GetBytes(strText).Length; //长度 注意不要用strText.Length;
                copydata.lpData = strText;                                   //内容

                ImportFromDLL.SendMessage(hwndRecvWindow, ImportFromDLL.WM_COPYDATA, hwndSendWindow, ref copydata);

                Console.WriteLine(strText);
                Thread.Sleep(1000);
            }

        }


        #region 获取自己的窗口句柄
        
        #endregion
        }



    public class WHwnd
    {
        /// <summary>
        /// 主窗体句柄
        /// </summary>
        public static System.Windows.Interop.HwndSource Hwnd;
        /// <summary>
        /// 获取窗体句柄
        /// </summary>
        /// <param name="window">窗体</param>
        public static IntPtr GetWindowHwndSource(DependencyObject window, bool isHwnd = true)
        {
            var formDependency = System.Windows.Interop.HwndSource.FromDependencyObject(window);
            System.Windows.Interop.HwndSource winformWindow = (formDependency as System.Windows.Interop.HwndSource);
            if (isHwnd)
                Hwnd = winformWindow;
            return winformWindow.Handle;
        }

    }
}

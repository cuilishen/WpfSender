using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfSender.Tools
{
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

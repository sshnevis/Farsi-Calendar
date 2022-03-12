using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;

namespace AmirCalendar.PopupControl
{
  internal static class NativeMethods
  {
    internal const int WM_NCHITTEST = 132;
    internal const int WM_NCACTIVATE = 134;
    internal const int WS_EX_NOACTIVATE = 134217728;
    internal const int HTTRANSPARENT = -1;
    internal const int HTLEFT = 10;
    internal const int HTRIGHT = 11;
    internal const int HTTOP = 12;
    internal const int HTTOPLEFT = 13;
    internal const int HTTOPRIGHT = 14;
    internal const int HTBOTTOM = 15;
    internal const int HTBOTTOMLEFT = 16;
    internal const int HTBOTTOMRIGHT = 17;
    internal const int WM_PRINT = 791;
    internal const int WM_USER = 1024;
    internal const int WM_REFLECT = 8192;
    internal const int WM_COMMAND = 273;
    internal const int CBN_DROPDOWN = 7;
    internal const int WM_GETMINMAXINFO = 36;

    [SuppressUnmanagedCodeSecurity]
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private extern static int AnimateWindow(HandleRef windowHandle, int time, global::AmirCalendar.PopupControl.NativeMethods.AnimationFlags flags);

    internal static void AnimateWindow(Control control, int time, global::AmirCalendar.PopupControl.NativeMethods.AnimationFlags flags)
    {
      try
      {
        new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
        global::AmirCalendar.PopupControl.NativeMethods.AnimateWindow(new HandleRef((object) control, control.Handle), time, flags);
      }
      catch (SecurityException)
      {
      }
    }

    internal static int HIWORD(int n)
    {
      return n >> 16 & (int) ushort.MaxValue;
    }

    internal static int HIWORD(IntPtr n)
    {
      return global::AmirCalendar.PopupControl.NativeMethods.HIWORD((int) (long) n);
    }

    internal static int LOWORD(int n)
    {
      return n & (int) ushort.MaxValue;
    }

    internal static int LOWORD(IntPtr n)
    {
      return global::AmirCalendar.PopupControl.NativeMethods.LOWORD((int) (long) n);
    }

    [Flags]
    internal enum AnimationFlags
    {
      Roll = 0,
      HorizontalPositive = 1,
      HorizontalNegative = 2,
      VerticalPositive = 4,
      VerticalNegative = 8,
      Center = 16,
      Hide = 65536,
      Activate = 131072,
      Slide = 262144,
      Blend = 524288,
      Mask = 1048575,
    }

    internal struct MINMAXINFO
    {
        public Point reserved;
        public Size maxSize;
        public Point maxPosition;
        public Size minTrackSize;
        public Size maxTrackSize;
    }
  }
}

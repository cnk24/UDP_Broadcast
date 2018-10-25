using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UDP_Broadcast
{
    public class IconHelper
    {
        public static Icon GetIcon(string sPath, bool bSizeJumbo = false)
        {
            WindowsShellAPI.SHFILEINFO shinfo = new WindowsShellAPI.SHFILEINFO();            
            IntPtr hImg = WindowsShellAPI.SHGetFileInfo(sPath, 0, out shinfo, (uint)Marshal.SizeOf(typeof(WindowsShellAPI.SHFILEINFO)), WindowsShellAPI.SHGFI.SHGFI_SYSICONINDEX);

            WindowsShellAPI.SHIL currentshil = WindowsShellAPI.SHIL.SHIL_EXTRALARGE;
            if (bSizeJumbo == true)
            {
                currentshil = WindowsShellAPI.SHIL.SHIL_JUMBO;
            }

            //WindowsShellAPI.IImageList imglist = null;
            //int rsult = WindowsShellAPI.SHGetImageList(currentshil, ref WindowsShellAPI.IID_IImageList, out imglist);
            //IntPtr hicon = IntPtr.Zero;
            //imglist.GetIcon(shinfo.iIcon, (int)WindowsShellAPI.ImageListDrawItemConstants.ILD_TRANSPARENT, ref hicon);

            IntPtr pimgList;
            int rsult = WindowsShellAPI.SHGetImageList(currentshil, ref WindowsShellAPI.IID_IImageList, out pimgList);
            IntPtr hicon = WindowsShellAPI.ImageList_GetIcon(pimgList, shinfo.iIcon, 0);

            Icon myIcon = Icon.FromHandle(hicon);

            return myIcon;
        }

    }

}

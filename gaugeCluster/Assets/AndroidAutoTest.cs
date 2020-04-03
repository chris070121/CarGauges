using UnityEngine;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System;
using System.Diagnostics;
using UnityEngine.UI;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

public class AndroidAutoTest : MonoBehaviour {

	Texture2D tex;
	// Use this for initialization
	void Start () {
		tex = new Texture2D(10, 10);
	}
	
	// Update is called once per frame
	void Update () {
        createBitmap();
     
        GetComponentInChildren<RawImage>().texture = tex;
	}

    private void createBitmap()
    {

        Process[] flash = Process.GetProcessesByName("Notepad");
        IntPtr hWnd = FindWindow(null, flash[0].MainWindowTitle.ToString());

        Bitmap bmp = null;
        IntPtr hdcFrom = GetDC(hWnd);
        IntPtr hdcTo = CreateCompatibleDC(hdcFrom);
        //X and Y coordinates of window
        RECT Rect = new RECT();
        GetWindowRect(hWnd, ref Rect);
        //Find the height and width of the process
        int Width = Rect.right - Rect.left;
        int Height = Rect.bottom - Rect.top;
        IntPtr hBitmap = CreateCompatibleBitmap(hdcFrom, Width, Height);
        if (hBitmap != IntPtr.Zero)
        {
            // adjust and copy
            IntPtr hLocalBitmap = SelectObject(hdcTo, hBitmap);
            BitBlt(hdcTo, 0, 0, Width, Height,
                hdcFrom, 0, 0, SRCCOPY);
            SelectObject(hdcTo, hLocalBitmap);
            //We delete the memory device context.
            DeleteDC(hdcTo);
            //We release the screen device context.
            ReleaseDC(hWnd, hdcFrom);
            //Image is created by Image bitmap handle and assigned to Bitmap variable.
            bmp = System.Drawing.Image.FromHbitmap(hBitmap);
            //Delete the compatible bitmap object. 
            DeleteObject(hBitmap);
            MemoryStream stream = new MemoryStream();
         
            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);            
            
            tex.LoadImage(stream.ToArray());
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    static IntPtr hWnd;
    public const int SRCCOPY = 13369376;
    public const int WM_CLICK = 0x00F5;
    [DllImport("user32.dll", SetLastError = true)]
    static extern bool GetWindowRect(IntPtr hWnd, ref RECT Rect);
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.dll", EntryPoint = "GetDC")]
    internal extern static IntPtr GetDC(IntPtr hWnd);
    [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC")]
    internal extern static IntPtr CreateCompatibleDC(IntPtr hdc);
    [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
    internal extern static IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
    [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
    internal extern static IntPtr DeleteDC(IntPtr hDc);
    [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
    internal extern static IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);
    [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
    internal extern static bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSource, int xSrc, int ySrc, int RasterOp);
    [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
    internal extern static IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
    [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
    internal extern static IntPtr DeleteObject(IntPtr hDc);
    //[DllImport("user32.dll")]
    //public static extern int SendMessage(
    //      int hWnd,      // handle to destination window
    //      uint Msg,       // message
    //      long wParam,  // first message parameter
    //      long lParam   // second message parameter
    //      );

}

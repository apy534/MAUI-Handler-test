using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace XamContextMenu.CustomControl
{
    public class MenuRequestedEventArgs
    {
        public MenuRequestedEventArgs(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public double X { get; }
        public double Y { get; }
        public double Width { get; }
        public double Height { get; }
    }

    public delegate void MenuRequested(object sender, MenuRequestedEventArgs e);

    public class ContextMenuView : View
    {
        public MenuRequested MenuRequested;

        public void RequestMenu(object sender, double x, double y, double width, double height)
        {
            MenuRequested?.Invoke(sender, new MenuRequestedEventArgs(x, y, width, height));
        }

        public bool CanCut { get; set; } = true;
        public bool CanCopy { get; set; } = true;
        public bool CanDelete { get; set; } = true;
        public bool HasProperties { get; set; } = true;

        public Action OnCopy { get; set; }
        public Action OnDelete { get; set; }
        public Action OnProperties { get; set; }
        public Action OnCut { get; set; }

        public IContextCommand[] Commands { get; set; }
    }
}


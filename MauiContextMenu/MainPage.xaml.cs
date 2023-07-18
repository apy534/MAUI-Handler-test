using XamContextMenu.CustomControl;

namespace MauiContextMenu
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        ContextMenuView _popupMenu;

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            if (_popupMenu == null)
            {
                _popupMenu = new ContextMenuView { InputTransparent = true };
                Grid.SetRow(_popupMenu, 0);
                Grid.SetColumn(_popupMenu, 0);
                layout.Children.Add(_popupMenu);
            }

            _popupMenu.CanDelete = true;
            _popupMenu.CanCopy = true;
            _popupMenu.CanCut = true;
            _popupMenu.HasProperties = false;

            _popupMenu.OnCut = () => DisplayAlert("Cut", "Cut Clicked", "OK");
            _popupMenu.OnDelete = () => DisplayAlert("Delete", "Delete Clicked", "OK");
            _popupMenu.OnCopy = () => DisplayAlert("Copy", "Copy Clicked", "OK");
            _popupMenu.OnProperties = () => DisplayAlert("Properties", "Properties Clicked", "OK");

            _popupMenu.IsVisible = true;
            _popupMenu.RequestMenu(layout, layout.AnchorX, button.AnchorY, 300, 80);
        }
    }
}

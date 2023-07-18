using System;
using CoreGraphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Handlers;
using UIKit;
using ObjCRuntime;
using XamContextMenu.CustomControl;

namespace MauiContextMenu.Platforms.iOS.Handler
{
    public class ContextMenuViewHandler : ViewHandler<ContextMenuView, ContextMenuiOS>
    {
        private nfloat _height;
        private nfloat _width;

        string ACTION_PREFIX = "custom_action";
        int MAX_CUSTOM_ACTIONS = 4;

        public static IPropertyMapper<ContextMenuView, ContextMenuViewHandler> PropertyMapper =
            new PropertyMapper<ContextMenuView, ContextMenuViewHandler>(ViewHandler.ViewMapper)
            {

            };

        public ContextMenuViewHandler() : base(PropertyMapper)
        {
        }

        protected override ContextMenuiOS CreatePlatformView()
        {
            return new ContextMenuiOS(VirtualView);
        }

        protected override void ConnectHandler(ContextMenuiOS platformView)
        {
            base.ConnectHandler(platformView);

            VirtualView.MenuRequested += OnMenuRequested;
        }

        protected override void DisconnectHandler(ContextMenuiOS platformView)
        {
            base.DisconnectHandler(platformView);

            VirtualView.MenuRequested -= OnMenuRequested;
        }

        private void OnMenuRequested(object _, MenuRequestedEventArgs e)
        {
            try
            {
                var _menu = UIMenuController.SharedMenuController;
                PlatformView.BecomeFirstResponder();
                var items = new List<UIMenuItem>();
                if (VirtualView.Commands?.Any() == true)
                {
                    var index = 0;
                    foreach (var item in VirtualView.Commands.Where(o => o.Command.CanExecute(null)).Take(MAX_CUSTOM_ACTIONS))
                    {
                        items.Add(new UIMenuItem
                        {
                            Title = item.Name,
                            Action = new Selector($"{ACTION_PREFIX}{index++}:")
                        });
                    }
                }
                if (VirtualView.HasProperties)
                {
                    items.Add(new UIMenuItem("Options...", new Selector("Properties:")));
                }
                _menu.MenuItems = items.ToArray();
                _menu.SetTargetRect(new CGRect(e.X, e.Y, e.Width, e.Height), PlatformView);
                _menu.MenuVisible = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}


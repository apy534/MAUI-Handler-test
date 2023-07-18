using System;
using CoreGraphics;
using Foundation;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using UIKit;
using ObjCRuntime;
using XamContextMenu.CustomControl;

namespace MauiContextMenu.Platforms.iOS.CustomRenderers
{
    public class ContextMenuViewRenderer : ViewRenderer<ContextMenuView, UIView>
    {
        private UIView _nativeControl;
        private ContextMenuView _xamarinControl;
        private nfloat _height;
        private nfloat _width;

        const string ACTION_PREFIX = "custom_action";
        const int MAX_CUSTOM_ACTIONS = 4;

        protected override void OnElementChanged(ElementChangedEventArgs<ContextMenuView> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                if (e.NewElement != null)
                {
                    _xamarinControl = e.NewElement;
                    _xamarinControl.MenuRequested += OnMenuRequested;
                }
                _height = UIScreen.MainScreen.Bounds.Height;
                _width = UIScreen.MainScreen.Bounds.Width;
                _nativeControl = new UIView(new CGRect(0, 0, _width, _height));
                SetNativeControl(_nativeControl);
            }
        }

        private void OnMenuRequested(object _, MenuRequestedEventArgs e)
        {
            try
            {
                var _menu = UIMenuController.SharedMenuController;
                BecomeFirstResponder();
                var items = new List<UIMenuItem>();
                if (Element.Commands?.Any() == true)
                {
                    var index = 0;
                    foreach (var item in Element.Commands.Where(o => o.Command.CanExecute(null)).Take(MAX_CUSTOM_ACTIONS))
                    {
                        items.Add(new UIMenuItem
                        {
                            Title = item.Name,
                            Action = new Selector($"{ACTION_PREFIX}{index++}:")
                        });
                    }
                }
                if (Element.HasProperties)
                {
                    items.Add(new UIMenuItem("Options...", new Selector("Properties:")));
                }
                _menu.MenuItems = items.ToArray();
                _menu.SetTargetRect(new CGRect(e.X, e.Y, e.Width, e.Height), _nativeControl);
                _menu.MenuVisible = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override bool CanBecomeFirstResponder => true;

        public override bool CanPerform(Selector action, NSObject withSender)
        {
            switch (action.Name)
            {
                case "Properties:":
                    return Element.HasProperties;
                case "cut:":
                    return Element.CanCut;
                case "copy:":
                    return Element.CanCopy;
                case "delete:":
                    return Element.CanDelete;
            }
            if (action.Name.StartsWith(ACTION_PREFIX))
            {
                var indexString = action.Name.Substring(ACTION_PREFIX.Length, action.Name.Length - ACTION_PREFIX.Length - 1);
                var index = int.Parse(indexString);
                return Element.Commands[index].Command.CanExecute(null);
            }
            return false;
        }

        [Export("Properties:")]
        public void Properties(UIMenuController controller)
        {
            Element?.OnProperties();
        }

        [Export("custom_action0:")]
        public void Action0(UIMenuController controller)
        {
            Element.Commands[0].Command.Execute(null);
        }

        [Export("custom_action1:")]
        public void Action1(UIMenuController controller)
        {
            Element.Commands[1].Command.Execute(null);
        }

        [Export("custom_action2:")]
        public void Action2(UIMenuController controller)
        {
            Element.Commands[2].Command.Execute(null);
        }

        [Export("custom_action3:")]
        public void Action3(UIMenuController controller)
        {
            Element.Commands[3].Command.Execute(null);
        }

        public override void Cut(NSObject sender)
        {
            Element?.OnCut();
        }

        public override void Copy(NSObject sender)
        {
            Element?.OnCopy();
        }

        public override void Delete(NSObject sender)
        {
            Element?.OnDelete();
        }
    }
}


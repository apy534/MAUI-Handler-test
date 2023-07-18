using System;
using CoreGraphics;
using Foundation;
using UIKit;
using XamContextMenu.CustomControl;
using ObjCRuntime;

namespace MauiContextMenu.Platforms.iOS.Handler
{
    public class ContextMenuiOS : UIView
    {
        private nfloat _height;
        private nfloat _width;

        const string ACTION_PREFIX = "custom_action";
        const int MAX_CUSTOM_ACTIONS = 4;

        ContextMenuView Mauiview;

        public ContextMenuiOS(ContextMenuView view)
        {
            Mauiview = view;

            _height = UIScreen.MainScreen.Bounds.Height;
            _width = UIScreen.MainScreen.Bounds.Width;
            Frame = new CGRect(0, 0, _width, _height);
        }

        public override bool CanBecomeFirstResponder => true;

        public override bool CanPerform(Selector action, NSObject withSender)
        {
            switch (action.Name)
            {
                case "Properties:":
                    return Mauiview.HasProperties;
                case "cut:":
                    return Mauiview.CanCut;
                case "copy:":
                    return Mauiview.CanCopy;
                case "delete:":
                    return Mauiview.CanDelete;
            }
            if (action.Name.StartsWith(ACTION_PREFIX))
            {
                var indexString = action.Name.Substring(ACTION_PREFIX.Length, action.Name.Length - ACTION_PREFIX.Length - 1);
                var index = int.Parse(indexString);
                return Mauiview.Commands[index].Command.CanExecute(null);
            }
            return true;
        }

        [Export("Properties:")]
        public void Properties(UIMenuController controller)
        {
            Mauiview?.OnProperties();
        }

        [Export("custom_action0:")]
        public void Action0(UIMenuController controller)
        {
            Mauiview.Commands[0].Command.Execute(null);
        }

        [Export("custom_action1:")]
        public void Action1(UIMenuController controller)
        {
            Mauiview.Commands[1].Command.Execute(null);
        }

        [Export("custom_action2:")]
        public void Action2(UIMenuController controller)
        {
            Mauiview.Commands[2].Command.Execute(null);
        }

        [Export("custom_action3:")]
        public void Action3(UIMenuController controller)
        {
            Mauiview.Commands[3].Command.Execute(null);
        }

        public override void Cut(NSObject sender)
        {
            Mauiview?.OnCut();
        }

        public override void Copy(NSObject sender)
        {
            Mauiview?.OnCopy();
        }

        public override void Delete(NSObject sender)
        {
            Mauiview?.OnDelete();
        }
    }
}


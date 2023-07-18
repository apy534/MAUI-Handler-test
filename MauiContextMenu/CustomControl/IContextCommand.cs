using System;
using System.Windows.Input;

namespace XamContextMenu.CustomControl
{
	public interface IContextCommand
	{
        string Name { get; }
        ICommand Command { get; }
    }
}


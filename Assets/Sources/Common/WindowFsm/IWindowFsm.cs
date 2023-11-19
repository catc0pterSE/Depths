using System;
using Sources.Common.WindowFsm.Windows;

namespace Sources.Common.WindowFsm
{
    public interface IWindowFsm
    {
        event Action<IWindow> Opened;
        event Action<IWindow> Closed;
        
        IWindow CurrentWindow { get; }
        
        void OpenWindow<TWindow>() where TWindow : IWindow;
        void CloseCurrentWindow();
        void ClearHistory();
    }
}
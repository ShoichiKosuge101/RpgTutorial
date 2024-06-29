using System;
using UniRx;

namespace Interface
{
    public interface IUIManager
    {
        IObservable<Unit> OnClickPlayerActionButton { get; }
        void SetActivePlayerActionButtons(bool isActive);
    }
}
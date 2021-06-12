using System;
using UniRx;

public interface IUpdateObservable : IFeature
{
	IObservable<Unit> OnUpdate(int updateOrder);
}

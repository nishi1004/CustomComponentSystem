using System;
using UniRx;

public interface IUpdater : IFeature
{
	void FireUpdateStreams();
}

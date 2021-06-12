using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public static class CustomComponentExtension
{
	/// <summary>
	/// ICustomComponentにAddTo
	/// </summary>
	/// <param name="disposable">紐づける元</param>
	/// <param name="component">紐づけ先</param>
	/// <returns></returns>
	public static IDisposable AddTo( this IDisposable disposable, ICustomComponent component )
	{
		return disposable.AddTo( component.Disposables );
	}
}

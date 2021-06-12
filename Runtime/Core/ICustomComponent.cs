using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public interface ICustomComponent : IDisposable
{
	/// <summary>
	/// 集約元
	/// </summary>
	ICustomComponentCollection Owner
	{
		get;
	}
	
	/// <summary>
	/// 機能の登録
	/// </summary>
	void Register(ICustomComponentCollection owner);
	
	/// <summary>
	/// 初期化。依存解決/イベント登録などを行う。
	/// </summary>
	void Initialize();
	
	/// <summary>
	/// 生死をともにする破棄対象
	/// </summary>
	CompositeDisposable Disposables
	{
		get;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// Component抽象クラス 標準C#クラス版
/// </summary>
[System.Serializable]
public abstract class AbstractCustomComponent : ICustomComponent
{
	/// <summary>
	/// 集約元
	/// </summary>
	public ICustomComponentCollection Owner
	{
		get;
		private set;
	}

	/// <summary>
	/// 破棄対象
	/// </summary>
	public CompositeDisposable Disposables
	{
		get;
	} = new CompositeDisposable();

	/// <summary>
	/// 登録
	/// </summary>
	/// <param name="owner">集約元</param>
	public virtual void Register(ICustomComponentCollection owner)
	{
		Owner = owner;
	}

	/// <summary>
	/// 初期化
	/// </summary>
	public virtual void Initialize()
	{

	}

	/// <summary>
	/// 破棄
	/// </summary>
	public virtual void Dispose()
	{
		Disposables.Clear();
	}
}

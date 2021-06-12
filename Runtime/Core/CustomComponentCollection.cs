using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

/// <summary>
/// 機能集約クラス C#クラス版
/// </summary>
public class CustomComponentCollection:ICustomComponentCollection
{
	/// <summary>
	/// 機能集約クラス
	/// </summary>
	private CustomComponentCollectionCore m_CollectionCore = new CustomComponentCollectionCore();

	/// <summary>
	/// 静的設定されたComponentCollectionを抽出する
	/// C#クラスの場合事前に静的定義されたものはない
	/// </summary>
	/// <returns></returns>
	ICustomComponent[] ICustomComponentCollection.ExtractComponent() => System.Array.Empty<ICustomComponent>();

	//以下、ExtractComponent以外のICustomComponentCollectionの実装は全てCustomComponentCollectionCoreに委譲する
	void IDisposable.Dispose() => m_CollectionCore.Dispose();

	void ICustomComponentCollection.Initialize() => m_CollectionCore.Initialize();

	bool ICustomComponentCollection.QueryInterface<T>(out T feature) => m_CollectionCore.QueryInterface(out feature);

	bool ICustomComponentCollection.QueryInterfaces<T>(out IEnumerable<T> components) => m_CollectionCore.QueryInterfaces(out components);

	void ICustomComponentCollection.Register(IEnumerable<ICustomComponent> components) => m_CollectionCore.Register(this,components);

	void ICustomComponentCollection.Register(ICustomComponent component) => m_CollectionCore.Register(this,component);

	void ICustomComponentCollection.RegisterInterface<T>(T feature) => m_CollectionCore.RegisterInterface(feature);

}


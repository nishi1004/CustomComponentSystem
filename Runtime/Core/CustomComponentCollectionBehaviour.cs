using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

/// <summary>
/// 機能集約クラス MonoBehaviour版
/// </summary>
public class CustomComponentCollectionBehaviour : MonoBehaviour, ICustomComponentCollection
{
	/// <summary>
	/// 機能集約クラス
	/// </summary>
	private CustomComponentCollectionCore m_CollectionCore = new CustomComponentCollectionCore();

	/// <summary>
	/// Unity Start <see cref="CustomComponentCollectionFactory"/>経由で生成されていない場合（シーンに配置etc)にCollectionの初期化を行う
	/// </summary>
	public void Start()
	{
		if( !m_CollectionCore.IsInitialized )
		{
			this.GetInitializedComponentCollection( null, null );
		}
	}

	/// <summary>
	/// 静的設定されたComponentCollectionを抽出する
	/// MonoBehaviourなのでPrefabにAddComponent済みのICustomComponent実装クラスを拾ってくる
	/// </summary>
	/// <returns></returns>
	ICustomComponent[] ICustomComponentCollection.ExtractComponent() => gameObject.GetComponents<ICustomComponent>();

	//以下、ExtractComponent以外のICustomComponentCollectionの実装は全てCustomComponentCollectionCoreに委譲する
	void IDisposable.Dispose() => m_CollectionCore.Dispose();

	void ICustomComponentCollection.Initialize() => m_CollectionCore.Initialize();

	bool ICustomComponentCollection.QueryInterface<T>( out T feature ) => m_CollectionCore.QueryInterface( out feature );

	bool ICustomComponentCollection.QueryInterfaces<T>( out IEnumerable<T> components ) => m_CollectionCore.QueryInterfaces( out components );

	void ICustomComponentCollection.Register( IEnumerable<ICustomComponent> components ) => m_CollectionCore.Register( this, components );

	void ICustomComponentCollection.Register( ICustomComponent component ) => m_CollectionCore.Register( this, component );

	void ICustomComponentCollection.RegisterInterface<T>( T feature ) => m_CollectionCore.RegisterInterface( feature );

}


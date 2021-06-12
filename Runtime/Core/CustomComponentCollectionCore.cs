using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

/// <summary>
/// 機能集約クラス
/// </summary>
public class CustomComponentCollectionCore
{
	private Dictionary<Type, List<IFeature>> m_Features = new Dictionary<Type, List<IFeature>>(); //!< Components

	private HashSet<ICustomComponent> m_ComponentHash = new HashSet<ICustomComponent>();

	/// <summary>
	/// 初期化済みか？
	/// </summary>
	public bool IsInitialized
	{
		get;
		private set;
	} = false;
	/// <summary>
	/// Feature Poolアクセス用のComponentCollection Id
	/// </summary>
	private int FeaturePoolIndex
	{
		get
		{
			if( m_FeaturePoolIndex < 0 )
			{
				m_FeaturePoolIndex = FeatureIndexGenerator.Instance.NewId();
			}

			return m_FeaturePoolIndex;
		}
	}

	/// <summary>
	/// インターフェースプールアクセス用のComponentCollection Id キャッシュ
	/// </summary>
	private int m_FeaturePoolIndex = -1;

	/// <summary>
	/// Dispose時にともにDisposeされるもの
	/// </summary>
	protected CompositeDisposable CompositeDisposable
	{
		get;
	} = new CompositeDisposable();

	/// <summary>
	/// Component追加
	/// </summary>
	public void Register( ICustomComponentCollection collection, IEnumerable<ICustomComponent> components )
	{
		foreach( var comp in components )
		{
			Register( collection, comp );
		}
	}

	/// <summary>
	/// Component追加
	/// </summary>
	/// <param name="component"></param>
	public void Register( ICustomComponentCollection collection, ICustomComponent component )
	{
		// コンポーネントに自らの機能を追加させる
		component.Register( collection );
		// 管理下におく
		m_ComponentHash.Add( component );
		// このクラスの寿命にあわせる
		component.AddTo( CompositeDisposable );
	}

	/// <summary>
	/// Component初期化
	/// </summary>
	public void Initialize()
	{
		foreach( var component in m_ComponentHash )
		{
			component.Initialize();
		}

		IsInitialized = true;
	}

	/// <summary>
	/// 機能をCollectionに登録する
	/// </summary>
	/// <typeparam name="T">追加機能インターフェイス</typeparam>
	/// <param name="component">追加機能</param>
	public void RegisterInterface<T>( T feature ) where T : IFeature
	{
		/// Dictionaryに追加
		if( m_Features.ContainsKey( typeof( T ) ) == false )
		{
			m_Features[typeof( T )] = new List<IFeature>();
		}

		m_Features[typeof( T )].Add( feature );

		// FeaturePool へ追加
		FeaturePool<T>.Entry( FeaturePoolIndex, feature, CompositeDisposable );
	}

	/// <summary>
	/// インターフェイスを要求する
	/// なかったらAssertする
	/// </summary>
	/// <typeparam name="T">機能インターフェイス</typeparam>
	/// <returns>機能インターフェイス</returns>
	public T RequireInterface<T>() where T : IFeature
	{
		T feature = default( T );
		bool ok = QueryInterface<T>( out feature );

		if( ok == false )
		{
			Debug.LogAssertion( $"RequireInterface 要求された機能:{typeof( T ).Name }を持っていない" );
		}

		return feature;
	}

	/// <summary>
	/// T型Featureの最初に一致したものを返す
	/// </summary>
	/// <typeparam name="T"> IFeature継承クラス </typeparam>
	/// <param name="comp"> Component検索結果 </param>
	/// <returns> true 存在する</returns>
	public bool QueryInterface<T>( out T feature ) where T : IFeature
	{
		if( FeaturePool<T>.IsEntried( FeaturePoolIndex ) == true )
		{
			feature = FeaturePool<T>.Get( FeaturePoolIndex );
			return true;
		}
		else
		{
			feature = default;
			return false;
		}
	}

	/// <summary>
	/// T型Featureを全て列挙
	/// </summary>
	/// <typeparam name="T"> IFeature継承クラス </typeparam>
	/// <param name="components"> Component検索結果列挙 </param>
	/// <returns> true 1つでも存在する</returns>
	public bool QueryInterfaces<T>( out IEnumerable<T> ret ) where T : IFeature
	{
		if( FeaturePool<T>.IsEntried( FeaturePoolIndex ) == true )
		{
			ret = FeaturePool<T>.GetAll( FeaturePoolIndex );
			return true;
		}
		else
		{
			ret = new T[0];
			return false;
		}
	}


	/// <summary>
	/// 破棄
	/// </summary>
	public void Dispose()
	{
		CompositeDisposable.Clear();
		m_Features.Clear();
		m_ComponentHash.Clear();

		if( m_FeaturePoolIndex >= 0 )
		{
			FeatureIndexGenerator.Instance.ReturnId( m_FeaturePoolIndex );
			m_FeaturePoolIndex = -1;
		}

	}
}


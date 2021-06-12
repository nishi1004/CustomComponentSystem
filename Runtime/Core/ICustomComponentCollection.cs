using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// CustomComponentの集約をあらわすインターフェイス
/// </summary>
public interface ICustomComponentCollection : IDisposable
{
	/// <summary>
	/// Component追加
	/// </summary>
	void Register( IEnumerable<ICustomComponent> components );

	/// <summary>
	/// Component追加
	/// </summary>
	/// <param name="component"></param>
	void Register( ICustomComponent component );

	/// <summary>
	/// 静的に定義された管理コンポーネントを抽出する
	/// </summary>
	ICustomComponent[] ExtractComponent();

	/// <summary>
	/// Component初期化
	/// </summary>
	void Initialize();

	/// <summary>
	/// 機能の取得を試みる
	/// </summary>
	/// <typeparam name="T">機能インターフェイス</typeparam>
	/// <param name="feature">機能。戻り値</param>
	/// <returns>機能が存在したか？</returns>
	bool QueryInterface<T>( out T feature ) where T : IFeature;

	/// <summary>
	/// T型機能を全て列挙を試みる
	/// </summary>
	/// <typeparam name="T"> 機能インターフェイス </typeparam>
	/// <param name="components"> 検索結果列挙 </param>
	/// <returns>true:存在</returns>
	bool QueryInterfaces<T>( out IEnumerable<T> components ) where T : IFeature;

	/// <summary>
	/// 機能追加
	/// </summary>
	/// <typeparam name="T"> 機能インターフェイス </typeparam>
	/// <param name="item"> 追加機能インターフェイス </param>
	void RegisterInterface<T>( T item ) where T : IFeature;
}

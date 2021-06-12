using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Custom Component Collection Factory
/// </summary>
public static class CustomComponentCollectionFactory
{

	/// <summary>
	/// ComponentCollection生成（Unityに依存しない、C#クラス版）
	/// </summary>
	/// <param name="components">注入コンポーネント</param>
	/// <param name="onRegister">機能登録処理</param>
	/// <returns></returns>
	public static ICustomComponentCollection CreateComponentCollection( IEnumerable<ICustomComponent> components, System.Action<ICustomComponentCollection> onRegister )
	{
		ICustomComponentCollection collection = new CustomComponentCollection();
		return collection.GetInitializedComponentCollection( components, onRegister );
	}

	/// <summary>
	/// ComponentCollection生成 （GameObjectからInstantiate）
	/// </summary>
	/// <param name="basePrefab">生成用プレハブ</param>
	/// <param name="components">注入コンポーネント </param>
	/// <param name="onRegister">機能登録時処理</param>
	/// <returns>生成したComponentCollection</returns>
	public static ICustomComponentCollection CreateComponentCollection( GameObject basePrefab, IEnumerable<ICustomComponent> components = null, System.Action<ICustomComponentCollection> onRegister = null )
	{
		GameObject ret = basePrefab == null ? new GameObject() : GameObject.Instantiate( basePrefab );
		return CreateComponentCollectionWithoutInstantiate( ret, components, onRegister );
	}

	/// <summary>
	/// componentCollection生成(Instance化されたGameObject上にComponentCollectionを構築)
	/// </summary>
	/// <param name="instanceObject">生成されたオブジェクト</param>
	/// <param name="components">注入コンポーネント</param>
	/// <param name="onRegister">機能登録時処理</param>
	/// <returns>生成したComponentCollection</returns>
	public static ICustomComponentCollection CreateComponentCollectionWithoutInstantiate( GameObject instanceObject, IEnumerable<ICustomComponent> components = null, System.Action<ICustomComponentCollection> onRegister = null )
	{
		components = components == null ? Enumerable.Empty<ICustomComponent>() : components;
		var collection = CreateComponentCollectionWithGameObject( instanceObject, components, onRegister );
		return collection;
	}

	/// <summary>
	/// 生成されたばかりのComponentCollectionに対し、Component関連の初期化を行う
	/// </summary>
	/// <param name="nonInitializedcollection">未初期化Collection</param>
	/// <param name="components">注入コンポーネント</param>
	/// <param name="onRegister">機能登録処理</param>
	/// <returns></returns>
	public static ICustomComponentCollection GetInitializedComponentCollection( this ICustomComponentCollection nonInitializedcollection, IEnumerable<ICustomComponent> components, System.Action<ICustomComponentCollection> onRegister )
	{
		components = components == null ? Enumerable.Empty<ICustomComponent>() : components;
		var compMonobehaviours = nonInitializedcollection.ExtractComponent();

		if( onRegister != null )
		{
			onRegister.Invoke( nonInitializedcollection );
		}

		// 注入されたICustomComponentCollectionと静的定義のComponentをまとめて登録・初期化
		nonInitializedcollection.RegisterAndInitialize( compMonobehaviours.Concat( components ) );
		return nonInitializedcollection;
	}

	#region private members

	/// <summary>
	/// ComponentCollection生成・MonoBehaviour版・内部用
	/// </summary>
	/// <param name="o">生成元プレハブ</param>
	/// <param name="components">注入コンポーネント</param>
	/// <param name="onRegister">機能登録処理</param>
	/// <returns>ICustomComponentCollectionインターフェイス</returns>
	private static ICustomComponentCollection CreateComponentCollectionWithGameObject( GameObject o, IEnumerable<ICustomComponent> components, System.Action<ICustomComponentCollection> onRegister )
	{
		ICustomComponentCollection collection = o.GetComponent<CustomComponentCollectionBehaviour>();

		if( collection == null )
		{
			collection = o.AddComponent<CustomComponentCollectionBehaviour>();
		}

		return collection.GetInitializedComponentCollection( components, onRegister );
	}

	/// <summary>
	/// 機能登録・初期化
	/// </summary>
	/// <param name="collection">対象Collection</param>
	/// <param name="components">注入コンポーネント</param>
	/// <param name="externalComponents"> 外部注入機能</param>
	public static void RegisterAndInitialize( this ICustomComponentCollection collection, IEnumerable<ICustomComponent> components )
	{
		collection.Register( components );
		collection.Initialize();
	}

	#endregion
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

/// <summary>
/// IFeature プール。 Query 高速化用。
/// </summary>
/// <typeparam name="T"></typeparam>
public static class FeaturePool<T> where T : IFeature
{
	/// <summary>
	/// 登録されているFeatureのPool
	/// </summary>
	private static List<T>[] ms_Features;

	/// <summary>
	/// この型のFeaturePoolに登録したオブジェクトのdisposable。Idの解放用。
	/// </summary>
	private static HashSet<IDisposable> ms_BoundDisposables;

	/// <summary>
	/// 指定id のFeatureがあるかどうか。
	/// </summary>
	/// <param name="id">指定id</param>
	/// <returns></returns>
	public static bool IsEntried(int id)
	{
		return ms_Features != null && id >= 0 && ms_Features[id] != null && ms_Features[id].Count > 0;
	}

	/// <summary>
	/// 指定idのFeature先頭一つ取得
	/// </summary>
	/// <param name="id">指定id
	/// <returns></returns>
	public static T Get(int id)
	{
		return ms_Features[id][0];
	}

	public static IReadOnlyList<T> GetAll(int id)
	{
		return ms_Features[id];
	}

	/// <summary>
	/// 指定idで機能を登録。
	/// 第三引数 CompositeDisposable.Dispose 時に登録抹消もする。
	/// </summary>
	/// <param name="id"></param>
	/// <param name="i"></param>
	/// <param name="compositeDisposable"></param>
	public static void Entry(int id, T i, CompositeDisposable compositeDisposable)
	{
		if (id < 0 || id >= FeatureIndexGenerator.ID_MAX)
		{
			Debug.LogError( $"IFeature 登録id={id.ToString()}が不正です" );
			return;
		}

		if (ms_Features == null)
		{
			ms_Features = new List<T>[FeatureIndexGenerator.ID_MAX];
		}

		if (ms_Features[id] == null)
		{
			ms_Features[id] = new List<T>();
		}

		ms_Features[id].Add(i);

		// disposable にid解放を紐付ける。
		if (ms_BoundDisposables == null)
		{
			ms_BoundDisposables = new HashSet<IDisposable>();
		}

		if (ms_BoundDisposables.Contains(compositeDisposable) == false)
		{
			ms_BoundDisposables.Add(compositeDisposable);
			Disposable.Create(() =>
			{
				Forget(id);
				ms_BoundDisposables.Remove(compositeDisposable);
			}).AddTo(compositeDisposable);
		}
	}

	/// <summary>
	/// 指定idの登録Featureをすべて忘れる。
	/// </summary>
	/// <param name="id"></param>
	private static void Forget(int id)
	{
		if (ms_Features == null)
		{
			return;
		}

		if (id < 0 || id >= FeatureIndexGenerator.ID_MAX)
		{
			Debug.LogError( $"IFeature 登録id={id.ToString()}が不正です" );
			return;
		}

		ms_Features[id].Clear();
	}
}
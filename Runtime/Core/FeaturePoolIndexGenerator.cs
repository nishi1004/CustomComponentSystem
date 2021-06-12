using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FeaturePool 用の ComponentCollection ごとのシリアル番号生成。
/// </summary>
public class FeatureIndexGenerator
{
	/// <summary>
	/// ID最大値
	/// </summary>
	public const int ID_MAX = 1024;

	/// <summary>
	/// singleton instance
	/// </summary>
	public static FeatureIndexGenerator Instance
	{
		get
		{
			if (ms_Instance == null)
			{
				ms_Instance = new FeatureIndexGenerator();
			}

			return ms_Instance;
		}
	}

	private static FeatureIndexGenerator ms_Instance = null;

	/// <summary>
	/// 未使用Idキュー
	/// </summary>
	private Queue<int> m_FreeIds;

	/// <summary>
	/// コンストラクタ
	/// </summary>
	private FeatureIndexGenerator()
	{
		m_FreeIds = new Queue<int>();

		for (int i = 0; i < ID_MAX; ++i)
		{
			m_FreeIds.Enqueue(i);
		}
	}

	/// <summary>
	/// 未使用Idを新規取得
	/// </summary>
	/// <returns></returns>
	public int NewId()
	{
		if (m_FreeIds.Count == 0)
		{
			Debug.LogAssertion($"Feature id がなくなりました.");
			return 0;
		}

		return m_FreeIds.Dequeue();
	}

	/// <summary>
	/// 不要になったIdを未使用に戻す。
	/// </summary>
	/// <param name="id"></param>
	public void ReturnId(int id)
	{
		if (m_FreeIds.Contains(id) == true)
		{
			Debug.LogError($"Feature Id:{id.ToString()} を多重解放.");
			return;
		}

		m_FreeIds.Enqueue(id);
	}
}
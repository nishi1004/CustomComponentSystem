using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 機能注入
/// </summary>
public interface IInstaller : IFeature
{
	void InstallToComponentCollection( ICustomComponentCollection nonInitializedCollection );
}

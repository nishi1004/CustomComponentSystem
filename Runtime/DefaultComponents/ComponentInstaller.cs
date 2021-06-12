using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Component機能注入
/// </summary>
[DefaultExecutionOrder( -1000 )]
public abstract class ComponentInstaller : AbstractCustomComponentBehaviour, IInstaller
{

	public virtual void Awake()
	{
		// シーン配置済みのCustomComponentCollectionに事前に注入
		var nonInitializedCustomComponentCollections = GetCustomComponentCollectionsInScene();

		foreach( var c in nonInitializedCustomComponentCollections )
		{
			InstallToComponentCollection( c );
		}
	}

	public override void Register( ICustomComponentCollection owner )
	{
		base.Register( owner );
		owner.RegisterInterface<IInstaller>( this );
	}

	public void InstallToComponentCollection( ICustomComponentCollection nonInitializedCollection )
	{
		InstallFeatures( nonInitializedCollection );
	}

	protected abstract void InstallFeatures( ICustomComponentCollection target );

	/// <summary>
	/// 現在のScene内のCustomComponentCollectionを収集する
	/// </summary>
	/// <returns></returns>
	public IEnumerable<ICustomComponentCollection> GetCustomComponentCollectionsInScene()
	{
		var scene = gameObject.scene;
		return scene.GetRootGameObjects().Select( e => e.GetComponent<ICustomComponentCollection>() ).Where( e => e != null );
	}

}

﻿using UnityEngine;

namespace SpaceGraphicsToolkit
{
	/// <summary>This component will rotate the current GameObject toward the SgtFloatingOrigin point. This makes directional lights compatible with the floating origin system.</summary>
	[ExecuteInEditMode]
	[HelpURL(SgtHelper.HelpUrlPrefix + "SgtFloatingLight")]
	[AddComponentMenu(SgtHelper.ComponentMenuPrefix + "Floating Light")]
	public class SgtFloatingLight : SgtLinkedBehaviour<SgtFloatingLight>
	{
		protected override void OnEnable()
		{
			base.OnEnable();

			SgtCamera.OnCameraPreCull += PreCull;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			SgtCamera.OnCameraPreCull -= PreCull;
		}

		private void PreCull(Camera camera)
		{
			if (SgtFloatingCamera.Instances.Count > 0)
			{
				var floatingCamera = SgtFloatingCamera.Instances.First.Value;

				transform.forward = floatingCamera.transform.position - transform.position;
			}
		}
	}
}

#if UNITY_EDITOR
namespace SpaceGraphicsToolkit
{
	using UnityEditor;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(SgtFloatingLight))]
	public class SgtFloatingLight_Editor : SgtEditor<SgtFloatingLight>
	{
		protected override void OnInspector()
		{
		}
	}
}
#endif
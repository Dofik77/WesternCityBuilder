/* Written by Kaz Crowe */
/* UltimateJoystickScreenSizeUpdater.cs */

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ThirdParty.Ultimate_Joystick.Scripts
{
	public class UltimateJoystickScreenSizeUpdater : UIBehaviour
	{
		protected override void OnRectTransformDimensionsChange ()
		{
			StartCoroutine( "YieldPositioning" );
		}

		IEnumerator YieldPositioning ()
		{
			yield return new WaitForEndOfFrame();

			UltimateJoystick[] allJoysticks = FindObjectsOfType( typeof( UltimateJoystick ) ) as UltimateJoystick[];

			for( int i = 0; i < allJoysticks.Length; i++ )
				allJoysticks[ i ].UpdatePositioning();
		}
	}
}
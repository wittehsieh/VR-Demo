using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Cameo
{
	public class VRBackToMainMenuButton : VRLookButton {

		protected override void onFinishLook ()
		{
			SceneFlowManager.Instance.ChangeToMainMenu ();
		}
	}
}


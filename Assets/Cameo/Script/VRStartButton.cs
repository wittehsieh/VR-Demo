using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Cameo
{
	public enum EnumDemoType {
		DEMO_SCENE,
		DEMO_CHARACTER
	}

	public class VRStartButton : VRLookButton {
		
		public EnumDemoType DemoType;

		protected override void onFinishLook ()
		{
			switch (DemoType) {
			case EnumDemoType.DEMO_SCENE:
				SceneFlowManager.Instance.ChangeToNextScene ();
				break;
			case EnumDemoType.DEMO_CHARACTER:
				SceneFlowManager.Instance.ChangeToCharacterScene ();
				break;
			default:
				break;
			}
		}
	}
}

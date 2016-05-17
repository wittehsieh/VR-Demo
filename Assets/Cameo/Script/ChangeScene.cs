using UnityEngine;
using System.Collections;

namespace Cameo{
	public class ChangeScene : MonoBehaviour {

		public void StartFadeOutAndChangeScene() {
			SceneFlowManager.Instance.ChangeToNextScene ();
		}
	}
}
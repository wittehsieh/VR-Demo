using UnityEngine;
using System.Collections;


namespace Cameo
{
	public class CharacterSceneCameraController : MonoBehaviour {

		public SamuraiController Samurai;

		public void OnCameraFlyFinished() {
			Samurai.Play();
		}
	}
}
using UnityEngine;
using System.Collections;

namespace Cameo
{
	public class VRBaseButton : MonoBehaviour {

		public delegate void VRBtnDelegate();

		public VRBtnDelegate onStartLookCallback = delegate {};
		public VRBtnDelegate onKeepLookCallback = delegate {};
		public VRBtnDelegate onStopLookCallback = delegate {};

		public void OnRayStartCast() {
			onStartLookCallback ();
		}

		public void OnRayStopCast() {
			onStopLookCallback ();
		}

		public void OnRayKeppLook() {
			onKeepLookCallback ();
		}
	}
}
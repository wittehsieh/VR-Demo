using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cameo
{
	public class VRRayCaster : MonoBehaviour {

		private VRBaseButton _lookingBtn;

		// Use this for initialization
		void Start () {
	
		}
	
		void FixedUpdate() 
		{
			RaycastHit hit;

			Vector3 fwd = transform.TransformDirection(Vector3.forward);
			Debug.DrawLine (transform.position, transform.position + fwd * 500, Color.red);

			VRBaseButton vrBtn = null;

			if (Physics.Raycast (transform.position, fwd, out hit, 100f)) {
				vrBtn = hit.collider.GetComponent<VRBaseButton> ();
			}

			if (_lookingBtn != vrBtn) {
				if (_lookingBtn != null) {
					_lookingBtn.onStopLookCallback ();
					_lookingBtn = null;
				} else {
					_lookingBtn = vrBtn;
					_lookingBtn.onStartLookCallback ();
				}
			} else {
				if (_lookingBtn != null) {
					_lookingBtn.onKeepLookCallback ();
				}
			}
		}
	}
}
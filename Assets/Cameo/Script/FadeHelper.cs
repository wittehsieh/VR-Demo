using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Cameo
{
	public class FadeHelper : MonoBehaviour {

		public delegate void OnFadeFinishedDelegate ();

		public UnityEngine.UI.Image Img;
		public Color FadeTo = Color.black;
		public Color FadeFrom = new Color(0, 0, 0, 0);
		public float FadeTime = 5.0f;

		private OnFadeFinishedDelegate OnFadeFinished = delegate {};
		private static FadeHelper _instance = null;
		private int _fadeDir = -1;
		private float _curFadeTime = 0;

		void Awake() {
			_instance = this;
			Fade (true, null);
		}
			
		void OnDestroy(){
			_instance = null;
		}

		public static void Fade(bool isFadeIn, OnFadeFinishedDelegate onFadeFinished) {
			if (_instance != null) {
				if (onFadeFinished != null) {
					_instance.OnFadeFinished += onFadeFinished;
				}
				_instance.StartCoroutine (_instance.FadeProcess (isFadeIn));
			} else {
				print ("Cannot find fadehelper");
			}
		}

		private IEnumerator FadeProcess(bool isFadeIn) {
			if(isFadeIn)
				Img.CrossFadeAlpha (0, FadeTime, false);
			else
				Img.CrossFadeAlpha (1, FadeTime, false);

			yield return new WaitForSeconds (FadeTime);

			OnFadeFinished ();
		}
	}
}
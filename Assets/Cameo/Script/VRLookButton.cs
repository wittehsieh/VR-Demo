using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Cameo
{
	public class VRLookButton : VRBaseButton {

		public AudioClip FinisheClip;
		public AudioClip KeepLookClip;

		public AudioSource AudioController;
		public Animator AnimatorController;
		public float LookDuring = 3;
		public Image ProgressBar;
		public Sprite OnKeepLookSprite;
		public Sprite IdleSprite;
		public Image ForegroundIcon;

		private float _curLookTime = 0;
		private bool _isFinished = false;

		// Use this for initialization
		void Start () {
			onStartLookCallback += onStartLookAction;
			onStopLookCallback += onStopLookAction;
			onKeepLookCallback += onKeepLookAction;

			AudioController = gameObject.GetComponent<AudioSource> ();
		}

		private void onStartLookAction()	{
			AnimatorController.SetTrigger ("StartLook");
			ForegroundIcon.sprite = OnKeepLookSprite;

			if (KeepLookClip != null) {
				AudioController.clip = KeepLookClip;
				AudioController.Play ();
			}

			onStartLook ();
		}

		private void onStopLookAction() {
			ForegroundIcon.sprite = IdleSprite;
			if(AudioController.isPlaying)
				AudioController.Stop ();
			AnimatorController.SetTrigger ("StopLook");
			ProgressBar.fillAmount = 0;
			_curLookTime = 0;
			_isFinished = false;
			onStopLook ();
		}

		private void onKeepLookAction() {
			if (_isFinished == true)
				return;
			
			_curLookTime += Time.deltaTime;
			ProgressBar.fillAmount = _curLookTime / LookDuring;

			if (_curLookTime > LookDuring) {
                StartCoroutine(BtnFinishProcess());
			} else {
				onKeepLook ();
			}
		}

        private IEnumerator BtnFinishProcess()
        {
            float waitTime = 0;

            if (FinisheClip != null)
            {
                AudioController.clip = FinisheClip;
                AudioController.Play();
                _isFinished = true;
                waitTime = FinisheClip.length;
            }
            yield return new WaitForSeconds(waitTime);

            onFinishLook();
        }


		protected virtual void onStartLook() {

		}

		protected virtual void onStopLook() {

		}

		protected virtual void onKeepLook() {

		}

		protected virtual void onFinishLook() {

		}
	}
}

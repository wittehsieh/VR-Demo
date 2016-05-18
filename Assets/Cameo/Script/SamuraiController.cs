using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cameo
{
	public class SamuraiController : MonoBehaviour {

		[System.Serializable]
		public class CharacterAction
		{
			public string ActionName;
			public AudioClip Clip;
			public float During;
		}

		public List<CharacterAction> Actions;
		private bool _isPlaying = false;

		private AudioSource _audioController;
		private Animator _animator;

		// Use this for initialization
		void Start () {
			_audioController = gameObject.GetComponent<AudioSource> ();
			_animator = gameObject.GetComponent<Animator> ();
        }

		public void PlayNextAction()
        {
            CharacterAction nextAction = Actions[Random.Range(0, Actions.Count)];
            StartCoroutine(playAction(nextAction));
        }

		public void Stop() {
			StopAllCoroutines ();
            _isPlaying = false;
			_animator.SetTrigger ("S_Idle");
		}

		private IEnumerator playAction(CharacterAction action) {
            _isPlaying = true;

            _animator.SetTrigger (action.ActionName);

			if (action.Clip != null) {
				_audioController.clip = action.Clip;
				_audioController.PlayDelayed (0f);
			}

			yield return new WaitForSeconds (action.During);

            _isPlaying = false;

        }

        void Update()
        {
            if (Input.GetKeyUp("space"))
            {
                if (_isPlaying)
                {
                    Stop();
                }
                PlayNextAction();
            }
        }
	}
}
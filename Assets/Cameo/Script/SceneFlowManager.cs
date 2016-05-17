using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace Cameo{
	public class SceneFlowManager : Singleton<SceneFlowManager> {

		public List<int> SceneDemo;
		public List<int> CharacterDemo;

		private List<int> _scnOrder = new List<int> ();

		private int _index = 0;

		IEnumerator Start() {
			startNewDEMO ();
			GameObject.DontDestroyOnLoad (gameObject);
			yield return new WaitForSeconds(5);
		}

		private void startNewDEMO()	{
			_scnOrder = new List<int> ();
			for (int i = 0; i < SceneDemo.Count; i++) {
				_scnOrder.Add(SceneDemo[i]);
			}
		}

		public void ChangeToMainMenu() {
			FadeHelper.Fade (false, ToMainMenu);
		}

		public void ChangeToNextScene() {
			_index = 0;
			if (_scnOrder.Count == 0) {
				startNewDEMO ();

			} else {
				int randomIndex = Random.Range (0, _scnOrder.Count);
				_index = _scnOrder [randomIndex];
				_scnOrder.RemoveAt (randomIndex);
			}
			print ("Change to " + _index.ToString ());
			FadeHelper.Fade (false, ToNextScene);
		}

		public void ChangeToCharacterScene() {
			FadeHelper.Fade (false, ToCharacterScene);
		}

		private void ToCharacterScene() {
			SceneManager.LoadScene (CharacterDemo[0]);
		}

		private void ToNextScene()
		{
			SceneManager.LoadScene (_index);
		}

		private void ToMainMenu()
		{
			SceneManager.LoadScene (0);
		}
	}
}
using UnityEngine;
using System.Collections;

namespace Cameo{
	public class ChangeScene : MonoBehaviour {

        public Animator AnimController;
        private int _level = 0;

        void Start()
        {
            if(!GameSystem.Instance.GameMode)
            {
                AnimController.SetLayerWeight(0, 1);
                AnimController.SetLayerWeight(1, 0);
            }
            else
            {
                AnimController.SetLayerWeight(0, 0);
                AnimController.SetLayerWeight(1, 1);
            }
                
        }

        public void StartLevel()
        {
            GameSystem.Instance.StartLevel(_level);
            GameSystem.Instance.RegisterLevelFinished(_level, FinishLevel);
            _level++;
        }

        private void FinishLevel()
        {
            AnimController.SetTrigger("ToNextLevel");
        }

        public void StartFadeOutAndChangeScene() {
			SceneFlowManager.Instance.ChangeToNextScene ();
		}
	}
}
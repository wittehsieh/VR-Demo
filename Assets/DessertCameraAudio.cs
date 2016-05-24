using UnityEngine;
using System.Collections;

namespace Cameo
{
    public class DessertCameraAudio : MonoBehaviour
    {
        private AudioSource _audioController;
        // Use this for initialization
        void Start()
        {
            _audioController = GetComponent<AudioSource>();
        }

        public void PlayJumpToWater()
        {
            _audioController.Play();
        }
    }

}

using UnityEngine;
using System.Collections;

namespace Cameo
{
    public class EffectHandler : MonoBehaviour
    {  
        private float _flyTime = 0;
        private float _during;
        private Vector3 _from;
        private Transform _to;

        public GameObject FlyEffect;
        public AudioClip FlyClip;
        public GameObject HitEffect;
        public AudioClip HitClip;
        public AudioSource AudioController;
        public float DeadTime;

        public void InitEffect(Vector3 from, Transform to, float during)
        {
            _during = during;
            _from = from;
            _to = to;

            if(FlyClip != null)
            {
                AudioController.clip = FlyClip;
                AudioController.Play();
            }

            FlyEffect.SetActive(true);
            HitEffect.SetActive(false);
        }

        void Update()
        {
            if(_flyTime < _during)
            {
                _flyTime += Time.deltaTime;
                transform.position = Vector3.Lerp(_from, _to.position, _flyTime / _during);

                if (_flyTime > _during)
                {
                    AudioController.Stop();

                    FlyEffect.SetActive(false);
                    HitEffect.SetActive(true);

                    if (HitClip != null)
                    {
                        AudioController.clip = HitClip;
                        AudioController.Play();
                    }

                    Destroy(gameObject, DeadTime);
                }
            }
        }
    }
}


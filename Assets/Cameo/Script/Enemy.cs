using UnityEngine;
using System.Collections;

namespace Cameo
{
    public class Enemy : MonoBehaviour
    {
        public int Level = 0;
        public VREnemyHitTrigger HitTrigger;
        
        public GameObject HitPoint;
        public GameObject Model;
		public GameObject BurnEffect;
		public GameObject DeadEffect;

        public AudioClip Appear;
        public AudioClip[] Dead;

        private AudioSource _audioController;
        private Transform _target;
        private FadeObjectInOut _fadeObjectHelper;

        void Awake()
        {
            GameSystem.Instance.RegisterEnemy(Level, this);
            HitTrigger.gameObject.SetActive(false);
			BurnEffect.gameObject.SetActive (false);
			DeadEffect.gameObject.SetActive (false);
            Model.SetActive(false);

            _audioController = GetComponent<AudioSource>();
            _fadeObjectHelper = Model.GetComponent<FadeObjectInOut>();
            GameObject targetObj = GameObject.Find("Camera");
            if (targetObj != null)
            {
                _target = targetObj.transform;
            }

            gameObject.SetActive(false);
        }

        void Update()
        {
            if (_target != null)
            {
                transform.LookAt(_target);
            }
        }

        private void OnHit()
        {
            StartCoroutine(playHitEffect());
        }

        public void OnStart()
        {
            gameObject.SetActive(true);
            StartCoroutine(playStartEffect());
        }

        private IEnumerator playStartEffect()
        {
            yield return new WaitForSeconds(Random.Range(0, 4));

            Model.SetActive(true);
			BurnEffect.gameObject.SetActive (true);

            _fadeObjectHelper.FadeIn();
            if(Appear != null)
            {
                _audioController.clip = Appear;
                _audioController.Play();
            }
            
            yield return new WaitForSeconds(1);
            HitTrigger.gameObject.SetActive(true);
            HitTrigger.OnHitCallback += OnHit;
        }

        private IEnumerator playHitEffect()
        {
            GameObject.Destroy(HitTrigger.gameObject);

            CastEffectFactory.CreateFlyEffect(0, _target.position, HitPoint.transform, 1);
            yield return new WaitForSeconds(1f);
            _fadeObjectHelper.FadeOut();
			DeadEffect.gameObject.SetActive (true);
            yield return new WaitForSeconds(1);

            if(Dead.Length > 0)
            {
                AudioClip clip = Dead[Random.Range(0, Dead.Length)];
                _audioController.clip = clip;
                _audioController.Play();
                yield return new WaitForSeconds(3);
            }

            GameSystem.Instance.OnLevelEnemyHit(Level);

            Destroy(gameObject);
        }
    }
}

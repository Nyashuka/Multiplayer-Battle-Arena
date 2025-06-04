using System;
using System.Collections;
using Core.MatchmakingComponents.MatchStates;
using Services.EventBus;
using Services.EventBus.EventBusArguments;
using Services.ServiceLocatorModule.Abstract;
using UnityEngine;
using UnityEngine.Audio;

namespace Services.Audio
{
    public class AudioService : MonoBehaviour, IService
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private AudioSource sfxAudioSourcePrefab;
        
        private AudioSourcePool _audioSourcePool;

        private void OnEnable()
        {
            GameEventBus.Instance.Subscribe(GameEventDefinitions.MatchStateChanged, OnMatchStateChanged);
        }

        private void OnMatchStateChanged(IEventBusArgs e)
        {
            if (e is MatchStateChangedEventArgs { State: MatchStateEnum.Warmup })
            {
                _audioSourcePool = new AudioSourcePool(sfxAudioSourcePrefab, 20);
            }
        }

        public void PlaySfx(AudioClip clip, Vector3 position)
        {
            var source = _audioSourcePool.GetAudioSource(position);
            
            source.clip = clip;
            source.Play();

            StartCoroutine(ReturnSfxAfterDelay(source, clip.length));
        }

        private IEnumerator ReturnSfxAfterDelay(AudioSource source, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            _audioSourcePool.ReturnAudioSource(source);
        }
    }
}
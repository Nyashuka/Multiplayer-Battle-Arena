using System.Collections.Generic;
using UnityEngine;

namespace Services.Audio
{
    public class AudioSourcePool
    {
        private readonly Queue<AudioSource> _audioSources = new();
        private readonly AudioSource _audioSourcePrefab;
        private readonly Transform _parent;

        public AudioSourcePool(AudioSource prefab, int count)
        {
            _parent = new GameObject("AudioSourcePool").transform;
            _audioSourcePrefab = prefab;
            InitializePool(count);
        }

        private AudioSource CreteNewAudioSource(Vector3 position)
        {
            return Object.Instantiate(_audioSourcePrefab, position, Quaternion.identity, _parent);
        }
        
        private void InitializePool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var source = CreteNewAudioSource(_parent.position);
                _audioSources.Enqueue(source);
            }
        }

        public AudioSource GetAudioSource(Vector3 position)
        {
            if (_audioSources.Count > 0)
            {
                var source = _audioSources.Dequeue();
                source.transform.position = position;
                return source;
            }
            
            return CreteNewAudioSource(position);
        }

        public void ReturnAudioSource(AudioSource audioSource)
        {
            _audioSources.Enqueue(audioSource);
        }
    }
}
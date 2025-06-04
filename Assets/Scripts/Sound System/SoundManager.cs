using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets.Scripts.Sound_System
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] SoundEmitter soundEmitterPrefab;
        [SerializeField] bool collectionCheck = true;
        [SerializeField] int defaultCapacity = 10;
        [SerializeField] int maxPoolSize = 100;

        IObjectPool<SoundEmitter> soundEmitterPool;
        readonly List<SoundEmitter> activeSoundEmitters = new();

        private void Start()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            //soundEmitterPool = new ObjectPool<SoundEmitter>(
                
            //);
        }
    }
}

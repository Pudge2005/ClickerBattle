using System;
using System.Threading;
using UnityEngine;

namespace Game.Core
{
    public sealed class UnityAsyncManager : MonoBehaviour
    {
        public const string GameObjectName = "Unity Async Manager";

        private static UnityAsyncManager _singleton;
        /// <summary>
        /// Cancel in OnDestroy().
        /// </summary>
        private static CancellationTokenSource _cancelOnDestroyCts;
        /// <summary>
        /// Cancel in HandleApplicationQuit().
        /// </summary>
        private static CancellationTokenSource _cancelOnAppQuitCts;


        private static UnityAsyncManager Singleton
        {
            get
            {
                EnsureSingleton();
                return _singleton;
            }
        }


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void RuntimeInit()
        {
            _singleton = null;

            DisposeCts(ref _cancelOnDestroyCts);
            DisposeCts(ref _cancelOnAppQuitCts);
        }


        private void Awake()
        {
            _cancelOnDestroyCts ??= new();
            _cancelOnAppQuitCts = new();
        }

        private void OnApplicationQuit()
        {
            _ = CancelCts(ref _cancelOnAppQuitCts);
        }

        private void OnDestroy()
        {
            _ = CancelCts(ref _cancelOnDestroyCts);

            if (_singleton == this)
            {
                _singleton = null;
            }

            //else -> new's Awake was called before old's OnDestroy
        }


        public static CancellationToken GetCancelOnDestroyToken()
        {
            EnsureSingleton();
            return _cancelOnDestroyCts.Token;
        }

        public static CancellationToken GetCancelOnApplicationQuitToken()
        {
            EnsureSingleton();
            return _cancelOnAppQuitCts.Token;
        }

        public static CancellationTokenSource CreateLinkedToOnDestroy(CancellationTokenSource cts)
        {
            EnsureSingleton();
            return CancellationTokenSource.CreateLinkedTokenSource(cts.Token, _cancelOnDestroyCts.Token);
        }

        public static CancellationTokenSource CreateLinkedToOnApplicationQuit(CancellationTokenSource cts)
        {
            EnsureSingleton();
            return CancellationTokenSource.CreateLinkedTokenSource(cts.Token, _cancelOnAppQuitCts.Token);
        }


        private static void EnsureSingleton()
        {
            if (_singleton == null)
            {
                SelfInitSingleton();
            }
        }

        private static void SelfInitSingleton()
        {
            var go = new GameObject(GameObjectName);
            _singleton = go.AddComponent<UnityAsyncManager>();
        }

        /// <returns>exception was thrown</returns>
        private static bool CancelCts(ref CancellationTokenSource cts)
        {
            bool exceptionWasThrown = false;

            try
            {
                cts?.Cancel();
            }
            catch (Exception ex)
            {
                exceptionWasThrown = true;
                Debug.LogError(cts + " " + ex.Message);
            }
            finally
            {
                DisposeCts(ref cts);
            }

            return exceptionWasThrown;
        }


        private static void DisposeCts(ref CancellationTokenSource cts)
        {
            try
            {
                cts?.Dispose();
            }
            catch (Exception ex)
            {
                Debug.LogError(cts + " " + ex.Message);
            }
            finally
            {
                cts = null;
            }
        }
    }

}

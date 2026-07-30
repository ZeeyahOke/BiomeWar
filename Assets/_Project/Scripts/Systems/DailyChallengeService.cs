using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace BiomeWar
{
    public class DailyChallengeService : ManagerBase<DailyChallengeService>
    {
        [Header("Remote config")]
        [SerializeField] string challengeUrl = "";
        [SerializeField] int timeoutSeconds = 8;

        public ChallengeModifier Active { get; private set; } = ChallengeModifier.Default();
        public bool IsOnline { get; private set; }
        public bool HasFetched { get; private set; }

        public event Action OnChallengeLoaded;

        protected override void Awake()
        {
            base.Awake();
            if (Instance != this) return;
            StartCoroutine(FetchRoutine());
        }

        public void Refetch() => StartCoroutine(FetchRoutine());

        IEnumerator FetchRoutine()
        {
            if (string.IsNullOrEmpty(challengeUrl))
            {
                FallBack("No challenge URL configured");
                yield break;
            }

            // Cache-bust so edits to the hosted file are picked up promptly.
            string url = challengeUrl + "?t=" + DateTime.UtcNow.Ticks;

            using (var request = UnityWebRequest.Get(url))
            {
                request.timeout = timeoutSeconds;

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    FallBack(request.error);
                    yield break;
                }

                ChallengeList list;

                try
                {
                    list = JsonUtility.FromJson<ChallengeList>(request.downloadHandler.text);
                }
                catch (Exception e)
                {
                    FallBack("Parse error: " + e.Message);
                    yield break;
                }

                if (list == null || list.modifiers == null || list.modifiers.Count == 0)
                {
                    FallBack("Empty modifier list");
                    yield break;
                }

                Active = SelectForToday(list, DateTime.UtcNow);
                IsOnline = true;
                HasFetched = true;

                OnChallengeLoaded?.Invoke();
            }
        }

        //every player on the same date gets the same modifier, with no server-side logic required.
        public static ChallengeModifier SelectForToday(ChallengeList list, DateTime utcNow)
        {
            if (list == null || list.modifiers == null || list.modifiers.Count == 0)
                return ChallengeModifier.Default();

            int index = utcNow.DayOfYear % list.modifiers.Count;
            return list.modifiers[index];
        }

        void FallBack(string reason)
        {
            Debug.LogWarning($"[DailyChallenge] Offline, using defaults. Reason: {reason}");
            Active = ChallengeModifier.Default();
            IsOnline = false;
            HasFetched = true;
            OnChallengeLoaded?.Invoke();
        }
    }
}

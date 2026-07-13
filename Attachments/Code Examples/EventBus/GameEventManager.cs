using System;
using GameAudio;
using GlobalData;
using QuestSystem;
using QuestSystem.Quests;
using UnityEngine;
using UnityEngine.Events;

namespace Managers
{
    public static class GameEventManager
    {
        public static class PlayerEvent
        {
            // Actions for modifying resources
            public static event UnityAction<ResourceType, float> OnResourceChanged;
            // Func for querying resources (Returns a float)
            public static event Func<ResourceType, float> OnResourceQuery;
            public static event UnityAction<bool> OnDischargeStateChanged;
            public static event UnityAction<AbilityType> OnAbilityUnlocked;
            

            public static void RaiseDischargeStateChanged(bool isDischarging)
            {
                OnDischargeStateChanged?.Invoke(isDischarging);
                //Debug.Log($"Discharging state changed to: {isDischarging}");
            }

            public static void RequestAddResource(ResourceType resourceType, float amount)
            {
                OnResourceChanged?.Invoke(resourceType, amount);
            }

            public static void RequestSpendResource(ResourceType resourceType, float amount)
            {
                OnResourceChanged?.Invoke(resourceType, -amount);
            }

            // Helper method so other scripts can easily check balances
            public static float GetCurrency(ResourceType resourceType)
            {
                // If nothing is listening (e.g. inventory is destroyed), return 0
                if (OnResourceQuery != null)
                    return OnResourceQuery.Invoke(resourceType);

                return -404f;
            }

            // Helper method to check affordability
            public static bool CanAfford(ResourceType resourceType, float amount)
            {
                return GetCurrency(resourceType) >= amount;
            }
            

            public static void RequestUnlockAbility(AbilityType ability)
            {
                OnAbilityUnlocked?.Invoke(ability);
                //Debug.Log($"Ability Unlocked: {ability}");
            }
        }

        public static class Audio
        {
            // The universal play event (handles SFX, UI, and starting Music)
            public static event UnityAction<AudioId, float, float> OnPlayRequested;

            // Dedicated events for persistent background music
            public static event UnityAction<float> OnStopMusicRequested;

            public static void Play(AudioId audioId, float volumeMult = 1f, float pitchMult = 1f)
            {
                OnPlayRequested?.Invoke(audioId, volumeMult, pitchMult);
            }

            public static void StopMusic(float fadeSeconds = 0f)
            {
                OnStopMusicRequested?.Invoke(fadeSeconds);
            }
        }

        public static class GameHazards
        {
           

            public static event UnityAction<HazardEffectType, float> OnEffectApplied;
            public static event UnityAction<Vector2> OnPullForceApplied;

            public static event UnityAction<HazardEffectType, float> OnContinuousEffectStarted;
            public static event UnityAction<HazardEffectType> OnContinuousEffectEnded;

            public static void ApplyEffect(HazardEffectType effectType, float value)
            {
                OnEffectApplied?.Invoke(effectType, value);
            }

            public static void ApplyPull(Vector2 force)
            {
                OnPullForceApplied?.Invoke(force);
            }

            public static void StartContinuousEffect(HazardEffectType effectType, float value)
            {
                OnContinuousEffectStarted?.Invoke(effectType, value);
            }

            public static void StopContinuousEffect(HazardEffectType effectType)
            {
                OnContinuousEffectEnded?.Invoke(effectType);
            }
        }

        public static class DifficultyEvent
        {
            public static event UnityAction<int> OnLevelDifficultyChange;

            public static void RaiseLevelDifficulty(int difficultyLevel)
            {
                OnLevelDifficultyChange?.Invoke(difficultyLevel);
                //Debug.Log("Difficulty changed to " + _startDifficultyLevel);
            }
        }

        public static class EnemyEvent
        {
            public static event UnityAction<EnemyType, Vector3, int> OnEnemiesRequest;

            public static void RaiseRequestEnemiesToSpawn(EnemyType type, Vector3 position, int count)
            {
                OnEnemiesRequest?.Invoke(type, position, count);
            }

            public static event UnityAction<EnemyType> OnEnemyKilled;

            public static void RaiseEnemyKilled(EnemyType type)
            {
                OnEnemyKilled?.Invoke(type);
            }

            public static event UnityAction<float> OnAttackRequest;

            public static void RequestAttackPlayer(float valueToRequest)
            {
                OnAttackRequest?.Invoke(valueToRequest);
            }

            public static event UnityAction<Vector3, int> OnElectricalDropRequested;

            public static void RequestElectricalDrop(Vector3 position, int count)
            {
                OnElectricalDropRequested?.Invoke(position, count);
            }
            /*
            public static event UnityAction<Vector3> OnEnemyDied;
            public static event UnityAction<ResourceType, float> OnEnemyRewardGranted;

            public static void RaiseEnemyDied(Vector3 deathPosition)
            {
                OnEnemyDied?.Invoke(deathPosition);
            }

            public static void RaiseEnemyRewardGranted(ResourceType resourceType, float amount)
            {
                OnEnemyRewardGranted?.Invoke(resourceType, amount);
            }
            */ // Not used for now

        }

        public static class QuestEvent
        {
            public static event UnityAction<ActData> OnActActivated;

            public static event UnityAction<QuestData> OnQuestActivated;

            public static event UnityAction<QuestData, int, int> OnQuestProgressUpdated;

            public static event UnityAction<QuestData> OnQuestComplete;

            public static event UnityAction<ActData> OnActComplete;

            public static void RaiseQuestActivated(QuestData questData)
            {
                OnQuestActivated?.Invoke(questData);
            }

            public static void RaiseQuestProgressUpdated(QuestData questData, int currentValue, int targetValue)
            {
                OnQuestProgressUpdated?.Invoke(questData, currentValue, targetValue);
            }

            public static void RaiseQuestComplete(QuestData questData)
            {
                OnQuestComplete?.Invoke(questData);
            }

            public static void RaiseActActivated(ActData actData)
            {
                OnActActivated?.Invoke(actData);
            }

            public static void RaiseActComplete(ActData actData)
            {
                OnActComplete?.Invoke(actData);
                Debug.Log("Act complete");
            }

           
        }
        public static class UIEvent
        {
            public static event UnityAction<int> OnCountdownUpdated;
            public static event UnityAction<float> OnRunTimerUpdated;
            public static event UnityAction<ScreenType> OnScreenChangeRequested;
            public static event UnityAction<RunSummaryData> OnRunSummaryReady;

            public static void RaiseCountdownUpdated(int number)
            {
                OnCountdownUpdated?.Invoke(number);
            }

            public static void RaiseRunTimerUpdated(float progress)
            {
                OnRunTimerUpdated?.Invoke(progress);
            }
            
            public static void RaiseScreenChange(ScreenType type)
            {
                OnScreenChangeRequested?.Invoke(type);
            }
            
            public static void RaiseRunSummaryReady(RunSummaryData data)
            {
                OnRunSummaryReady?.Invoke(data);
            }
        }
    }
}
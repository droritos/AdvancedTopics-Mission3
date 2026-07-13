using System;
using Player;
using UnityEngine.Events;
using Utilities;

namespace Managers
{
    public static class EventManager
    {
        // Category 1: Combat Events
        public static class Combat
        {
            public static event Action<PlayerManager, string> PlayerKilled;
            public static event UnityAction<PlayerManager, PlayerManager> OnPlayerHitPlayer;

            public static void RaisePlayerKilled(PlayerManager player, string reason) 
            {
                PlayerKilled?.Invoke(player, reason);
            }

            public static void RaiseOnPlayerHit(PlayerManager hurtPlayer, PlayerManager attackerPlayer)
            {
                OnPlayerHitPlayer?.Invoke(hurtPlayer, attackerPlayer);
            }
        }
        
        public static class Session
        {
            public static event UnityAction<int> OnPlayerJoined; // Can Update UI or other needed object to react
            public static event UnityAction<int> OnPlayerLeft;
            public static event UnityAction<int,bool> OnPlayerReady;
            public static UnityAction OnAllPlayersReady;
            
            public static void RaiseOnPlayerJoined(int playerIndex) => OnPlayerJoined?.Invoke(playerIndex);
            public static void RaiseOnPlayerLeft(int playerIndex) => OnPlayerLeft?.Invoke(playerIndex);
            public static void RaiseOnPlayerReady(int playerIndex ,bool state) => OnPlayerReady?.Invoke(playerIndex,state);

            public static void RaiseOnAllPlayersReady()
            {
                UnityEngine.Debug.Log("All players ready!");
                OnAllPlayersReady?.Invoke();
            }
        }

        public static class Match
        {
            public static event UnityAction<int[]> OnScoreChanged;
            public static event UnityAction OnMatchEnded;
            public static event UnityAction OnMatchStarted
            {
                add => EventManager.Session.OnAllPlayersReady += value;
                remove => EventManager.Session.OnAllPlayersReady -= value;
            } // This == OnAllPlayersReady 
            public static void RaiseOnScoreChanged(int[] scores) => OnScoreChanged?.Invoke(scores);
            public static void RaiseEndMatch() => OnMatchEnded?.Invoke();
        }

        public static class EffectEvent
        {
            public static event UnityAction<ShakeType> OnShakeRequest;
            public static void RequestShake(ShakeType shakeType) => OnShakeRequest?.Invoke(shakeType);
        }

        //public static class GUI
        //{
        //    
        //}
    }
}
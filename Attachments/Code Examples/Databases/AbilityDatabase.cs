using System;
using System.Collections.Generic;
using GlobalData;
using Abilities; 
using UnityEngine;

namespace DataBase
{
    // A little struct to hold the Enum + Data pairing for the Unity Inspector
    [Serializable]
    public struct AbilityEntry
    {
        public AbilityType abilityType;
        public BaseAbilityData abilityData;
    }

    [CreateAssetMenu(fileName = "AbilityDatabase", menuName = "Database/Ability Database")]
    public class AbilityDatabase : ScriptableObject
    {
        [Header("Inspector Setup")]
        [Tooltip("Add your Ability Data ScriptableObjects here.")]
        [SerializeField] private List<AbilityEntry> abilityEntries = new List<AbilityEntry>();

        // The fast-lookup dictionary
        private Dictionary<AbilityType, BaseAbilityData> _abilityDictionary;

        /// <summary>
        /// Returns the ScriptableObject Data for the requested ability.
        /// </summary>
        public BaseAbilityData GetAbilityData(AbilityType type)
        {
            if (type == AbilityType.None) return null;

            if (_abilityDictionary == null || _abilityDictionary.Count != abilityEntries.Count)
            {
                InitializeDictionary();
            }

            if (_abilityDictionary.TryGetValue(type, out BaseAbilityData foundData))
            {
                return foundData;
            }

            Debug.LogWarning($"AbilityDatabase: Ability '{type}' is not set in the database!");
            return null; 
        }

        private void InitializeDictionary()
        {
            _abilityDictionary = new Dictionary<AbilityType, BaseAbilityData>();
        
            foreach (var entry in abilityEntries)
            {
                if (!_abilityDictionary.ContainsKey(entry.abilityType))
                {
                    _abilityDictionary.Add(entry.abilityType, entry.abilityData);
                }
                else
                {
                    Debug.LogWarning($"AbilityDatabase: Duplicate entries for '{entry.abilityType}'!");
                }
            }
        }
    }
}
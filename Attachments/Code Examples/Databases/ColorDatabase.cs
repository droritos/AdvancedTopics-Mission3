using System.Collections.Generic;
using GlobalData;
using UnityEngine;

namespace DataBase
{
    [CreateAssetMenu(fileName = "ColorDatabase", menuName = "Database/Color Database")]
    public class ColorDatabase : ScriptableObject
    {
        [Header("Inspector Setup")]
        [Tooltip("Add your colors here in the inspector.")]
        // This List is what you see in Unity
        [SerializeField] private List<ColorEntry> colorEntries = new List<ColorEntry>();

        // This Dictionary is what we use in code for fast lookups
        private Dictionary<ColorType, Color> colorDictionary;

        /// <summary>
        /// The function you call to get a color by its Enum.
        /// </summary>
        public Color GetColor(ColorType type)
        {
            // If the dictionary hasn't been built yet, build it!
            if (colorDictionary == null || colorDictionary.Count != colorEntries.Count)
            {
                InitializeDictionary();
            }

            // Try to find the color
            if (colorDictionary.TryGetValue(type, out Color foundColor))
            {
                return foundColor;
            }

            // Fallback: If you forgot to add the enum to your database, warn you and return Magenta
            Debug.LogWarning($"ColorDatabase: Color '{type}' is not set in the database! Returning bright Magenta.");
            return Color.magenta; 
        }

        // Helper function to turn the Inspector List into a Runtime Dictionary
        private void InitializeDictionary()
        {
            colorDictionary = new Dictionary<ColorType, Color>();
        
            foreach (var entry in colorEntries)
            {
                // Make sure we don't accidentally add the same enum twice in the inspector
                if (!colorDictionary.ContainsKey(entry.colorType))
                {
                    colorDictionary.Add(entry.colorType, entry.color);
                }
                else
                {
                    Debug.LogWarning($"ColorDatabase: You have duplicate entries for '{entry.colorType}'! Only the first one will be used.");
                }
            }
        }
    }
}
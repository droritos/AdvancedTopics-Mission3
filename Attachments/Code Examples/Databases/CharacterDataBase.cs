using UnityEngine;
using System.Collections.Generic;
using Structs;
using Utilities; // Required to use Lists!

namespace Scriptable_Objects.Data_Base_SO
{
    [CreateAssetMenu(fileName = "CharacterDataBase", menuName = "Database/CharacterDataBase")]
    public class CharacterDataBase : ScriptableObject
    {
        // 1. We replace the Dictionary with a List of our custom struct
        [SerializeField] private List<CharacterDataEntry> characterEntries;
        [SerializeField] private List<Color> colors;
        public CharacterData GetCharacterDataOf(CharacterType characterType)
        {
            // 1. Loop through every 'entry' in our 'characterEntries' list
            foreach (CharacterDataEntry entry in characterEntries)
            {
                // 2. Check if the type of the current entry matches the one we are looking for
                if (entry.characterType == characterType)
                {
                    // 3. We found a match! Return the associated data and exit the method.
                    return entry.characterData;
                }
            }

            // 4. If the loop finishes and we never found a match
            return null;
        }

        public Color GetColorById(int playerPlayerId)
        {
            return colors[playerPlayerId];
        }
    }


}


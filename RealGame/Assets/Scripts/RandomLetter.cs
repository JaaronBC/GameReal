using UnityEngine;
using System.Collections.Generic;

public class RandomLetter : MonoBehaviour
{
    [SerializeField] private SpellbookController spellbookController;
   
    public char RandomConsonant()
    {
        if (BattleDataHolder.ConsonantsLeft.Count > 0)
        {
            char randomConsonant = GetRandomElement(BattleDataHolder.ConsonantsLeft);
            BattleDataHolder.ConsonantsLeft.Remove(randomConsonant);
            Debug.Log("Random Consonant: " + randomConsonant);
            return randomConsonant;
        }
        else
        {
            Debug.Log("No consonants left!");
            return '\0';
        }
    }
    public char RandomVowel()
    {
        if (BattleDataHolder.VowelsLeft.Count > 0)
        {
            char randomVowel = GetRandomElement(BattleDataHolder.VowelsLeft);
            BattleDataHolder.VowelsLeft.Remove(randomVowel);
            Debug.Log("Random Vowel: " + randomVowel); 
            return randomVowel;
        }
        else
        {
            Debug.Log("No vowels left!");
            return '\0';
        }

    }
    char GetRandomElement(HashSet<char> set)
    {
        int index = Random.Range(0, set.Count);
        foreach (char element in set)
        {
            if (index == 0)
                return element;
            index--;
        }
        return '\0'; // Should never reach here
    }
}

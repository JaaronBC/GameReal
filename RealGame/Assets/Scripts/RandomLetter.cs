using UnityEngine;
using System.Collections.Generic;

public class RandomLetter : MonoBehaviour
{
    [SerializeField] private SpellbookController spellbookController;
   
    public void RandomConsonant()
    {
        if (BattleDataHolder.ConsonantsLeft.Count > 0)
        {
            char randomConsonant = GetRandomElement(BattleDataHolder.ConsonantsLeft);
            BattleDataHolder.ConsonantsLeft.Remove(randomConsonant);
            spellbookController.AddLetter(randomConsonant);
            Debug.Log("Random Consonant: " + randomConsonant);
        }
        else
        {
            Debug.Log("No consonants left!");
        }
    }
    public void RandomVowel()
    {
        if (BattleDataHolder.VowelsLeft.Count > 0)
        {
            char randomVowel = GetRandomElement(BattleDataHolder.VowelsLeft);
            BattleDataHolder.VowelsLeft.Remove(randomVowel);
            spellbookController.AddLetter(randomVowel);
            Debug.Log("Random Vowel: " + randomVowel); 
        }
        else
        {
            Debug.Log("No vowels left!");
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

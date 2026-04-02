using UnityEngine;
using System.Collections.Generic;

public class WordDatabase : MonoBehaviour
{
    public HashSet<string> shapeWords = new HashSet<string> 
    { 
        "ball", 
        "bolt" 
    };
    public HashSet<string> elementWords = new HashSet<string> { 
        "fire", 

        "water",

        "earth",

        "air",

        "shock", 

        "ice",

        "light",
        
        "dark"
    };
    public HashSet<string> metaWords = new HashSet<string> 
    { 
        "strong","stronger","bigger","larger",
        "enhanced","empowered","enhance","empower","enchant",
        "power","powerful","might","mighty","force","forceful",
        "intense","intensify","amplify","amplified","boost","boosted",
        "surge","surged","overcharge","overcharged","charged","charge", 
        "fierce","fiercer","furious","rage","raging","wild",
        "unleashed","unleash","brutal","brutality","savage","relentless",


    };
}

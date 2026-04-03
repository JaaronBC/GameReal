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
        //Put all fire words here
        "fire", 
        //Put all water words here
        "water",
        //Put all earth words here
        "earth",
        //Put all air words here
        "air",
        //Put all shock words here
        "shock", 
        //put all Ice words here
        "ice",
        //Put all light words here
        "light",
        //Put all dark words here
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

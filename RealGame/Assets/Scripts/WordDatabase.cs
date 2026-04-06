using UnityEngine;
using System.Collections.Generic;

public class WordDatabase : MonoBehaviour
{
    public HashSet<string> shapeWords = new HashSet<string> 
    { 
        "ball", 
        "bolt",
        "missile",
        "beam",
        "laser",
        "ray",
        "slash",

    };
    public HashSet<string> elementWords = new HashSet<string> { 
        //Put all fire words here
        "fire","flame","flames","burn","burning","blaze","blazing",
        "inferno","infernal","scorch","scorching","ember","embers",
        "smoke","smolder","smoldering","char","charred",
        //Put all water words here
        "water", "wet", "drench", "drenched", "soak", "soaked", "splash", "splashed",
        "flood", "flooded", "torrent", "torrential", "wave", "waves", "tsunami", "tsunamic", "ocean",
        "sea", "river", "stream", "brook", "pond", "lake", "aquatic", "marine", "hydro", "hydration",
        "hydrate", 
        //Put all earth words here
        "earth", "rock", "stone", "dirt", "soil", "mud", "sand", "gravel", "clay", "boulder",
        "mountain", "hill", "cave", "cliff", "crag", "quarry", "geode", "gem", "mineral", "crystal",
        "earthquake", "seismic", "tectonic", "lithic", "terra",
        //Put all air words here
        "air", "breeze", "gust", "wind", "gale", "draft", "whirlwind", "tornado", "hurricane", "cyclone",
        "zephyr", "aerial", "atmosphere", "breath", "sky", "cloud", "storm", "tempest", 
        //Put all shock words here
        "shock", "electric", "electricity", "thunder", "lightning", "bolt", "jolt", "zap", "static",
        "surge", "overcharge", "overcharged", "charged", "charge", "shockwave", "shocking", "electrify", 
        "electrified", "stun", "stunning", "paralyze", "paralyzing", "voltage", "current", "ampere", "watt",
        //put all Ice words here
        "ice", "frost", "frostbite", "frosty", "chill", "chilling", "freeze", "freezing", "frozen", "glacier",
        "snow", "snowy", "blizzard", "hail", "icy", "subzero", "frigid", "arctic", "polar", "winter",
        //Put all light words here
        "light", "bright", "radiant", "luminous", "glowing", "shining", "brilliant", "dazzling", "sparkling", 
        "gleaming", "glimmering", "illuminated", "illumination", "sun", "sunlight", "sunshine", "daylight",
        "radiance", "halo", "flare", "flash", "holy", "prismatic",
        //Put all dark words here
        "dark", "shadow", "dim", "gloomy", "shady", "dark", "darkness", "night", "nocturnal", "eclipse", "obscure",
        "twilight", "midnight", "abyss", "void", "dusk", "crepuscular", "tenebrous", "sable", "pitch", "ebony",
        "charcoal", "murky", "somber", "dusky", "gloaming", "shadowy"
    };
    public HashSet<string> metaWords = new HashSet<string> 
    { 
        //remove any duplicates in elemntal words
        "strong","stronger","bigger","larger",
        "enhanced","empowered","enhance","empower","enchant",
        "power","powerful","might","mighty","force","forceful",
        "intense","intensify","amplify","amplified","boost","boosted", 
        "fierce","fiercer","furious","rage","raging","wild",
        "unleashed","unleash","brutal","brutality","savage","relentless",
        "devastating","devastate","cataclysmic","cataclysm","apocalyptic","apocalypse",
        "mighty","mightier","colossal","colossus","titanic","titan","gigantic","gigant",
        "enormous","enormity","monstrous","monstrosity","legendary","legend","mythic",
        "myth", "epic", "victorious", "victory", "triumphant", "triumph", "unstoppable", "unstoppability",
        "overwhelming","overwhelm","unrelenting","unrelent","merciless","mercilessness","savage","savagery",
        "fierce"
    };

    public HashSet<string> fireWords = new HashSet<string> 
    { 
        "fire","flame","flames","burn","burning","blaze","blazing",
        "inferno","infernal","scorch","scorching","ember","embers",
        "smoke","smolder","smoldering","char","charred"
    };
    public HashSet<string> waterWords = new HashSet<string> 
    { 
        "water", "wet", "drench", "drenched", "soak", "soaked", "splash", "splashed",
        "flood", "flooded", "torrent", "torrential", "waves", "tsunami", "tsunamic", "ocean",
        "sea", "river", "stream", "brook", "pond", "lake", "aquatic", "marine", "hydro", "hydration",
        "hydrate", 
    };
    public HashSet<string> earthWords = new HashSet<string> 
    { 
        "earth", "rock", "stone", "dirt", "soil", "mud", "sand", "gravel", "clay", "boulder",
        "mountain", "hill", "cave", "cliff", "crag", "quarry", "geode", "gem", "mineral", "crystal",
        "earthquake", "seismic", "tectonic", "lithic", "terra",
    };
    public HashSet<string> airWords = new HashSet<string> 
    { 
        "air", "breeze", "gust", "wind", "gale", "draft", "whirlwind", "tornado", "hurricane", "cyclone",
        "zephyr", "aerial", "atmosphere", "breath", "sky", "cloud", "storm", "tempest"
    };
    public HashSet<string> shockWords = new HashSet<string> 
    { 
        "shock", "electric", "electricity", "thunder", "lightning", "bolt", "jolt", "zap", "static",
        "surge", "overcharge", "overcharged", "charged", "charge", "shockwave", "shocking", "electrify", 
        "electrified", "stun", "stunning", "paralyze", "paralyzing", "voltage", "current", "ampere", "watt"
    };
    public HashSet<string> iceWords = new HashSet<string>
    {
        "ice", "frost", "frostbite", "frosty", "chill", "chilling", "freeze", "freezing", "frozen", "glacier",
        "snow", "snowy", "blizzard", "hail", "icy", "subzero", "frigid", "arctic", "polar", "winter"
    };
    public HashSet<string> lightWords = new HashSet<string>
    {
        "light", "bright", "radiant", "luminous", "glowing", "shining", "brilliant", "dazzling", "sparkling", 
        "gleaming", "glimmering", "illuminated", "illumination", "sun", "sunlight", "sunshine", "daylight",
        "radiance", "halo", "flare", "flash", "holy", "prismatic"

    };
    public HashSet<string> darkWords = new HashSet<string>
    {
        "dark", "shadow", "dim", "gloomy", "shady", "dark", "darkness", "night", "nocturnal", "eclipse", "obscure",
        "twilight", "midnight", "abyss", "void", "dusk", "crepuscular", "tenebrous", "sable", "pitch", "ebony",
        "charcoal", "murky", "somber", "dusky", "gloaming", "shadowy"
    };

    public HashSet<string> validWords;
    void Awake()
    {
        validWords = new HashSet<string>();

        if (shapeWords != null)
            validWords.UnionWith(shapeWords);

        if (elementWords != null)
            validWords.UnionWith(elementWords);

        if (metaWords != null)
            validWords.UnionWith(metaWords);
    }
}
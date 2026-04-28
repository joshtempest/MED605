using UnityEngine;

public class Identifier : MonoBehaviour
{
    [Header("Clean/service")]
    [SerializeField] bool cleanGaffel;
    [SerializeField] bool cleanKniv;
    [SerializeField] bool cleanSke;
    [SerializeField] bool cleanTallerken;

    [Header("Dirty")]
    [SerializeField] bool dirtyGaffel;
    [SerializeField] bool dirtyKniv;
    [SerializeField] bool dirtySke;
    [SerializeField] bool dirtyTallerken;

    [Header("Food")]
    [SerializeField] bool marmelade;
    [SerializeField] bool poelse;
    [SerializeField] bool smoer;

    public string IdentifyObject()
    {
        if (cleanGaffel) return "rG";
        else if (cleanKniv) return "rK";
        else if (cleanSke) return "rS";
        else if (cleanTallerken) return "rT";
        else if (dirtyGaffel) return "bG";
        else if (dirtyKniv) return "bK";
        else if (dirtySke) return "bS";
        else if (dirtyTallerken) return "bT";
        else if (marmelade) return "m";
        else if (poelse) return "p";
        else if (smoer) return "s";
        else return null;
    }
}

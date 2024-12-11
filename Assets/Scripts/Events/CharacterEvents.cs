using UnityEngine;
using UnityEngine.Events;

public class CharacterEvents
{
    // Character damage and damage value
    public static UnityAction<GameObject, int> characterDamaged;

    // Character Healed and amount healed
    public static UnityAction<GameObject, int> characterHealed;
}
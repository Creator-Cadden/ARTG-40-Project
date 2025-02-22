using UnityEngine;

[CreateAssetMenu(fileName = "NewFish", menuName = "Fishing Game/Fish")]
public class Fish : ScriptableObject
{
    public string[] buttonSequence;        // The sequence of buttons to press (e.g., "A", "B", "X", etc.)
    public float timeToMash = 2f;          // Time to mash each button in seconds
    public float timeBetweenSteps = 1f;    // Time between each button press
    public float difficultyMultiplier = 1.2f; // How much difficulty increases per step (multiplier)
    public float requiredMashRate = 2f;    // Minimum required presses per second to succeed
}
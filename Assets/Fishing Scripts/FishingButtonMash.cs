using UnityEngine;
using TMPro;  // For TextMeshPro
using System.Collections;

public class FishingButtonMash : MonoBehaviour
{
    public TextMeshProUGUI promptText;  // Reference to the TextMeshProUGUI component
    public Fish currentFish;            // Current fish the player is trying to catch
    public Movement playerMovement;     // Reference to Movement script to disable movement
    public Transform player;            // Reference to the player object

    private float currentTime = 0f;     // Time left for the button press window
    private float buttonPressRate = 0f; // Rate at which the player is pressing the button (presses per second)
    private float lastPressTime = 0f;   // Time of the last button press
    private int currentStep = 0;        // Current step in the button sequence
    private bool canMash = false;       // Can the player press the button
    private bool isFishing = false;     // Is the fishing game ongoing?

    void Start()
    {
        if (currentFish != null)
        {
            promptText.text = "Press F to start fishing!";
        }
        else
        {
            Debug.LogError("No fish assigned!");
        }
    }

    void Update()
    {
        // Detect if the player presses F to start the fishing game
        if (!isFishing && Input.GetKeyDown(KeyCode.F))
        {
            StartFishingGame();  // Start the fishing game when "F" is pressed
        }

        if (isFishing)
        {
            // Handle fishing game logic (only active when fishing starts)
            if (canMash)
            {
                currentTime -= Time.deltaTime;

                // Show countdown to the player
                promptText.text = "Mash " + currentFish.buttonSequence[currentStep] + " - Time Left: " + Mathf.Ceil(currentTime);

                // Check for button presses and track press rate
                if (Input.GetKeyDown(KeyCode.A) && currentFish.buttonSequence[currentStep] == "A")
                {
                    TrackButtonPress();
                }
                else if (Input.GetKeyDown(KeyCode.B) && currentFish.buttonSequence[currentStep] == "B")
                {
                    TrackButtonPress();
                }
                else if (Input.GetKeyDown(KeyCode.X) && currentFish.buttonSequence[currentStep] == "X")
                {
                    TrackButtonPress();
                }
                else if (Input.GetKeyDown(KeyCode.Y) && currentFish.buttonSequence[currentStep] == "Y")
                {
                    TrackButtonPress();
                }

                // If time runs out or button was pressed too slowly, fail the task
                if (currentTime <= 0f || buttonPressRate < currentFish.requiredMashRate)
                {
                    LoseFish();
                }
            }
        }
    }

    public void StartFishingGame()
    {
        // Ensure the button sequence is not empty
        if (currentFish == null || currentFish.buttonSequence.Length == 0)
        {
            Debug.LogError("Button sequence is empty or Fish is not assigned!");
            return;
        }

        // Start fishing logic
        isFishing = true;

        // Reset the current step to 0
        currentStep = 0;
        buttonPressRate = 0f;  // Reset button mash rate
        lastPressTime = Time.time; // Reset the time tracker

        // Disable player movement
        if (playerMovement != null)
        {
            playerMovement.DisableMovement();  // Disable movement while fishing
        }

        // Start the fishing process using the coroutine
        StartCoroutine(ButtonSequence());
    }

    private IEnumerator ButtonSequence()
    {
        while (currentStep < currentFish.buttonSequence.Length)
        {
            // Countdown before the player starts mashing the button
            int countdownTime = 3; // 3-second countdown before the button mash starts
            while (countdownTime > 0)
            {
                promptText.text = "Get ready! " + countdownTime + "...";
                yield return new WaitForSeconds(1f);
                countdownTime--;
            }

            // Reset the button mash rate and time for the next step
            buttonPressRate = 1; //0f prev
            currentTime = currentFish.timeToMash;

            // Prompt the player to mash the correct button
            promptText.text = "Mash " + currentFish.buttonSequence[currentStep] + " - Time Left: " + Mathf.Ceil(currentTime);
            canMash = true;

            // Wait for the player to mash the button within the time window
            yield return new WaitForSeconds(currentFish.timeToMash);

            // If the player pressed the button too slowly or wrong, fail the task
            if (buttonPressRate < currentFish.requiredMashRate)
            {
                LoseFish();
                yield break;  // Exit if failed
            }

            // If the button was pressed fast enough, move to the next step
            currentStep++;

            // Display message between steps saying "The fish is not pulling"
            promptText.text = "The fish is not pulling...";
            yield return new WaitForSeconds(currentFish.timeBetweenSteps);

            // Reset and prepare for the next step
            currentFish.timeToMash *= currentFish.difficultyMultiplier;
        }

        // If all steps are completed, the fish is caught
        CatchFish();
    }

    private void TrackButtonPress()
    {
        float currentPressTime = Time.time;

        // Calculate the time difference between the current press and the last press
        float timeDifference = currentPressTime - lastPressTime;

        // Update the button press rate based on time difference
        if (timeDifference < 1f)  // If the press is within 1 second, count it
        {
            buttonPressRate += 1f / timeDifference;  // Track presses per second
        }

        lastPressTime = currentPressTime;  // Update the last press time
    }

    private void LoseFish()
    {
        Debug.Log("You lost the fish!");
        EndFishing();  // End fishing and reset
    }

    public void CatchFish()
    {
        Debug.Log("Fish caught!");
        EndFishing();  // End fishing after catching the fish
    }

    private void EndFishing()
    {
        // Re-enable player movement after fishing
        if (playerMovement != null)
        {
            playerMovement.EnableMovement();  // Enable movement again
        }

        // Reset the fishing state and prompt text
        isFishing = false;
        promptText.text = "Press F to start fishing!";  // Show the start prompt
    }
}

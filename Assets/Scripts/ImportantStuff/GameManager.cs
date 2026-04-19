using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Story State")]
    public StoryState state = new StoryState();

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

[System.Serializable]
public class StoryState
{
    // Prologue
    public bool prologueDialoguePlayed = false;
    public bool prologueDone = false;

    // First shift - inside
    public bool firstShiftIntroPlayed = false;
    public bool firstShiftInsideDone = false;

    // First shift - outside
    public bool firstShiftOutsideIntroPlayed = false;
    public bool firstShiftTruckInspected = false;
    public bool firstShiftOutsideDone = false;

    // Second shift - loading
    public bool shift2IntroPlayed = false;
    public bool shift2TruckLoaded = false;

    // Second shift - driving
    public bool shift2DrivingDialoguePlayed = false;

    // Second shift - outside
    public bool shift2OutsideDialoguePlayed = false;
    public bool shift2HasTape = false;
    public bool shift2FuelTankDone = false;
    public bool shift2FuelDialoguePlayed = false;
    public bool shift2Done = false;
    public bool shift2HasFuelJerrican = false;

    public bool shift2FuelTankRefilled = false;
    public bool shift2MovingSound1Done = false;
    public bool shift2MovingSound2Done = false;
    public bool shift2ThirdInsideDialoguePlayed = false;
    public bool shift2FinalOutsideDialoguePlayed = false;
    public bool shift2ObjectADone = false;
    public bool shift2ObjectBDone = false;
    public bool shift2FinalInsideDialoguePlayed = false;
    public bool shift2FinalTruckLoaded = false;

    // Add more shifts here as you go
}
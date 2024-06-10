using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class GameModeManager : MonoBehaviour {
    public List<GameMode> gameModes;

    void Start() {
        PopulateGameModes();
    }

    private void PopulateGameModes() {
        gameModes = new List<GameMode>();

        // basic game mode: Original design of the game where everything is balanced
        int[] upgradeValues = new int[17] { 4, 4, 1, 3, 1, 2, 2, 2, 2, 1, 2, 1, 2, 1, 1, 1, 1 };
        GameMode Base = new("Base Mode", upgradeValues);
        gameModes.Add(Base);

        // Stormy Night: Game mode where there's lots of "wind" and "wind against player"
        upgradeValues = new int[17] { 3, 2, 1, 1, 1, 0, 10, 0, 0, 0, 6, 2, 3, 3, 2, 1, 2 };
        GameMode Stormy = new("Stormy Night", upgradeValues);
        gameModes.Add(Stormy);

        // Angelic: All buffs and good neutrals 
        upgradeValues = new int[17] { 4, 3, 1, 3, 3, 2, 3, 0, 0, 0, 0, 0, 2, 4, 0, 0, 1 };
        GameMode Angelic = new("Angelic", upgradeValues);
        gameModes.Add(Angelic);

        // Mine Field/Demonic: All the debuffs and negative neutrals
        upgradeValues = new int[17] { 0, 0, 0, 0, 0, 0, 0, 4, 2, 2, 2, 1, 0, 0, 1, 1, 1 };
        GameMode MineField = new("Mine Field", upgradeValues);
        gameModes.Add(MineField);

        // Ball Mania: Lots of split balls and bonus lives
        upgradeValues = new int[17] { 3, 3, 6, 2, 1, 2, 1, 0, 0, 0, 0, 2, 2, 1, 1, 1, 10 };
        GameMode BallMania = new("Ball Mania", upgradeValues);
        gameModes.Add(BallMania);

        // Drunken Brawl: Lots of “Flip Enemy Controls”, “Flip Controls”, “Flip Both Controls”
        upgradeValues = new int[17] { 2, 2, 1, 2, 2, 10, 2, 2, 2, 6, 2, 0, 0, 0, 0, 8, 1 };
        GameMode DrunkenBrawl = new("Drunken Brawl", upgradeValues);
        gameModes.Add(DrunkenBrawl);

        // Speed Racer: Lots of “Faster Paddle” and “Faster Both Paddles” and shorter
        upgradeValues = new int[17] { 0, 5, 1, 4, 0, 1, 1, 4, 0, 1, 0, 0, 0, 5, 0, 0, 1 };
        GameMode SpeedRacer = new("Speed Racer", upgradeValues);
        gameModes.Add(SpeedRacer);

        // Dumbo Style: Lots of Slowness and Elongation
        upgradeValues = new int[17] { 5, 0, 0, 0, 4, 0, 0, 0, 2, 0, 0, 0, 5, 0, 3, 0, 0 };
        GameMode Dumbo = new("Dumbo", upgradeValues);
        gameModes.Add(Dumbo);

        // We’re in this together: Heavier stats for neutral upgrades rather than buffs/debuffs
        upgradeValues = new int[17] { 1, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 6, 6, 6, 6, 4, 3 };
        GameMode Together = new("We're in this together", upgradeValues);
        gameModes.Add(Together);

        // Vanilla: No upgrades, but ball gets faster faster. TODO: how?
        upgradeValues = new int[17] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        GameMode Vanilla = new("Vanilla", upgradeValues);
        gameModes.Add(Vanilla);

        foreach (GameMode mode in gameModes) {
            //Debug.Log(mode.ToString());
        }
    }

    // Update is called once per frame
    void Update() {

    }
}

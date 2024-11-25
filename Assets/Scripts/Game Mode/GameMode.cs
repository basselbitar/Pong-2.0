using System;
using TMPro;

[Serializable]
public class GameMode
{
    private readonly string _name;
    private readonly int[] _upgradeWeights;

    // TODO: some game modes might need to modify how many lives the players start with, or their speed(s) or ball speed
    private readonly float _ballBouncinessMultiplier;

    public GameMode(string name, int[] upgradeWeights, float ballBouncinessMultiplier) {
        _name = name;
        _upgradeWeights = upgradeWeights;
        _ballBouncinessMultiplier = ballBouncinessMultiplier;
    }

    public string GetName() {
        return _name;
    }

    public string GetImageURL() {
        return "Game Modes Images/" + _name;
    }

    public int[] GetUpgradeWeights() {
        return _upgradeWeights;
    }

    public float GetBallBouncinessMultiplier() { 
        return _ballBouncinessMultiplier; 
    }

    override public string ToString() {
        int total = 0;
        string str = _name + ": ";

        int count = 0;

        foreach (var weight in GetUpgradeWeights()) {
            total += weight;
            str += weight.ToString() + ", ";
            count++;
            if(count == 7 || count == 11) {
                str += "| ";
            }
        }

        str += "| Total: " + total.ToString();
        return str;
    }

}

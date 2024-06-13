using System;

[Serializable]
public class GameMode
{
    private readonly string _name;
    private readonly int[] _upgradeWeights;

    // TODO: some game modes might need to modify how many lives the players start with, or their speed(s) or ball speed

    public GameMode(string name, int[] upgradeWeights) {
        _name = name;
        _upgradeWeights = upgradeWeights;
    }

    public string GetName() {
        return _name;
    }

    public int[] GetUpgradeWeights() {
        return _upgradeWeights;
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

using System.Collections.Generic;
using System;

public static class Shuffler {
    private static Random r;

    public static List<GameMode> Shuffle(List<GameMode> items, int randomIndex) {
        GameMode[] itemsArray = items.ToArray();

        r = new Random(randomIndex);

        for (int i = 0; i < itemsArray.Length - 1; i++) {
            int pos = r.Next(i, itemsArray.Length);
            GameMode temp = itemsArray[i];
            itemsArray[i] = itemsArray[pos];
            itemsArray[pos] = temp;
        }
        return new List<GameMode>(itemsArray);
    }
}
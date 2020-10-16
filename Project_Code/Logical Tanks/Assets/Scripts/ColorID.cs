using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorID
{
    Red,
    Orange,
    Yellow,
    Green,
    Blue,
    Purple,
    Pink,
    None
}

public static class ColorIDExtensions
{
    public static ColorID GetMatchingColor(this Material color)
    {
        string matName = color.name.ToLower();
        if(matName.Contains("red")) { return ColorID.Red; }
        else if(matName.Contains("orange")) { return ColorID.Orange; }
        else if(matName.Contains("yellow")) { return ColorID.Yellow; }
        else if(matName.Contains("green")) { return ColorID.Green; }
        else if(matName.Contains("blue")) { return ColorID.Blue; }
        else if(matName.Contains("purple")) { return ColorID.Purple; }
        else if(matName.Contains("pink")) { return ColorID.Pink; }
        return ColorID.None;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SwipeConverter
{
    private static readonly Dictionary<string, string> _swipeMap = new Dictionary<string, string>()
    {
        { "L8" , "U"  }, { "R8" , "U'" }, { "U2" , "R"  }, { "D2" , "R'" },
        { "L9" , "U"  }, { "R9" , "U'" }, { "U3" , "R"  }, { "D3" , "R'" },
        { "L10", "U"  }, { "R10", "U'" }, { "U4" , "R"  }, { "D4" , "R'" },
        { "L16", "U"  }, { "R16", "U'" }, { "U18", "R"  }, { "D18", "R'" },
        { "L17", "U"  }, { "R17", "U'" }, { "U19", "R"  }, { "D19", "R'" },
        { "L18", "U"  }, { "R18", "U'" }, { "U20", "R"  }, { "D20", "R'" },
        { "L24", "U"  }, { "R24", "U'" }, { "U32", "R"  }, { "D32", "R'" },
        { "L25", "U"  }, { "R25", "U'" }, { "U38", "R"  }, { "D38", "R'" },
        { "L26", "U"  }, { "R26", "U'" }, { "U39", "R"  }, { "D39", "R'" },
        { "L32", "U"  }, { "R32", "U'" }, { "U42", "R"  }, { "D42", "R'" },
        { "L33", "U"  }, { "R33", "U'" }, { "U43", "R"  }, { "D43", "R'" },
        { "L34", "U"  }, { "R34", "U'" }, { "U44", "R"  }, { "D44", "R'" },
        { "L40", "F"  }, { "R4" , "F"  }, { "U26", "F"  }, { "D8" , "F"  },
        { "L41", "F"  }, { "R5" , "F"  }, { "U27", "F"  }, { "D14", "F"  },
        { "L42", "F"  }, { "R6" , "F"  }, { "U28", "F"  }, { "D15", "F"  },
        { "L4" , "F'" }, { "R40", "F'" }, { "U8" , "F'" }, { "D26", "F'" },
        { "L5" , "F'" }, { "R41", "F'" }, { "U14", "F'" }, { "D27", "F'" },
        { "L6" , "F'" }, { "R42", "F'" }, { "U15", "F'" }, { "D28", "F'" },
        { "L12", "D'" }, { "R12", "D"  }, { "U0" , "L'" }, { "D0" , "L"  },
        { "L13", "D'" }, { "R13", "D"  }, { "U6" , "L'" }, { "D6" , "L"  },
        { "L14", "D'" }, { "R14", "D"  }, { "U7" , "L'" }, { "D7" , "L"  },
        { "L20", "D'" }, { "R20", "D"  }, { "U16", "L'" }, { "D16", "L"  },
        { "L21", "D'" }, { "R21", "D"  }, { "U22", "L'" }, { "D22", "L"  },
        { "L22", "D'" }, { "R22", "D"  }, { "U23", "L'" }, { "D23", "L"  },
        { "L28", "D'" }, { "R28", "D"  }, { "U40", "L'" }, { "D40", "L"  },
        { "L29", "D'" }, { "R29", "D"  }, { "U46", "L'" }, { "D46", "L"  },
        { "L30", "D'" }, { "R30", "D"  }, { "U47", "L'" }, { "D47", "L"  },
        { "L36", "D'" }, { "R36", "D"  }, { "U34", "L'" }, { "D34", "L"  },
        { "L37", "D'" }, { "R37", "D"  }, { "U35", "L'" }, { "D35", "L"  },
        { "L38", "D'" }, { "R38", "D"  }, { "U36", "L'" }, { "D36", "L"  },
        { "L0" , "B"  }, { "R44", "B"  }, { "U10", "B"  }, { "D24", "B"  },
        { "L1" , "B"  }, { "R45", "B"  }, { "U11", "B"  }, { "D30", "B"  },
        { "L2" , "B"  }, { "R46", "B"  }, { "U12", "B"  }, { "D31", "B"  },
        { "L44", "B'" }, { "R0" , "B'" }, { "U24", "B'" }, { "D10", "B'" },
        { "L45", "B'" }, { "R1" , "B'" }, { "U30", "B'" }, { "D11", "B'" },
        { "L46", "B'" }, { "R2" , "B'" }, { "U31", "B'" }, { "D12", "B'" },
    };

    private static string DefineDirection(Vector2 swipeDirection)
    {
        if (Mathf.Abs(swipeDirection.x) > Mathf.Abs(swipeDirection.y))
            if (swipeDirection.x < 0)
                return "L"; //left
            else
                return "R"; //right
        else
            if (swipeDirection.y < 0)
                return "D"; //down
            else
                return "U"; //up
    }

    public static string GetMove(byte faceletId, Vector2 swipeDirection)
    {
        string key = DefineDirection(swipeDirection) + faceletId;
        if (_swipeMap.ContainsKey(key))
            return _swipeMap[key];
        return null;
    }
}

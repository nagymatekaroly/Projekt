using UnityEngine;

public static class LevelMapper
{
    public static int GetLevelDatabaseIdByBuildIndex(int buildIndex)
    {
        switch (buildIndex)
        {
            case 8: return 1;  // Level 1
            case 9: return 2;  // Level 2
            case 10: return 3; // Level 3
            case 11: return 4; // Level 4
            case 12: return 5; // Level 5
            case 7: return 6;  // Tutorial
            default:
                Debug.LogWarning("❌ Ismeretlen buildIndex: " + buildIndex);
                return 0;
        }
    }
}

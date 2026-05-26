using UnityEngine;

public static class  GameSessionData
{
   // Total keys
    private const string KEY_TOTAL_COINS = "TotalCoins";
    private const string KEY_TOTAL_DIAMONDS = "TotalDiamonds";
    private const string KEY_TOTAL_KEYS = "TotalKeys";
    private const string KEY_TOTAL_EXP = "TotalExperience";

    private const string KEY_COLLECTED_COINS = "CollectedCoins";
    private const string KEY_COLLECTED_DIAMONDS = "CollectedDiamonds";
    private const string KEY_COLLECTED_KEYS = "CollectedKeys";
    private const string KEY_COLLECTED_EXP = "CollectedExperience";
    private const string KEY_HAS_REWARD = "HasPendingReward";

    private const string KEY_DISPLAYED_COINS = "DisplayedCoins";
    private const string KEY_DISPLAYED_DIAMONDS = "DisplayedDiamonds";
    private const string KEY_DISPLAYED_KEYS = "DisplayedKeys";
    private const string KEY_DISPLAYED_EXP = "DisplayedExperience";

    public static int DisplayedCoins
    {
        get => PlayerPrefs.GetInt(KEY_DISPLAYED_COINS, 0);
        set { PlayerPrefs.SetInt(KEY_DISPLAYED_COINS, value); PlayerPrefs.Save(); }
    }
    public static int DisplayedDiamonds
    {
        get => PlayerPrefs.GetInt(KEY_DISPLAYED_DIAMONDS, 0);
        set { PlayerPrefs.SetInt(KEY_DISPLAYED_DIAMONDS, value); PlayerPrefs.Save(); }
    }
    public static int DisplayedKeys
    {
        get => PlayerPrefs.GetInt(KEY_DISPLAYED_KEYS, 0);
        set { PlayerPrefs.SetInt(KEY_DISPLAYED_KEYS, value); PlayerPrefs.Save(); }
    }
    public static int DisplayedExperience
    {
        get => PlayerPrefs.GetInt(KEY_DISPLAYED_EXP, 0);
        set { PlayerPrefs.SetInt(KEY_DISPLAYED_EXP, value); PlayerPrefs.Save(); }
    }
    public static int TotalCoins
    {
        get => PlayerPrefs.GetInt(KEY_TOTAL_COINS, 0);
        private set { PlayerPrefs.SetInt(KEY_TOTAL_COINS, value); PlayerPrefs.Save(); }
    }
    public static int TotalDiamonds
    {
        get => PlayerPrefs.GetInt(KEY_TOTAL_DIAMONDS, 0);
        private set { PlayerPrefs.SetInt(KEY_TOTAL_DIAMONDS, value); PlayerPrefs.Save(); }
    }
    public static int TotalKeys
    {
        get => PlayerPrefs.GetInt(KEY_TOTAL_KEYS, 0);
        private set { PlayerPrefs.SetInt(KEY_TOTAL_KEYS, value); PlayerPrefs.Save(); }
    }
    public static int TotalExperience
    {
        get => PlayerPrefs.GetInt(KEY_TOTAL_EXP, 0);
        private set { PlayerPrefs.SetInt(KEY_TOTAL_EXP, value); PlayerPrefs.Save(); }
    }

    // ✅ Collected — jo collect ho chuka hai
    private static int CollectedCoins
    {
        get => PlayerPrefs.GetInt(KEY_COLLECTED_COINS, 0);
        set { PlayerPrefs.SetInt(KEY_COLLECTED_COINS, value); PlayerPrefs.Save(); }
    }
    private static int CollectedDiamonds
    {
        get => PlayerPrefs.GetInt(KEY_COLLECTED_DIAMONDS, 0);
        set { PlayerPrefs.SetInt(KEY_COLLECTED_DIAMONDS, value); PlayerPrefs.Save(); }
    }
    private static int CollectedKeys
    {
        get => PlayerPrefs.GetInt(KEY_COLLECTED_KEYS, 0);
        set { PlayerPrefs.SetInt(KEY_COLLECTED_KEYS, value); PlayerPrefs.Save(); }
    }
    private static int CollectedExperience
    {
        get => PlayerPrefs.GetInt(KEY_COLLECTED_EXP, 0);
        set { PlayerPrefs.SetInt(KEY_COLLECTED_EXP, value); PlayerPrefs.Save(); }
    }

    public static bool HasPendingReward
    {
        get => PlayerPrefs.GetInt(KEY_HAS_REWARD, 0) == 1;
        private set { PlayerPrefs.SetInt(KEY_HAS_REWARD, value ? 1 : 0); PlayerPrefs.Save(); }
    }

    // ✅ Uncollected = Total - Collected (yahi RewardPanel me dikhao)
    public static int UncollectedCoins => TotalCoins - CollectedCoins;
    public static int UncollectedDiamonds => TotalDiamonds - CollectedDiamonds;
    public static int UncollectedKeys => TotalKeys - CollectedKeys;
    public static int UncollectedExperience => TotalExperience - CollectedExperience;

    // ✅ Session end pe call karo
    public static void SaveSessionData(int coins, int diamonds, int keys, int exp)
    {
        TotalCoins += coins;
        TotalDiamonds += diamonds;
        TotalKeys += keys;
        TotalExperience += exp;
        HasPendingReward = true;
    }

    // ✅ Collect button pe — collected update karo
    public static void ClearPendingReward()
    {
        // Abhi tak jo uncollected tha vo collect ho gaya
        CollectedCoins = TotalCoins;
        CollectedDiamonds = TotalDiamonds;
        CollectedKeys = TotalKeys;
        CollectedExperience = TotalExperience;
        HasPendingReward = false;
    }

    public static void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}

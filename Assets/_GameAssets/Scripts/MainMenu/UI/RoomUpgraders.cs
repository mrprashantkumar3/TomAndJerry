using TMPro;
using UnityEngine;

public class RoomUpgraders : MonoBehaviour
{
    [SerializeField] private GameObject roomUgraders1;
    [SerializeField] private GameObject roomUgraders2;
    [SerializeField] private GameObject roomUgraders3;
    [SerializeField] private GameObject roomUgraders4;
    [SerializeField] private GameObject roomUgraders5;
    [SerializeField] private TMP_Text textLevel;
    public int level;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        level = 1;
    }

    public void SetLevel(int lvl)
    {
        if (lvl == 1)
        {
            roomUgraders1.SetActive(true);
            roomUgraders2.SetActive(false);
            roomUgraders3.SetActive(false);
            roomUgraders4.SetActive(false);
            roomUgraders5.SetActive(false);
            textLevel.text = "Level : " + level.ToString();
        }
         if (lvl == 2)
        {
            roomUgraders1.SetActive(false);
            roomUgraders2.SetActive(true);
            roomUgraders3.SetActive(false);
            roomUgraders4.SetActive(false);
            roomUgraders5.SetActive(false);
            textLevel.text = "Level : " + level.ToString();
        }
         if (lvl == 3)
        {
            roomUgraders1.SetActive(false);
            roomUgraders2.SetActive(false);
            roomUgraders3.SetActive(true);
            roomUgraders4.SetActive(false);
            roomUgraders5.SetActive(false);
            textLevel.text = "Level : " + level.ToString();
        }
         if (lvl == 4)
        {
            roomUgraders1.SetActive(false);
            roomUgraders2.SetActive(false);
            roomUgraders3.SetActive(false);
            roomUgraders4.SetActive(true);
            roomUgraders5.SetActive(false);
            textLevel.text = "Level : " + level.ToString();
        }
         if (lvl == 5)
        {
            roomUgraders1.SetActive(false);
            roomUgraders2.SetActive(false);
            roomUgraders3.SetActive(false);
            roomUgraders4.SetActive(false);
            roomUgraders5.SetActive(true);
            textLevel.text = "Level : " + level.ToString();
        }
    }
    public void Ugrade()
    {
        if (level < 5)
        {
            level += 1;
            SetLevel(level);
        }
        else
        {
            Debug.Log("Max Level");
        }
    }

}

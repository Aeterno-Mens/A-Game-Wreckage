using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public static Upgrade Instance;
    [SerializeField] int Tier;

    public void Awake()
    {
        Instance = this;
    }
    public void CheckUpgrade() {
        int tmp=0;
        if (GameHandler.Instance.GameState == GameState.Player1Turn)
            tmp = GameHandler.Instance.UpgradeP1;
        else if (GameHandler.Instance.GameState == GameState.Player2Turn)
            tmp = GameHandler.Instance.UpgradeP2;
        if (Tier > tmp)
        {
            this.transform.Find("Veil").gameObject.SetActive(true);
        }
        else
        {
            this.transform.Find("Veil").gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        CheckUpgrade();
    }
}

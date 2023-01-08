using TMPro;
using UnityEngine;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] private GameObject organicDisplay;
    [SerializeField] private TMP_Text organicAmt;

    [SerializeField] private GameObject metalDisplay;
    [SerializeField] private TMP_Text metalAmt;

    [SerializeField] private GameObject gemDisplay;
    [SerializeField] private TMP_Text gemAmt;

    [SerializeField] private GameObject matterDisplay;
    [SerializeField] private TMP_Text matterAmt;

    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
    }

    private void Update()
    {
        float organic = gameManager.GetAmt("Organic");
        float metal = gameManager.GetAmt("Metal");
        float gem = gameManager.GetAmt("Gems");
        float matter = gameManager.GetAmt("Dense Matter");

        organicDisplay.SetActive(organic != -1);
        metalDisplay.SetActive(metal != -1);
        gemDisplay.SetActive(gem != -1);
        matterDisplay.SetActive(matter != -1);

        organicAmt.text = organic.ToString();
        metalAmt.text = metal.ToString();
        gemAmt.text = gem.ToString();
        matterAmt.text = matter.ToString();
    }
}

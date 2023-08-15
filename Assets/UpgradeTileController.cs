using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Upgrades;

public class UpgradeTileController : MonoBehaviour
{
    public Image image;

    public Image arrow;

    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public TextMeshProUGUI initial;
    public TextMeshProUGUI next;
    public TextMeshProUGUI dna;
    public TextMeshProUGUI tech;

    public TextMeshProUGUI max;
    public GameObject dnaObject;
    public GameObject techObject;
    public Button buy;
    public UIMouseOverGrow grow;

    public Color afford;
    public Color cantAfford;

    public CanvasGroup group;

    public void SetTile(Upgrade upgrade, int level) {
        if (upgrade.empty) {
            group.alpha = 0;
            group.blocksRaycasts = false;
        }
        else {
            group.alpha = 1;
            group.blocksRaycasts = true;
        }

        if (level == upgrade.maxLevel) {
            image.sprite = (upgrade.image.Length > 1) ? upgrade.image[level - 1] : upgrade.image[0];
            title.text = upgrade.upgradeName;
            description.text = (upgrade.description.Length > 1) ? upgrade.description[level - 1] : upgrade.description[0];
            arrow.enabled = false;
            initial.enabled = false;
            next.enabled = false;
            dnaObject.SetActive(false);
            techObject.SetActive(false);
            max.enabled = true;
            buy.enabled = false;
            grow.enabled = false;
        }
        else {
            image.sprite = (upgrade.image.Length > 1) ? upgrade.image[level] : upgrade.image[0];
            title.text = upgrade.upgradeName;
            description.text = (upgrade.description.Length > 1) ? upgrade.description[level] : upgrade.description[0];
            arrow.enabled = upgrade.indicators.Length > 0;
            initial.enabled = true;
            next.enabled = true;
            dnaObject.SetActive(true);
            techObject.SetActive(true);
            initial.text = (upgrade.indicators.Length > 0 && level >= 0) ? upgrade.indicators[level] : "";
            next.text = (upgrade.indicators.Length > 0) ? upgrade.indicators[level] : "";
            dna.text = upgrade.dna[level].ToString();
            tech.text = upgrade.tech[level].ToString();
            max.enabled = false;
            buy.enabled = true;
            grow.enabled = true;

            if (upgrade.dna[level] < Player.dnaCount)
                dna.color = cantAfford;
            else
                dna.color = afford;
            
            if (upgrade.tech[level] < Player.techCount)
                dna.color = cantAfford;
            else
                dna.color = afford;
        }
    }
}

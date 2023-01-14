using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UpgradeCardUIController : GameUIComponent, IPointerClickHandler
{  
    public int Index;

    UpgradeStation Station;

    CanvasGroup Card;

    public string Name {
        set {
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = value;
        }
    }
    public string Description {
        set {
            transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = value;
        }
    }
    public int CurrentLevel {
        set {
            transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = value.ToString();
        }
    }
    public string CurrentValue {
        set {
            transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = value.ToString();
        }
    }
    public int NewLevel {
        set {
            transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = value.ToString();
        }
    }
    public string NewValue {
        set {
            transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = value.ToString();
        }
    }
    public int DNACost {
        set {
            transform.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = value.ToString();
        }
    }
    public int TechCost {
        set {
            transform.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = value.ToString();
        }
    }

    public void OnPointerClick(PointerEventData e) {
        UpgradeStationManager.MostRecent.Buy(Index);
    }

    public void Hide() {
        Card.alpha = 0;
        Card.blocksRaycasts = false;
    }

    public void Show() {
        Card.alpha = 1;
        Card.blocksRaycasts = true;
    }

    void Start() {
        Station = GetComponentInParent<UpgradeStation>();
        Card = GetComponent<CanvasGroup>();
    }
}

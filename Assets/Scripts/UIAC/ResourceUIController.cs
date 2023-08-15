using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceUIController : MonoBehaviour
{
    public float numberChangeRate;

    private float smoothedDna;

    private float smoothedTech;

    private int actualDna;

    private int actualTech;

    public int dna {
        get {
            return actualDna;
        }
        set {
            actualDna = value;
        }
    }

    public int tech {
        get {
            return actualTech;  
        }
        set {
            actualTech = value;
        }
    }

    public TextMeshProUGUI dnaCount;
    public TextMeshProUGUI techCount;

    void Update() {
        smoothedDna += Mathf.Clamp((float) actualDna - smoothedDna, -numberChangeRate * Time.deltaTime, numberChangeRate * Time.deltaTime);
        smoothedTech += Mathf.Clamp((float) actualTech - smoothedTech, -numberChangeRate * Time.deltaTime, numberChangeRate * Time.deltaTime);

        dnaCount.text = Mathf.RoundToInt(smoothedDna).ToString();
        techCount.text = Mathf.RoundToInt(smoothedTech).ToString();
    }

    public ResourceUIController() {
        UIManager.resourceUI = this;
    }
}

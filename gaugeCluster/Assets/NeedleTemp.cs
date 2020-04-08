using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using System.Collections;
using System.Threading;

public class NeedleTemp : MonoBehaviour {
    public Transform needle;
    public Transform speedLabelTemplate;

    public float MAX_SPEED_ANGLE;
    public float ZERO_SPEED_ANGLE;

    public float displayedSpeedMax;
    public float displayedSpeedMin;
    public float speed = 0f;
    public int amountOfLabels;
    public Transform parentTransform;
    public Text numberForTextLabel;
    public bool showMarkers;

    // Use this for initialization
    void Start () {
        CreateSpeedLabels();
        speedLabelTemplate.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update () {
        //Thread.Sleep(50);
        //speed += 1f;
        //Debug.Log("speed = " + speed);
        if (speed > displayedSpeedMax)
        {
            needle.eulerAngles = new Vector3(0, 0, MAX_SPEED_ANGLE);
        }
        else if (speed < displayedSpeedMin)
        {
            needle.eulerAngles = new Vector3(0, 0, ZERO_SPEED_ANGLE);
        }
        else
        {
            needle.eulerAngles = new Vector3(0, 0, GetSpeedRotation());
        }
	}

    private void CreateSpeedLabels()
    {
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;
        //float offset = displayedSpeedMin / displayedSpeedMax;
        if (showMarkers)
        {
            for (int i = 0; i <= amountOfLabels; i++)
            {
                Transform speedLabelTransform = Instantiate(speedLabelTemplate, new Vector3(0, 0, 0), Quaternion.identity) as Transform;

                float labelSpeedNormalized = ((float)i / amountOfLabels);
                

                float speedLabelAngle = ZERO_SPEED_ANGLE - labelSpeedNormalized * totalAngleSize;
                speedLabelTransform.eulerAngles = new Vector3(0, 0, speedLabelAngle);
                float labelValue = ((displayedSpeedMax - displayedSpeedMin) * labelSpeedNormalized) + displayedSpeedMin; //speedmax - speedmin - offset = new range i.e. (265-0-100) = 165, multiply by the normalizedValue and add the offset i.e. (165 * 0.25) + 100
                speedLabelTransform.Find("speedText").GetComponent<Text>().text = Mathf.RoundToInt(labelValue).ToString();
                speedLabelTransform.Find("speedText").eulerAngles = Vector3.zero;
               // speedLabelTransform.Find("speedText").gameObject.SetActive(false);
                speedLabelTransform.gameObject.SetActive(true);
                speedLabelTransform.transform.SetParent(parentTransform, false);

            }
        }
    }

    private float GetSpeedRotation()
    {
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;
        float speedNormalized = (speed - displayedSpeedMin) / (displayedSpeedMax - displayedSpeedMin);
        return ZERO_SPEED_ANGLE - speedNormalized * totalAngleSize;
    }
}

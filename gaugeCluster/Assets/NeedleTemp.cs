using UnityEngine;

using UnityEngine.UI;
using System.Collections;
using System.Threading;
using System.Collections.Generic;

public class NeedleTemp : MonoBehaviour
{
    public Transform needle;
    public Transform speedLabelTemplate;
    public Transform curvesTemplate;

    public float MAX_SPEED_ANGLE;
    public float ZERO_SPEED_ANGLE;

    public float displayedSpeedMax;
    public float displayedSpeedMin;
    public float speed = 0f;
    public int amountOfLabels;
    public int amountOfCurves;
    public Transform parentTransform;
    public Transform curvedParentTransform;

    public Text numberForTextLabel;
    public bool showMarkers;
    public float labelOffset;
    public bool adjustNeedleSpeed;
    private float oldSpeed = 0f;

    List<Transform> curvedLineList = new List<Transform>();

    // Use this for initialization
    void Start()
    {
        CreateSpeedLabels();
        CreateEmptyGameObjects();
        speedLabelTemplate.gameObject.SetActive(false);
        curvesTemplate.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (speed > displayedSpeedMax)
        {
            speed = displayedSpeedMax;
        }
        else if (speed < displayedSpeedMin)
        {
            speed = displayedSpeedMin;
        }
        else
        {
            float step = 1000;
            //if (adjustNeedleSpeed == true)
            //{
            //    float result = Mathf.Abs(speed - oldSpeed);
            //    Debug.Log(result);
            //    if (result > 1.2 || speed == 0)
            //    {
            //        step = 20 * Time.deltaTime;
            //    }
            //    else
            //    {
            //        step = 10 * Time.deltaTime;
            //    }
            //}
            //needle.eulerAngles = Vector3.MoveTowards(needle.eulerAngles, new Vector3(0, 0, GetSpeedRotation(speed)), step);
      
            for (int i = 0; i < curvedLineList.Count; i++)
            {
                float x = ((i / 1f) / (amountOfCurves / 1f));
                float amount = (x * speed);
                Debug.Log(amount);
                curvedLineList[i].eulerAngles = Vector3.MoveTowards(curvedLineList[i].eulerAngles, new Vector3(0, 0, GetSpeedRotation(amount)), step);

            }
            oldSpeed = speed;
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
                speedLabelTransform.eulerAngles = new Vector3(0, 0, speedLabelAngle + labelOffset);
                float labelValue = ((displayedSpeedMax - displayedSpeedMin) * labelSpeedNormalized) + displayedSpeedMin; //speedmax - speedmin - offset = new range i.e. (265-0-100) = 165, multiply by the normalizedValue and add the offset i.e. (165 * 0.25) + 100
                speedLabelTransform.Find("speedText").GetComponent<Text>().text = Mathf.RoundToInt(labelValue).ToString();
                speedLabelTransform.Find("speedText").eulerAngles = Vector3.zero;
                // speedLabelTransform.Find("speedText").gameObject.SetActive(false);
                speedLabelTransform.gameObject.SetActive(true);
                speedLabelTransform.transform.SetParent(parentTransform, false);

            }
        }
    }

    private void CreateEmptyGameObjects()
    {
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;
        if (curvesTemplate != null)
        {
            for (int i = 0; i <= amountOfCurves; i++)
            {

                Transform curvedPointTransform = Instantiate(curvesTemplate, new Vector3(0, 0, 0), Quaternion.identity) as Transform;
                curvedPointTransform.transform.SetParent(curvedParentTransform, false);
                curvedLineList.Add(curvedPointTransform);

            }
        }
    }

    private float GetSpeedRotation(float amount)
    {
        float totalAngleSize = ZERO_SPEED_ANGLE - MAX_SPEED_ANGLE;
        float speedNormalized = (amount - displayedSpeedMin) / (displayedSpeedMax - displayedSpeedMin);
        return ZERO_SPEED_ANGLE - speedNormalized * totalAngleSize;
    }
}

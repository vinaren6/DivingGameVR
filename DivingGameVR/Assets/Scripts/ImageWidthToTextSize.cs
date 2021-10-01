using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageWidthToTextSize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject ChildGameObject1 = gameObject.transform.GetChild(0).gameObject;
        RectTransform rt = (RectTransform)ChildGameObject1.transform;
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rt.rect.width);
        //gameObject.transform.wid
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

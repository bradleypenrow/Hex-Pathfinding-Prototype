using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls Box selection of armies
/// </summary>
public class SelectBox : MonoBehaviour
{
    [SerializeField]
    private RectTransform selectSquareImage;

    Vector3 startPos;
    Vector3 endPos;

    GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        selectSquareImage.gameObject.SetActive(false);
        manager = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {            
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity)) 
            {
                startPos = Camera.main.WorldToScreenPoint(hit.point);
                startPos.z = 0;
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            manager.AddArmy(startPos, endPos);
            selectSquareImage.gameObject.SetActive(false);
        }
        if (Input.GetMouseButton(0))
        {
            if(!selectSquareImage.gameObject.activeInHierarchy)
                selectSquareImage.gameObject.SetActive(true);

            endPos = Input.mousePosition;

            Vector3 center = (startPos + endPos) / 2f;

            float sizeX = Mathf.Abs(startPos.x - endPos.x);
            float sizeY = Mathf.Abs(startPos.y - endPos.y);

            selectSquareImage.sizeDelta = new Vector2(sizeX, sizeY);
            selectSquareImage.position = center;
        }
    }
}

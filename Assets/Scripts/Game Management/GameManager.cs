using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls base game logic of selecting and requesting moves.
/// </summary>
public class GameManager : MonoBehaviour
{
    public LayerMask friendlyUnits;
    public LayerMask hexes;
    public List<Army> selectedArmies = new List<Army>();
    public List<Army> allArmies = new List<Army>();

    // Update is called once per frame
    void Update()
    {
        //Select an individual army by left-clicking on them
        if (Input.GetMouseButtonDown(0))
        {
            selectedArmies.Clear();
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit,Mathf.Infinity, friendlyUnits))
            {
                selectedArmies.Add(hit.collider.gameObject.GetComponent<Army>());
            }
        }

        //Right-click to move them to a province
        if (Input.GetMouseButtonUp(1))
        {
            if (selectedArmies.Count > 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, hexes))
                {
                    if (hit.collider.gameObject != null)
                    {
                        foreach (Army a in selectedArmies)
                        {
                            a.RequestArmyPath(hit.collider.gameObject.GetComponent<Province>());
                        }
                    }
                }
            }
        }
    }

    //Select army to move with box selector. Called from SelectBox.cs
    public void AddArmy(Vector3 startPos, Vector3 endPos)
    {
        Rect selectRect = new Rect(startPos.x, startPos.y, endPos.x - startPos.x, endPos.y - startPos.y);

        foreach (Army a in allArmies)
        {
            if (a != null)
            {
                if (selectRect.Contains(Camera.main.WorldToScreenPoint(a.gameObject.transform.position), true))
                {
                    selectedArmies.Add(a);
                }
            }
        }
    }
}

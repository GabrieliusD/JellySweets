using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectWithMouse : MonoBehaviour
{
    public Transform ControllablePlayerSpawnLocation;
    public Transform ControlUI;
    bool charSelected = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!charSelected)
        if(Input.GetMouseButtonDown(0))
        {
            
            Debug.Log("ray cast");
            RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hitInfo)
            {
                if (hitInfo.collider.tag == "Player")
                {
                    ControlUI.gameObject.SetActive(true);
                    ControllablePlayerSpawnLocation.gameObject.SetActive(true);
                    ControllablePlayerSpawnLocation.GetComponent<SpriteRenderer>().sprite = hitInfo.transform.GetComponent<Player>().charObj.icon;
                    hitInfo.transform.gameObject.SetActive(false);
                        charSelected = true;
                    //this.gameObject.SetActive(false);
                }
            }
        }
    }

    public void setCharSelected()
    {
        charSelected = false;
    }

}

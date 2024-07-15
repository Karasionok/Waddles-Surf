using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    public BoxCollider bc;
    public Vector3 newSize;
    public Vector3 basicSize;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) & this.transform.position.y <= 1.2f)
        {
            bc.size = newSize;
            this.transform.localScale = newSize;
            this.transform.position = this.transform.position + new Vector3(0, -0.3f, 0);
        }
        if (Input.GetKeyUp(KeyCode.S) & this.transform.position.y <= 1.2f)
        {
            bc.size = basicSize;
            this.transform.localScale = basicSize;
            this.transform.position = this.transform.position + new Vector3(0, 0.3f, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Vector2Int _boundary = new Vector2Int(200, 200);
    private int _divider = 2;

    // Start is called before the first frame update
    void Start()
    {
        CreateRectangle();
    }
    
    private void CreateRectangle()
    {
        int width = _boundary.x / _divider;
        int height = _boundary.y / _divider;
            
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if(x == 0 || y == 0)
                    continue;
                    
                var northwest = new Vector2Int((10 + width) * x, (10 + height) * y);
                Debug.Log(northwest);
            }   
        }
    }
}

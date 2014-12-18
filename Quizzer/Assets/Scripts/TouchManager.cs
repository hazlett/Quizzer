using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour {
    private static TouchManager instance;
    public static TouchManager Instance { get { return instance; } }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        //SwipePan();
        PinchZoom();
    }
    private void SwipePan()
    {
        if (Input.touchCount == 1)
        {
            Touch t = Input.touches[0];
            Utility.GUIPOSITION += new Vector3(t.deltaPosition.x, t.deltaPosition.y, 0);
        }
    }
    private void PinchZoom()
    {
        if (Input.touchCount > 1)
        {
            Touch t1 = Input.touches[0];
            Touch t2 = Input.touches[1];

            Vector2 t1Prev = t1.position - t1.deltaPosition;
            Vector2 t2Prev = t2.position - t2.deltaPosition;

            float prevTouchDeltaMag = (t1Prev - t2Prev).magnitude;
            float touchDeltaMag = (t1.position - t2.position).magnitude;

            float deltaMagDiff = prevTouchDeltaMag - touchDeltaMag;

            Utility.SCREENHEIGHT += deltaMagDiff;
            Utility.SCREENWIDTH += deltaMagDiff * ((float)Utility.STARTINGRESOLUTION.width / (float)Utility.STARTINGRESOLUTION.height);
        }
    }
    private static Vector2 SwipeScroll(Vector2 scroll)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            if (touch.phase == TouchPhase.Moved)
            {
                scroll.y += touch.deltaPosition.y;
            }
        }
        return scroll;
    }
}

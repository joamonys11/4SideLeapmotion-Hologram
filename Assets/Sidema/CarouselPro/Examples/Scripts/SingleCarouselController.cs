using UnityEngine;
using System.Collections;
using Sidema.CarouselPro;

public class SingleCarouselController : MonoBehaviour
{
    public Carousel carousel;

    bool triggered;
    
    void Update()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        if (!triggered && Mathf.Abs(horizontalAxis) > 0f)
        {
            triggered = true;
            if (horizontalAxis < 0f)
            {
                carousel.Previous();
            }
            else
            {
                carousel.Next();
            }

        }

        if (horizontalAxis < 0.01f)
            triggered = false;
    }
}

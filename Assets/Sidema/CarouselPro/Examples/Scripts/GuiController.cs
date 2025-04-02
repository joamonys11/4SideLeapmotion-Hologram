using UnityEngine;
using UnityEngine.UI;
using Sidema.CarouselPro;

public class GuiController : MonoBehaviour {

    public MultiCarouselsController multiCarouselsController;

    public Text currentCarouselText;
    public Text selectedSlotIndexText;
	
    void Start()
    {
        foreach(var carousel in multiCarouselsController.carousels)
        {
            carousel.onSelectionChanged += UpdateText;
            carousel.onShow += OnCarouselShow;
        }

        var activeCarousel = FindObjectOfType<Carousel>();
        UpdateText(activeCarousel, activeCarousel.selectedSlotIndex);
    }

    void OnCarouselShow(Carousel carousel)
    {
        UpdateText(carousel, carousel.selectedSlotIndex);
    }

    void UpdateText(Carousel carousel, int index)
    {
        currentCarouselText.text = carousel.name;
        selectedSlotIndexText.text = index.ToString();
    }
}

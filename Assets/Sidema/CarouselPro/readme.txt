CarouselPro copyright (c) 2016 Sidéma SPRL
v1.0

Requires DOTween for animations (https://www.assetstore.unity3d.com/en/#!/content/27676)

Create the objects you want to insert in the carousel.
Drop the Carousel prefab in your scene. Add some slots and drag and drop your objects in the fields. That's it.

You can customize several parameters:

Element placement

  Radius                              the radius of the carousel
  Counterclockwise                    distribute objects in counterclockwise fashion

Rotation

  Rotate Carousel                     whether to rotate the carousel when changing selection
  Rotation duration                   the duration of the rotation animation
  Rotation easing                     easing curve for the rotation animation

Slot selection animations

  Slot scaling factor unselected      scaling factor to apply to unselected elements
  Slot scaling factor selected        scaling factor to apply to selected element
  Slot scaling duration               the duration of the scaling animation during transtion between selected and unselected states
  Slot scaling easing                 easing curve for the scaling animation

  Slot offset unselected              offset to apply to the position of unselected elements
  Slot offset selected                offset to apply to the position of selected element
  Slot offset duration                the duration the offset animation during transtion between selected and unselected states
  Slot offset easing                  easing curve for the offseting animation

  Slot rotation speed unselected      speed of rotation of the unselected elements
  Slot rotation speed selected        speed of rotation of the selected element


Visibility transition animations

  Visibility transition duration      the duration of the visibility transition animation (only for UP and DOWN types of transition)
  Show transition curve               curve to use for show transition animation (only for UP and DOWN types of transition)
  Hide transition curve               curve to use for hide transition animation (only for UP and DOWN types of transition)
  Visibility transition ease          easing curve for transition animation (only for UP and DOWN types of transition)

# ImageDithering
GUI for applying the Floyd–Steinberg dithering algorithm to images.

## How It Works
When running *Program.cs*, the user can press the *load Image*-button to choose a desired image file from a directory.
The program will then apply the dithering algoritm to the chosen image and show the result.
By clicking the *Save Image*-button the generated dithered image gets saved to a desired location.

Floyd–Steinberg dithering is an image dithering algorithm first published in 1976 by Robert W. Floyd and Louis Steinberg.
The algorithm achieves dithering using error diffusion, meaning it pushes (adds) the residual quantization error of a pixel onto its neighboring pixels.

The folder *images* contains 4 example images and their dithered result.

## Screenshot
![screenshot](/screenshot.png)

## Author
* Stan Verlaan

## Acknowledgements
For more information about the general idea and pseudocode, see the following Wikipedia page:
* Floyd–Steinberg dithering (https://en.wikipedia.org/wiki/Floyd%E2%80%93Steinberg_dithering)

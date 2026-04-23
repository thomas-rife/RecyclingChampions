# Recycling Champions

## Unity Version

Unity 6.3

## How to run

1. Open Unity Hub
2. Add this project folder
3. Open the project
4. Open the scene in:
   `Assets/Scenes/XR_Test`

## Setup

1. Clone the `MediaPipeWebhook` object detection repo
2. Run the `MediapipeWebhook.py` script through terminal
3. Run the `XR_Test` scene
4. Hold up a supported item such as a water bottle to the camera, the scene should detect the object and react in Unity
5. Unity displays the object name and description
6. Press `1`/`2`/`3`, numpad `1`/`2`/`3`, or `b`/`n`/`m` to choose a trash can
7. When you select, immediate feedback is given on whether you selected the correct one and an arc is drawn from you to the correct bin for throwing
8. The game finishes 20 seconds after selecting

## Notes

- The webhook listener is currently set to `http://127.0.0.1:8080/`, so the detector and Unity app are expected to run on the same machine for this demo setup

## Features

- Trash detection
- Scorekeeping
- Item descriptions and explanations based on the held item
- Parabola drawn from location to correct trash bin
- Arrow indicator

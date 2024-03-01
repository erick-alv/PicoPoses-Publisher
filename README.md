## Description

This repo contains the implementation of the Unity application to publish the poses of the Pico4. It was developed with Unity 2021.3.13f1. You can either build it yourself or find the APK [here](Build/PicoPoses.apk).

## Instructions


Before running this application, the MQTT broker should be already running. You also need to know its IP-adress.

To use the application you have to:
- Start the application in the Pico4
- At the beginning you will see an UI where you can insert the IP-adress of the MQTT broker. In order to interact with it you can use the trigger button of the controllers.
- On the right there is also a panel where you can select the frequency with which the messages are published (36Hz or 72Hz)
- In order to visualize the pose of the controllers and headset we use some semitransparent spheres. In order to deactivate them, you can press the grip button of the right controller.

Now we describe how to interact with the simulation. For this, the program of the simulation should be already running and connected to the MQTT broker.
- In order to begin you should colocate yourself in the default pose described in the paper (standing straight, arms at the side and straight downwards). Then you can press the button 'a' on the right controller and the simulation will start.
- If you want to restart the character of the simulation you can press the button 'b'. Then you should colocate again in the default pose and press 'a' again.



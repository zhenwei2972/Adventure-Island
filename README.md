# Adventure-Island
![](/images/adventure-island-logo.jpg)

Inspired by navigation systems in Games; with 3D Map tiles, users can explore the real world through game like user experiences. Explore unfamiliar cities in a new way that highlights hidden gems around the usual tourist attractions and rewards you for exploring.


# Instructions

Playable android build is available [here](https://drive.google.com/drive/folders/1Nd7OBv_l6md38VjfUcCRLmvWcEmLJTkb?usp=sharing).


Enable developer mode, install the APK on your android device, enable Location permissions and try out the application! 
Ensure that in app settings, Locations is enabled whenever you are using the app, this is required for all the APIs to work

# Notes

- **3rd Party Assets have been removed.** They should be replaced before making a build. 
- **Please replace the API key for access to the Places API and Map Tiles API.** The API key in this repo is deactivated.
- **3D Objects are geoanchored in Singapore.** We've provided a dropdown so you can select different locations in Singapore to "teleport" to.



Use the app to search explore your surroundings, search for nearby places of interest with the integrated search bar made with google places API. 
the app will guide you to help you figure out what's nearby, designed for mobile to make exploring fun, seamless and will scale to any location in the world. 
Helpful data such as Place rating, Name, Image and even straight line haversine distance is displayed.

Discover nearby food places, entertainment locations and even shopping centres near you, a helpful list is displayed on screen simply click on the buttons that appear after a search. 

For the bargain deal hunter, search for promotions that pop up across the map, once you find them, head down to the physical location switch to AR mode and pick up the AR coupon to redeem a nice bargain! 

# Build Instructions
If you intend to make a build please replace the API key with your own Google Cloud API key that has Map Tiles and Places API enabled
Do add your API key at the following locations.

1)
CesiumGeoreference -> Cesium3DTileSet -> URL 
Please append your key with the following format 
https://tile.googleapis.com/v1/3dtiles/root.json?key="YOUR KEY HERE"

2)
In CesiumGeoreference game object, there will be a PointOfInterest component. Please set the apikey string value to your API key.



import { useEffect, useState } from "react";
import * as ScreenOrientation from "expo-screen-orientation";
import { Platform } from "react-native";

export function useScreenOrientation() {
    // orientation stores the current screen orientation.
    // setOrientation updates the orientation state. Initially set to "portrait".
    const[orientation, setOrientation] = useState<"portrait" | "landscape">("portrait");

    useEffect (() => {
        // function to check and update orientation
        const updateOrientation = async () => {
            if (Platform.OS === "web") return;

            try{
                // Asks the device if it’s portrait or landscape.
                const currentOrientation =  await ScreenOrientation.getOrientationAsync(); 

                /*  If screen is portrait (up or down):
                Set state → "portrait".
                unlockAsync() → Allows free rotation. */
                if (currentOrientation === ScreenOrientation.Orientation.PORTRAIT_UP ||
                    currentOrientation === ScreenOrientation.Orientation.PORTRAIT_DOWN) {
                    setOrientation("portrait");
                    await ScreenOrientation.unlockAsync();
                } else if (currentOrientation === ScreenOrientation.Orientation.LANDSCAPE_LEFT ||
                    currentOrientation === ScreenOrientation.Orientation.LANDSCAPE_RIGHT) {
                    setOrientation("landscape");
                    await ScreenOrientation.unlockAsync();
                }
            } catch (error) {
                console.log("Error getting orientation", error);
            }
        };

        // call initially
        updateOrientation();

        /* Listen for orientation changes
            This adds a listener that listens for changes in screen orientation.
            Whenever the user rotates the screen, 
            this function runs, updates the state, and allows rotation. */
        const subscription = ScreenOrientation.addOrientationChangeListener(async (event) => {
            if (Platform.OS === "web") return;

            // 1. addOrientationChangeListener creates a listener for screen rotation
            //      the function is auto executed when the oritation changhes 
            //      cuz this lisneres subscribes to orientation events
            // 2. subscription stores the listener reference. 
            //      X no need to call,as it is a reference/object stores the listener, runs auto efeter changes
            // 3. the listener updats setOrientation when the screen rotates
            // 4. when the component unmounts, remove the listener to prevent memory leaks
            try {
                if (event.orientationInfo.orientation === ScreenOrientation.Orientation.PORTRAIT_UP ||
                    event.orientationInfo.orientation === ScreenOrientation.Orientation.PORTRAIT_DOWN) {
                    setOrientation("portrait");
                    await ScreenOrientation.unlockAsync();
                } else if (event.orientationInfo.orientation === ScreenOrientation.Orientation.LANDSCAPE_LEFT ||
                    event.orientationInfo.orientation === ScreenOrientation.Orientation.LANDSCAPE_RIGHT) {
                    setOrientation("landscape");
                    await ScreenOrientation.unlockAsync();
                }
            } catch (error) {
                console.log("Error getting orientation", error);
            }
        });

        /* 
        This removes the listener when the component unmounts.
        (unmount means the component is removed from the screen and no longer visible)
        If we don't remove the listener, every time the screen changes, 
        a new listener is added and never removed. The app keeps adding more listeners, 
        causing memory leaks and performance issues.
        This wastes memory and slows down the app
        */
        return ()=> {
            // Clean up,  Stops listening when the user navigates away (prevents bugs).
            subscription.remove();
        };
    }, []);  // Empty array to run only once

    return orientation;
}
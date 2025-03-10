// Runs side effects (like starting and stopping the sensor)
import { useEffect } from "react"; 
// The motion sensor that detects acceleration
import { Accelerometer } from "expo-sensors";
/* Used to navigate to another screen (/camera) when shaking
    expo-router uses file-based routing, meaning each screen is automatically a route.
    You don’t need to manually define routes in a separate routes.tsx file like in React Web (react-router-dom).
    Instead, the file structure itself defines the navigation. */
import { useRouter } from "expo-router";
import { Alert } from "react-native";
import * as Haptics from "expo-haptics";

/* This hook listens for shake events using Accelerometer
    navigates to the cameraScreen when a shake is detected 
    ensures that the navigation does not tigger multiple times too frequently
    cleans up when the compoment is unmounted
*/
export default function useShakeDetector() {
    // router is a function that lets us navigate to different pages in our app.
    // Here, it will redirect to /camera when the phone shakes.

    const router = useRouter();

    useEffect (() => {
        // Variables to keep track of the last shake event
        // to prevents multiple triggers in a short time
        let lastUpdated = Date.now();

        // Set a Sensitivity Threshold
        // This is the minimum acceleration required to trigger a shake event.
        // The higher the number, the more force is needed to trigger a shake.
        // If the acceleration is greater than 1.5, we consider it a shake.
        let Threshold = 3;    
        
        //Accelerometer.addListener starts listening for motion data.
        // It provides { x, y, z } → The motion values for the X, Y, and Z axes.
        const subscription = Accelerometer.addListener(({ x, y, z }) => {
            const now = Date.now();
            // This calculates total motion strength in 3D space.
            // If this value is high, the phone is likely shaking.
            const acceleration = Math.sqrt(x * x + y * y + z * z);

            if (acceleration > Threshold && now - lastUpdated > 1000) {
                lastUpdated = now;
                // when phone is shaken, Trigger Haptic Feedback
                // Haptics.impactAsync(Haptics.ImpactFeedbackStyle.Medium); 
                
                // show an alert before navigating to the camera screen
                Alert.alert("Shake detected! ", "Click ok to open camera", [
                    { text: "OK", onPress: () => router.push("/camerascreen") },
                ]);

                // Auto-navigate after 3 seconds if user does nothing
                // setTimeout(() => {
                //     router.push("/camera");
                // }, 3000);
            }
        });

        // cleaning up when component unmounts
        return () => subscription.remove();
    }, []);
}

import { Stack } from "expo-router";
import { LogBox, View, Text, Platform } from "react-native";
import { StatusBar } from "expo-status-bar";
import { DeviceProvider } from "./context/DeviceContext";
import { useEffect, useState } from "react";
import { useScreenOrientation } from "./hooks/useScreenOrientation";
import useShakeDetector from "./hooks/useShakeDetector";

//  Hides all warning messages in the console (useful for development)
LogBox.ignoreAllLogs(true);

export default function RootLayout() {
  
  // Call the useScreenOrientation hook
  const orientaiton = useScreenOrientation();
  // Call the useShakeDetector hook
  useShakeDetector();

  return (
    <>
      <DeviceProvider>
        <StatusBar style="light" />
        <Stack>
          <Stack.Screen name="(tabs)"  options={{ headerShown: false, }} />
          
          <Stack.Screen name="camerascreen" options={{headerShown: false}} />
          <Stack.Screen name="+not-found" options={{}} />
        </Stack>
      </DeviceProvider>
    </>
  );
}

import { Stack } from "expo-router";
import { LogBox, View, Text, Platform } from "react-native";
import { StatusBar } from "expo-status-bar";
import { DeviceProvider } from "./context/DeviceContext";
import { useEffect, useState } from "react";
import { useScreenOrientation } from "./hooks/useScreenOrientation";
import useShakeDetector from "./hooks/useShakeDetector";
import { AuthProvider, useAuth } from "./context/AuthContext";

//  Hides all warning messages in the console (useful for development)
LogBox.ignoreAllLogs(true);

export default function RootLayout() {
  
  // Call the useScreenOrientation hook
  const orientaiton = useScreenOrientation();
  // Call the useShakeDetector hook
  useShakeDetector();

  return (
    
    <AuthProvider> {/* Provides global login/token state */}
      <DeviceProvider> {/* Provides device state */}
        <StatusBar style="light" />
        <MainNavigator />  {/* Decides what screens user sees based on login */}
      </DeviceProvider>
    </AuthProvider>
    
  );
}
// navigation stack for the app based on login state
function MainNavigator() {
  const { token } = useAuth();
  return token? <TabsNavigator /> : <AuthNavigator />;
}

function TabsNavigator() {
  return(
    <Stack>
      <Stack.Screen name="(tabs)"  options={{ headerShown: false, }} />
      <Stack.Screen name="camerascreen" options={{headerShown: false}} />
      <Stack.Screen name="+not-found" options={{}} />
    </Stack>
  );
}

function AuthNavigator() {
  return(
    <Stack>
      <Stack.Screen name="(auth)" options={{headerShown: false}} />
     
    </Stack>
  );
}



// app/camera.tsx
import React, { useState, useEffect } from "react";
import { View, Text, StyleSheet, SafeAreaView, TouchableOpacity, Pressable } from "react-native";
import { CameraView } from "expo-camera";
import { useRouter } from "expo-router";
import  useQRScanner from "@/app/hooks/useQRScanner";
import { Device } from "../types/deviceRelated";
import IconButton from "@/components/IconButton";
import DeviceDetail from "@/components/DeviceDetail";

export default function CameraScreen() {
  const router = useRouter();

  // 1. store the scanned device
  const [device, setDevice] = useState<Device | null>(null);
  
  // 2. Store the camera type (back/front)
  const [type, setType] = useState<"back" | "front">("back");
  
  // 3. state for viewing the device details or not
  const [showDeviceDetail, setShowDeviceDetail] = useState(false);
  
  // 4. If a device is scanned, save the device, and show the device detail modal
  const handleDeviceScanned = (device: Device | null) => {
      if (device ) {
          setDevice(device); // save the device
          setShowDeviceDetail(true); // show the device details modal
        }
  };
  // if modal is close, set device to null and go back to device list
  // const handleModalClose = () => {
  //   setShowDeviceDetail(false);
  //   setDevice(null);
  //   router.push("/device");
  // };

  // 5. Call `useQRScanner`, passing `handleDeviceScanned` as `onScanned` 
  // instead of `setDevice` (which is local to this component).
  const {permission, askPermission, handleScan} = useQRScanner(handleDeviceScanned);


  // 6. Toggle between front/back camera
  const flipCamera = () => {
    setType((prev) => (prev === "back" ? "front" : "back"));
  };

  // debug permission:
  console.log("Camera permission", permission);

  // 7. If still requesting or denied, show a message
  if (permission === null) {
    return (
      <SafeAreaView style={styles.container}>
        <Pressable onPress={askPermission}>
          <Text style={styles.text}>Tap here to request camera permission</Text>
        </Pressable>
        <TouchableOpacity onPress={() => router.back()} style={styles.button}>
          <Text style={styles.buttonText}>Tap here to go back</Text>
        </TouchableOpacity>
      </SafeAreaView>
    );
  }
  if (permission.granted === false) {
    return (
      <SafeAreaView style={styles.container}>
        <Text style={styles.text}>No access to camera</Text>
        <TouchableOpacity onPress={() => router.back()} style={styles.button}>
          <Text style={styles.buttonText}>Tap here to go back</Text>
        </TouchableOpacity>
      </SafeAreaView>
    );
  }

  // 8. Permission granted → Show camera and scanning ui
  return (
    <SafeAreaView style={styles.container}>
      {/*  Header */}
      <View style={styles.header}>
        <Text style={styles.headerText}>QR Scanner</Text>
      </View>

      {/*  Camera feed (Enable this once you're ready) */}
      <View style={styles.cameraContainer}>
        <CameraView 
            style={styles.camera} 
            facing={type} 
            onBarcodeScanned={handleScan} // Call handleScan when QR code is scanned
            barcodeScannerSettings={{ barcodeTypes: ["qr"] }} // Scan only QR codes
        />
      </View>

      {/* Overlay Content */}
      <View style={styles.overlay}>
        <Text style={styles.text}>Scan QR code here</Text>

        <View style={styles.buttonContainer}>
            {/* Back Navigation Button */}
            <IconButton icon="arrow-back-ios" label="Go Back" onPress={() => router.back()} />
            
            {/* Toggle Camera Button */}
            <IconButton icon="flip-camera-ios" label="Flip Camera" onPress={flipCamera} />

        </View>
      </View>

        {/* Show the device detail modal */}
        <DeviceDetail
            isVisible={showDeviceDetail}
            onClose={() => 
              {setShowDeviceDetail(false); 
              setDevice(null)}}
            device={device}
        /> 
    </SafeAreaView>
  );
}

// Styling
const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#25292e", // Keeps the same dark background style
    alignItems: "center",
    justifyContent: "center",
  },
  header: {
    paddingVertical: 10,
    alignItems: "center",
    justifyContent: "center",
    zIndex: 2, // ✅ Ensure the header is above the camera
  },
  headerText: {
    color: "white",
    fontSize: 18,
    fontWeight: "bold",
    borderRadius: 8,
  },
  cameraContainer: {
    flex: 1, // ✅ Ensures camera takes full space but respects header
    width: "100%",
  },
  camera: {
    flex: 1, // ✅ Allows the camera to fill the available space without covering header
  },
  overlay: {
    position: "absolute",
    bottom: 50,
    width: "90%",
    alignSelf: "center",
    backgroundColor: "rgba(36, 35, 35, 0.6)",
    padding: 15,
    borderRadius: 8,
  },
  text: {
    color: "white",
    fontSize: 18,
    marginVertical: 5,
    textAlign: "center",
  },
  buttonContainer: {
    flexDirection: "row",
    justifyContent: "space-between",
  },
  button: {
    backgroundColor: "#444",
    paddingVertical: 10,
    paddingHorizontal: 20,
    borderRadius: 5,
    marginVertical: 5,
  },
  buttonText: {
    color: "white",
    fontSize: 16,
    textAlign: "center",
  },
});


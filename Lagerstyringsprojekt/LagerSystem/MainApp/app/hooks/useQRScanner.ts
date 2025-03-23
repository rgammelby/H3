import { useState, useEffect, useContext } from "react"; 
import { useCameraPermissions, BarcodeScanningResult } from "expo-camera";
import { DeviceContext } from "@/app/context/DeviceContext"; //  Use the global device list
import { Alert } from "react-native";
import { Device } from "@/app/types/deviceRelated";

export default function useQRScanner(onScanned: (device: Device | null) => void) {
    // 1. ask for camera permission on 1st load
    // Returns both permission & function to request it
    // so the camerascreen can decide when to request it.
    const [permission, askPermission] = useCameraPermissions();
    
    // 2. keep track of scaning state
    // Set to true immediately after scanning, so we don’t scan multiple times too fast.
    // After 3 seconds, it resets to false (so the user can scan again).
    const [scanned, setScanned] = useState<boolean>(false);
    const [lastScanned, setLastScanned] = useState<string | null>(null);
    
    // 3. access the global device list
    // tells TypeScript that deviceList contains an array of Device objects, fixing the type issue
    const { deviceList }  = useContext(DeviceContext) as { deviceList: Device[] };


     // Explicitly ask for permission when component mounts
     useEffect(() => {
        if (!permission || !permission.granted) {
            console.log("Requesting camera permission...");
            askPermission();
        }
    }, [permission]);

    console.log("permission", permission);

    // 4. Handle scanning a qr code:
    // This function runs whenever the user scans a QR code.
    // It receives a BarcodeScanningResult object, which contains two properties: type and data.
    // type → The format of the scanned code (e.g., QR, barcode).
    // data → The actual QR code content: the device id.
    const handleScan =({ type, data }: { type: string; data: string }) => {
        
        // If a scan just happened (scanned === true), ignore this one.
        // If the QR code is the same as the last one, ignore it.
        if (scanned || data === lastScanned) return; // ignore duplicated scan

        setScanned(true);
        setLastScanned(data);

        console.log("Scanned:", data);
        // Find the device by the QR code
        const matchedDevice = deviceList.find((device: Device) => device.id.toString() === data);

        // If a matching device is found, call onScanned(matchedDevice).
        // onScanned was passed from camera.tsx, so now the screen updates the UI.
        if (matchedDevice) {
            onScanned(matchedDevice);
            console.log("Device Found:", matchedDevice);

        } else {
            Alert.alert("Device Not Found", "The QR code does not match any device in the database.");
            onScanned(null);
        }

        // Waits 3 seconds, then resets scanned = false.
        // This allows the user to scan another QR code.
        setTimeout(() => setScanned(false), 3000);
  };

  return { permission, askPermission, handleScan };
}

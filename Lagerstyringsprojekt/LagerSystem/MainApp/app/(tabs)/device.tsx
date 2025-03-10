import React, { useContext, useState } from "react";
import { View, Text, TextInput, ScrollView, SafeAreaView, ActivityIndicator, StyleSheet, TouchableOpacity } from "react-native";
import { DeviceContext } from "../context/DeviceContext";
import DeviceOverviewImage from "@/components/DeviceOverviewImage";
import DeviceDetail from "@/components/DeviceDetail";
import { Device } from "@/app/types/deviceRelated";

export default function DeviceOverviewScreen() {
  const context = useContext(DeviceContext);

  // If context is not ready (maybe the API is still loading)
  // shows a loading spinner (<ActivityIndicator />).
  if (!context) {
    return <ActivityIndicator />;
  }

   // Group items into pairs for 2 per row
   // cretaes an empty list and fills it with pairs of devices
   // context.deviceList = [ A, B, C, D, E ];
   // groupedDevices = [ [A, B], [C, D], [E] ];
   const groupedDevices = [];
   for (let i = 0; i < context.deviceList.length; i += 2) {
     groupedDevices.push(context.deviceList.slice(i, i + 2));
   }

   // Track selected device for modal
   const [showDeviceDetail, setShowDeviceDetail] = useState<Device | null> (null);

  return (
    <SafeAreaView style={styles.safeArea}>
    <View style={styles.container}>
      <Text style={styles.header}>All Devices</Text>

      <ScrollView>
        {groupedDevices.map((pair, index) => (
          <View key={index} style={styles.row}>
            {/* Render each pair of devices.
              Creates a "card" (View) for each device.
              Uses key={item.id} → Unique identifier for React to track devices.*/}
            {pair.map((item) => (
              
              <TouchableOpacity 
                key={item.id} 
                style={styles.card}
                onPress={() => setShowDeviceDetail(item)} // Open modal on press
              >
                {/* Device Image */}
                <DeviceOverviewImage imageUri={item.deviceOverviewDetail?.image || "Unknown"}  />

                {/* Device model name */}
                <Text style={styles.modelText}>{item.id} {item.deviceOverviewDetail?.model}</Text>

                <View style={styles.deviceDetails}>
                  {/* Device type with gray background and rounded corners*/}
                  <View style={styles.deviceTypeContainer}>
                    <Text style={styles.deviceType}>
                      {item.deviceOverviewDetail?.deviceTypeDetail?.type_name || "Unknown"}
                    </Text>
                  </View>
                  
                  {/* Device status */}
                  <View >
                  

                  <View style={[styles.availabilityDot, {
                     backgroundColor: item.statusDetail?.status_type?.toLowerCase() === "available" ? 'green' : 'red' 
                  }]} />
                  </View>
                 
                </View>
              </ TouchableOpacity>
            ))}
            {/* If there is an odd number of items, fill the space */}
            {pair.length === 1 && <View style={styles.emptyCard} />}
          </View>
        ))}
      </ScrollView>
    </View>
    
    {/* Device Detail Modal - Uses separate DeviceDetail component */}
    <DeviceDetail
        isVisible={!!showDeviceDetail}
        // !!showDeviceDetail:
        // If showDeviceDetail = null, it returns false (modal is hidden).
        // If showDeviceDetail = item, it returns true (modal is visible).
        onClose={() => setShowDeviceDetail(null)}
        device={showDeviceDetail}
      />
  </SafeAreaView>
);
}

// ✅ Styles for Grid Layout
const styles = StyleSheet.create({
safeArea: {
  flex: 1,
  backgroundColor: "#f8f8f8",
},
container: {
  flex: 1,
  padding: 15,
},
header: {
  fontSize: 22,
  fontWeight: "bold",
  textAlign: "center",
  marginBottom: 15,
},
row: {
  flexDirection: "row",
  justifyContent: "space-between",
  marginBottom: 10,
},
card: {
  width: "48%", // Almost half of the screen with spacing
  backgroundColor: "#fff",
  padding: 10,
  borderRadius: 10,
  shadowColor: "#000",
  shadowOpacity: 0.1,
  shadowRadius: 5,
  elevation: 3, // Android shadow
  alignItems: "center",
},
emptyCard: {
  width: "48%", // Keeps alignment when there is an odd number of items
  backgroundColor: "transparent",
},
modelText: {
  fontSize: 16,
  fontWeight: "bold",
  textAlign: "center",
},
deviceDetails: {
  flexDirection: "row",
  alignItems: "center", // Align items vertically
  justifyContent: "space-between",
  width: "100%",
  marginTop: 5,
},
//  Gray background with rounded corners for device type
deviceTypeContainer: {
  backgroundColor: "#e0e0e0",
  paddingVertical: 4,
  paddingHorizontal: 8,
  borderRadius: 8, // Rounded corners
},
deviceType: {
  fontSize: 14,
  color: "#555",
  fontWeight: "bold",
},
// Status with colored dot (Green for Available, Red for Not Available)

availabilityDot: {
  width: 20,
  height: 20,
  borderRadius: 20, // Circle shape
  marginRight: 5,
},

});
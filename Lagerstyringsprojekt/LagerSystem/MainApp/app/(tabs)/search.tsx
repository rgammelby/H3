import Reacr, { useContext, useState } from "react";
import { View, Text, TextInput, ScrollView, FlatList, ActivityIndicator, StyleSheet } from "react-native";
import { DeviceContext } from "../context/DeviceContext";

export default function SearchScreen() {
  const context = useContext(DeviceContext);

  if (!context) {
    return <ActivityIndicator />;
  }

  return(
    <View style={{flex:1, padding: 20}}>
      <Text style={{fontSize:22, fontWeight:"bold"}}>All Devices</Text>
      
      <FlatList
        data={context.deviceList}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <View style={{
            padding: 10,
            borderBottomWidth: 1,
            borderBottomColor: "#ccc",  }}
          >
           <Text style={{ fontWeight: "bold" }}>Model: {item.deviceOverviewDetail?.model ?? "Unknown"}</Text>
                        <Text>Status: {item.statusDetail?.status_type ?? "Unknown"}</Text>
                        <Text>Stored in: {item.locationDetail?.designation ?? "Unknown Cupboard"}</Text>
                        <Text>Room: {item.locationDetail?.roomDetail?.designation ?? "Unknown Room"}</Text>
          </View>
        )}
      />
    </View>
  )
}


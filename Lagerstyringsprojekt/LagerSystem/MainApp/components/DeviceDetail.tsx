import { Modal, View, Text, Pressable, StyleSheet, TouchableOpacity, Alert } from "react-native";
import MaterialIcons from "@expo/vector-icons/MaterialIcons";
import { PropsWithChildren, useState, useContext } from 'react';
import DeviceOverviewImage from './DeviceOverviewImage';
import { Device } from "@/app/types/deviceRelated";
import DateTimePicker from "@react-native-community/datetimepicker";
import { Platform } from "react-native";

import { createBorrowActivity } from "@/app/services/activityService"; 

import { DeviceContext } from "@/app/context/DeviceContext";
import { useAuth } from "@/app/context/AuthContext"; //  Auth context
import { AddActivity } from "@/app/types/activityRelated"; // The interface

type Props = PropsWithChildren<{
  isVisible: boolean;
  onClose: () => void;
  device: Device | null;
}>;

export default function ModalView({ isVisible, onClose, device }: Props) {
   
    if (!device) return null; // Prevent rendering if no device is selected

    // Get the device context, use to update devicelist if borrow is successful
    const deviceContext = useContext(DeviceContext);

    
    const today = new Date();
    // endDate starts as null, meaning the condition if (endDate) is false.
    // This ensures that only the "Borrow" button appears at first.
    const [endDate, setEndDate] = useState<Date | null>(null);;
    const [showPicker, setShowPicker] = useState(false);

    const { user } = useAuth(); // Get the user from the context

    const handleDateChange = (event: any, selectedDate?: Date) => {
        if (event.type === "dismissed") {
            // If the user cancels the picker, reset state
            setShowPicker(false);
            return;
        }
    
        if (selectedDate && selectedDate > new Date()) {
            setEndDate(selectedDate);
            setShowPicker(false); // Hide the picker after selection

        } else {
            Alert.alert("Invalid Date", "Please select a future date");
            setShowPicker(true); // Show the picker again
        }
    };

    async function handleConfirmBorrow() {
       try {
        if (!user) { 
            Alert.alert("Error", "No user logged in");
            return;
        }
           
        if (!endDate) {
            Alert.alert("Error", "No end date selected");
            return;
        }
        
        if (!device) {
            Alert.alert("Error", "No device selected");
            return;
        }

        // build addActivity object
        const addActivity: AddActivity = {
            user_id: user.id,
            device_id: device.id,
            activity_type: 1, // Borrow
            start_date: today,
            end_date: endDate,
            created_at: today,
            notes: "Borrowing device",
        };

        // Call service function to create the new borrow activity
        await createBorrowActivity(addActivity);

        if (!deviceContext) {
            // The context is not ready yet—maybe return or do something else
            console.log("DeviceContext is null. Not calling fetchDevices.");
            return;
        }
    
        // refresh the device list
        deviceContext.fetchDevices();
        
        // If successful, show a message and close the modal
        Alert.alert("Success", "Device borrowed successfully!");
        onClose();
      } catch (err) {
        console.error("Borrow failed:", err);
        Alert.alert("Error", "Could not borrow device. Please try again.");
      }
    }

    return (
        <Modal animationType="slide" transparent={true} visible={isVisible} onRequestClose={onClose}>
            <View style={styles.modalContainer}>
                
                <View style={styles.titleContainer}>
                    <Text style={styles.title}>Borrow Device</Text>
                    <Pressable onPress={onClose}>
                        <MaterialIcons name="close" size={22} color="#fff" />
                    </Pressable>
                 </View>

                 {/* Device Details */}
                 <View style={styles.contentContainer}>
                    {/* Device Image */}
                    <DeviceOverviewImage 
                        imageUri={device.deviceOverviewDetail?.image || "Unknown"} 
                        style = {{width: 180, height: 180}}
                    />
                    
                    {/* Device ModelName */}
                    <Text style={styles.deviceName}>{device.deviceOverviewDetail?.model}</Text>
                    {/* Category Label */}
                    <View style={styles.categoryLabel}>
                        <Text style={styles.categoryText}>📂 {device.deviceOverviewDetail?.deviceTypeDetail?.type_name || "Unknown"}</Text>
                    </View>
                    
                    <Text style={styles.deviceInfo}>📍 Location: {device.locationDetail?.roomDetail?.designation || "Unknown"}, {device.locationDetail?.designation || "Unknown"}</Text>
                    <Text style={styles.deviceInfo}>📦 Status: {device.statusDetail?.status_type || "N/A"}</Text>
                
                    {/* Displaying Start and End Dates (Only after selecting a return date) 
                    The && condition means this only renders when endDate is selected.*/}
                    {endDate &&(
                        <View style={styles.dateContainer}>
                            <Text style={styles.deviceInfo}>📅 Start Date: {today.toDateString()}</Text>
                            <Text style={styles.deviceInfo}>📅 Return Date: {endDate.toDateString()}</Text>
                            
                            <View style={styles.buttonContainer}>
                                <TouchableOpacity 
                                    style={[styles.borrowButton, { backgroundColor: "green" }]} 
                                    onPress={() => {
                                        handleConfirmBorrow();
                                        onClose();
                                    }}
                                    >
                                    <Text style={styles.borrowButtonText}>Confirm</Text>
                                </TouchableOpacity>

                                <TouchableOpacity style={[styles.borrowButton, { backgroundColor: "red" }]} onPress={() => setEndDate(null)}>
                                    <Text style={styles.borrowButtonText}>Cancel</Text>
                                </TouchableOpacity>
                            </View>
                        </View>
                    )}
                </View>
                    {/* Show Borrow Button Initially */}
                    {!endDate && (
                        <TouchableOpacity
                            style={[
                                styles.borrowButton,
                                { backgroundColor: device.statusDetail?.status_type?.toLowerCase() === "available" ? "green" : "gray" },
                            ]}
                            disabled={device.statusDetail?.status_type?.toLowerCase() !== "available"} // Disable if not available
                            onPress={() => setShowPicker(true)}
                        >
                        <Text style={styles.borrowButtonText}>{"Borrow"}</Text>
                        </TouchableOpacity>
                    )}
                
                    {/* Show date picker only when borrow button clicked */}
                    {showPicker && (
                        <DateTimePicker
                            value={today}
                            mode="date"
                            display={Platform.OS === "ios" ? "spinner" : "default"}
                            minimumDate={new Date()}
                            onChange={handleDateChange}         
                        />
                    )}
            </ View>
        </Modal>
    );
}

const styles = StyleSheet.create({
    modalContainer: {
        flex: 1,
        alignItems: "center", // Centers horizontally
        flexDirection: "column",
        backgroundColor: '#fff',
        justifyContent: "flex-start", // Align content to the top
        paddingTop: 100, // Ensure some spacing from the top
        top: 50,
    },
    titleContainer: {
        height: '10%',
        width : '100%',
        position: 'absolute',
        top: 0,
        backgroundColor: '#464C55',
        borderTopRightRadius: 10,
        borderTopLeftRadius: 10,
        paddingHorizontal: 20,
        flexDirection: 'row',
        alignItems: 'center',
        justifyContent: 'space-between',
    },
    contentContainer: {
        backgroundColor: '#F6F6F6',
        padding: 20,
        borderRadius: 10,
        alignItems: 'center',
        width: '90%',
        justifyContent: "center",
        maxWidth: 400, // Prevents it from being too wide on larger screens
    },
    deviceName: {
        fontSize: 20,
        fontWeight: "bold",
        marginTop: 10,
        color: "#464C55",
    },
    categoryLabel: {
        backgroundColor: "#707070",
        paddingVertical: 5,
        paddingHorizontal: 10,
        borderRadius: 15,
        marginTop: 10,
    },
    categoryText: {
        color: '#fff',
        fontSize: 16,
    },
    deviceInfo: {
        fontSize: 16,
        marginTop: 5,
    },
    title: {
        color: '#fff',
        fontSize: 20,
    },
    borrowButton: {
        paddingVertical: 8,
        paddingHorizontal: 16,
        borderRadius: 6,
        marginTop: 10,
    },
    borrowButtonText: {
        color: "white",
        fontWeight: "bold",
        fontSize: 16,
    },
    dateContainer: {
        marginTop: 50,
        marginBottom: 15,
        alignItems: 'center',
    },
    buttonContainer: {
        flexDirection: "row",
        justifyContent: "space-between",
        width: "60%",
        marginTop: 10,
        paddingHorizontal: 10, // Adds some padding inside the container
    },
});

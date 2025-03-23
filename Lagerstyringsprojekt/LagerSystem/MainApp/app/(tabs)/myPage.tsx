import React, { useEffect, useState, useCallback, useContext } from "react";
import { useFocusEffect } from "@react-navigation/native";
import { Text, View, Modal, StyleSheet, SafeAreaView, TouchableOpacity, ScrollView} from "react-native";
import { useAuth} from "../context/AuthContext";
import { AddActivity, Activity } from "../types/activityRelated";
import { GetActivitiesByUserId, GetAllActivityTypes } from "../services/activityService";
import { returnDevice } from "../services/activityService";

import { DeviceContext } from "@/app/context/DeviceContext";

export default function MyPageScreen(){
  const { user, logout, updateUser } = useAuth();  // Get user and logout function from context
  const [activities, setActivities] = useState<Activity[]>([]);

  // Get the device context, use to get device info
  const deviceContext = useContext(DeviceContext);

  // Load the user’s activities and types when the screen mounts or when `user` changes
  // not call await GetActivitiesByUserId(user.id)
  // because useEffect() cannot be async
  // so make a wrapper function inside useEffect()
  // and call that function
  useEffect(() => {
    fetchActivitiesAndTypes();
  }, [user]);

  // pack await method in a function, as useEffect() cannot be async
  // fetch all activities and types, and merge with activities
  async function fetchActivitiesAndTypes(){
    try {
      // fetch both activities and types at the same time
      if (user) {
        const [fetchedActivities, fetchedTypes] = await Promise.all([
          GetActivitiesByUserId(user.id),
          GetAllActivityTypes()
        ]);

        // merge the activity types into the activities
        if (fetchedActivities && fetchedTypes) {
          const enrichedActivities = fetchedActivities.map(activity => ({
            ...activity,
            activityDetail: fetchedTypes.find(type => type.id === activity.activity_type),
          }));

          console.log("Enriched activities:", enrichedActivities);
          // Store merged array in state
          setActivities(enrichedActivities);
        }
      }
    } catch (error) {
      console.error("Failed to fetch activities and types:", error);
    }
  }

  
  //  a grouping function to group activities by lifecycleId
  //  iterate over each activity.
  //  use activity.lifecycle_id as the key in an object (group).
  //  Each key points to an array of all activities that share that lifecycle_id.
  function groupByLifecycleId(activities: Activity[]): Record<string, Activity[]> {
    return activities.reduce((groups, activity) => {
      const key = activity.lifecycle_id;
      if (!groups[key]) {
        groups[key] = [];
      }
      groups[key].push(activity);
      return groups;
    }, {} as Record<string, Activity[]>);
  }
  
  // Filter ongoing borrow anf history based on lifecycleId AND activity status
  const grouped = groupByLifecycleId(activities); // returns an object
  const groupArrays = Object.values(grouped);   // array of arrays

  // Partition them into two arrays
  const ongoingGroups = groupArrays.filter(
    (group) => !group.some((act) => act.activity_type === 2)
  );

  const historyGroups = groupArrays.filter(
    (group) => group.some((act) => act.activity_type === 2)
  );

  // for return, pack the asyn method in activitySerivce in a function
  async function handleReturn(activity: Activity)  {
    try {
      await returnDevice(activity);
      await fetchActivitiesAndTypes();

      if (!deviceContext) {
        console.error("deviceContext is null. Cannot fetch devices.");
        return;
      }
      // refresh the device list
      deviceContext.fetchDevices();

    } catch (err) {
      console.error("Return failed:", err);
      alert("Could not return device. Please try again.");
    }
  }

  useFocusEffect(
    useCallback(() => {
      fetchActivitiesAndTypes();
    }, [])
  );


  return (
      <SafeAreaView style={styles.safeArea}>
        <ScrollView contentContainerStyle={styles.scroll}>
        <View style={styles.container}>
          
            {/* Profile Card */}
          <View style={styles.card} > 
            <Text style={styles.cardTitle}>My Profile</Text>
            <Text style={styles.text}>Name: {user?.first_name } {user?.last_name}</Text>
            <Text style={styles.text}>Email: {user?.email}</Text>
            <Text style={styles.text}>Phone: {user?.telephone}</Text>

            <View style={styles.buttonRow}>
            
              {/* Edit Button */}
              <TouchableOpacity
                style={[styles.customButton, { backgroundColor: "#4CAF50" }]}
                onPress={() => alert("Edit Profile")}
              >
                <Text style={styles.customButtonText}>EDIT PROFILE</Text>
              </TouchableOpacity>

              {/* Logout Button */}
              <TouchableOpacity
                style={[styles.customButton, { backgroundColor: "#4CAF50" }]}
                onPress={logout}
              >
                <Text style={styles.customButtonText}>LOGOUT</Text>
              </TouchableOpacity>

            </View>
          </View>
        </View>

        <View style={styles.container}>
          
          {/* Activities Card */}
          <View style={styles.card}>

            <Text style={styles.cardTitle}>My Records</Text>
             
             {/* Ongoing Borrows */}
            <View style={styles.section}>
              <Text style={styles.sectionTitle}>Ongoing Borrows</Text>

              {ongoingGroups.length === 0 ? (
                <Text>No ongoing borrows.</Text>
              ) : (
                ongoingGroups.map((group, idx) => {
                  // 1) Find the borrow activity in this group
                  const borrowActivity = group.find((a) => a.activity_type === 1);
                  if (!borrowActivity) {
                    // If there's no borrow record, skip
                    return null;
                  }

                  // 2) Look up the device from deviceContext
                  const device = deviceContext?.deviceList.find(
                    (d) => d.id === borrowActivity.device_id
                  );
                  const deviceName = device?.deviceOverviewDetail?.model || "Unknown Model";
                  const deviceType =
                    device?.deviceOverviewDetail?.deviceTypeDetail?.type_name ||
                    "Unknown Type";
                  
                  // 3) Format the dates
                  const startDate = new Date(borrowActivity.start_date).toLocaleString();
                  const endDate = new Date(borrowActivity.end_date).toLocaleString();

                  return (
                    <View key={idx} style={styles.activityItem}>
                      <Text style={styles.cardTitle}>
                        {borrowActivity.device_id} {deviceName} ({deviceType})
                      </Text>
                      <Text>Activity ID: {borrowActivity.id}</Text>
                      <Text>Start: {startDate}</Text>
                      <Text>End: {endDate}</Text>
                      <Text>Notes: {borrowActivity.notes}</Text>

                      <TouchableOpacity
                        style={styles.returnButton}
                        onPress={() => handleReturn(borrowActivity)}
                      >
                        <Text style={styles.returnButtonText}>Return</Text>
                      </TouchableOpacity>
                    </View>
                  );
                })
              )}
            </View>


              {/* History */}
              <View style={styles.section}>
                <Text style={styles.sectionTitle}>History</Text>

                {historyGroups.length === 0 ? (
                  <Text>No past history.</Text>
                ) : (
                  historyGroups.map((group, idx) => {
                    const borrowActivity = group.find(a => a.activity_type === 1);
                    const returnActivity = group.find(a => a.activity_type === 2);
                    if (!borrowActivity) return null;

                    const device = deviceContext?.deviceList.find(
                      d => d.id === borrowActivity.device_id
                    );
                    const deviceName = device?.deviceOverviewDetail?.model || "Unknown Model";
                    const deviceType = device?.deviceOverviewDetail?.deviceTypeDetail?.type_name || "Unknown Type";

                    const borrowDate = new Date(borrowActivity.start_date).toLocaleString();
                    const dueDate = new Date(borrowActivity.end_date).toLocaleString();
                    const returnDate = returnActivity
                      ? new Date(returnActivity.end_date).toLocaleString()
                      : "N/A";

                    return (
                      <View key={idx} style={styles.activityItem}>
                        <Text style={styles.cardTitle}>
                          {borrowActivity.device_id} {deviceName} ({deviceType})
                        </Text>
                        <Text>Borrowed On: {borrowDate}</Text>
                        <Text>Due Date: {dueDate}</Text>
                        <Text>Returned On: {returnDate}</Text>
                        {/* Optionally show notes */}
                      </View>
                    );
                  })
                )}
              </View>
                        
          </View>
        </View>
        </ScrollView> 
      </SafeAreaView>
        
    );
}

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
      text:{
        color: "#000",
        marginBottom: 20,
        fontSize: 18,
        alignContent: "flex-start",
    },
    card: {
      backgroundColor: "#fff",
      padding: 20,
      borderRadius: 10,
      shadowColor: "#000",
      shadowOpacity: 0.1,
      shadowOffset: { width: 0, height: 2 },
      shadowRadius: 5,
      elevation: 5,
      marginBottom: 20,
    },
    cardTitle: {
      fontSize: 18,
      fontWeight: "bold",
      marginBottom: 10,
      textAlign: "center",
      padding: 10
    },
    profileContainer: {
      width: "100%",
      maxWidth: 400,
      backgroundColor: "#FFFFFF",
      padding: 20,
      borderRadius: 8,
  
      // Add a slight shadow on iOS, or elevation on Android
      shadowColor: "#000",
      shadowOpacity: 0.1,
      shadowRadius: 4,
      elevation: 2,
  
      alignItems: "center",
    },
    profileTitle: {
      fontSize: 18,
      fontWeight: "bold",
      textAlign: "center",
      marginBottom: 20,
    },
    buttonRow: {
      flexDirection: "row",
      justifyContent: "space-between",
      width: "90%",
      marginVertical: 5,
      alignItems: 'center',
    },
    customButton: {
      paddingVertical: 12,
      paddingHorizontal: 20,
      borderRadius: 5,
      marginRight: 10,
    },
    customButtonText: {
      color: "#fff",
      fontWeight: "bold",
    },  
    scroll: {
      
      backgroundColor: '#f8f8f8',
    },
    section: {
      marginBottom: 20,
      backgroundColor: "#fff",
      padding: 10,
      borderRadius: 8,
      // etc.
    },
    sectionTitle: {
      fontSize: 18,
      fontWeight: "bold",
      marginBottom: 10,
    },
    activityItem: {
      backgroundColor: "#fff",
      marginVertical: 10,
      padding: 15,
      borderRadius: 8,
      shadowColor: "#000",
      shadowOffset: { width: 0, height: 2 },
      shadowOpacity: 0.2,
      shadowRadius: 4,
      elevation: 3,
    },
    returnButton: {
      paddingVertical: 8,
      paddingHorizontal: 16,
      borderRadius: 6,
      marginTop: 10,
      // Add a background color so your white text is visible
      backgroundColor: "#4CAF50",
    },
    returnButtonText: {
      color: "#fff",
      fontWeight: "bold",
      fontSize: 16,
    },
});
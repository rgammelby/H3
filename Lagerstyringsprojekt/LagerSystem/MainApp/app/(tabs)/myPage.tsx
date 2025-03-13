import React, { useContext } from "react";
import { Text, View, StyleSheet, SafeAreaView, TouchableOpacity,} from "react-native";
import { useAuth} from "../context/AuthContext";

export default function MyPageScreen(){
  const { user, logout } = useAuth();  // Get user and logout function from context

  console.log("User in MyPageScreen: ", user);
  return (
      <SafeAreaView style={styles.safeArea}>
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
          
          {/* User Info */}
          <View style={styles.card}>
            <Text style={styles.cardTitle}>My borrow</Text>
            <Text style={styles.text}>Name: {user?.first_name } {user?.last_name}</Text>
            <Text style={styles.text}>Email: {user?.email}</Text>
            <Text style={styles.text}>Phone: {user?.telephone}</Text>

          {/* Edit Button */}
          
          </View>
        </View>
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
      fontSize: 20,
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
        
});
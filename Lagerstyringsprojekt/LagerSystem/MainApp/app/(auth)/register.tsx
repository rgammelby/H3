import React, { useState } from "react";
import { 
  Text, View, StyleSheet, SafeAreaView, 
  Alert, TextInput, TouchableOpacity,
} from "react-native";
import { useRouter } from "expo-router"; // If you're using expo-router
import { useAuth } from "../context/AuthContext"; // The same hook from your code

export default function RegisterScreen() {
  // Get the register function from AuthContext
  const { register } = useAuth();
  const router = useRouter();

  // Local state for form fields
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName]   = useState("");
  const [email, setEmail]         = useState("");
  const [password, setPassword]   = useState("");
  const [telephone, setTelephone] = useState("");

  // Called when user presses "Register" button
  async function handleRegister() {
    try {
      // Basic validation
      if (!firstName || !lastName || !email || !password || !telephone) {
        Alert.alert("Error", "Please fill in all required fields.");
        return;
      }

      // Build the user registration object
      const userInfo = {
        first_name: firstName,
        last_name: lastName,
        email,
        password,
        telephone,
      };

      // Call the register function from AuthContext
      await register(userInfo);

      Alert.alert("Success", "Registration successful!");
      // Optionally navigate to login or auto-login
      router.push("/(auth)/login");
    } catch (err) {
      console.error("Registration failed:", err);
      Alert.alert("Error", "Could not register user. Please try again.");
    }
  }

  function handleCancel() {
    // Clear fields and navigate away
    setFirstName("");
    setLastName("");
    setEmail("");
    setPassword("");
    setTelephone("");
     
    router.push("/(auth)/login");
  }

  return (
    <SafeAreaView style={styles.safeArea}>
      <View style={styles.container}>
        {/* Header text */}
        <Text style={styles.title}>Register</Text>

        {/* Form container */}
        <View style={styles.formContainer}>
          <TextInput
            style={styles.input}
            placeholder="First Name"
            value={firstName}
            onChangeText={setFirstName}
          />
          <TextInput
            style={styles.input}
            placeholder="Last Name"
            value={lastName}
            onChangeText={setLastName}
          />
          <TextInput
            style={styles.input}
            placeholder="Email"
            value={email}
            onChangeText={setEmail}
            autoCapitalize="none"
          />
          <TextInput
            style={styles.input}
            placeholder="Password"
            value={password}
            onChangeText={setPassword}
            secureTextEntry
          />
          <TextInput
            style={styles.input}
            placeholder="Telephone"
            value={telephone}
            onChangeText={setTelephone}
            keyboardType="phone-pad"
          />

          <View style={styles.buttonRow}>
            <TouchableOpacity
              style={[styles.customButton, { backgroundColor: "#4CAF50" }]}
              onPress={handleRegister}
            >
              <Text style={styles.customButtonText}>REGISTER</Text>
            </TouchableOpacity>

            <TouchableOpacity
              style={[styles.customButton, { backgroundColor: "gray" }]}
              onPress={handleCancel}
            >
              <Text style={styles.customButtonText}>CANCEL</Text>
            </TouchableOpacity>
          </View>
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
    paddingTop: 40,
    flex: 1,
    justifyContent: "flex-start",
    alignItems: "center",
    padding: 20,
  },
  title: {
    fontSize: 26,
    fontWeight: "bold",
    marginBottom: 20,
  },
  formContainer: {
    width: "100%",
    maxWidth: 400,
    backgroundColor: "#FFFFFF",
    padding: 20,
    borderRadius: 8,
    shadowColor: "#000",
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 2,
    alignItems: "center",
  },
  input: {
    width: "100%",
    padding: 10,
    marginVertical: 8,
    backgroundColor: "#fff",
    borderWidth: 1,
    borderColor: "#ccc",
    borderRadius: 5,
  },
  buttonRow: {
    flexDirection: "row",
    justifyContent: "space-between",
    width: "90%",
    marginVertical: 5,
    alignItems: "center",
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

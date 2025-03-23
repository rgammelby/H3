import React, { useState } from "react";
import { 
  Text, View, Button, StyleSheet, 
  SafeAreaView, Alert, TextInput, TouchableOpacity,
} from "react-native";
import { useRouter } from "expo-router";
import { useAuth } from "../context/AuthContext";

export default function LoginScreen() {
  const { login } = useAuth();
  const router = useRouter();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleLogin = async () => {
    const success = await login({ email, password });
    if (success) {
      Alert.alert("Login successful!");
      router.push("/(tabs)");
    } else {
      Alert.alert("Login failed", "Please check your email and password");
    }
  };

  const handleCancel = () => {
    setEmail("");
    setPassword("");
    // Optionally navigate away or just clear fields
  };

  const handleForgotPassword = () => {
    Alert.alert("Forgot Password", "Here is the forgot password flow here!");
  };

  const handleGoToRegister = () => {
    router.push("/(auth)/register");
  };

  return (
    <SafeAreaView style={styles.safeArea}>
      <View style={styles.container}>
        {/* Header text */}
        <Text style={styles.title}>Login</Text>
        
        {/* Form container */}
        <View style={styles.formContainer}>
         
          <TextInput
            style={styles.input}
            placeholder="Email"
            value={email}
            onChangeText={setEmail}
          />
          <TextInput
            style={styles.input}
            placeholder="Password"
            value={password}
            onChangeText={setPassword}
            secureTextEntry
          />
         {/* Forgot Password link (touchable text) */}
            <TouchableOpacity onPress={handleForgotPassword} style={styles.forgotPasswordContainer} >
            <Text style={styles.forgotPasswordText}>Forgot Password?</Text>
          </TouchableOpacity>

          {/* login and cancel */}
          <View style={styles.buttonRow}>
            <TouchableOpacity
                style={[styles.customButton, { backgroundColor: "#4CAF50" }]}
                onPress={handleLogin}
            >
                <Text style={styles.customButtonText}>LOGIN</Text>
            </TouchableOpacity>
            <TouchableOpacity
                style={[styles.customButton, { backgroundColor: "gray" }]}
                onPress={handleCancel}
            >
                <Text style={styles.customButtonText}>CANCEL</Text>
            </TouchableOpacity>
          </View>

          {/* Register link */}
          <TouchableOpacity onPress={handleGoToRegister}>
            <Text style={styles.registerLink}>New user? Register here</Text>
          </TouchableOpacity>
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
     // Move form towards the top
    paddingTop: 40,
    flex: 1,
    // center content
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

    // Add a slight shadow on iOS, or elevation on Android
    shadowColor: "#000",
    shadowOpacity: 0.1,
    shadowRadius: 4,
    elevation: 2,

    alignItems: "center",
  },
  formTitle: {
    fontSize: 18,
    marginBottom: 20,
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
  forgotPasswordContainer: {
    alignSelf: "flex-end",
    marginBottom: 16,
  },
  forgotPasswordText: {
    color: "gray",
    fontSize: 14,
    textDecorationLine: "underline",
  },
  registerLink: {
    marginTop: 10,
    textAlign: "center",
    color: "#007BFF",
    textDecorationLine: "underline",
  },
});

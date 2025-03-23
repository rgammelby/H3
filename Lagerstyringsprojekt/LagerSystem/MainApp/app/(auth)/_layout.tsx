// import { Tabs } from 'expo-router';
// import Ionicons from '@expo/vector-icons/Ionicons';

// export default function TabLayout() {
//   return (
//     <Tabs 
//        screenOptions={{
//         tabBarActiveTintColor: '#ffd33d',
//         headerStyle: {
//             backgroundColor: '#25292e',
            
//           },
//           headerTitleAlign: 'center',
//           headerShadowVisible: false,
//           headerTintColor: '#fff',
//           tabBarStyle: {
//           backgroundColor: '#25292e',
//           },
//       }} 
//     >
//       <Tabs.Screen 
//         name="login" 
//         options={{ 
//             title: 'Login',
//             tabBarIcon:({ color, focused }) => (
//               <Ionicons name={focused ? 'log-in-sharp' : 'log-in-outline'} size={24} color={color} />
//             ),
//         }} 
//       />
//         <Tabs.Screen 
//          name="register" 
//          options={{ 
//              title: 'Register',
//              tabBarIcon:({ color, focused }) => (
//                  <Ionicons name={focused ? 'person-add-sharp' : 'person-add-outline'} size={24} color={color} />
//              ),
//          }} 
//        />
      
//     </Tabs>
//   );
// }

// app/(auth)/_layout.tsx (clearly fixed!)
import { Stack } from "expo-router";

export default function AuthLayout() {
  return (
    <Stack
      screenOptions={{
      headerStyle: { backgroundColor: "#25292e" },
      headerTintColor: "#fff",
      headerTitleAlign: "center",
      headerShadowVisible: false,
      headerShown: false, // Hide all headers in the auth stack
    }}
    >
      <Stack.Screen name="index" options={{ title: "Index", headerShown: false }} />
      <Stack.Screen name="login" options={{ title: "Login", headerShown: false }} />
      <Stack.Screen name="register" options={{  title: "Register", headerShown: false }} />
    </Stack>
  );
}


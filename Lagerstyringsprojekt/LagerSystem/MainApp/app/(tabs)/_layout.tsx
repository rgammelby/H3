import { Tabs, Redirect } from 'expo-router';
import Ionicons from '@expo/vector-icons/Ionicons';
import { useAuth } from '../context/AuthContext'; // add a token check here, incase someone without token tries to access this page by using the url directly
export default function TabLayout() {
  const { token } = useAuth();

  // If no token, redirect to (auth)/login
  if (!token) {
    return <Redirect href='/(auth)/login' />;
  }

  return (
    <Tabs 
       screenOptions={{
        tabBarActiveTintColor: '#ffd33d',
        headerStyle: {
            backgroundColor: '#25292e',
            
          },
          headerTitleAlign: 'center',
          headerShadowVisible: false,
          headerTintColor: '#fff',
          tabBarStyle: {
          backgroundColor: '#25292e',
          },
      }} 
    >
      <Tabs.Screen 
        name="index" 
        options={{ 
            title: 'Home',
            tabBarIcon:({ color, focused }) => (
              <Ionicons name={focused ? 'home-sharp' : 'home-outline'} size={24} color={color} />
            ),
        }} 
      />
        <Tabs.Screen 
         name="search" 
         options={{ 
             title: 'Search',
             tabBarIcon:({ color, focused }) => (
                 <Ionicons name={focused ? 'search' : 'search-outline'} size={24} color={color} />
             ),
         }} 
       />
       <Tabs.Screen 
        name="device" 
        options={{ 
            title: 'Devices',
            tabBarIcon:({ color, focused }) => (
                <Ionicons name={focused ? 'laptop-sharp' : 'laptop-outline'} size={24} color={color} />
            ),
        }} 
      />
      <Tabs.Screen 
        name="myPage" 
        options={{ 
            title: 'My Page',
            tabBarIcon:({ color, focused }) => (
                <Ionicons name={focused ? 'information-circle' : 'information-circle-outline'} size={24} color={color} />
            ),
        }} 
      />
    </Tabs>
  );
}

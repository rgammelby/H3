import { Tabs } from 'expo-router';
import Ionicons from '@expo/vector-icons/Ionicons';

export default function TabLayout() {
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
        name="about" 
        options={{ 
            title: 'Abouttt',
            tabBarIcon:({ color, focused }) => (
                <Ionicons name={focused ? 'information-circle' : 'information-circle-outline'} size={24} color={color} />
            ),
        }} 
      />
    </Tabs>
  );
}

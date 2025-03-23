import { API_BASE_URL } from "./apiConfig";
import { AddActivity, Activity, ActivityType } from "../types/activityRelated";


export async function GetActivitiesByUserId(userId: number): Promise<Activity[] | null> {
    // get the curernt user
    // const user = useAuth();
    // 'user' here is actually the entire AuthContextType, 
    // which has user, token, login, etc. but NOT user.id
    // const auth = useAuth(); auth.user.id will work
    //
    // const { user } = useAuth();
    // 'user' here is actually the entire AuthContextType, 
    // which has user, token, login, etc. but NOT user.id
    // destructure { user } from the auth object

    const response = await fetch(`${API_BASE_URL}GetByUserId?id=${userId}`);

    if (!response.ok) {
        throw new Error('Failed to fetch activities');
    }

    const activities: Activity[] = await response.json();
    return activities;
}

export async function GetAllActivityTypes(): Promise<ActivityType[] | null> {
    const response = await fetch(`${API_BASE_URL}GetAllActivityTypes`);

    if (!response.ok) {
        throw new Error('Failed to fetch activity types');
    }

    const activityTypes: ActivityType[] = await response.json();
    return activityTypes;
}

 // for return, pass the corresponding borrow as parameter
export async function returnDevice(activity: Activity): Promise<void> {
   const now = new Date();
    
   // Build query params (or body) for your /AddActivity endpoint
   const queryParams = new URLSearchParams({
    device_id: activity.device_id.toString(),
    activity_type: "2", // return
    user_id: activity.user_id.toString(),
    start_date: activity.start_date.toString(),
    end_date: now.toString(),
    created_on: now.toString(),
    notes: "Returning device",
  });
  const response = await fetch(`${API_BASE_URL}AddActivity?${queryParams.toString()}`, {
    method: "POST",
  });

  if (!response.ok) {
    throw new Error("Failed to create return activity");
  }
}

// borrow
export async function createBorrowActivity(addActivity: AddActivity): Promise<Activity> {
    const queryParams = new URLSearchParams({
      device_id: addActivity.device_id.toString(),
      activity_type: "1", // borrow
      user_id: addActivity.user_id.toString(),
      start_date: addActivity.start_date.toISOString(),
      end_date: addActivity.end_date.toISOString(),
      created_on: addActivity.created_at.toISOString(),
      notes: addActivity.notes,
    });
  console.log(`${API_BASE_URL}/AddActivity?${queryParams.toString()}`);
    const response = await fetch(`${API_BASE_URL}AddActivity?${queryParams.toString()}`, {
      method: "POST",
    });

  
    if (!response.ok) {
      throw new Error("Failed to create borrow activity");
    }
  
    // If the server returns the newly created activity, parse it:
    const newActivity: Activity = await response.json();
    return newActivity;
  }
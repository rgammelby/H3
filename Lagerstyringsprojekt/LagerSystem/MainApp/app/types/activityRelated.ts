export interface Activity {
    id: number;
    user_id: number;
    device_id: number;

    activity_type: number;
    activityDetail?: ActivityType;

    start_date: Date;
    end_date: Date;
    created_at: Date;
    notes: string;
    lifecycle_id: string;
}

export interface ActivityType {
    id: number;
    activity_type: string;
}

export  interface AddActivity {
    user_id: number;
    device_id: number;
    
    activity_type: number;

    start_date: Date;
    end_date: Date;
    created_at: Date;
    notes: string;
}

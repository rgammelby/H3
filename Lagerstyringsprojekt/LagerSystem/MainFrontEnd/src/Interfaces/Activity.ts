export interface IActivity{
    id: number;
    user_id: number;
    device_id: number;
    activity_type: number;
    start_date: string;
    end_date: string;
    created_at: string;
    notes: string;
    lifecycle_id: string;
}
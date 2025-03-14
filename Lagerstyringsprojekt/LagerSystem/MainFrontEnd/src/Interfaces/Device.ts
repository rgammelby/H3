export interface IDevice{
    id: number;
    device_overview_id: number;
    is_archived: boolean;
    description: string;
    status: number;
    location: number;
    qr: string;
}

export interface IDeviceForm{
    device_overview_id: number;
    is_archived: boolean;
    description: string;
    status: number;
    location: number;
    qr: string;
}
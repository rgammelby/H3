export interface DeviceStatus {
    id: number;
    status_type: string;
}

export interface DeviceType {
    id: number;
    type_name: string;
}

// export interface Image {
//     id: number;
//     image_name: string;
// }

export interface DeviceOverview {
    id: number;
    model: string;

    device_type: number;
    deviceTypeDetail?: DeviceType;

    image: string;
    qty: number;
    available_qty: number;
    last_ordered: string; //  Date String
}

export interface Room{
    id: number;
    designation: string;
}

export interface Cupboard{
    id: number;
    designation: string;
    room_id: number;
    roomDetail?: Room;
}

export interface Device{
    id: number;

    status: number;
    statusDetail?: DeviceStatus;

    location: number;
    locationDetail?: Cupboard;

    device_overview_id: number;
    deviceOverviewDetail?: DeviceOverview;
    
    description?: string;
    qr?: string;
    is_archived: boolean;
}

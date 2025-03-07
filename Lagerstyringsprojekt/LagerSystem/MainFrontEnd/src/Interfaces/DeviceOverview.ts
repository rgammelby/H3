export interface IAllDeviceOverview {
    id: number;
    model: string;
    device_type: number;
    qty: number;
    available_qty: number;
    last_ordered: string;
}

export interface IUpdateDeviceOverview {
    id: number;
    model: string;
    device_type: number;
    image: string;
    qty: number;
    last_ordered: string;
}
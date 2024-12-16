export interface DialogClosedResult {
    isSuccess: boolean;
}

export type LoadState = 'Loading' | 'Loaded' | 'Error';

export type PaymentChannel = 'mtn' | 'atl' | 'vod';
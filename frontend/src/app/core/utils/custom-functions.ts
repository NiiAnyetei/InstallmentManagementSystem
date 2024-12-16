import parsePhoneNumberFromString from 'libphonenumber-js';
import { toast } from 'ngx-sonner';

const mtn = ['024', '025', '053', '054', '055', '059'];
const atl = ['027', '057', '026', '056'];
const vod = ['020', '050'];

export function handleRequestError(msg: any) {
  const title = 'Error';
  toast.error(title, {
    position: 'bottom-right',
    description: msg.error?.detail ? msg.error?.detail : msg.message,
    action: {
      label: 'Close',
      onClick: () => console.log('Action!'),
    },
    actionButtonStyle: 'background-color:#DC2626; color:white;',
  });
}

export function handleRequestSuccess(msg: any) {
  const title = 'Success';
  toast.success(title, {
    position: 'bottom-right',
    description: msg.message,
    action: {
      label: 'Close',
      onClick: () => console.log('Action!'),
    },
    actionButtonStyle: 'background-color:#DC2626; color:white;',
  });
}

export function getCarrier(phoneNumber: string) {
  const parsedPhoneNumber = parsePhoneNumberFromString(phoneNumber, 'GH');

  if(!parsedPhoneNumber) return;

  const nationalNumber = `0${parsedPhoneNumber.nationalNumber}`;

  const networkCode = nationalNumber.slice(0, 3);

  if(mtn.includes(networkCode)) return 'mtn';
  if(atl.includes(networkCode)) return 'atl';
  if(vod.includes(networkCode)) return 'vod';

  return;
}

export function getPageRange(totalItems: number, currentPage: number, itemsPerPage: number): { start: number; end: number } {
  const start = totalItems ? (currentPage - 1) * itemsPerPage + 1 : 0;
  const end = Math.min(currentPage * itemsPerPage, totalItems!);
  return { start, end };
}
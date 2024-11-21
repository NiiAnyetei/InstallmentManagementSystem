import { toast } from 'ngx-sonner';

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
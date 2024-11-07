import { toast } from 'ngx-sonner';

export function handleRequestError(error: any) {
  // const msg = 'An error occurred while fetching users';
  // const msg = error.error.detail;
  const msg = 'Error';
  toast.error(msg, {
    position: 'bottom-right',
    description: error.error?.detail ? error.error?.detail : error.message,
    action: {
      label: 'Close',
      onClick: () => console.log('Action!'),
    },
    actionButtonStyle: 'background-color:#DC2626; color:white;',
  });
}

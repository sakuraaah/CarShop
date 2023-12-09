import { message } from 'antd';

function useHandleError() {

  const handleError = (
    error,
    dontShowMessages
  ) => {
    if (!dontShowMessages) {
      let errorMsgArr = []

      if (error.hasOwnProperty('errors')) {
        Object.entries(error.errors).forEach(([key, errorMsg]) => {
          if (errorMsg && errorMsg[0]) {
            errorMsgArr.push(errorMsg[0]);
          }
        });
      }

      const errorString = errorMsgArr.length ? errorMsgArr.join('\n') : 'TODO Pieprasījuma kļūda';
        
      message.error(errorString, 15);
    }

    document.body.scrollTop = document.documentElement.scrollTop = 0; //pure js scroll to top, supported in all browsers TODO
    console.error(error);
  };

  return [handleError];
}

export default useHandleError;

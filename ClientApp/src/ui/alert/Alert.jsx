import React from 'react';
import { Alert as AntdAlert } from 'antd';

export const Alert = (props) => {
  return (
    <AntdAlert 
      {...props}
      closable={props.closable ?? true}
      showIcon={props.showIcon ?? true}
    />
  )
}

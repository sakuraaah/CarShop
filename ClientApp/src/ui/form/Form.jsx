import React from 'react';
import { Form as AntdForm } from 'antd';

export const Form = (props) => {
  const children = props.children
  
  return (
    <AntdForm 
      {...props}
      layout="vertical"
      validateMessages={{ required: 'Lauks ir obligāts' }}
    >
      {children}
    </AntdForm>
  );
};

import React from 'react';
import { Popconfirm as AntdPopconfirm } from 'antd';

export const Popconfirm = ({
  children,
  ...props
}) => {
  return (
    <AntdPopconfirm 
      {...props}
      okText={props.okText ?? 'Yes'}
      cancelText={props.cancelText ?? 'No'}
    >
      {children}
    </AntdPopconfirm>
  )
}

import React from 'react';
import { Button as AntdButton } from 'antd';

export const Button = (props) => {
  return (
    <AntdButton 
      {...props}
      size={props.size ?? 'large'}
    >
      {props.label}
    </AntdButton>
  )
}
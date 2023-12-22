import React from 'react';
import styled from 'styled-components';
import { InputNumber as AntdInputNumber } from 'antd';
import { FormItem } from '../formItem'

export const InputNumber = (props) => {
  const formItemProps = {
    name: props.name,
    label: props.label,
    rules: props.rules,
    className: props.className,
    style: props.style
  }

  // delete props.name
  // delete props.label
  // delete props.rules
  // delete props.className
  // delete props.style

  const StyledInputNumber = styled(AntdInputNumber)`
    width: 100%;
  `;

  return (
    <FormItem {...formItemProps} >
      <StyledInputNumber
        { ...props } 
        size={props.size ?? 'large'}
      />
    </FormItem>
  );
};

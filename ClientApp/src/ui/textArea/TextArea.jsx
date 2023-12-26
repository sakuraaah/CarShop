import React from 'react';
import { Input as AntdInput } from 'antd';
import { FormItem } from '../formItem'

export const TextArea = (props) => {
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

  return (
    <FormItem {...formItemProps} >
      <AntdInput.TextArea 
        { ...props } 
      />
    </FormItem>
  );
};

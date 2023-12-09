import React from 'react';
import { Select as AntdSelect, Form } from 'antd';
import { FormItem } from '../../ui/formItem'

export const Select = (props) => {
  const formItemProps = {
    name: props.name,
    label: props.label,
    rules: props.rules,
    className: props.className,
    style: props.style
  }

  const children = props.children

  // delete props.name
  // delete props.label
  // delete props.rules
  // delete props.className
  // delete props.style

  return (
    <FormItem {...formItemProps} >
      <AntdSelect {...props} >
        {children}
      </AntdSelect>
    </FormItem>
  );
};

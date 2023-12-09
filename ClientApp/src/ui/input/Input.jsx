import React from 'react';
import { Input as AntdInput, Form } from 'antd';

export const Input = (props) => {
  const formItemProps = {
    name: props.name,
    label: props.label,
    rules: props.rules,
    className: props.className,
    style: props.style
  }

  delete props.name
  delete props.label
  delete props.rules
  delete props.className
  delete props.style

  return (
    <Form.Item {...formItemProps} >
      <AntdInput {...props} />
    </Form.Item>
  );
};

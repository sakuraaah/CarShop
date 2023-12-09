import React from 'react';
import { DatePicker as AntdDatePicker, Form } from 'antd';

export const DatePicker = (props) => {
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
      <AntdDatePicker {...props} />
    </Form.Item>
  )
}
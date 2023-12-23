import React from 'react';
import { Switch as AntdSwitch } from 'antd';
import { FormItem } from '../formItem'

export const Switch = (props) => {
  const formItemProps = {
    name: props.name,
    label: props.label,
    rules: props.rules,
    className: props.className,
    style: props.style
  }

  return (
    <FormItem {...formItemProps} >
      <AntdSwitch 
        {...props}
      />
    </FormItem>
  )
}

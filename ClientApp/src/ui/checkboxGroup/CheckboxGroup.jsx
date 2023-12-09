import { Checkbox as AntdCheckbox, Form } from 'antd';
import { Checkbox } from '../checkbox';
import React from 'react';

export const CheckboxGroup = (props) => {
  const formItemProps = {
    name: props.name,
    label: props.label,
    rules: props.rules,
    className: props.className,
    style: props.style
  }

  const items = props.options

  // delete props.name
  // delete props.label
  // delete props.rules
  // delete props.className
  // delete props.style
  // delete props.options

  return (
    <Form.Item {...formItemProps} >
      <AntdCheckbox.Group {...props} >
        {items
          ? items.map((checkbox) => {
              return (
                <Checkbox 
                  key={checkbox.value} 
                  label={checkbox.label} 
                  value={checkbox.value} 
                />
              )
            })
          : 'TODO No items found'}
      </AntdCheckbox.Group>
    </Form.Item>
  )
}

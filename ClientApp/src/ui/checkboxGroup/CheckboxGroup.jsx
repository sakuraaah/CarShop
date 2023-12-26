import React from 'react';
import { Checkbox as AntdCheckbox } from 'antd';
import { FormItem } from '../../ui/formItem'
import useQueryApiClient from '../../utils/useQueryApiClient';

export const CheckboxGroup = ({
  url,
  sameAsLabel,
  ...props
}) => {
  const formItemProps = {
    name: props.name,
    label: props.label,
    rules: props.rules,
    className: props.className,
    style: props.style
  }

  const { data: options } = useQueryApiClient({
    request: {
      url: url,
      disableOnMount: !url
    }
  });

  // delete props.name
  // delete props.label
  // delete props.rules
  // delete props.className
  // delete props.style
  // delete props.options

  return (
    <FormItem {...formItemProps} >
      <AntdCheckbox.Group 
        {...props}
        options={props.options ?? options?.data?.map((option) => ({
          label: option.name,
          value: sameAsLabel ? option.name : option.id
        }))}
      />
    </FormItem>
  )
}

import React from 'react';
import { Select as AntdSelect } from 'antd';
import { FormItem } from '../../ui/formItem'
import useQueryApiClient from '../../utils/useQueryApiClient';

export const Select = (props) => {

  const formItemProps = {
    name: props.name,
    label: props.label,
    rules: props.rules,
    className: props.className,
    style: props.style
  }

  const { data: options, isLoading } = useQueryApiClient({
    request: {
      url: props.url,
      disableOnMount: !props.url
    }
  });

  // delete props.name
  // delete props.label
  // delete props.rules
  // delete props.className
  // delete props.style

  return (
    <FormItem {...formItemProps} >
      <AntdSelect 
        {...props} 
        size={props.size ?? 'large'}
        allowClear={props.allowClear ?? true}
        options={props.options ?? options?.data?.map((option) => ({
          label: option.name,
          value: option.id
        }))}
        loading={isLoading}
      />
    </FormItem>
  );
};

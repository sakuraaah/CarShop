import React from 'react';
import { Form as AntdForm } from 'antd';
import styled from 'styled-components';
import {
  Button,
  Collapse,
  DatePicker,
  Form,
  Input,
  Select
} from '../../ui';
import { 
  ButtonList,
  FilterWrapper
} from '../../styles/layout/form';

export const DefaultFilters = ({
  filterItems,
  setFilters
}) => {
  const [form] = AntdForm.useForm();

  const onSubmit = (values) => {
    const parsedFilters = {
      ...values,
      StartDate: values.StartDate?.format('YYYY-MM-DD'),
      EndDate: values.EndDate?.format('YYYY-MM-DD')
    }
    setFilters(parsedFilters)
  };

  const onClear = () => {
    form.resetFields()
    setFilters({})
  };

  const renderFilters = (
    <Form form={form} onFinish={onSubmit}>
      <FilterWrapper>
        <DatePicker 
          name={'StartDate'}
          label={'Created from'}
        />
        <DatePicker 
          name={'EndDate'}
          label={'Created until'}
        />
        <Input
          name={'Username'}
          label={'User'}
        />
        <Select
          name={'Status'}
          label={'Status'}
          url={'api/statuses'}
          sameAsLabel
        />
      </FilterWrapper>

      {filterItems.map((filterGroup, key) => (
        <FilterWrapper key={key}>
          {filterGroup.map((item, itemKey) => {
            switch (item.type) {
              case 'input':
                return (
                  <Input
                    key={itemKey}
                    name={item.name}
                    label={item.label}
                  />
                )

              case 'select':
                return (
                  <Select
                    key={itemKey}
                    name={item.name}
                    label={item.label}
                    url={item.apiUrl}
                    sameAsLabel
                  />
                )

              default:
                return <></>
            }
          })}
        </FilterWrapper>
      ))}

      <ButtonList>
        <Button 
          htmlType="submit"
          type="primary" 
          label={'Filter'}
        />
        <Button 
          onClick={onClear}
          label={'Clear'}
        />
      </ButtonList>
    </Form>
  )

  const items = [
    {
      key: '1',
      label: 'Filters',
      children: renderFilters
    }
  ];

  return (
    <Collapse items={items} />
  );
};

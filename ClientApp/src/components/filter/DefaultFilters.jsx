import React from 'react';
import { Form as AntdForm } from 'antd';
import dayjs from 'dayjs';
import {
  Button,
  CheckboxGroup,
  Collapse,
  DatePicker,
  Form,
  Input,
  InputNumber,
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
      ...( values.StartDate && { StartDate: values.StartDate.format('YYYY-MM-DD') }),
      ...( values.EndDate && { EndDate: values.EndDate.format('YYYY-MM-DD') }),
      ...( values.YearFrom && { YearFrom: values.YearFrom.year() }),
      ...( values.YearTo && { YearTo: values.YearTo.year() }),
      ...( values.FeatureList && { FeatureList: values.FeatureList.join(',') }),
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
        {filterItems.addUser && (
          <Input
            name={'Username'}
            label={'Seller'}
          />
        )}
        {filterItems.addStatus && (
          <Select
            name={'Status'}
            label={'Status'}
            url={'api/statuses'}
            sameAsLabel
          />
        )}
      </FilterWrapper>

      {filterItems.items.map((filterGroup, key) => (
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

              case 'inputNumber':
                return (
                  <InputNumber
                    key={itemKey}
                    name={item.name}
                    label={item.label}
                  />
                )

              case 'dateYear':
                return (
                  <DatePicker 
                    key={itemKey}
                    name={item.name}
                    label={item.label}
                    picker="year"
                    disabledDate={(date) => date > dayjs()}
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

              case 'checkboxes':
                return (
                  <CheckboxGroup
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

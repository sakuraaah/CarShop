import React from 'react';
import { useParams } from 'react-router-dom';
import { Flex, Form as AntdForm } from 'antd';
import { Button, Form, SideBySide, Input } from '../ui';
import useQueryApiClient from '../utils/useQueryApiClient';

export const NewProductPage = () => {
  const[form] = AntdForm.useForm();
  const { id } = useParams();

  const onSubmit = (updateStatus, status = 'Confirmed') => {
    form.validateFields()
      .then((values) => {

        if (!id) {
          createPost(values);
        }
      })
      .catch((errorInfo) => {
        form.scrollToField(errorInfo.errorFields[0]?.name, { behavior: 'smooth', block: 'center', scrollMode: 'if-needed' })
      });
  }

  const { appendData: createPost, isLoading: createLoading } = useQueryApiClient({
    request: {
      url: `api/posts`,
      method: 'POST'
    }
  });

  return (
    <Form form={form} >
      <SideBySide
        left={
          <>
            <Input
              name="header"
              label={'Header'}
              rules={[{ required: true }]}
            />
            <Input
              name="text"
              label={'Text'}
              rules={[{ required: true }]}
            />
          </>
        }
      />
      <Button 
        htmlType="submit" 
        onClick={() => onSubmit(false)} 
        type="primary" 
        label={'Save'} 
      />
    </Form>
  )
}

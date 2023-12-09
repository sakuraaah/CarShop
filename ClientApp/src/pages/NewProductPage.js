import React from 'react';
import { Form as AntdForm } from 'antd';
import { CrudForm } from '../components/form/CrudForm';
import {
  Input,
  Label,
  SideBySide, 
} from '../ui';
import { 
  BorderBottom,
  StyledPage, 
  StyledWrapper,
} from '../styles/layout/form';

export const NewProductPage = () => {
  const [form] = AntdForm.useForm();

  return (
    <StyledPage>
      <CrudForm 
        form={form}
        url={'api/posts'}
        formLabel={'Create post'}
      >        
        <StyledWrapper>
          <Label label={'Post info:'} extraBold />

          <BorderBottom />

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
        </StyledWrapper>
      </CrudForm>
    </StyledPage>
  )
}

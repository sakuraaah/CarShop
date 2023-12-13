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
        url={'api/rent-submissions'}
        formLabel={'Create post'}
      >        
        <StyledWrapper>
          <Label label={'Post info:'} extraBold />

          <BorderBottom />

          <SideBySide
            left={
              <Input
                name="AplNr"
                label={'Apliecības numurs'}
                rules={[{ required: true }]}
              />
            }
            right={
              <Input
                name="RegNr"
                label={'Reģistrācijas numurs'}
                rules={[{ required: true }]}
              />
            }
          />

          <SideBySide
            left={
              <Input
                name="CategoryId"
                label={'Kategorija'}
                rules={[{ required: true }]}
              />
            }
          />

          <SideBySide
            left={
              <Input
                name="MarkId"
                label={'Marka'}
                rules={[{ required: true }]}
              />
            }
            right={
              <Input
                name="Model"
                label={'Modelis'}
                rules={[{ required: true }]}
              />
            }
          />

          <SideBySide
            left={
              <Input
                name="Year"
                label={'Gads'}
                rules={[{ required: true }]}
              />
            }
            right={
              <Input
                name="Mileage"
                label={'Nobraukums'}
                rules={[{ required: true }]}
              />
            }
          />
        </StyledWrapper>
      </CrudForm>
    </StyledPage>
  )
}

import React, {useState} from 'react';
import { Form as AntdForm } from 'antd';
import { CrudForm } from '../components/form/CrudForm';
import {
  DatePicker,
  ImageUpload,
  Input,
  InputNumber,
  Label,
  Select,
  SideBySide, 
} from '../ui';
import { 
  BorderBottom,
  StyledPage, 
  StyledWrapper,
} from '../styles/layout/form';
import dayjs from 'dayjs';

export const CrudRentSubmissionPage = () => {
  const [form] = AntdForm.useForm();
  const [imageUrl, setImageUrl] = useState();

  const parseFormToSubmit = (values) => {
    return {
      imgSrc: values.imgSrc,
      aplNr: values.aplNr,
      regNr: values.regNr,
      categoryId: values.categoryId,
      markId: values.markId,
      model: values.model,
      mileage: values.mileage,
      year: values.year.year()
    }
  }

  const parseResponseToForm = (values) => {
    setImageUrl(values.imgSrc)

    return {
      imgSrc: values.imgSrc,
      aplNr: values.aplNr,
      regNr: values.regNr,
      categoryId: values.categoryId,
      markId: values.markId,
      model: values.model,
      mileage: values.mileage,
      year: dayjs().set('year', values.year)
    }
  }

  return (
    <StyledPage>
      <CrudForm 
        form={form}
        url={'rent-submission'}
        apiUrl={'api/rent-submissions'}
        name={'Rent submission'}
        parseFormToSubmit={parseFormToSubmit}
        parseResponseToForm={parseResponseToForm}
      >
        <StyledWrapper>
          <Label label={'Rent submission image:'} extraBold />

          <BorderBottom />

          <ImageUpload
            form={form}
            imageUrl={imageUrl}
            setImageUrl={setImageUrl}
            name="imgSrc"
            rules={[{ required: true }]}
          />
        </StyledWrapper>

        <StyledWrapper>
          <Label label={'Rent submission info:'} extraBold />

          <BorderBottom />

          <SideBySide
            left={
              <Input
                name="aplNr"
                label={'Technical passport number'}
                placeholder="AB1234567"
                rules={[{ required: true }]}
              />
            }
            right={
              <Input
                name="regNr"
                label={'License plate number'}
                placeholder="LV1234"
                rules={[{ required: true }]}
              />
            }
          />

          <SideBySide
            left={
              <Select
                name="categoryId"
                label={'Category'}
                url={'api/categories'}
                rules={[{ required: true }]}
              />
            }
          />

          <SideBySide
            left={
              <Select
                name="markId"
                label={'Mark'}
                url={'api/marks'}
                rules={[{ required: true }]}
              />
            }
            right={
              <Input
                name="model"
                label={'Model'}
                rules={[{ required: true }]}
              />
            }
          />

          <SideBySide
            left={
              <InputNumber
                name="mileage"
                label={'Mileage'}
                min={0}
                precision={0}
                addonAfter={"km"}
                rules={[{ required: true }]}
              />
            }
          />

          <SideBySide
            left={
              <DatePicker
                name="year"
                label={'Year'}
                picker="year"
                disabledDate={(date) => date > dayjs()}
                rules={[{ required: true }]}
              />
            }
          />
        </StyledWrapper>
      </CrudForm>
    </StyledPage>
  )
}

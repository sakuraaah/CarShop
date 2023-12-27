import React, {useState} from 'react';
import { Form as AntdForm } from 'antd';
import { CrudForm } from '../components/form/CrudForm';
import {
  CheckboxGroup,
  DatePicker,
  ImageUpload,
  Input,
  InputNumber,
  Label,
  Select,
  SideBySide,
  TextArea
} from '../ui';
import { 
  BorderBottom,
  StyledPage, 
  StyledWrapper,
} from '../styles/layout/form';
import dayjs from 'dayjs';

export const CrudBuyItemPage = () => {
  const [form] = AntdForm.useForm();

  const [disabled, setDisabled] = useState(false);
  const [imageUrl, setImageUrl] = useState();

  const parseFormToSubmit = (values) => {
    return {
      imgSrc: values.imgSrc,

      aplNr: values.aplNr,
      regNr: values.regNr,
      categoryId: values.categoryId,
      markId: values.markId,
      model: values.model,
      bodyTypeId: values.bodyTypeId,
      colorId: values.colorId,
      seats: values.seats,
      engPower: values.engPower,
      mileage: values.mileage,
      year: values.year.year(),

      features: values.features.map((feature) => ({
        name: feature
      })),

      price: values.price,
      description: values.description,
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
      bodyTypeId: values.bodyTypeId,
      colorId: values.colorId,
      seats: values.seats,
      engPower: values.engPower,
      mileage: values.mileage,
      year: dayjs().set('year', values.year),

      features: values.features.map((feature) => feature.name),

      price: values.price,
      description: values.description,
    }
  }

  return (
    <StyledPage>
      <CrudForm 
        form={form}
        url={'buy-item'}
        apiUrl={'api/buy-items'}
        name={'Vehicle'}
        parseFormToSubmit={parseFormToSubmit}
        parseResponseToForm={parseResponseToForm}
        disabled={disabled}
        setDisabled={setDisabled}
      >
        <StyledWrapper>
          <Label label={'Vehicle image:'} extraBold />

          <BorderBottom />

          <ImageUpload
            form={form}
            imageUrl={imageUrl}
            setImageUrl={setImageUrl}
            name="imgSrc"
            rules={[{ required: true }]}
            disabled={disabled}
          />
        </StyledWrapper>

        <StyledWrapper>
          <Label label={'Product details:'} extraBold />

          <BorderBottom />

          <SideBySide
            left={
              <InputNumber
                name="price"
                label={'Price'}
                min={0}
                precision={2}
                addonAfter={"€"}
                rules={[{ required: true }]}
              />
            }
          />

          <SideBySide
            full={
              <TextArea
                name="description"
                label={'Description'}
              />
            }
          />
        </StyledWrapper>

        <StyledWrapper>
          <Label label={'Vehicle info:'} extraBold />

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
              <Select
                name="bodyTypeId"
                label={'Body type'}
                url={'api/body-types'}
                rules={[{ required: true }]}
              />
            }
          />

          <SideBySide
            left={
              <Select
                name="colorId"
                label={'Color'}
                url={'api/colors'}
                rules={[{ required: true }]}
              />
            }
          />

          <SideBySide
            left={
              <InputNumber
                name="seats"
                label={'Seats'}
                min={1}
                max={9}
                precision={0}
                rules={[{ required: true }]}
              />
            }
          />

          <SideBySide
            left={
              <InputNumber
                name="engPower"
                label={'Engine power'}
                min={0}
                precision={0}
                addonAfter={"kW"}
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

          <BorderBottom />

          <SideBySide
            left={
              <CheckboxGroup
                name="features"
                label={'Features'}
                url={'api/features'}
                sameAsLabel
              />
            }
          />
        </StyledWrapper>
      </CrudForm>
    </StyledPage>
  )
}
